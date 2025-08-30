using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] GameObject cutsceneObject;
    [SerializeField] Camera mainCamera;

    private void Start()
    {
        Time.timeScale = 0f; // Pause the game time.

        // Start the first cutscene
        cutsceneObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
