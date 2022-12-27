using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameController : MonoBehaviour
{
    float m_default_posX;
    float m_default_posY;
    float m_default_posZ;

    public Camera mainCamera;
    public GameObject Bishop_White;
    public GameObject Bishop_Black;
    public GameObject Knight_White;
    public GameObject Knight_Black;
    public GameObject Pawn_White;
    public GameObject Pawn_Black;
    public GameObject Rook_White;
    public GameObject Rook_Black;
    public static int size_row;
    public static int size_col;
    public static List<ChessPiece> grid;
    public static int type_hero_white;
    public static int type_hero_black;

    ChessPiece[,] posChess;
    ChessPiece currentDragging;
    private RaycastHit version;
    float rayLength;
    Camera currentCamera;
    ChessBoard m_chessboard;
    List<Vector2Int> availableMove;
    List<ChessPiece> whiteDefeat;
    List<ChessPiece> blackDefeat;
    bool blockRoad;
    int turn;

    float timer;
    string rivalID;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        rivalID = "123456";

        m_default_posX = 0;
        m_default_posY = 0.3f;
        m_default_posZ = 0;
        rayLength = 100;
        blockRoad = false;
        turn = 0;
        posChess = new ChessPiece[8, 8];
        whiteDefeat = new List<ChessPiece>();
        blackDefeat = new List<ChessPiece>();
        m_chessboard = FindObjectOfType<ChessBoard>();
        // SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        // SceneManager.LoadScene("Customize Game Menu", LoadSceneMode.Additive);
        mainCamera.transform.position = new Vector3(-size_row * 0.4625f, 6f, -2f);

        DisplayChessBoard();
        // DisplayChessBoardWithGrid(grid);  
        foreach (var piece in grid)
        {
            Debug.Log(piece.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.current;
            return;
        }

        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out version, rayLength))
        {
            if (version.collider.tag == "Cell" || version.collider.tag == "Bishop_White" || version.collider.tag == "Bishop_Black" || version.collider.tag == "Knight_White" || version.collider.tag == "Knight_Black" || version.collider.tag == "Pawn_White" || version.collider.tag == "Pawn_Black" || version.collider.tag == "Rook_White" || version.collider.tag == "Rook_Black")
            {
                Vector2Int index = LookupChessPos(version.transform.gameObject);

                if (blockRoad == false)
                {
                    if (version.collider.tag != "Cell")
                    {
                        ChessPiece tmp = version.transform.gameObject.GetComponent<ChessPiece>();
                        if (tmp.getIsDead() == false && tmp.team == turn)
                        {
                            availableMove = tmp.GetAvailableMoves(ref posChess, size_col, size_row);
                            m_chessboard.DrawRoad(availableMove);
                        }
                        else
                            m_chessboard.ResetRoadColor();
                    }
                    else
                    {
                        m_chessboard.ResetRoadColor();
                    }
                }

                //Move chess
                if (Input.GetMouseButtonDown(0) && isValidIndex(index))
                {
                    if (posChess[index.x, index.y] != null)
                    {
                        //Is it our turn?
                        if (true)
                        {
                            currentDragging = posChess[index.x, index.y];
                            blockRoad = true;
                        }
                    }
                }

                if (currentDragging != null && Input.GetMouseButtonUp(0))
                {
                    Vector2Int previousPos = new Vector2Int(currentDragging.currentX, currentDragging.currentY);
                    index = LookupChessPos(version.transform.gameObject);
                    bool validMove = ContainsValidMove(availableMove, new Vector2Int(index.x, index.y));
                    if (!validMove)
                    {
                        currentDragging.SetPosition(new Vector3(-previousPos.x, m_default_posY, previousPos.y));
                        availableMove = null;
                        currentDragging = null;
                        blockRoad = false;
                    }
                    else
                    {
                        MoveTo(currentDragging, index.x, index.y);
                        if (turn == 0)
                            turn = 1;
                        else
                            turn = 0;
                        availableMove = null;
                        currentDragging = null;
                        blockRoad = false;
                    }
                }
            }

            if (currentDragging)
            {
                Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * m_default_posY);
                float distance = 0.0f;
                if (horizontalPlane.Raycast(ray, out distance))
                {
                    currentDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * 1.5f);
                }
            }
        }
    }

    public void DisplayChessBoard()
    {
        m_chessboard.DisplayChessBoard(size_row, size_col);
    }

    GameObject DisplayChess(GameObject chess, float cell_x, float cell_z)
    {
        Vector3 spawnPos = new Vector3(m_default_posX - cell_x, m_default_posY, m_default_posZ + cell_z);
        //Update posChess
        // posChess[(int)cell_x, (int)cell_z] = chess.GetComponent<ChessPiece>();
        // posChess[(int)cell_x, (int)cell_z].SetPosition(spawnPos, true);
        // posChess[(int)cell_x, (int)cell_z].currentX = (int)cell_x;
        // posChess[(int)cell_x, (int)cell_z].currentY = (int)cell_z;

        GameObject clone = Instantiate(chess, spawnPos, Quaternion.Euler(-90f, 0f, -90f)) as GameObject;
        clone.transform.parent = transform;

        posChess[(int)cell_x, (int)cell_z] = clone.GetComponent<ChessPiece>();
        posChess[(int)cell_x, (int)cell_z].currentX = (int)cell_x;
        posChess[(int)cell_x, (int)cell_z].currentY = (int)cell_z;

        return clone;
    }

    void DisplayChessDefault()
    {
        //Spawn Rook
        DisplayChess(Rook_White, 0, 0);
        DisplayChess(Rook_White, 7, 0);
        DisplayChess(Rook_Black, 0, 7);
        DisplayChess(Rook_Black, 7, 7);
        //Spawn Knight
        DisplayChess(Knight_White, 1, 0);
        DisplayChess(Knight_White, 6, 0);
        DisplayChess(Knight_Black, 1, 7);
        DisplayChess(Knight_Black, 6, 7);
        //Spawn Bishop
        DisplayChess(Bishop_White, 2, 0);
        DisplayChess(Bishop_White, 5, 0);
        DisplayChess(Bishop_Black, 2, 7);
        DisplayChess(Bishop_Black, 5, 7);
        //Spawn Pawn
        for (int i = 0; i < size_col; i++)
        {
            DisplayChess(Pawn_White, i, 1);
            DisplayChess(Pawn_Black, i, 6);
        }
    }

    Vector2Int LookupChessPos(GameObject hitVer)
    {
        // if (type == 1)
        // {
        //     ChessPiece temp = hitVer.GetComponent<ChessPiece>();
        //     return new Vector2Int(temp.currentX, temp.currentY);
        // }
        // else
        // {
        //     Cell temp = hitVer.GetComponent<Cell>();
        //     return vector2Int((int)temp.transfor)
        // }

        return new Vector2Int((int)-hitVer.transform.position.x, (int)hitVer.transform.position.z);

    }

    bool MoveTo(ChessPiece cp, int desX, int desY)
    {
        Vector2Int previousPos = new Vector2Int(cp.currentX, cp.currentY);
        ChessPiece ocp = posChess[desX, desY];

        if (ocp != null)
        {
            if (ocp.team == 0)
            {
                int n = blackDefeat.Count;
                blackDefeat.Add(ocp);
                ocp.SetPosition(new Vector3(m_default_posX + 1 + (n % 2) * 0.75f, m_default_posY, m_default_posZ + 7 - (n / 2) * 0.75f));
                ocp.SetScale(Vector3.one * 1500f);
                ocp.setIsDead(true);
            }
            else
            {
                int n = whiteDefeat.Count;
                whiteDefeat.Add(ocp);
                ocp.SetPosition(new Vector3(m_default_posX - 8 - (n % 2) * 0.75f, m_default_posY, m_default_posZ + (n / 2) * 0.75f));
                ocp.SetScale(Vector3.one * 1500f);
                ocp.setIsDead(true);
            }
        }

        //Di chuyển
        cp.currentX = desX;
        cp.currentY = desY;
        cp.SetPosition(new Vector3(m_default_posX - desX, m_default_posY, m_default_posZ + desY), false);

        posChess[desX, desY] = cp;
        posChess[previousPos.x, previousPos.y] = null;

        return true;
    }

    bool ContainsValidMove(List<Vector2Int> moves, Vector2Int destination)
    {
        if (moves == null)
            return false;
        for (int i = 0; i < moves.Count; i++)
        {
            if (destination == moves[i])
            {
                return true;
            }
        }
        return false;
    }

    bool isValidIndex(Vector2Int ind)
    {
        if (ind.x >= 0 && ind.x < size_row && ind.y >= 0 && ind.y < size_col)
            return true;
        else
            return false;
    }

    public SaveFile GetSaveFile()
    {
        SaveFile saveFile = new SaveFile();

        saveFile.timer = timer;
        saveFile.blackDefeat = blackDefeat;
        saveFile.whiteDefeat = whiteDefeat;

        saveFile.len = size_col;
        saveFile.wid = size_row;

        saveFile.rivalID = rivalID;

        foreach (ChessPiece piece in posChess)
        {
            ChessPiece temp = Instantiate(piece);
            saveFile.pieces.Add(temp);
        }

        return saveFile;
    }

    public void SaveGame()
    {
        DataController.SaveGame(GetSaveFile());
    }

    public void LoadGame()
    {
        SaveFile saveFile = DataController.LoadGame(rivalID);

        timer = saveFile.timer;
        blackDefeat = saveFile.blackDefeat;
        whiteDefeat = saveFile.whiteDefeat;

        size_col = saveFile.len;
        size_row = saveFile.wid;

        rivalID = saveFile.rivalID;

        posChess = new ChessPiece[size_col, size_row];

        foreach (ChessPiece piece in saveFile.pieces)
        {
            posChess[piece.currentX, piece.currentY] = Instantiate(piece);
        }

        // Re-load the game
    }

    public void DisplayChessBoardWithGrid(LayoutGrid grid)
    {
        for (int i = 0; i < grid.grid.Length; i++)
        {
            var temp = grid.grid[i].GetComponent<SimpleGridSlot>();
            if (temp.type != ChessPieceType.None)
            {
                if (temp.team == ChessPieceTeam.White)
                {
                    if (temp.type == ChessPieceType.Pawn)
                    {
                        DisplayChess(Pawn_White, temp.x, temp.y);
                    }
                    if (temp.type == ChessPieceType.Bishop)
                    {
                        DisplayChess(Bishop_White, temp.x, temp.y);
                    }
                    if (temp.type == ChessPieceType.Knight)
                    {
                        DisplayChess(Knight_White, temp.x, temp.y);
                    }
                    if (temp.type == ChessPieceType.Rook)
                    {
                        DisplayChess(Rook_White, temp.x, temp.y);
                    }
                }
                if (temp.team == ChessPieceTeam.Black)
                {
                    if (temp.type == ChessPieceType.Pawn)
                    {
                        DisplayChess(Pawn_Black, temp.x, temp.y);
                    }
                    if (temp.type == ChessPieceType.Bishop)
                    {
                        DisplayChess(Bishop_Black, temp.x, temp.y);
                    }
                    if (temp.type == ChessPieceType.Knight)
                    {
                        DisplayChess(Knight_Black, temp.x, temp.y);
                    }
                    if (temp.type == ChessPieceType.Rook)
                    {
                        DisplayChess(Rook_Black, temp.x, temp.y);
                    }
                }
            }
        }
    }
}
