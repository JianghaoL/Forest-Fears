using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject creditsCanvas;
    [SerializeField] GameObject menuCanvas;

    [SerializeField] MusicManager musicManager;

    public void OnStartPressed()
    {
        musicManager.PlayButtonSFX(); // Play the button sound effect
        //musicManager.StopMusic(); // Stop the current music
        SceneManager.LoadScene("tutorial");
    }

    public void OnAllScenesPressed()
    {
        musicManager.PlayButtonSFX(); // Play the button sound effect
        print("all scenes");
    }

    public void OnCreditsPressed()
    {
        musicManager.PlayButtonSFX(); // Play the button sound effect
        creditsCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }

    public void OnMenuPressed()
    {
        musicManager.PlayButtonSFX(); // Play the button sound effect
        creditsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void OnQuitPressed()
    {
        musicManager.PlayButtonSFX(); // Play the button sound effect
        print("quit");
        Application.Quit();
    }

}


