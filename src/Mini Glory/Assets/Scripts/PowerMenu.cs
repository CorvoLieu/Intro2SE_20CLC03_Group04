using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMenu : MonoBehaviour
{
    public InfoPanel info;
    private int currentlySelect;

    public void SelectTank()
    {
        info.CurrentlySelect = currentlySelect = 1;
    }

    public void SelectCanon()
    {
        info.CurrentlySelect = currentlySelect = 2;
    }

    public void SelectClown()
    {
        info.CurrentlySelect = currentlySelect = 3;
    }

    public void GetSelect()
    {
        Debug.Log("Currently selected: " + currentlySelect);
    }
}
