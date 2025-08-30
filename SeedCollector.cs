using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollector : MonoBehaviour
{
    public int numSeeds;
    public bool[] seedsCollected = new bool[18];
    public SaveManager sm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int counter = 0;
        foreach(bool b in seedsCollected){
            if(b)
                counter++;
        }
        numSeeds = counter;
    }

    public void CollectSeed(int index){
        seedsCollected[index] = true;
        sm.SaveGame();
    }
}
