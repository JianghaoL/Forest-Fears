using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] AK.Wwise.State state;
    bool isTriggered = false;
    [SerializeField] bool canRetrigger; // For degbugging purposes, allows retriggering the music state

    private void OnTriggerEnter(Collider other)
    {
        //if (isTriggered) return; // Prevent multiple triggers

        // Check if the object entering the trigger is the player

        if (!isTriggered || canRetrigger)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<MusicManager>().SetMusicState(state);
                isTriggered = true; // Set the flag to true to prevent re-triggering
            }
        }
    }
}
