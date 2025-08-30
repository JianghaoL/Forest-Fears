using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomJump : MonoBehaviour
{

    [SerializeField]
    float bounceForce;

    void Start()
    {
        //isBouncy = false; //When watered set true
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")){
            Rigidbody rigidTemp = collider.gameObject.GetComponent<Rigidbody>();
            rigidTemp.velocity = new Vector3(rigidTemp.velocity.x, 0, 0);
            rigidTemp.AddForce(new Vector3(0f, bounceForce, 0f), ForceMode.Impulse);
        }
    }
}
