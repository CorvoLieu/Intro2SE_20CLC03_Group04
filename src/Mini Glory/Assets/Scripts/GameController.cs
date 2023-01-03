using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    float m_default_posX;
    float m_default_posY;
    float m_default_posZ;

    public TextMeshProUGUI m_mana;
    public TextMeshProUGUI m_turnRelife;
    public Button m_ultiButton;
    public Camera mainCamera;
    public Camera clientCamera;
    public GameObject Bishop_White;
    public GameObject Bishop_Black;
    public GameObject Knight_White;
    public GameObject Knight_Black;
    public GameObject Pawn_White;
    public GameObject Pawn_Black;
    public GameObject Rook_White;
    public GameObject Rook_Black;
    public GameObject Hero_Black_Tank;
    public GameObject Hero_Black_Cannon;
    public GameObject Hero_Black_Clown;
    public GameObject Hero_White_Tank;
    public GameObject Hero_White_Cannon;
    public GameObject Hero_White_Clown;
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
    public List<ChessPiece> whiteDefeat;
    public List<ChessPiece> blackDefeat;
    bool blockRoad;
    int turn;
    int m_ultiCounterWhite;
    int m_ultiCounterBlack;
    int turn_relifeWhite;
    int turn_relifeBlack;

    float timer;
    string rivalID;

    public static int[] updateQueue = { -1, -1, -1, -1 };

    // Start is called before the first frame update
    void Start()
    {
        m_ultiButton.gameObject.SetActive(false);
        m_turnRelife.gameObject.SetActive(false);
        timer = 0;
        rivalID = "123456";
        m_default_posX = 0;
        m_default_posY = 0.3f;
        m_default_posZ = 0;
        rayLength = 100;
        blockRoad = false;
        turn = 0;
        m_ultiCounterWhite = 1;
        m_ultiCounterBlack = 1;
        turn_relifeWhite = -1;
        turn_relifeBlack = -1;
        posChess = new ChessPiece[size_row, size_col];
        whiteDefeat = new List<ChessPiece>();
        blackDefeat = new List<ChessPiece>();
        m_chessboard = FindObjectOfType<ChessBoard>();
        // SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        // SceneManager.LoadScene("Customize Game Menu", LoadSceneMode.Additive);
        mainCamera.transform.position = new Vector3(-size_row * 0.4625f, 6f, -2f);
        clientCamera.transform.position = new Vector3(-size_row * 0.4625f, 6f, 6f);

        if (NetExecute.currentTeam == 0)
            clientCamera.gameObject.SetActive(false);
        else
            mainCamera.gameObject.SetActive(false);

        DisplayChessBoard();
        DisplayChessBoardWithGrid(grid);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateQueue[0] != -1)
        {
            MoveTo(updateQueue[0], updateQueue[1], updateQueue[2], updateQueue[3]);
            updateQueue[0] = -1;
            updateQueue[1] = -1;
            updateQueue[2] = -1;
            updateQueue[3] = -1;
        }

        if (CheckWinGame() == 0)
        {
            //Process screen white win and black lose
            SceneManager.LoadScene("WinGame");
        }
        if (CheckWinGame() == 1)
        {
            //Process screen white lose and black win
            SceneManager.LoadScene("WinGame");
        }
        
        m_mana.text = "Mana: " + m_ultiCounterWhite.ToString();
        m_turnRelife.text = "Turn Relife: " + turn_relifeWhite.ToString();
        if (m_ultiCounterWhite >= 3)
        {
            m_ultiButton.gameObject.SetActive(true);
        }
        else
        {
            m_ultiButton.gameObject.SetActive(false);
        }

        if (turn_relifeWhite == 0)
        {
            Relife_White();
            m_turnRelife.gameObject.SetActive(false);
            turn_relifeWhite = -1;
        }
        if (turn_relifeBlack == 0)
        {
            Relife_Black();
            turn_relifeBlack = -1;
        }

        if (!currentCamera)
        {
            currentCamera = Camera.current;
            return;
        }

        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out version, rayLength))
        {
            if (version.collider.tag == "Cell" || version.collider.tag == "Bishop_White" || version.collider.tag == "Bishop_Black" || version.collider.tag == "Knight_White" || version.collider.tag == "Knight_Black" || version.collider.tag == "Pawn_White" || version.collider.tag == "Pawn_Black" || version.collider.tag == "Rook_White" || version.collider.tag == "Rook_Black" || version.collider.tag == "Hero_White" || version.collider.tag == "Hero_Black")
            {
                if (blockRoad == false)
                {
                    if (version.collider.tag != "Cell")
                    {
                        ChessPiece tmp = version.transform.gameObject.GetComponent<ChessPiece>();
                        if (tmp.getIsDead() == false && tmp.team == turn && tmp.stunned == 0 && NetExecute.currentTeam == turn)
                        {
                            availableMove = tmp.GetAvailableMoves(ref posChess, size_row, size_col);
                            Debug.Log(availableMove.Count.ToString());
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
                Vector2Int index = LookupChessPos(version.transform.gameObject);
                if (Input.GetMouseButtonDown(0) && isValidIndex(index) && NetExecute.currentTeam == turn)
                {
                    // Debug.Log("Clicked" + index.x.ToString());
                    // Debug.Log("Clicked" + index.y.ToString());
                    if (posChess[index.x, index.y] != null)
                    {
                        //Is it our turn?
                        if (posChess[index.x, index.y].team == turn)
                        {
                            currentDragging = posChess[index.x, index.y];
                            blockRoad = true;
                        }
                    }
                }

                if (currentDragging != null)
                {
                    Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * m_default_posY);
                    float distance = 0.0f;
                    if (horizontalPlane.Raycast(ray, out distance))
                    {
                        currentDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * 1.5f);
                    }
                }

                if (currentDragging != null && Input.GetMouseButtonUp(0))
                {
                    Vector2Int previousPos = new Vector2Int(currentDragging.currentX, currentDragging.currentY);
                    if (version.collider.tag == "Cell")
                        index = LookupCell(version.transform.gameObject);
                    else
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
                        availableMove = null;
                        currentDragging = null;
                        blockRoad = false;
                        stunne_discount();
                    }
                }
            }
            else
            {
                m_chessboard.ResetRoadColor();
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

        // return new Vector2Int((int)-hitVer.transform.position.x, (int)hitVer.transform.position.z);
        ChessPiece temp = hitVer.GetComponent<ChessPiece>();
        for (int i = 0; i < size_row; i++)
            for (int j = 0; j < size_col; j++)
            {
                if (posChess[i, j] == temp)
                    return new Vector2Int(i, j);
            }
        return new Vector2Int(-1, -1);
    }

    Vector2Int LookupCell(GameObject hitVer)
    {
        return new Vector2Int((int)-hitVer.transform.position.x, (int)hitVer.transform.position.z);
    }

    public bool MoveTo(int prevX, int prevY, int desX, int desY)
    {
        Debug.Log($"Call Move: {prevX} {prevY} -> {desX} {desY}");
        return MoveTo(posChess[prevX, prevY], desX, desY, false);
    }

    bool MoveTo(ChessPiece cp, int desX, int desY, bool isSending = true)
    {
        Vector2Int previousPos = new Vector2Int(cp.currentX, cp.currentY);
        ChessPiece ocp = posChess[desX, desY];

        if (ocp != null)
        {
            if (ocp.team == 0)
            {
                int n = blackDefeat.Count;
                blackDefeat.Add(ocp);
                ocp.SetPosition(new Vector3(m_default_posX + 1 + (n % 2) * 0.75f, m_default_posY, m_default_posZ + (size_col - 1) - (n / 2) * 0.75f));
                if (ocp.type == ChessPieceType.Hero)
                {
                    ocp.SetScale(new Vector3(0.5f, 0.5f, 0.5f));
                    turn_relifeWhite = 4;
                    m_turnRelife.gameObject.SetActive(true);
                }
                else
                    ocp.SetScale(Vector3.one * 1500f);
                ocp.setIsDead(true);
            }
            else
            {
                int n = whiteDefeat.Count;
                whiteDefeat.Add(ocp);
                ocp.SetPosition(new Vector3(m_default_posX - (size_row) - (n % 2) * 0.75f, m_default_posY, m_default_posZ + (n / 2) * 0.75f));
                if (ocp.type == ChessPieceType.Hero)
                {
                    ocp.SetScale(new Vector3(0.5f, 0.5f, 0.5f));
                    turn_relifeBlack = 4;
                }
                else
                    ocp.SetScale(Vector3.one * 1500f);
                ocp.setIsDead(true);
            }
        }

        if (isSending)
        {
            Debug.Log("[CLIENT] Sending make move");
            var nmm = new NetMakeMove();

            nmm.originalX = cp.currentX;
            nmm.originalY = cp.currentY;
            nmm.destinationX = desX;
            nmm.destinationY = desY;
            Client.Instance.SendToServer(nmm);
        }

        //Di chuyển
        cp.currentX = desX;
        cp.currentY = desY;
        cp.SetPosition(new Vector3(m_default_posX - desX, m_default_posY, m_default_posZ + desY), false);

        posChess[desX, desY] = cp;
        posChess[previousPos.x, previousPos.y] = null;
        ChangeTurn();

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
                if (posChess[destination.x, destination.y] == null)
                    return true;
                if (posChess[destination.x, destination.y].type == ChessPieceType.Hero)
                {
                    if (posChess[destination.x, destination.y].life <= 1)
                        return true;
                    else
                    {
                        posChess[destination.x, destination.y].life--;
                        ChangeTurn();
                        return false;
                    }
                }
                else
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

    public void DisplayChessBoardWithGrid(List<ChessPiece> grid)
    {
        foreach (var temp in grid)
        {
            if (temp.team == 0)
            {
                if (temp.type == ChessPieceType.Pawn)
                {
                    DisplayChess(Pawn_White, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Bishop)
                {
                    DisplayChess(Bishop_White, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Knight)
                {
                    DisplayChess(Knight_White, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Rook)
                {
                    DisplayChess(Rook_White, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Hero)
                {
                    ChessPiece clone = new ChessPiece();
                    if (type_hero_white == 1)
                    {
                        clone = DisplayChess(Hero_White_Tank, temp.currentX, temp.currentY).GetComponent<ChessPiece>();
                    }
                    else if (type_hero_white == 2)
                    {
                        clone = DisplayChess(Hero_White_Cannon, temp.currentX, temp.currentY).GetComponent<ChessPiece>();
                    }
                    else if (type_hero_white == 3)
                    {
                        clone = DisplayChess(Hero_White_Clown, temp.currentX, temp.currentY).GetComponent<ChessPiece>();
                    }
                    clone.SetScale(new Vector3(0.8f, 0.8f, 0.8f), true);
                    clone.SetRotation(Vector3.zero);
                }

            }
            if (temp.team == 1)
            {
                if (temp.type == ChessPieceType.Pawn)
                {
                    DisplayChess(Pawn_Black, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Bishop)
                {
                    DisplayChess(Bishop_Black, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Knight)
                {
                    DisplayChess(Knight_Black, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Rook)
                {
                    DisplayChess(Rook_Black, temp.currentX, temp.currentY);
                }
                if (temp.type == ChessPieceType.Hero)
                {
                    ChessPiece clone = new ChessPiece();
                    if (type_hero_black == 1)
                    {
                        clone = DisplayChess(Hero_Black_Tank, temp.currentX, temp.currentY).GetComponent<ChessPiece>();
                    }
                    else if (type_hero_black == 2)
                    {
                        clone = DisplayChess(Hero_Black_Cannon, temp.currentX, temp.currentY).GetComponent<ChessPiece>();
                    }
                    else if (type_hero_black == 3)
                    {
                        clone = DisplayChess(Hero_Black_Clown, temp.currentX, temp.currentY).GetComponent<ChessPiece>();
                    }
                    clone.SetScale(new Vector3(0.8f, 0.8f, 0.8f), true);
                    clone.SetRotation(new Vector3(0f, 180f, 0f));
                }
            }
        }
    }

    public void Ulti_white()
    {
        m_ultiCounterWhite = 0;
        if (turn == 0)
        {
            for (int i = 0; i < size_row; i++)
            {
                for (int j = 0; j < size_col; j++)
                {
                    if (posChess[i, j] != null)
                        if (posChess[i, j].type == ChessPieceType.Hero && posChess[i, j].team == 0)
                        {
                            posChess[i, j].ulti(ref posChess, size_row, size_col);
                            turn = 1;
                            return;
                        }
                }
            }
        }
    }

    public void Ulti_black()
    {
        m_ultiCounterBlack = 0;
        if (turn == 0)
        {
            for (int i = 0; i < size_row; i++)
            {
                for (int j = 0; j < size_col; j++)
                {
                    if (posChess[i, j] != null)
                        if (posChess[i, j].type == ChessPieceType.Hero && posChess[i, j].team == 1)
                        {
                            posChess[i, j].ulti(ref posChess, size_row, size_col);
                            turn = 1;
                            return;
                        }
                }
            }
        }
    }

    void stunne_discount()
    {
        for (int i = 0; i < size_row; i++)
            for (int j = 0; j < size_col; j++)
            {
                if (posChess[i, j] != null)
                    if (posChess[i, j].stunned != 0)
                    {
                        posChess[i, j].stunned--;
                    }
            }
    }

    public void White_Defeat(ChessPiece ocp, int x, int y)
    {
        int n = blackDefeat.Count;
        blackDefeat.Add(ocp);
        ocp.SetPosition(new Vector3(m_default_posX + 1 + (n % 2) * 0.75f, m_default_posY, m_default_posZ + (size_col - 1) - (n / 2) * 0.75f));
        if (ocp.type == ChessPieceType.Hero)
            ocp.SetScale(new Vector3(0.5f, 0.5f, 0.5f));
        else
            ocp.SetScale(Vector3.one * 1500f);
        ocp.setIsDead(true);
        posChess[x, y] = null;
    }

    public void Black_Defeat(ChessPiece ocp, int x, int y)
    {
        int n = whiteDefeat.Count;
        whiteDefeat.Add(ocp);
        ocp.SetPosition(new Vector3(m_default_posX - (size_row) - (n % 2) * 0.75f, m_default_posY, m_default_posZ + (n / 2) * 0.75f));
        if (ocp.type == ChessPieceType.Hero)
            ocp.SetScale(new Vector3(0.5f, 0.5f, 0.5f));
        else
            ocp.SetScale(Vector3.one * 1500f);
        ocp.setIsDead(true);
        posChess[x, y] = null;
    }

    void Relife_White()
    {
        foreach (ChessPiece piece in blackDefeat)
        {
            if (piece.type == ChessPieceType.Hero)
            {
                if (piece != null)
                {
                    for (int i = 0; i < size_row; i++)
                        for (int j = 0; j < size_col; j++)
                        {
                            if (posChess[i, j] == null)
                            {
                                piece.setIsDead(false);
                                posChess[i, j] = piece;
                                piece.SetPosition(new Vector3(m_default_posX - i, m_default_posY, m_default_posZ + j), false);
                                piece.SetScale(new Vector3(0.8f, 0.8f, 0.8f), false);
                                return;
                            }
                        }
                }
            }
        }
    }

    void Relife_Black()
    {
        foreach (ChessPiece piece in whiteDefeat)
        {
            if (piece.type == ChessPieceType.Hero)
            {
                if (piece != null)
                {
                    for (int i = size_row - 1; i >= 0; i--)
                        for (int j = size_col - 1; j >= 0; j--)
                        {
                            if (posChess[i, j] == null)
                            {
                                piece.setIsDead(false);
                                posChess[i, j] = piece;
                                piece.SetPosition(new Vector3(m_default_posX - i, m_default_posY, m_default_posZ + j), false);
                                piece.SetScale(new Vector3(0.8f, 0.8f, 0.8f), false);
                                return;
                            }
                        }
                }
            }
        }
    }

    void ChangeTurn()
    {
        if (turn == 0)
            turn = 1;
        else
            turn = 0;

        if (m_ultiCounterWhite < 3)
        {
            m_ultiCounterWhite++;
        }
        if (m_ultiCounterBlack < 3)
        {
            m_ultiCounterBlack++;
        }
        if (turn_relifeBlack > 0)
        {
            turn_relifeBlack--;
        }
        if (turn_relifeWhite > 0)
        {
            turn_relifeWhite--;
        }
    }
    public static void EnterGame(int team)
    {
        if (team == 0)
        {
            SceneManager.LoadScene(2);
            return;
        }

        SceneManager.LoadScene(6);
    }

    //No win return -1, white win return 0, black win return 1
    int CheckWinGame()
    {
        int countWhite = 0;
        int countBlack = 0;
        for (int i = 0; i < size_row; i++)
            for (int j = 0; j < size_col; j++)
            {
                if (posChess[i, j] != null)
                {
                    if (posChess[i, j].team == 0)
                        countWhite++;
                    else
                        countBlack++;
                }
            }

        if (countWhite <= 2)
            return 1;
        if (countBlack <= 2)
            return 0;
        return -1;
    }
    public static void updatePos(int prevX, int prevY, int desX, int desY)
    {
        updateQueue[0] = prevX;
        updateQueue[1] = prevY;
        updateQueue[2] = desX;
        updateQueue[3] = desY;
    }
}

