using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : HeroPiece
{
    public override string ToString()
    {
        return "Clown";
    }
    // Start is called before the first frame update
    void Start()
    {
        life = 1;
        nextUlti = 3;
        ultiCounter = 0;
    }

    public override void ulti(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        for (int x = currentX - 1, y = currentY - 1; x <= currentX + 1 && y <= currentY + 1; x++, y++)
        {
            if(x >= 0 && x < tileCountX && y >= 0 && y < tileCountY)
            {
                board[x, y].stunned = 2;
            }
        }
    }

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        int x, y;

        // Top
        x = currentX + 1;
        y = currentY + 2;
        if(x < tileCountX && y < tileCountY)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX;
        y = currentY + 2;
        if(x < tileCountX && y < tileCountY)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX - 1;
        y = currentY + 2;
        if(x >= 0 && y < tileCountY)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        //Right
        x = currentX + 2;
        y = currentY + 1;
        if(x < tileCountX && y < tileCountY)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX + 2;
        y = currentY;
        if(x < tileCountX && y < tileCountY)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX + 2;
        y = currentY - 1;
        if(x < tileCountX && y >= 0)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        // Bottom
        x = currentX + 1;
        y = currentY - 2;
        if(x < tileCountX && y >= 0)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX;
        y = currentY - 2;
        if(x < tileCountX && y >= 0)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX - 1;
        y = currentY - 2;
        if(x >= 0 && y >= 0)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        // Left
        x = currentX - 2;
        y = currentY - 1;
        if(x >= 0 && y >= 0)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX - 2;
        y = currentY;
        if(x >= 0 && y >= 0)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        x = currentX - 2;
        y = currentY + 1;
        if(x >= 0 && y < tileCountY)
            if(board[x, y] == null)
                r.Add(new Vector2Int(x, y));

        return r;
    }
}
