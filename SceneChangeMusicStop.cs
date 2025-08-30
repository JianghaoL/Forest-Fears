using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeMusicStop : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;
    // Update is called once per frame
    private void OnDestroy()
    {
        // Check if the MusicManager is assigned
        if (musicManager != null)
        {
            // Stop the music when this script is destroyed
            musicManager.StopMusic();
        }
        else
        {
            Debug.LogWarning("MusicManager is not assigned in SceneChangeMusicStop.");
        }
    }
}
