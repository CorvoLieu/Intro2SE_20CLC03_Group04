using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LayoutGrid : MonoBehaviour
{
    private int wid;
    private int len;
    public GameObject slotPrefab;
    [HideInInspector] public (int, int) heroSlot;
    public GameObject[] grid;
    private GridLayoutGroup layout;
    private static Color32 disabledSlotColor;

    void Start()
    {
        layout = GetComponent<GridLayoutGroup>();
        disabledSlotColor = new Color32(185, 34, 39, 255);
        heroSlot = (-1, -1);
    }

    public void updateGrid(int len, int wid)
    {
        if (grid != null)
            clearGrid();
        newGrid(len, wid);
        layout.constraintCount = len;

        for (int x = 0; x < len; ++x)
            for (int y = 0; y < wid; ++y)
            {
                var clone = grid[x * wid + y] = Instantiate(slotPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                clone.transform.SetParent(transform);
                var temp = clone.GetComponent<SimpleGridSlot>();
                temp.x = x;
                temp.y = y;
                if (x >= len / 2)
                {
                    clone.GetComponent<RawImage>().color = disabledSlotColor;
                    temp.isDisable = true;
                    temp.team = ChessPieceTeam.Black;
                }
                else
                {
                    temp.team = ChessPieceTeam.White;
                }
            }
    }

    private void clearGrid()
    {
        Debug.Log("Clearing Old Grid");
        for (int x = 0; x < grid.Length; ++x)
        {
            Destroy(grid[x]);
        }

        Array.Clear(grid, 0, grid.Length);
        grid = null;
    }
    private void newGrid(int len, int wid)
    {
        //Debug.Log("Create New Grid: " + len.ToString() + ';' + wid.ToString());
        grid = new GameObject[len * wid];
        this.len = len;
        this.wid = wid;
    }

    public void syncSide(int x, int y, Dragable item)
    {
        var opposite = grid[(len - 1 - x) * wid + (wid - 1 - y)].GetComponent<SimpleGridSlot>();
        opposite.setItemForce(item);
    }

    public void removeHero()
    {
        if (heroSlot.Item1 != -1 && heroSlot.Item2 != -1)
        {
            //Reset opposite hero
            var heroOpposite = grid[(len - 1 - heroSlot.Item1) * wid + (wid - 1 - heroSlot.Item2)];
            heroOpposite.GetComponent<SimpleGridSlot>().resetItem();

            //Reset current hero
            var hero = grid[heroSlot.Item1 * wid + heroSlot.Item2];
            hero.GetComponent<SimpleGridSlot>().resetItem();
            heroSlot = (-1, -1);
        }
    }

    public void resetOpposite(int x, int y)
    {
        var opposite = grid[(len - 1 - x) * wid + (wid - 1 - y)].GetComponent<SimpleGridSlot>();
        opposite.resetItem();
    }

    public List<ChessPiece> getList()
    {
        List<ChessPiece> result = new List<ChessPiece>();

        foreach (var piece in grid)
        {
            var temp = piece.GetComponent<SimpleGridSlot>();
            if (temp.type != ChessPieceType.None)
            {
                ChessPiece newPiece = new ChessPiece();
                newPiece.currentX = wid - temp.y - 1;
                newPiece.currentY = temp.x;
                newPiece.type = temp.type;
                newPiece.team = (temp.team == ChessPieceTeam.White) ? 0 : 1;

                result.Add(newPiece);
            }
        }

        return result;
    }
}
