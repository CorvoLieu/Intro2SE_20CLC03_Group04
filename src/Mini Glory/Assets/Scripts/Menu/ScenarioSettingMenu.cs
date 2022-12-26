using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenarioSettingMenu : MonoBehaviour
{
    public Slider sliderWid;
    public Slider sliderLen;
    public LayoutGrid grid;
    private int len;
    private int wid;
    // Start is called before the first frame update
    void Start()
    {
        len = (int)sliderLen.value;
        wid = (int)sliderWid.value;
        grid.updateGrid(len, wid);
    }

    public void changeLen()
    {
        len = (int)sliderLen.value;
        grid.updateGrid(len, wid);
    }
    public void changeWid()
    {
        wid = (int)sliderWid.value;
        grid.updateGrid(len, wid);
    }

    public void getVal()
    {
        Debug.Log("len: " + len.ToString() + "; wid: " + wid.ToString());
        // Debug.Log(SceneManager.GetSceneByName("Scenes/SampleScene").name);
        // SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        GameController.size_col = len;
        GameController.size_row = wid;
        SceneManager.LoadScene("Choose Power");
        // SceneManager.UnloadSceneAsync("Customize Game Menu");
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scenes/SampleScene"));
    }

    public int getWid()
    {
        return wid;
    }
    public int getLen()
    {
        return len;
    }
}
