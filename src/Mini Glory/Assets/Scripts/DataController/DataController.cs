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
        SaveGame(GetSaveFile());
        var temp = LoadGame("123456");
        temp.rivalID = "654321";
        SaveGame(temp);
    }

    private SaveFile GetSaveFile()
    {
        SaveFile saveFile = new SaveFile();

        // Test Cases
        saveFile.len = 5;
        saveFile.wid = 6;

        saveFile.timer = 5f;
        saveFile.rivalID = "123456";

        var newTank = new Tank();
        newTank.team = 0;
        newTank.currentX = 1;
        newTank.currentY = 2;
        saveFile.heroes.Add(newTank);
        var newClown = new Clown();
        newClown.team = 0;
        newClown.currentX = 1;
        newClown.currentY = 2;
        saveFile.heroes.Add(newClown);

        var newKnight = new Knight();
        newKnight.team = 0;
        newKnight.currentX = 5;
        newKnight.currentY = 9;
        saveFile.pieces.Add(newKnight);
        var newRook = new Rook();
        newRook.team = 1;
        newRook.currentX = 7;
        newRook.currentY = 4;
        saveFile.pieces.Add(newRook);
        var newPawn = new Pawn();
        newPawn.team = 0;
        newPawn.currentX = 1;
        newPawn.currentY = 8;
        saveFile.pieces.Add(newPawn);

        return saveFile;
    }

    private ChessPiece GetXMLLoadPieceAttrib(XmlNode xmlPiece, ChessPiece result)
    {
        result.team = int.Parse(xmlPiece.Attributes["team"].Value);
        result.currentX = int.Parse(xmlPiece.Attributes["x"].Value);
        result.currentY = int.Parse(xmlPiece.Attributes["y"].Value);
        // result.type;
        result.stunned = int.Parse(xmlPiece.Attributes["stunned"].Value);
        result.setIsDead(bool.Parse(xmlPiece.Attributes["isDead"].Value));

        return result;
    }

    private void GetXMLLoadPiece(XmlNode xmlPiece, ref SaveFile result)
    {
        ChessPiece piece;
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
        piece = GetXMLLoadPieceAttrib(xmlPiece, piece);
        result.pieces.Add(piece);
    }
    private void GetXMLLoadHero(XmlNode xmlHero, ref SaveFile result)
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
        hero = (HeroPiece)GetXMLLoadPieceAttrib(xmlHero, hero);
        result.heroes.Add(hero);
    }

    public SaveFile LoadGame(string rivalID)
    {
        SaveFile result = new SaveFile();
        if (File.Exists(Application.dataPath + "/../Data/" + rivalID + ".txt"))
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + "/../Data/" + rivalID + ".txt");

            // get save data
            // len
            XmlNode len = xmlDocument.GetElementsByTagName("Len")[0];
            Debug.Log(len.InnerText);
            result.len = int.Parse(len.InnerText);
            // wid
            XmlNode wid = xmlDocument.GetElementsByTagName("Wid")[0];
            result.wid = int.Parse(wid.InnerText);
            // timer
            XmlNode timer = xmlDocument.GetElementsByTagName("Timer")[0];
            result.timer = float.Parse(timer.InnerText);

            result.rivalID = rivalID;

            XmlNodeList pieces = xmlDocument.GetElementsByTagName("Piece");
            foreach (XmlNode piece in pieces)
            {
                GetXMLLoadPiece(piece, ref result);
            }
            XmlNodeList heroes = xmlDocument.GetElementsByTagName("Hero");
            foreach (XmlNode hero in heroes)
            {
                GetXMLLoadHero(hero, ref result);
            }
        }
        return result;
    }

    private void GetXMLSaveData(ChessPiece piece, ref XmlElement root, XmlDocument xmlDocument)
    {
        root.InnerText = piece.ToString();
        root.SetAttribute("team", piece.team.ToString());
        root.SetAttribute("x", piece.currentX.ToString());
        root.SetAttribute("y", piece.currentY.ToString());
        root.SetAttribute("type", piece.type.ToString());
        root.SetAttribute("stunned", piece.stunned.ToString());
        root.SetAttribute("isDead", piece.getIsDead().ToString());
    }

    private XmlElement GetXMLSavePiece(ChessPiece piece, XmlDocument xmlDocument)
    {
        XmlElement root = xmlDocument.CreateElement("Piece");

        GetXMLSaveData(piece, ref root, xmlDocument);

        return root;
    }

    private XmlElement GetXMLSaveHero(HeroPiece hero, XmlDocument xmlDocument)
    {
        XmlElement root = xmlDocument.CreateElement("Hero");
        GetXMLSaveData(hero, ref root, xmlDocument);

        root.SetAttribute("life", hero.life.ToString());
        root.SetAttribute("nextUlti", hero.nextUlti.ToString());
        root.SetAttribute("ultiCounter", hero.ultiCounter.ToString());
        root.SetAttribute("ulti_ed", hero.ulti_ed.ToString());

        return root;
    }

    public void SaveGame(SaveFile saveFile)
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
        // time
        XmlElement timer = xmlDocument.CreateElement("Timer");
        timer.InnerText = saveFile.timer.ToString();
        root.AppendChild(timer);
        // id
        XmlElement ID = xmlDocument.CreateElement("rivalID");
        ID.InnerText = saveFile.rivalID;
        root.AppendChild(ID);

        // piece
        XmlElement pieceList = xmlDocument.CreateElement("PieceList");
        foreach (var piece in saveFile.pieces)
        {
            XmlElement xmlPiece = GetXMLSavePiece(piece, xmlDocument);
            pieceList.AppendChild(xmlPiece);
        }
        root.AppendChild(pieceList);

        // hero
        XmlElement heroList = xmlDocument.CreateElement("HeroList");
        foreach (var hero in saveFile.heroes)
        {
            XmlElement xmlHero = GetXMLSaveHero(hero, xmlDocument);
            heroList.AppendChild(xmlHero);
        }
        root.AppendChild(heroList);

        xmlDocument.AppendChild(root);
        xmlDocument.Save(Application.dataPath + "/../Data/" + saveFile.rivalID + ".txt");
        if (File.Exists(Application.dataPath + "/../Data/" + saveFile.rivalID + ".txt"))
        {
            Debug.Log("File Saved");
        }
        else
        {
            Debug.Log("Error");
        }
    }
}