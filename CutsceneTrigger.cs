using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public int cutsceneIndex; // Index of the cutscene to trigger

    [SerializeField] CutsceneManager cutsceneManager; // Reference to the CutsceneManager

    void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            // Start the cutscene using the CutsceneManager
            
            cutsceneManager.StartCutscene(cutsceneIndex);
            other.gameObject.GetComponent<PlayerScript>().enabled = false; // Disable player controls during cutscene
            gameObject.SetActive(false); // Deactivate the trigger after activation
        }

        
    }
}
