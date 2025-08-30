using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAmbience : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event cityAmbiencePlay, cityAmbienceStop, forestAmbienceStart;
    // Start is called before the first frame update
    void Start()
    {
        cityAmbiencePlay.Post(gameObject);
    }

    public void StopCityAmbience()
    {
        cityAmbienceStop.Post(gameObject);
    }

    public void StartForestAmbience()
    {
        forestAmbienceStart.Post(gameObject);
    }
}
