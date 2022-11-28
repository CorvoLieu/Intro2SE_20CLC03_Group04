using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPiece : ChessPiece
{
    public int life = 0;
    public int nextUnlti = 0;
    public int ultiCounter = -1;
    public bool ulti_ed = false;

    public virtual void ulti(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {

    }
}
