using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semisolidplatform : MonoBehaviour
{
    public GameObject player;
    public BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y >= transform.position.y + (player.transform.localScale.y * (player.GetComponent<CapsuleCollider>().height / 2)) + (transform.localScale.y * (col.size.y / 2)) - 0.1f + (Mathf.Min(player.GetComponent<Rigidbody>().velocity.y, 0) * Time.fixedDeltaTime)){
            col.enabled = true;
        }
        else{
            col.enabled = false;
        }
    }
}
