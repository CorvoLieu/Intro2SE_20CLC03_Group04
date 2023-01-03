using Unity.Networking.Transport;
using UnityEngine;

public class NetID : NetMessage
{
    public int PlayerID { get; set; }

    public NetID()
    {
        Code = OpCode.ID;
    }
    public NetID(DataStreamReader reader)
    {
        Code = OpCode.ID;
        Deserialize(reader);
    }


    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(PlayerID);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        PlayerID = reader.ReadInt();
    }


    public override void ReceivedOnClient()
    {
        NetUtility.C_ID?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_ID?.Invoke(this, cnn);
    }
}
