using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
//using UnityEngine.SceneManagement;

public class ChessBoard : MonoBehaviour
{
    public GameObject cell;
    // public int size_row;
    // public int size_col;

    // private RaycastHit version;
    // float rayLength;
    // Camera currentCamera;
    // (int, int) index;
    // // int pre_type_chess; 
    // ChessPieceType pre_type_chess; 

    Bishop m_bishop;
    Rook m_rook;
    Pawn m_pawn;
    Knight m_knight;
    List<Vector2Int> availableMove;


    Cell[][] m_cells;

    
    //// multi logic
    //private int playerCount = -1;
    //private int currentTeam = -1;
    //private bool localGame = true;


    // Start is called before the first frame update
    
    
    void Start()
    {
        // DisplayChessBoard(size_row, size_col);
        availableMove = new List<Vector2Int>();
        // rayLength = 100;
        // index = (-1, -1);
        // pre_type_chess = ChessPieceType.Empty;
        // pre_type_chess = -1;

        //RegisterEvents();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!currentCamera)
        // {
        //     currentCamera = Camera.current;
        //     return;
        // }

        // Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out version, rayLength))
        // {
        //     // if (version.collider.tag == "Cell")
        //     // {
        //     //     ResetPre_Cell_color();

        //     //     //test
        //     //     // ResetRoadColor(index.Item1, index.Item2);

        //     //     index = LookupCell(version.transform.gameObject);
        //     //     if (index.Item1 != -1 && index.Item2 != -1)
        //     //     {
        //     //         m_cells[index.Item1][index.Item2].ChangeColor(2);
        //     //         pre_type_chess = m_cells[index.Item1][index.Item2].get_chp_type();
                    
        //     //         //test
        //     //         // DrawRoad(index.Item1, index.Item2);
        //     //     }
        //     // }
        //     if (version.collider.tag == "Bishop_White" || version.collider.tag == "Bishop_Black" || version.collider.tag == "Knight_White" || version.collider.tag == "Knight_Black" || version.collider.tag == "Pawn_White" || version.collider.tag == "Pawn_Black" || version.collider.tag == "Rook_White" || version.collider.tag == "Rook_Black")
        //     {
        //         ResetPre_Cell_color();

        //         //test
        //         // ResetRoadColor(index.Item1, index.Item2);

        //         index = LookupCell(version.transform.gameObject);
        //         if (index.Item1 != -1 && index.Item2 != -1)
        //         {
        //             m_cells[index.Item1][index.Item2].ChangeColor(2);
        //             pre_type_chess = m_cells[index.Item1][index.Item2].get_chp_type();
                    
        //             //test
        //             // DrawRoad(index.Item1, index.Item2);
        //         }
        //     }
        // }
    }

    public void DisplayChessBoard(int n_row, int n_col)
    {
        m_cells = new Cell[n_row][];

        for (int i = 0; i < n_row; i++)
        {
            m_cells[i] = new Cell[n_col];
            for (int j = 0; j < n_col; j++)
            {
                GameObject clone = DisplayCell(-i, 0, j);
                clone.transform.parent = transform;
                clone.name = i + " " + j;
                m_cells[i][j] = clone.GetComponent<Cell>();
                if ((i + j ) % 2 == 0)
                {
                    m_cells[i][j].ChangeColor(0);
                }
                else
                {
                    m_cells[i][j].ChangeColor(1);
                }
            }
        }
    }

    GameObject DisplayCell(float posX, float posY, float posZ)
    {
        Vector3 spawnPos = new Vector3(posX, posY, posZ);
        return Instantiate(cell, spawnPos, Quaternion.identity) as GameObject;
    }

    public Vector2Int LookupCell(Cell temp, int size_row, int size_col)
    {
        for (int i = 0; i < size_row; i++)
        {
            for (int j = 0; j < size_col; j++)
            {
                if (m_cells[i][j] == temp)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return new Vector2Int();
    }

    // void ResetPre_Cell_color()
    // {
    //     if (index != (-1,-1))
    //     {
    //         if ((index.Item1 + index.Item2) % 2 == 0)
    //         {
    //             m_cells[index.Item1][index.Item2].ChangeColor(0);
    //         }
    //         else
    //         {
    //             m_cells[index.Item1][index.Item2].ChangeColor(1);
    //         }
    //     }
    // }

    // void DrawRookRoad(int posR, int posC)
    // {
    //     if (posR == -1 || posC == -1)
    //         return;

    //     for (int i = 0; i < size_row; i++)
    //     {
    //         for (int j = 0; j < size_col; j++)
    //         {
    //             if ((i == posR && j != posC) || (i != posR && j == posC))
    //             {
    //                 m_cells[i][j].ChangeColor(3);
    //             }
    //         }
    //     }
    // }

    // void ResetRookRoadColor(int posR, int posC)
    // {
    //     if (posR == -1 || posC == -1)
    //         return;

    //     for (int i = 0; i < size_row; i++)
    //     {
    //         for (int j = 0; j < size_col; j++)
    //         {
    //             if (i == posR || j == posC)
    //             {
    //                 if ((i + j ) % 2 == 0)
    //                 {
    //                     m_cells[i][j].ChangeColor(0);
    //                 }
    //                 else
    //                 {
    //                     m_cells[i][j].ChangeColor(1);
    //                 }
    //             }
    //         }
    //     }
    // }

    public void DrawRoad(List<Vector2Int> availableChess)
    {
        ResetRoadColor();
        availableMove = availableChess;

        for (int i = 0; i < availableMove.Count; i++)
        {
            m_cells[availableMove[i].x][availableMove[i].y].ChangeColor(3);
        }
    }

    public void ResetRoadColor()
    {
        for (int i = 0; i < availableMove.Count; i++)
        {
            if ((availableMove[i].x + availableMove[i].y) % 2 == 0)
                m_cells[availableMove[i].x][availableMove[i].y].ChangeColor(0);
            else
                m_cells[availableMove[i].x][availableMove[i].y].ChangeColor(1);
        }

        availableMove = new List<Vector2Int>();
    }






    //#region
    //private void RegisterEvents()
    //{
    //    NetUtility.S_WELCOME += OnWelcomeServer;

    //    NetUtility.C_WELCOME += OnWelcomeClient;
    //    NetUtility.C_START_GAME += OnStartGameClient;
    //}
    //private void UnRegisterEvents()
    //{
    //    NetUtility.S_WELCOME -= OnWelcomeServer;

    //    NetUtility.C_WELCOME -= OnWelcomeClient;
    //    NetUtility.C_START_GAME -= OnStartGameClient;
    //}


    //// Server
    //private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    //{
    //    NetWelcome nw = msg as NetWelcome;
    //    nw.AssignedTeam = ++playerCount;
    //    Server.Instance.SendToClient(cnn, nw);

    //    // if there are enough 2 connections, start the game
    //    if (playerCount == 1)
    //         Server.Instance.Broadcast(new NetStartGame());
    //}


    //// Client
    //private void OnWelcomeClient(NetMessage msg)
    //{
    //    NetWelcome nw = msg as NetWelcome;
    //    currentTeam = nw.AssignedTeam;

    //    Debug.Log($"My assigned team is {nw.AssignedTeam}");
    //}
    //private void OnStartGameClient(NetMessage msg)
    //{
    //    // We just need to go to the game scene
    //    SceneManager.LoadScene(1);
    //}
    //#endregion
}
