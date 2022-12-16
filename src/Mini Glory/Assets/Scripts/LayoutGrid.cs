using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGrid : MonoBehaviour
{
    public GameObject slot;
    private List<Vector2Int> grid;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateGrid(int len, int wid)
    {
        if (grid != null)
            grid.Clear();
        newGrid(len, wid);

        for (int x = 0; x < len; ++x)
            for (int y = 0; y < wid; ++y)
            {

            }
    }

    public void newGrid(int len, int wid)
    {
        grid = new List<Vector2Int>(len * wid);
    }
}
