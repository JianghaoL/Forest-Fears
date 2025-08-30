using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TagSwitchMapping
{
    public string tag;
    public AK.Wwise.Switch surfaceSwitch;
}

public class PlayerMovementAudioHandler : MonoBehaviour
{
    [Header("Player Footstep")]
    [SerializeField] AK.Wwise.Event playFootstepEvent;

    [SerializeField] TagSwitchMapping[] tagSwitchMapping;

    [Header("Other Movement Sounds")]
    [SerializeField] AK.Wwise.Event grassClimbPlayEvent, grassClimbStopEvent, grassClimbPauseEvent, grassClimbResumeEvent, vineJumpPlayEvent, vineJumpStopEvent;
    [SerializeField] PlayerScript playerScript;
    [SerializeField] AK.Wwise.RTPC playerVelocity;
    [SerializeField] bool isVineJumpEventPlaying;

    public void PlayFootstepRunning(AnimationEvent animationEvent)
    {
        PlayFootstepSFX();
    }

    public void PlayFootstepSFX()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * 2.5f, Vector3.down, 5f);

        int hitIndex = 0;
        bool fallout = false;
        while (hitIndex < hits.Length && !fallout)
        {
            Collider collider = hits[hitIndex].collider;

            for (int surfaceIndex = 0; surfaceIndex < tagSwitchMapping.Length; surfaceIndex++)
            {
                TagSwitchMapping tagSwitch = tagSwitchMapping[surfaceIndex];

                if (collider.CompareTag(tagSwitch.tag))
                {
                    tagSwitch.surfaceSwitch.SetValue(gameObject);
                    //Debug.Log("tagSwitch: " + tagSwitch.tag + " " + tagSwitch.surfaceSwitch);
                    if(tagSwitch.tag != "normal ground")
                        fallout = true;
                    
                    break;
                }
            }
            hitIndex++;
        }


        playFootstepEvent.Post(gameObject);
    }

    public void PlayGrassClimb()
    {
        grassClimbPlayEvent.Post(gameObject);
    }

    public void StopGrassClimb()
    {
        grassClimbStopEvent.Post(gameObject);
    }

    public void PauseGrassClimb()
    {
        grassClimbPauseEvent.Post(gameObject);
    }

    public void ResumeGrassClimb()
    {
        grassClimbResumeEvent.Post(gameObject);
    }

    public void PlayVineJump()
    {
        if (!isVineJumpEventPlaying){
            vineJumpPlayEvent.Post(gameObject);
            isVineJumpEventPlaying = true;
        }
    }

    public void StopVineJump()
    {
        vineJumpStopEvent.Post(gameObject);
        isVineJumpEventPlaying = false;
    }

    private void Start()
    {
        isVineJumpEventPlaying = false;
    }

    private void Update()
    {
        playerVelocity.SetGlobalValue(playerScript.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        // Debug.Log((playerScript.gameObject.GetComponent<Rigidbody>().velocity.magnitude));
    }
}
