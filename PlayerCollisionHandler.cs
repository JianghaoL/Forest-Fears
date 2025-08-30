using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{

    public bool canGrabVine;
    public Collider vineCollider;
    public bool vineUnlocked;

    void Start()
    {
      canGrabVine = false;
      vineUnlocked = false;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Vine Grab")){
            Debug.Log("enter trigger zone");
            vineCollider = otherCollider;
            if(vineUnlocked)
                canGrabVine = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vine Grab")){
            canGrabVine = false;
        }
    }
}
