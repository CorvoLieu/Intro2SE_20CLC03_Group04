using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType
{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight = 3,
    Bishop = 4,
    Hero = 5,
}

public enum ChessPieceTeam
{
    White = 0,
    Black = 1
}

public class ChessPiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;
    public int stunned = 0;
    public int life = 0;

    bool isDead = false;
    protected Vector3 desiredPosition;
    private Vector3 desiredScale = Vector3.one * 1700f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        type = ChessPieceType.None;
    }
    private void Start() 
    {
        desiredPosition = new Vector3((float)-currentX, 0.3f, (float)currentY);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }

    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();
        return r;
    }

    public void SetPosition(Vector3 position, bool force = false)
    {
        desiredPosition = position;
        if (force)
        {
            transform.position = desiredPosition;
            Debug.Log(transform.position);
        }
    }

    public virtual void SetScale(Vector3 scale, bool force = false)
    {
        desiredScale = scale;
        if (force)
            transform.localScale = desiredScale;
    }

    public virtual void SetRotation(Vector3 rot)
    {
        transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }

    public void setIsDead(bool dead)
    {
        isDead = dead;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    public override string ToString()
    {
        return "ChessPieces";
    }

    public virtual void ulti(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {

    }
}
