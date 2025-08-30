using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyChangeManager : MonoBehaviour
{
    public KeyCode[] curControls;
    bool listening;
    int curIndexListening;
    public SaveManager sm;
    public TMP_Text[] displays;
    // Start is called before the first frame update
    void Start()
    {
        listening = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(listening){
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
                if(Input.GetKeyDown(vKey)){
                    listening = false;
                    curControls[curIndexListening] = vKey;
                    sm.SaveGame();
                }
            }
        }
        for(int i = 0; i < 11; i++){
            displays[i].text = curControls[i].ToString();
        }
    }

    public void Listen(int index){
        listening = true;
        curIndexListening = index;
    }
}
