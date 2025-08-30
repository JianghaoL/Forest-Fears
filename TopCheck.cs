using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCheck : MonoBehaviour
{
    public GameObject player;
    PlayerScript pScript;
    PlayerTutorial pScriptTutorial;
    public int colTouching;
    public int colGrassTouching;
    // Start is called before the first frame update
    void Start()
    {
        pScript = player.GetComponent<PlayerScript>();
        pScriptTutorial = player.GetComponent<PlayerTutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pScript == null){
            if(colTouching > 0){
                pScriptTutorial.groundOnTopSide = true;
            }
            else if(colTouching == 0){
                pScriptTutorial.groundOnTopSide = false;
            }
            else{
                Debug.LogError("Negative numer of colTouching");
            }
        }
        else{
            if(colTouching > 0){
                pScript.groundOnTopSide = true;
            }
            else if(colTouching == 0){
                pScript.groundOnTopSide = false;
            }
            else{
                Debug.LogError("Negative numer of colTouching");
            }

            if(colGrassTouching > 0){
                pScript.grassOnTopSide = true;
            }
            else if(colGrassTouching == 0){
                pScript.grassOnTopSide = false;
            }
            else{
                Debug.LogError("Negative numer of colGrassTouching");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))){
            colTouching++;
        }
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Wall"))){
            colGrassTouching++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))){
            colTouching--;
        }
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Wall"))){
            colGrassTouching--;
        }
    }
}
