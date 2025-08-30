using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public GameObject realModel;
    Animator anim;
    Vector3 realModelPositionBase;
    // Start is called before the first frame update
    void Start()
    {
        anim = realModel.GetComponent<Animator>();
        realModelPositionBase = realModel.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("MoveUp") || anim.GetCurrentAnimatorStateInfo(0).IsName("MoveDown")){
            realModel.transform.localPosition = realModelPositionBase - new Vector3(0, 0.5f, 0);
        }
        else{
            realModel.transform.localPosition = realModelPositionBase;
        }
    }
}
