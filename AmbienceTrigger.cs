using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
    TutorialAmbience tutorialAmbience;
    void Start()
    {
        tutorialAmbience = FindObjectOfType<TutorialAmbience>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialAmbience.StopCityAmbience();
            tutorialAmbience.StartForestAmbience();
        }
    }
}
