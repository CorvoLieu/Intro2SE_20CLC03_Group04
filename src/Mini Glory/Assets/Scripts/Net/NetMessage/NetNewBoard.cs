using Unity.Networking.Transport;
using UnityEngine;

public class NetNewBoard : NetMessage
{
    public int len { get; set; }
    public int wid { get; set; }
    public int hero1Ability { get; set; }
    public int hero2Ability { get; set; }
    public ChessPieceType[,] board;

    public NetNewBoard()
    {
        Code = OpCode.NEW_BOARD;
    }
    public NetNewBoard(DataStreamReader reader)
    {
        Code = OpCode.NEW_BOARD;
        Deserialize(reader);
    }


    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(len);
        writer.WriteInt(wid);
        writer.WriteInt(hero1Ability);
        writer.WriteInt(hero2Ability);
        for (int x = 0; x < wid; ++x)
            for (int y = 0; y < len; ++y)
            {
                writer.WriteByte((byte)board[x, y]);
            }
    }
    public override void Deserialize(DataStreamReader reader)
    {
        len = reader.ReadInt();
        wid = reader.ReadInt();
        hero1Ability = reader.ReadInt();
        hero2Ability = reader.ReadInt();
        board = new ChessPieceType[wid, len];
        for (int x = 0; x < wid; ++x)
            for (int y = 0; y < len; ++y)
            {
                board[x, y] = (ChessPieceType)reader.ReadByte();
            }
    }


    public override void ReceivedOnClient()
    {
        NetUtility.C_NEW_BOARD?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_NEW_BOARD?.Invoke(this, cnn);
    }
}
