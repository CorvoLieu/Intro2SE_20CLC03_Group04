using Unity.Networking.Transport;

public class NetChat : NetMessage
{
    public string msg { set; get; }

    public NetChat()
    {
        Code = OpCode.CHAT;
    }
    public NetChat(DataStreamReader reader)
    {
        Code = OpCode.CHAT;
        Deserialize(reader);
    }


    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(msg.Length);
        foreach(var c in msg)
        {
            writer.WriteByte((byte)c);
        }
    }
    public override void Deserialize(DataStreamReader reader)
    {
        int len = reader.ReadInt();
        msg = "";
        for(int i = 0; i < len; ++i)
            msg += (char)reader.ReadByte();
    }


    public override void ReceivedOnClient()
    {
        NetUtility.C_CHAT?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_CHAT?.Invoke(this, cnn);
    }

}
