using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStopper : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    void Awake()
    {
        musicManager.PauseMusic(); // Pause the music when this script is loaded
    }
}
