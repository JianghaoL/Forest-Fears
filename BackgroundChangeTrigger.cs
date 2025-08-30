using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChangeTrigger : MonoBehaviour
{

    [SerializeField] GameObject backgroundBlue;
    [SerializeField] GameObject backgroundGreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            backgroundBlue.SetActive(false);
            backgroundGreen.SetActive(true);
        }
       
    }
}
