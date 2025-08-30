using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherSetter : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;

    private void Awake()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(true); // Activate each GameObject in the array
        }
    }
}
