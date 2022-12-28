using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPiece : ChessPiece
{
    public int life = 0;
    public int nextUlti = 0;
    public int ultiCounter = -1;
    public bool ulti_ed = false;

    public virtual void ulti(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {

    }
    public virtual string toString()
    {
        return "HeroPiece";
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        type = ChessPieceType.Hero;
    }
    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();
        return r;
    }
}
