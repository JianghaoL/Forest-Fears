using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public float volume;
    public KeyCode[] controls;
    public bool[] seedSave;
    public SeedCollector seedManager;
    public KeyChangeManager kcMan;
    public PlayerScript pScript;
    public bool isMenu;
    public TMP_Text numSeeds;
    public Slider volSlider;

    [SerializeField] MusicManager musicManager;
    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
        // if(isMenu)
            volSlider.value = volume;
        musicManager.SetGameVolume(volume * 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5)){
            FactoryReset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(isMenu){
            
            
            
        }
        int temp = 0;
        foreach(bool b in seedSave){
            if(b)
                temp++;
        }
        if(temp < 10)
            numSeeds.text = "x0" + temp;
        else
            numSeeds.text = "x" + temp;
        volume = volSlider.value;
        musicManager.SetGameVolume(volume * 100f);
        SaveGame();
    }
    public void SaveGame(){
        if(seedManager != null)
            seedSave = seedManager.seedsCollected;
        if(pScript != null)
            controls = pScript.controls;
        else
            controls = kcMan.curControls;
        SaveSystem.SaveGame(this);
    }
    public void LoadGame(){
        if(!SaveSystem.checkForFile())
			FactoryReset();
        SaveData data = SaveSystem.LoadGame();
        volume = data.vol;
        controls = data.controlScheme;
        seedSave = data.seeds;
        if(seedManager != null)
            seedManager.seedsCollected = seedSave;
        if(pScript != null)
            pScript.controls = controls;
        else
            kcMan.curControls = controls;
    }
    
    void FactoryReset(){
        controls = new KeyCode[]{KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Space, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Return, KeyCode.LeftShift};
        seedSave = new bool[18];
        volume = 1;
        SaveSystem.SaveGame(this);
    }
}
