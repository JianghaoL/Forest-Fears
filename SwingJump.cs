//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SwingJump : MonoBehaviour
{

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    PlayerCollisionHandler playerCollisionHandler;

    Vector3 tangentDirection;

    [SerializeField]
    PlayerScript playerScript;

    public RopeMotion ropeScript;
    
    public GameObject model;

    float startSwingTime;
    Vector3 startPos;

    [SerializeField] PlayerMovementAudioHandler playerMovementAudioHandler;

    void StartSwing(Collider grabPointCollider){
        playerScript.enabled = false;

        playerMovementAudioHandler.PlayVineJump();

        //Debug.Log("Start swinging");
        RaycastHit hit;

        Vector3 swingRopeDirection = grabPointCollider.transform.parent.parent.transform.position - transform.position;
        ropeScript = grabPointCollider.transform.parent.parent.gameObject.GetComponent<RopeMotion>();
        if(!ropeScript.swinging){
            ropeScript.dir = (GetComponent<PlayerScript>().facingRight);
            startPos = model.transform.position;
        }
        ropeScript.swinging = true;

        playerScript.anim.SetBool("climbing", true);
        playerScript.anim.SetBool("still", true);
        

        // if (Physics.Raycast(transform.position, swingRopeDirection, out hit, 100f, layerMask)){
            if(ropeScript.dir)
                tangentDirection = new Vector3(swingRopeDirection.y, -swingRopeDirection.x, swingRopeDirection.z);
            else
                tangentDirection = new Vector3(-swingRopeDirection.y, swingRopeDirection.x, swingRopeDirection.z);

            Vector3 tangentVelocity = Vector3.Normalize(tangentDirection) * 10;

            GetComponent<Rigidbody>().velocity = tangentVelocity.normalized * 15 * Mathf.Sin(((ropeScript.effectiveAngle - (ropeScript.leftmostRot - 5)) / ((ropeScript.rightmostRot + 5) - (ropeScript.leftmostRot - 5))) * Mathf.PI);
            // Debug.Log(((ropeScript.effectiveAngle - (ropeScript.leftmostRot - 5)) / ((ropeScript.rightmostRot + 5) - (ropeScript.leftmostRot - 5))) * Mathf.PI);
            transform.position = grabPointCollider.transform.parent.parent.transform.position + Vector3.Normalize(swingRopeDirection) * -ropeScript.targetLength;
            transform.eulerAngles = new Vector3(0, 0, ropeScript.effectiveAngle - 90);
            if(Time.time - startSwingTime < playerScript.vineSnapTime){
                model.transform.position = startPos + ((grabPointCollider.transform.parent.parent.transform.position + Vector3.Normalize(swingRopeDirection) * -ropeScript.targetLength) - startPos) * Mathf.Sqrt((Time.time - startSwingTime) / playerScript.vineSnapTime);
            }
            else{
                model.transform.localPosition = Vector3.zero;
            }
            model.transform.Translate(Vector3.left * 0.4f);
        // }
    }

    public void StopSwing(){
        playerMovementAudioHandler.StopVineJump();

        model.transform.localPosition = Vector3.zero;
        playerScript.facingRight = (ropeScript.effectiveAngle >= 90);
        playerScript.horiVel = GetComponent<Rigidbody>().velocity.x;
        Debug.Log("ah: " + playerScript.facingRight + "," + ropeScript.effectiveAngle + "," + GetComponent<Rigidbody>().velocity + "," + playerScript.horiVel);
        if(ropeScript != null){
            ropeScript.swinging = false;
            ropeScript.DoneSwinging();
        }
        transform.eulerAngles = Vector3.zero;
        playerScript.enabled = true;
        playerCollisionHandler.canGrabVine = false;
    }

    void Update()
    {
        if(playerCollisionHandler.canGrabVine){
            if (Input.GetKey(playerScript.controls[4])){
                StartSwing(playerCollisionHandler.vineCollider);
            }
            else{
                startSwingTime = Time.time;
            }

            if (Input.GetKeyUp(playerScript.controls[4])){
                StopSwing();
            }
            
        }
    }
}
