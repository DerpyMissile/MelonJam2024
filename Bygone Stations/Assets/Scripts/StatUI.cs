using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public Scrollbar hp;
    public Scrollbar flow;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHP(int value){
        hp.size = (float)value/100.0f;
    }

    public void ChangeFlow(int value){
        flow.size = (float)value/100.0f;
    }
}
