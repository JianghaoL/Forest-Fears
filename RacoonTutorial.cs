using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacoonTutorial : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < 16 || (transform.position.x > 23 && transform.position.x < 40))
            transform.position = new Vector3(transform.position.x + (player.transform.position.x + 10 - transform.position.x) * 0.5f, -0.5f, 0);
        if(transform.position.x >= 16 && transform.position.x <= 23){
            float progress = (transform.position.x - 16) / 7;
            transform.position = new Vector3(transform.position.x + (player.transform.position.x + 10 - transform.position.x) * 0.5f, -0.5f + (-Mathf.Pow(2 * progress - 1, 2) + 1) * 4, 0);
        }
        if(transform.position.x >= 40 && transform.position.x <= 44){
            float progress = (transform.position.x - 40) / 4;
            transform.position = new Vector3(transform.position.x + (player.transform.position.x + 10 - transform.position.x) * 0.5f, -0.5f + (-Mathf.Pow(progress - 1, 2) + 1) * 4.6f, 0);
        }
        if(transform.position.x > 44)
            transform.position = new Vector3(transform.position.x + (player.transform.position.x + 10 - transform.position.x) * 0.5f, 4.1f, 0);
    }
}
