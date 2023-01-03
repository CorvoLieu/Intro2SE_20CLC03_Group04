using Unity.Networking.Transport;

internal class NetDisconnect : NetMessage
{
    public NetDisconnect()
    {
        Code = OpCode.DISCONNECT;
    }
    public NetDisconnect(DataStreamReader reader)
    {
        Code = OpCode.DISCONNECT;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
    }
    public override void Deserialize(DataStreamReader reader)
    { 
        
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_DISCONNECT?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_DISCONNECT?.Invoke(this, cnn);
    }
}