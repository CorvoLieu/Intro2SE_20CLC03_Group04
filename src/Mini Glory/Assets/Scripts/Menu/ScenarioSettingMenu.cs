using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSettingMenu : MonoBehaviour
{
    public Slider sliderWid;
    public Slider sliderLen;
    private int len;
    private int wid;
    // Start is called before the first frame update
    void Start()
    {
        len = (int)sliderLen.value;
        wid = (int)sliderWid.value;
    }

    public void changeLen()
    {
        len = (int)sliderLen.value;
    }
    public void changeWid()
    {
        wid = (int)sliderWid.value;
    }

    public void getVal()
    {
        Debug.Log("len: " + len.ToString() + "; wid: " + wid.ToString());
    }
}
