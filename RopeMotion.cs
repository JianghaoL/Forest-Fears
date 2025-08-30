using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class RopeMotion : MonoBehaviour
{
    public bool swinging;
    GameObject player;
    
    [SerializeField]
    [Tooltip("true = left to right, false = right to left")]
    bool initialDir;
    public float leftmostRot;
    public float rightmostRot;
    public float targetLength;
    public bool dir;
    public float effectiveAngle;
    float swingResetVel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        effectiveAngle = 90;
        // dir = initialDir;
    }

    // Update is called once per frame
    void Update()
    {
        if(swinging){
            float tempEulerX = Mathf.Acos((transform.position.x - player.transform.position.x) / Vector3.Distance(transform.position, player.transform.position)) * Mathf.Rad2Deg;
            
            // Debug.Log(transform.localEulerAngles);
            if(leftmostRot > tempEulerX && !dir){
                tempEulerX = leftmostRot;
                DoneSwinging();
            }
            if(tempEulerX > rightmostRot && dir){
                tempEulerX = rightmostRot;
                DoneSwinging();
                // Debug.Log("baaaaaa");
            }
            effectiveAngle = tempEulerX;
            transform.localEulerAngles = new Vector3(tempEulerX, -90, -90);
        }
        else{
            if((swingResetVel > 0 && effectiveAngle < 90) || (swingResetVel < 0 && effectiveAngle > 90)){
                swingResetVel += (90 - effectiveAngle) * 0.3f * Time.deltaTime * 4;
            }
            else{
                swingResetVel += (90 - effectiveAngle) * 0.6f * Time.deltaTime * 4;
            }
            
            effectiveAngle += swingResetVel * Time.deltaTime * 4;
            transform.localEulerAngles = new Vector3(effectiveAngle, -90, -90);
        }
    }

    public void DoneSwinging(){
        dir = !dir;
    }
}
