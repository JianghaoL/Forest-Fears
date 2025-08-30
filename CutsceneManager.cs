using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] GameObject[] cutsceneObjects;
    [SerializeField] Camera mainCamera;

    private void Start()
    {
        // Time.timeScale = 0f; // Pause the game time.

        // Ensure all cutscene objects are inactive at the start
        foreach (GameObject cutsceneObject in cutsceneObjects)
        {
            cutsceneObject.SetActive(false);
        }

        // Start the first cutscene
        // cutsceneObjects[0].SetActive(true);
    }

    public void StartCutscene(int cutsceneIndex)
    {
        mainCamera.gameObject.SetActive(false); // Optionally deactivate the main camera during cutscenes

        foreach (GameObject cutsceneObject in cutsceneObjects)
        {
            cutsceneObject.SetActive(false); // Deactivate all cutscene objects
        }

        // Activate the specified cutscene object

        cutsceneObjects[cutsceneIndex].SetActive(true);
        
    }
}

