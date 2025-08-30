using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTutorial : MonoBehaviour
{
    Rigidbody rigid;
    public KeyCode[] controls;
    public float horiVel;
    public bool canRight;
    public bool canJump;
    int jumpBuffer;
    public GameObject model;
    public bool onGround;
    Animator anim;
    public LayerMask groundMask;
    public bool groundOnSide;
    public bool groundOnTopSide;
    public Collider colTouchingOnSide;
    public GameObject jumpTutorialText;
    public GameObject semiTutorialText;
    public GameObject walkTutorialText;
    CapsuleCollider col;

    [SerializeField] PlayerMovementAudioHandler playerMovementAudioHandler;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        canRight = false;
        canJump = false;
        anim = model.GetComponentInChildren<Animator>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(controls[3]) || Input.GetKeyDown(controls[8])) && !canRight){
            canRight = true;
            Debug.Log("pressed right");
            walkTutorialText.SetActive(false);
        }

        if(Input.GetKeyDown(controls[4]))
        {
            jumpBuffer = Mathf.RoundToInt(0.12f * (1 / Time.fixedDeltaTime));
            Debug.Log(jumpBuffer);
        }
        model.transform.eulerAngles = Vector3.zero;
    }

    void FixedUpdate()
    {
        if(canRight){
            if(rigid.velocity.x < 5.95f){
                horiVel += 6 / (0.2f * (1 / Time.fixedDeltaTime));
                // Debug.Log(horiVel);
                rigid.velocity = new Vector3(horiVel, rigid.velocity.y, 0);
            }
            else{
                rigid.velocity = new Vector3(6, rigid.velocity.y, 0);
            }
            if(onGround){
                anim.SetBool("walking", true);
            }
        }
        Ray baseray = new Ray(transform.position, Vector3.down);
        onGround = Physics.SphereCast(baseray, 0.5f, 0.55f, groundMask) && rigid.velocity.y <= 0;
        anim.SetBool("grounded", onGround);
        if(jumpBuffer > 0 && canJump){
            jumpBuffer -= 1;
            if(onGround){
                rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                rigid.AddRelativeForce(0, 1250, 0);
                jumpBuffer = 0;
                anim.SetBool("walking", false);
                anim.SetTrigger("jump");
                playerMovementAudioHandler.PlayFootstepSFX();
            }
        }
        if(groundOnSide && !groundOnTopSide){
            StartCoroutine(LedgeMantle(colTouchingOnSide, true));
        }
    }

    IEnumerator LedgeMantle(Collider ledgeCol, bool facingRight){
        canRight = false;
        Vector3 targetPos0;
        Vector3 startPos0 = transform.position;
        anim.SetBool("grab", true);
        if(facingRight)
            targetPos0 = new Vector3(ledgeCol.bounds.center.x - ledgeCol.bounds.extents.x - col.radius * transform.localScale.x + 0.46f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y - 0.82f, 0);
        else
            targetPos0 = new Vector3(ledgeCol.bounds.center.x + ledgeCol.bounds.extents.x + col.radius * transform.localScale.x - 0.46f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y - 0.82f, 0);
        for(int i = 0; i < 0.05f * (1 / Time.fixedDeltaTime); i++){
            transform.position = startPos0 + (targetPos0 - startPos0) * ((float) i / (0.05f * 100));
            yield return new WaitForFixedUpdate();
        }
        transform.position = targetPos0;
        rigid.isKinematic = true;
        yield return new WaitForSeconds(0.25f);
        anim.SetTrigger("mantle");
        Vector3 startPos = transform.position;
        Vector3 targetPos;
        if(facingRight)
            targetPos = new Vector3(transform.position.x + 0.6f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y + (col.height / 2) * transform.localScale.y, 0);
        else
            targetPos = new Vector3(transform.position.x - 0.6f, ledgeCol.bounds.center.y + ledgeCol.bounds.extents.y + (col.height / 2) * transform.localScale.y, 0);
        for(int i = 0; i < 0.56f * (1 / Time.fixedDeltaTime); i++){
            // transform.position = startPos + (targetPos - startPos) * ((float) i / (ledgeMantleTime * 100));
            yield return new WaitForFixedUpdate();
        }
        transform.position = targetPos;
        // model.transform.localPosition = Vector3.zero;
        rigid.isKinematic = false;
        canRight = true;
        horiVel = 0;
        anim.SetBool("grab", false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.CompareTag("jumptutorial")){
            StartCoroutine(FirstJump());
        }
        if(other.gameObject.CompareTag("semitutorial")){
            StartCoroutine(FirstSemi());
        }
    }

    IEnumerator FirstJump(){
        canJump = true;
        for(int i = 0; i < 50; i++){
            Time.timeScale = 1 - (((float) i) / 50);
            jumpTutorialText.GetComponent<Image>().color = new Color(1, 1, 1, (((float) i + 1) / 50) * 0.5f);
            if(Input.GetKey(KeyCode.Space)){
                Time.timeScale = 1;
                break;
            }
            yield return new WaitForSecondsRealtime(0.04f);
        }
        if(Time.timeScale != 1){
            while(!Input.GetKey(controls[4])){
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        Time.timeScale = 1;
        jumpTutorialText.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    IEnumerator FirstSemi(){
        for(int i = 0; i < 50; i++){
            Time.timeScale = 1 - (((float) i) / 50);
            semiTutorialText.GetComponent<Image>().color = new Color(1, 1, 1, (((float) i + 1) / 50) * 0.5f);
            if(Input.GetKey(KeyCode.Space)){
                Time.timeScale = 1;
                break;
            }
            yield return new WaitForSecondsRealtime(0.04f);
        }
        if(Time.timeScale != 1){
            while(!Input.GetKey(controls[4])){
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        Time.timeScale = 1;
        semiTutorialText.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
}
