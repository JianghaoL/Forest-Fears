using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedScript : MonoBehaviour
{
    public GameObject seedCollectParticles;
    public SeedCollector seedManager;
    float startY;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, -120 * Time.deltaTime, 0);
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time * Mathf.PI) * 0.25f, transform.position.z);
        if(seedManager.seedsCollected[index])
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            Instantiate(seedCollectParticles, transform.position, Quaternion.identity);
            seedManager.CollectSeed(index);
            Destroy(gameObject);
        }
    }
}
