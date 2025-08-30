using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    public GameObject player;
    PlayerScript pScript;
    PlayerTutorial pScriptTutorial;
    public int colTouching;
    public List<Collider> colsTouching;

    public int colGrassTouching;
    public List<Collider> colsGrassTouching;
    // Start is called before the first frame update
    void Start()
    {
        pScript = player.GetComponent<PlayerScript>();
        pScriptTutorial = player.GetComponent<PlayerTutorial>();
        colsTouching = new List<Collider>();
    }

    void FixedUpdate()
    {
        if(pScript.facingRight){
            transform.localPosition = new Vector3(0.319f, 0, 0);
        }
        else{
            transform.localPosition = new Vector3(-0.319f, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pScript == null){
            transform.localPosition = new Vector3(0.319f, 0, 0);
    
            if(colTouching > 0){
                pScriptTutorial.colTouchingOnSide = colsTouching[0];
                pScriptTutorial.groundOnSide = true;
            }
            else if(colTouching == 0){
                pScriptTutorial.colTouchingOnSide = null;
                pScriptTutorial.groundOnSide = false;
            }
            else{
                Debug.LogError("Negative numer of colTouching");
            }
        }
        else{
            if(pScript.facingRight){
                transform.localPosition = new Vector3(0.319f, 0, 0);
            }
            else{
                transform.localPosition = new Vector3(-0.319f, 0, 0);
            }

            if(colTouching > 0){
                pScript.colTouchingOnSide = colsTouching[0];
                pScript.groundOnSide = true;
            }
            else if(colTouching == 0){
                pScript.colTouchingOnSide = null;
                pScript.groundOnSide = false;
            }
            else{
                Debug.LogError("Negative numer of colTouching");
            }

            if(colGrassTouching > 0){
                pScript.colGrassTouchingOnSide = colsGrassTouching[0];
                pScript.grassWallOnSide = true;
            }
            else if(colGrassTouching == 0){
                pScript.colGrassTouchingOnSide = null;
                pScript.grassWallOnSide = false;
            }
            else{
                Debug.LogError("Negative numer of colGrassTouching");
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))){
            colsTouching.Add(other);
            colTouching++;
        }

        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Wall"))){
            colsGrassTouching.Add(other);
            colGrassTouching++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))){
            colsTouching.Remove(other);
            colTouching--;
        }

        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Wall"))){
            colsGrassTouching.Remove(other);
            colGrassTouching--;
        }
    }
}
