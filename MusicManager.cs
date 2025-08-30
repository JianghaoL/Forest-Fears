using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event musicEvent;
    [SerializeField] AK.Wwise.Event musicStopEvent; // Event to stop the music
    [SerializeField] AK.Wwise.Event musicPauseEvent;
    [SerializeField] AK.Wwise.Event musicResumeEvent;
    [SerializeField] AK.Wwise.Event ambienceEvent; // Event for ambient sounds, if needed
    [SerializeField] AK.Wwise.RTPC musicVolumeRTPC; // RTPC for controlling music volume

    [SerializeField] AK.Wwise.Event buttonSFX;

    private AK.Wwise.State musicState;

    private void Start()
    {
        musicVolumeRTPC.SetGlobalValue(100f); // Set the initial music volume to 100%
        // Start the music event when the game starts
        // musicStopEvent.Post(gameObject); // Ensure any previous music is stopped
        musicEvent.Post(gameObject);
    }

    public void PauseMusic()
    {
        // Pause the music event
        musicPauseEvent.Post(gameObject);
    }

    public void ResumeMusic()
    {
        // Resume the music event
        musicResumeEvent.Post(gameObject);
    }

    public void SetMusicState(AK.Wwise.State newState)
    {
        // Set the music state
        musicState = newState;
        musicState.SetValue();
    }

    public void PlayButtonSFX()
    {
        // Play the button sound effect
        buttonSFX.Post(gameObject);
    }

    public void SetGameVolume(float volume)
    {
        // Set the music volume using the RTPC
        musicVolumeRTPC.SetGlobalValue(volume);
    }

    public void StopMusic()
    {
        // Stop the music event
        musicStopEvent.Post(gameObject);
    }

    public void StopAmbience()
    {
        ambienceEvent.Post(gameObject); // Stop the ambience event if needed
    }
}

