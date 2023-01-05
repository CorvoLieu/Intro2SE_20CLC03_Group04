using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class DataController : MonoBehaviour
{
    //Pointer to save data
    // chessboard
    // chess pieces
    // timer
    // client

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // SaveGame(GetSaveFile());
        // var temp = LoadGame("123456");
        // temp.rivalID = "654321";
        // SaveGame(temp);
    }

    

    private static ChessPiece GetXMLLoadDetail(XmlNode xmlPiece, ChessPiece result)
    {
        result.team = int.Parse(xmlPiece.Attributes["team"].Value);
        result.currentX = int.Parse(xmlPiece.Attributes["x"].Value);
        result.currentY = int.Parse(xmlPiece.Attributes["y"].Value);
        switch (xmlPiece.Attributes["type"].Value)
        {
            case "Bishop": result.type = ChessPieceType.Bishop; break;
            case "Rook": result.type = ChessPieceType.Rook; break;
            case "Knight": result.type = ChessPieceType.Knight; break;
            case "Pawn": result.type = ChessPieceType.Pawn; break;
            default: result.type = ChessPieceType.None; Debug.LogError($"Unregcognize type: {xmlPiece.Attributes["type"].Value}"); break;
        }
        // result.type;
        result.stunned = int.Parse(xmlPiece.Attributes["stunned"].Value);
        result.setIsDead(bool.Parse(xmlPiece.Attributes["isDead"].Value));

        return result;
    }

    private static ChessPiece GetXMLLoadPiece(XmlNode xmlPiece)
    {
        ChessPiece piece = null;
        switch (xmlPiece.InnerText)
        {
            case "Knight":
                {
                    piece = new Knight();
                    break;
                }
            case "Rook":
                {
                    piece = new Rook();
                    break;
                }
            case "Bishop":
                {
                    piece = new Bishop();
                    break;
                }
            case "Pawn":
                {
                    piece = new Pawn();
                    break;
                }
            default:
                {
                    throw new System.Exception("Unrecognize Piece \"" + xmlPiece.InnerText + "\" in save file");
                }
        }
        piece = GetXMLLoadDetail(xmlPiece, piece);
        return piece;
    }
    
    private static void GetXMLLoadHero(XmlNode xmlHero, ref SaveFile result)
    {
        HeroPiece hero;
        switch (xmlHero.InnerText)
        {
            case "Tank":
                {
                    hero = new Tank();
                    break;
                }
            case "Clown":
                {
                    hero = new Clown();
                    break;
                }
            case "Canoneer":
                {
                    hero = new Canoneer();
                    break;
                }
            default:
                {
                    throw new System.Exception("Unrecognize Hero \"" + xmlHero.InnerText + "\" in save file");
                }
        }
        hero.life = int.Parse(xmlHero.Attributes["life"].Value);
        hero.ultiCounter = int.Parse(xmlHero.Attributes["ultiCounter"].Value);
        hero.nextUlti = int.Parse(xmlHero.Attributes["nextUlti"].Value);
        hero.ulti_ed = bool.Parse(xmlHero.Attributes["ulti_ed"].Value);
        hero = (HeroPiece)GetXMLLoadDetail(xmlHero, hero);
        result.heroes.Add(hero);
    }

    public static SaveFile LoadGame(string rivalID)
    {
        SaveFile result = new SaveFile();
        if (File.Exists(Application.dataPath + "/Data/" + rivalID + ".txt"))
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + "/Data/" + rivalID + ".txt");

            // get save data
            // len
            XmlNode len = xmlDocument.GetElementsByTagName("Len")[0];
            result.len = int.Parse(len.InnerText);
            // wid
            XmlNode wid = xmlDocument.GetElementsByTagName("Wid")[0];
            result.wid = int.Parse(wid.InnerText);
            // turn
            XmlNode turn = xmlDocument.GetElementsByTagName("turn")[0];
            result.turn = int.Parse(turn.InnerText);

            result.rivalID = rivalID;

            // pieceList
            XmlNode pieceListRoot = xmlDocument.GetElementsByTagName("PieceList")[0];
            XmlNodeList pieces = pieceListRoot.ChildNodes;
            foreach (XmlNode piece in pieces)
            {
                result.pieces.Add(GetXMLLoadPiece(piece));
            }
            // whiteDefeat
            XmlNode wdListRoot = xmlDocument.GetElementsByTagName("deadWhiteList")[0];
            pieces = wdListRoot.ChildNodes;
            foreach (XmlNode piece in pieces)
            {
                result.whiteDefeat.Add(GetXMLLoadPiece(piece));
            }
            // blackDefeat
            XmlNode bdListRoot = xmlDocument.GetElementsByTagName("deadWhiteList")[0];
            pieces = bdListRoot.ChildNodes;
            foreach (XmlNode piece in pieces)
            {
                result.blackDefeat.Add(GetXMLLoadPiece(piece));
            }
            XmlNodeList heroes = xmlDocument.GetElementsByTagName("Hero");
            foreach (XmlNode hero in heroes)
            {
                GetXMLLoadHero(hero, ref result);
            }
        }
        return result;
    }

    private static void GetXMLSaveDetail(ChessPiece piece, ref XmlElement root, XmlDocument xmlDocument)
    {
        root.InnerText = piece.ToString();
        root.SetAttribute("team", piece.team.ToString());
        root.SetAttribute("x", piece.currentX.ToString());
        root.SetAttribute("y", piece.currentY.ToString());
        root.SetAttribute("type", piece.type.ToString());
        root.SetAttribute("stunned", piece.stunned.ToString());
        root.SetAttribute("isDead", piece.getIsDead().ToString());
    }

    private static XmlElement GetXMLSavePiece(ChessPiece piece, XmlDocument xmlDocument)
    {
        XmlElement root = xmlDocument.CreateElement("Piece");

        GetXMLSaveDetail(piece, ref root, xmlDocument);

        return root;
    }

    private static XmlElement GetXMLSaveHero(HeroPiece hero, XmlDocument xmlDocument)
    {
        XmlElement root = xmlDocument.CreateElement("Hero");
        GetXMLSaveDetail(hero, ref root, xmlDocument);

        root.SetAttribute("life", hero.life.ToString());
        root.SetAttribute("nextUlti", hero.nextUlti.ToString());
        root.SetAttribute("ultiCounter", hero.ultiCounter.ToString());
        root.SetAttribute("ulti_ed", hero.ulti_ed.ToString());

        return root;
    }

    public static void SaveGame(SaveFile saveFile)
    {
        XmlDocument xmlDocument = new XmlDocument();

        // root node
        XmlElement root = xmlDocument.CreateElement("Save");
        root.SetAttribute("FileName", saveFile.rivalID);

        // element of root
        // len
        XmlElement len = xmlDocument.CreateElement("Len");
        len.InnerText = saveFile.len.ToString();
        root.AppendChild(len);
        // wid
        XmlElement wid = xmlDocument.CreateElement("Wid");
        wid.InnerText = saveFile.wid.ToString();
        root.AppendChild(wid);
        // id
        XmlElement ID = xmlDocument.CreateElement("rivalID");
        ID.InnerText = saveFile.rivalID;
        root.AppendChild(ID);
        // turn
        XmlElement turn = xmlDocument.CreateElement("turn");
        turn.InnerText = saveFile.turn.ToString();
        root.AppendChild(turn);

        // pieceList
        XmlElement pieceList = xmlDocument.CreateElement("PieceList");
        foreach (var piece in saveFile.pieces)
        {
            XmlElement xmlPiece = GetXMLSavePiece(piece, xmlDocument);
            pieceList.AppendChild(xmlPiece);
        }
        root.AppendChild(pieceList);
        // deadWhiteList
        XmlElement deadWhiteList = xmlDocument.CreateElement("deadWhiteList");
        foreach (var piece in saveFile.whiteDefeat)
        {
            XmlElement xmlPiece = GetXMLSavePiece(piece, xmlDocument);
            deadWhiteList.AppendChild(xmlPiece);
        }
        root.AppendChild(deadWhiteList);
        // deadBlackList
        XmlElement deadBlackList = xmlDocument.CreateElement("deadBlackList");
        foreach (var piece in saveFile.blackDefeat)
        {
            XmlElement xmlPiece = GetXMLSavePiece(piece, xmlDocument);
            deadBlackList.AppendChild(xmlPiece);
        }
        root.AppendChild(deadBlackList);

        // hero
        XmlElement heroList = xmlDocument.CreateElement("HeroList");
        foreach (var hero in saveFile.heroes)
        {
            XmlElement xmlHero = GetXMLSaveHero(hero, xmlDocument);
            heroList.AppendChild(xmlHero);
        }
        root.AppendChild(heroList);

        xmlDocument.AppendChild(root);
        xmlDocument.Save(Application.dataPath + "/Data/" + saveFile.rivalID + ".txt");
        if (File.Exists(Application.dataPath + "/Data/" + saveFile.rivalID + ".txt"))
        {
            Debug.Log("File Saved");
        }
        else
        {
            Debug.Log("Error");
        }
    }
}