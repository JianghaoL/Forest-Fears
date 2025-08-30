using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;

//using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rigid;
    public CapsuleCollider col;

    //0 - up, 1 - down, 2 - left, 3 - right, 4 - jump, add more as needed (assign in editor for now but later manage with a settings manager)
    public KeyCode[] controls;

    public float maxSpeed;
    public float maxClimbSpeed;
    public float accelTime;
    public float decelTime;
    public float jumpForce;
    public LayerMask groundMask;
    public LayerMask semiSolidMask;
    public float jumpBufferTime;
    public float ledgeSnapTime;
    public float ledgeHangTime;
    public float ledgeMantleTime;
    
    public bool moveLocked;

    public float horiVel;
    float climbVel;
    public bool onGround;
    public bool facingRight;
    public bool groundOnTopSide;
    public bool groundOnBottomSide;
    public bool groundOnSide;
    public Collider colTouchingOnSide;
    public GameObject FrontCheck;
    public bool grassWallOnSide;
    public Collider colGrassTouchingOnSide;
    public bool grassOnTopSide;
    public bool grassOnBottomSide;
    public bool grassGroundOnBottomSide;

    bool grabbingWall;
    int grabCooldown;

    public int jumpBuffer;

    public GameObject model;
    public Animator anim;
    bool onGroundLastFrame;
    float yVelLastFrame;

    public bool grassHurts;
    public GameObject fadeBlackUI;
    public float fadeBlackTime;
    Vector3 lastSafePos;

    public float vineSnapTime;

    public GameObject grassClimbTutorialText;
    public GameObject vineSwingTutorialText;
    public GameObject mushroomBounceTutorialText;

    public GameObject[] tutorials;

    public float noLedgeGrabTimer;
    PlayerCollisionHandler pColHan;
    public LayerMask grassMask;
    // Start is called before the first frame update

    [SerializeField] PlayerMovementAudioHandler playerMovementAudioHandler;
    [SerializeField] GameObject grassTutorial;
    bool tutorialActive = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        moveLocked = false;
        anim = model.GetComponentInChildren<Animator>();
        StartCoroutine(tutorialTextFallDown(grassTutorial));
        grassHurts = true;
        pColHan = GetComponent<PlayerCollisionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(controls[4]))
        {
            jumpBuffer = Mathf.RoundToInt(jumpBufferTime * (1 / Time.fixedDeltaTime));
            Debug.Log(jumpBuffer);
        }
        if(facingRight){
            model.transform.eulerAngles = Vector3.zero;
        }
        else{
            model.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if(Input.GetKeyDown(KeyCode.F1)){
            GrassClimbTutorial();
        }
    }

    //I set the fixed timestep to be 1/100th of a sec
    void FixedUpdate()
    {
        
        grabbingWall = grassWallOnSide && (Input.GetKey(controls[9]) || Input.GetKey(controls[10])) && (grabCooldown <= 0);
        grabCooldown--;
        if(moveLocked){
            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;
        }
        else if(grabbingWall){
            anim.SetBool("climbing", true);
            rigid.useGravity = false;
            if(facingRight){
                transform.position = new Vector3(colGrassTouchingOnSide.bounds.center.x - colGrassTouchingOnSide.bounds.extents.x - 0.4f, transform.position.y, transform.position.z);
            }
            else{
                transform.position = new Vector3(colGrassTouchingOnSide.bounds.center.x + colGrassTouchingOnSide.bounds.extents.x + 0.4f, transform.position.y, transform.position.z);
            }
            if((Input.GetKey(controls[0]) && !Input.GetKey(controls[1])) || (Input.GetKey(controls[5]) && !Input.GetKey(controls[6]))){
                climbVel += maxClimbSpeed / (accelTime * (1 / Time.fixedDeltaTime));
                anim.SetBool("climbing", true);
            }
            if((Input.GetKey(controls[1]) && !Input.GetKey(controls[0])) || (Input.GetKey(controls[6]) && !Input.GetKey(controls[5])))
            {
                climbVel -= maxClimbSpeed / (accelTime * (1 / Time.fixedDeltaTime));
                anim.SetBool("climbing", true);
            }
            float maxRedone = maxClimbSpeed;
            float minRedone = -maxClimbSpeed;
            if(!grassOnTopSide)
                maxRedone = 0;
            if(!grassOnBottomSide)
                minRedone = 0;
            climbVel = Mathf.Clamp(climbVel, minRedone, maxRedone);
            if( (Input.GetKey(controls[0]) || Input.GetKey(controls[5])) && (Input.GetKey(controls[1]) || Input.GetKey(controls[6]))
                || (!Input.GetKey(controls[0]) && !Input.GetKey(controls[1]) && !Input.GetKey(controls[5]) && !Input.GetKey(controls[6])) )
            {
                climbVel = 0;
                anim.SetBool("still", true);
            }
            else{
                anim.SetBool("still", false);
            }
            rigid.velocity = new Vector3(0, climbVel, 0);
            if(jumpBuffer > 0){
                jumpBuffer = 0;
                if(climbVel == 0){
                    if(Input.GetKey(controls[0]) || Input.GetKey(controls[5])){
                        rigid.AddRelativeForce(0, jumpForce * 0.9f, 0);
                        anim.SetBool("climbing", false);
                        anim.SetTrigger("jump");
                    }
                    else if(Input.GetKey(controls[1]) || Input.GetKey(controls[6])){
                        rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                        rigid.AddRelativeForce(0, -jumpForce * 0.9f, 0);
                        anim.SetBool("climbing", false);
                        anim.SetTrigger("jump");
                    }
                    else{
                        rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                        rigid.AddRelativeForce(0, jumpForce * 0.7f, 0);
                        anim.SetBool("climbing", false);
                        anim.SetTrigger("jump");
                    }
                }
                else{
                    if(Input.GetKey(controls[1]) || Input.GetKey(controls[6])){
                        rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                        rigid.AddRelativeForce(0, -jumpForce * 0.7f, 0);
                        anim.SetBool("climbing", false);
                        anim.SetTrigger("jump");
                    }
                    else{
                        rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                        rigid.AddRelativeForce(0, jumpForce * 0.7f, 0);
                        anim.SetBool("climbing", false);
                        anim.SetTrigger("jump");
                    }
                }
                if(!((facingRight && (Input.GetKey(controls[3]) || Input.GetKey(controls[8]))) || (!facingRight && (Input.GetKey(controls[2]) || Input.GetKey(controls[7]))) || ((Input.GetKey(controls[0]) || Input.GetKey(controls[5]) || Input.GetKey(controls[1]) || Input.GetKey(controls[6])) && !((facingRight && (Input.GetKey(controls[2]) || Input.GetKey(controls[7]))) || (!facingRight && (Input.GetKey(controls[3]) || Input.GetKey(controls[8]))))))){
                    Debug.Log("i tried");
                    if(facingRight)
                        horiVel = -maxSpeed;
                    else
                        horiVel = maxSpeed;
                    rigid.velocity = new Vector3(horiVel, rigid.velocity.y, 0);
                }
                facingRight = !facingRight;
                noLedgeGrabTimer = 3;
                grabbingWall = false;
                grabCooldown = 5;
                if(Input.GetKey(KeyCode.LeftShift)){
                    // Debug.Break();
                }
            }
        }
        else{
            anim.SetBool("climbing", false);
            rigid.useGravity = true;
            Ray baseray = new Ray(transform.position, Vector3.down);
            if((Input.GetKey(controls[3]) && !Input.GetKey(controls[2])) || (Input.GetKey(controls[8]) && !Input.GetKey(controls[7]))){
                horiVel += maxSpeed / (accelTime * (1 / Time.fixedDeltaTime));
                facingRight = true;
                if(!(rigid.velocity.y > 0 && Physics.SphereCast(baseray, 0.5f, 0.55f, semiSolidMask)))
                    anim.SetBool("walking", true);
            }
            if((Input.GetKey(controls[2]) && !Input.GetKey(controls[3])) || (Input.GetKey(controls[7]) && !Input.GetKey(controls[8])))
            {
                horiVel -= maxSpeed / (accelTime * (1 / Time.fixedDeltaTime));
                facingRight = false;
                if(!(rigid.velocity.y > 0 && Physics.SphereCast(baseray, 0.5f, 0.55f, semiSolidMask)))
                    anim.SetBool("walking", true);
            }
            horiVel = Mathf.Clamp(horiVel, -maxSpeed, maxSpeed);
            if( (Input.GetKey(controls[2]) && Input.GetKey(controls[3]) || (Input.GetKey(controls[8]) && Input.GetKey(controls[7])) || (Input.GetKey(controls[2]) && Input.GetKey(controls[8])) || (Input.GetKey(controls[3]) && Input.GetKey(controls[7])))
                || ((!Input.GetKey(controls[2]) && !Input.GetKey(controls[3])) && (!Input.GetKey(controls[8]) && !Input.GetKey(controls[7]))) )
            {
                if(onGround){
                    if(horiVel > 0){
                        horiVel -= Mathf.Min(horiVel, maxSpeed / (accelTime * (1 / Time.fixedDeltaTime)));
                    }
                    if(horiVel < 0){
                        horiVel += Mathf.Min(Mathf.Abs(horiVel), maxSpeed / (accelTime * (1 / Time.fixedDeltaTime)));
                    }
                }
                else{
                    if(horiVel > 0){
                        horiVel -= Mathf.Min(horiVel, maxSpeed / (accelTime * 3 * (1 / Time.fixedDeltaTime)));
                    }
                    if(horiVel < 0){
                        horiVel += Mathf.Min(Mathf.Abs(horiVel), maxSpeed / (accelTime * 3 * (1 / Time.fixedDeltaTime)));
                    }
                }
                anim.SetBool("walking", false);
            }
            if(rigid.velocity.x == 0 && Mathf.Abs(horiVel) > (maxSpeed / (accelTime * (1 / Time.fixedDeltaTime))) * 2)
                horiVel = 0;
            rigid.velocity = new Vector3(horiVel, rigid.velocity.y, 0);
            onGround = Physics.SphereCast(baseray, 0.5f, 0.55f, groundMask) && rigid.velocity.y <= 0;
            if(jumpBuffer > 0){
                jumpBuffer -= 1;
                if(onGround){
                    rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                    rigid.AddRelativeForce(0, jumpForce, 0);
                    jumpBuffer = 0;
                    anim.SetBool("walking", false);
                    anim.SetTrigger("jump");
                    playerMovementAudioHandler.PlayFootstepSFX();
                }
            }
            Ray baseray0 = new Ray(transform.position + new Vector3(0.45f, 0, 0), Vector3.down);
            Ray baseray00 = new Ray(transform.position + new Vector3(-0.45f, 0, 0), Vector3.down);
            if(!moveLocked && onGround && !grassGroundOnBottomSide && Physics.SphereCast(baseray0, 0.1f, 0.95f, groundMask) && Physics.SphereCast(baseray00, 0.1f, 0.95f, groundMask) && !Physics.SphereCast(baseray0, 0.1f, 0.95f, semiSolidMask) && !Physics.SphereCast(baseray00, 0.1f, 0.95f, semiSolidMask) && !Physics.SphereCast(baseray0, 0.1f, 0.95f, grassMask) && !Physics.SphereCast(baseray00, 0.1f, 0.95f, grassMask)){
                lastSafePos = transform.position;
                if(Input.GetKey(KeyCode.T))
                    Debug.Break();
            }
            if(!facingRight){
                if(groundOnSide && !groundOnTopSide && noLedgeGrabTimer <= 0 && !colTouchingOnSide.gameObject.CompareTag("swingsidetoside")){
                    StartCoroutine(LedgeMantle(colTouchingOnSide));
                }
            }
            else{
                if(groundOnSide && !groundOnTopSide && noLedgeGrabTimer <= 0 && !colTouchingOnSide.gameObject.CompareTag("swingsidetoside")){
                    StartCoroutine(LedgeMantle(colTouchingOnSide));
                }
            }
        }
        Ray baseray2 = new Ray(transform.position, Vector3.down);
        if(!(rigid.velocity.y > 0 && Physics.SphereCast(baseray2, 0.5f, 0.55f, semiSolidMask)))
            anim.SetBool("grounded", onGround);
        anim.SetBool("movingUp", (rigid.velocity.y > 1.5f) && !onGround && !onGroundLastFrame);
        if(onGround && !onGroundLastFrame && yVelLastFrame < -1f){
            anim.SetTrigger("land");
            playerMovementAudioHandler.PlayFootstepSFX();
        }

        onGroundLastFrame = onGround;
        yVelLastFrame = rigid.velocity.y;
        noLedgeGrabTimer--;
        if(noLedgeGrabTimer > 5){
            Debug.Log(noLedgeGrabTimer);
            noLedgeGrabTimer = 0;
            Debug.Break();
        }
    }

    IEnumerator LedgeMantle(Collider ledgeCol){
        moveLocked = true;
        grabCooldown = 5;
        Vector3 targetPos0;
        Vector3 startPos0 = transform.position;
        anim.SetBool("grab", true);
        if(transform.position.x < ledgeCol.bounds.center.x)
            targetPos0 = new Vector3(ledgeCol.bounds.center.x - ledgeCol.bounds.extents.x - col.radius * transform.localScale.x + 0.46f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y - 0.82f, 0);
        else
            targetPos0 = new Vector3(ledgeCol.bounds.center.x + ledgeCol.bounds.extents.x + col.radius * transform.localScale.x - 0.46f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y - 0.82f, 0);
        for(int i = 0; i < ledgeSnapTime * (1 / Time.fixedDeltaTime); i++){
            transform.position = startPos0 + (targetPos0 - startPos0) * ((float) i / (ledgeSnapTime * 100));
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(ledgeCol.gameObject.name);
        transform.position = targetPos0;
        rigid.isKinematic = true;
        yield return new WaitForSeconds(ledgeHangTime);
        anim.SetTrigger("mantle");
        Vector3 startPos = transform.position;
        Vector3 targetPos;
        if(transform.position.x < ledgeCol.bounds.center.x)
            targetPos = new Vector3(transform.position.x + 0.6f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y + (col.height / 2) * transform.localScale.y, 0);
        else
            targetPos = new Vector3(transform.position.x - 0.6f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y + (col.height / 2) * transform.localScale.y, 0);
        float startTime = Time.time;
        for(int i = 0; i < ledgeMantleTime * (1 / Time.fixedDeltaTime); i++){
            // transform.position = startPos + (targetPos - startPos) * ((float) i / (ledgeMantleTime * 100));
            yield return new WaitForFixedUpdate();
            if(Time.time - startTime > ledgeMantleTime)
                break;
        }
        transform.position = targetPos;
        // model.transform.localPosition = Vector3.zero;
        // yield return new WaitForFixedUpdate();
        moveLocked = false;
        rigid.isKinematic = false;
        anim.SetBool("grab", false);
        anim.ResetTrigger("mantle");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Ground")) || other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Wall")) || other.gameObject.layer.Equals(LayerMask.NameToLayer("mushroom"))){
            if(grassHurts && !moveLocked){
                StartCoroutine(Respawn());
            }
        }
        if(other.gameObject.GetComponent<CutsceneTrigger>() != null){
            Time.timeScale = 0;
            if(other.gameObject.GetComponent<CutsceneTrigger>().cutsceneIndex == 1)
                grassHurts = false;
            if(other.gameObject.GetComponent<CutsceneTrigger>().cutsceneIndex == 2)
                pColHan.vineUnlocked = true;
            if(other.gameObject.GetComponent<CutsceneTrigger>().cutsceneIndex == 4)
                StartCoroutine(waitToEnd());
            else
                StartCoroutine(tutorialTextFallDown(tutorials[other.gameObject.GetComponent<CutsceneTrigger>().cutsceneIndex]));
            rigid.velocity = Vector3.zero;
            this.enabled = false;
        }
    }

    IEnumerator Respawn(){
        moveLocked = true;
        for(int i = 0; i < fadeBlackTime / Time.fixedDeltaTime; i++){
            fadeBlackUI.GetComponent<Image>().color = new Color(0, 0, 0, i / (fadeBlackTime / Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        transform.position = lastSafePos;
        moveLocked = false;
        for(int i = 0; i < fadeBlackTime / Time.fixedDeltaTime; i++){
            fadeBlackUI.GetComponent<Image>().color = new Color(0, 0, 0, 0 - i / (fadeBlackTime / Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        fadeBlackUI.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    void GrassClimbTutorial(){
        StartCoroutine(tutorialTextFallDown(grassClimbTutorialText));
    }

    IEnumerator tutorialTextFallDown(GameObject g){
        moveLocked = true;
        tutorialActive = true;
        g.SetActive(true);
        anim.SetBool("walking", false);
        for(int i = 0; i < 250; i++){
            float progress = ((float) i) / 250;
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Abs(Mathf.Cos(4 * Mathf.PI * progress)) * Mathf.Pow(progress - 1, 2) * 1000);
            if(Input.GetKey(KeyCode.Escape)){
                g.SetActive(false);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        if(g.activeSelf){
            while(!Input.GetKey(KeyCode.Escape)){
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        g.SetActive(false);
        moveLocked = false;
        tutorialActive = false;
    }

    public bool GetTutorialActive()
    {
        return tutorialActive;
    }

    IEnumerator waitToEnd(){
        yield return new WaitForSecondsRealtime(33.75f);
        SceneManager.LoadScene(3);
    }
}
