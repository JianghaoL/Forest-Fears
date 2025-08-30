using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingBranch : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float period;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float posInPeriod = Time.time;
        while(posInPeriod >= period)
            posInPeriod -= period;
        float baseT = ((posInPeriod - period / 2) / (period / 2));
        if(posInPeriod <= period / 2)
            baseT = (posInPeriod / (period / 2));
        if(posInPeriod <= period / 2)
            transform.position = startPos + ((endPos - startPos) * ((-Mathf.Cos(Mathf.PI * baseT) + 1) / 2));
        else
            transform.position = endPos - ((endPos - startPos) * ((-Mathf.Cos(Mathf.PI * baseT) + 1) / 2));
    }
}
