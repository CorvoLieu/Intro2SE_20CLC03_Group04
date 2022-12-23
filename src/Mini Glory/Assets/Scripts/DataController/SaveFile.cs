using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public int wid;
    public int len;
    public List<ChessPiece> pieces = new List<ChessPiece>();
    public List<HeroPiece> heroes = new List<HeroPiece>();
    public float timer;
    public string rivalID;
}
