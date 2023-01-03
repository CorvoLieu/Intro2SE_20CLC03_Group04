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
    public Canvas parent;
    public static NetNewBoard netNewBoard = new NetNewBoard();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(parent);
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
        // GameController.size_col = len;
        // GameController.size_row = wid;
        // GameController.grid = grid.getList();
        netNewBoard.board = new ChessPieceType[wid, len];
        for(int i = 0; i < wid; i++)
        {
            for (int j = 0; j < len; j++)
            {
                netNewBoard.board[i, j] = ChessPieceType.None;
            }
        }

        netNewBoard.wid = wid;
        netNewBoard.len = len;
        var tempList = grid.getList();
        foreach (var piece in tempList)
        {
            netNewBoard.board[piece.currentX, piece.currentY] = piece.type;
        }
        
        parent.gameObject.SetActive(false);
        SceneManager.LoadScene("Choose Power");
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
