using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public KeyCode[] controlScheme;
    public bool[] seeds;
    public float vol;

    public SaveData(SaveManager sm){
        controlScheme = sm.controls;
        seeds = sm.seedSave;
        vol = sm.volume;
    }
}
