using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTester : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] GameObject[] musicTriggers;

    private int currentTriggerIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            if (player != null && currentTriggerIndex > 0)
            {
                currentTriggerIndex--;
                player.transform.position = musicTriggers[currentTriggerIndex].transform.position;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            if (player != null && currentTriggerIndex < musicTriggers.Length)
            {
                player.transform.position = musicTriggers[currentTriggerIndex].transform.position;
                currentTriggerIndex++;
            }
        }
    }
}
