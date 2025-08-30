using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    public GameObject player;
    PlayerScript pScript;
    public int colTouching;
    public int colGrassTouching;
    public int colGrassGroundTouching;
    // Start is called before the first frame update
    void Start()
    {
        pScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(colTouching > 0){
            pScript.groundOnBottomSide = true;
        }
        else if(colTouching == 0){
            pScript.groundOnBottomSide = false;
        }
        else{
            Debug.LogError("Negative numer of colTouching");
        }

        if(colGrassTouching > 0){
            pScript.grassOnBottomSide = true;
        }
        else if(colGrassTouching == 0){
            pScript.grassOnBottomSide = false;
        }
        else{
            Debug.LogError("Negative numer of colGrassTouching");
        }

        if(colGrassGroundTouching > 0){
            pScript.grassGroundOnBottomSide = true;
        }
        else if(colGrassGroundTouching == 0){
            pScript.grassGroundOnBottomSide = false;
        }
        else{
            Debug.LogError("Negative numer of colGrassGroundTouching");
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
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Ground"))){
            colGrassGroundTouching++;
            pScript.grassGroundOnBottomSide = true;
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
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Grass Ground"))){
            colGrassGroundTouching--;
        }
    }
}
