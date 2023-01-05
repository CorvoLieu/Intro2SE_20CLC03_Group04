using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScenarioSettingMenu : MonoBehaviour
{
    public Slider sliderWid;
    public Slider sliderLen;
    public LayoutGrid grid;
    private int len;
    private int wid;
    public Canvas parent;
    public static NetNewBoard netNewBoard = new NetNewBoard();
    public TMP_Text lenCounter;
    public TMP_Text widCounter;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(parent);
        len = (int)sliderLen.value;
        wid = (int)sliderWid.value;
        lenCounter.text = len.ToString();
        widCounter.text = wid.ToString();
        grid.updateGrid(len, wid);
    }

    public void changeLen()
    {
        len = (int)sliderLen.value;
        lenCounter.text = len.ToString();
        grid.updateGrid(len, wid);
    }
    public void changeWid()
    {
        wid = (int)sliderWid.value;
        widCounter.text = wid.ToString();
        grid.updateGrid(len, wid);
    }

    public void getVal()
    {
        var tempList = grid.getList();

        if(tempList.Count <= 3)
        {
            NotifCenter.notif.Enqueue("Please put at least 3 pieces on the board");
            return;
        }

        Debug.Log("len: " + len.ToString() + "; wid: " + wid.ToString());
        netNewBoard.board = new ChessPieceType[wid, len];
        for(int i = 0; i < wid; i++)
        {
            for (int j = 0; j < len; j++)
            {
                netNewBoard.board[i, j] = ChessPieceType.None;
            }
        }

        netNewBoard.turn = 0;
        netNewBoard.wid = wid;
        netNewBoard.len = len;
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
