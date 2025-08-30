using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassWallAudio : MonoBehaviour
{
    [SerializeField] PlayerMovementAudioHandler m_MovementAudioHandler;

    private void Start()
    {
        m_MovementAudioHandler = GameObject.FindGameObjectWithTag("PlayerAudio").transform.GetComponent<PlayerMovementAudioHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            {
                m_MovementAudioHandler.PlayGrassClimb();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_MovementAudioHandler.StopGrassClimb();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Rigidbody>().velocity.magnitude == 0)
            {
                m_MovementAudioHandler.PauseGrassClimb();
            }
            else
            {
                m_MovementAudioHandler.ResumeGrassClimb();
            }
        }
    }
}
