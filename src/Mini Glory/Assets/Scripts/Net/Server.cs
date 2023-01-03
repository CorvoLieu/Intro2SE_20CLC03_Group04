using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;



public class Server : MonoBehaviour
{
    #region Singleton implementation
    public static Server Instance { set; get; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion



    public NetworkDriver driver;
    public NativeList<NetworkConnection> connections;

    private bool isActive = false;
    private const float keepAliveTickRate = 20.0f;
    private float lastKeepAlive;

    public Action connectionDropped;



    //methods
    public void Init(ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = port;

        if (driver.Bind(endpoint) != 0)
        {
            Debug.Log("Unable to bind on port " + endpoint.Port);
            return;
        }
        else
        {
            driver.Listen();
            Debug.Log("Currently listening on port " + endpoint.Port);
        }

        connections = new NativeList<NetworkConnection>(2, Allocator.Persistent);
        isActive = true;
    }
    public void Shutdown()
    {
        if (isActive)
        {
            Debug.Log("[SERVER] Shutting down Server");
            driver.Dispose();
            connections.Dispose();
            isActive = false;
        }
    }
    public void OnDestroy()
    {
        Debug.Log("[SERVER] OnDestroy called");
        Shutdown();
    }

    public void FixedUpdate()
    {
        if (!isActive)
        {
            Debug.Log("[SERVER] is not actived");
            return;
        }

        KeepAlive();

        driver.ScheduleUpdate().Complete();

        CleanupConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }
    private void KeepAlive()
    {
        // Debug.Log("[SERVER] Keep client alive");
        if (Time.time - lastKeepAlive > keepAliveTickRate)
        {
            lastKeepAlive = Time.time;
            Broadcast(new NetKeepAlive());
        }
    }
    private void CleanupConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            // Debug.Log("[SERVER] Clean up connection called");
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }
    }
    private void AcceptNewConnections()
    {
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            // Debug.Log("[SERVER] Accept new connection");
            connections.Add(c);
        }
    }
    private void UpdateMessagePump()
    {
        // Debug.Log("[SERVER] Update server pump called");
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                // Debug.Log("[SERVER] Detect new msg");
                if (cmd == NetworkEvent.Type.Data)
                {
                    NetUtility.OnData(stream, connections[i], this);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    OnDisconnect();
                }
            }
        }
    }

    // server specific
    public void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        Debug.Log($"[SERVER] Sending {msg} to CLIENT");
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    public void Broadcast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
            if (connections[i].IsCreated)
            {
                Debug.Log($"[SERVER] Sending {msg.Code} to : {connections[i].InternalId}");
                SendToClient(connections[i], msg);
            }
    }
    public void OnDisconnect()
    {
        Debug.Log("Client desconnected from server");
        connections[0] = default(NetworkConnection);
        connections[1] = default(NetworkConnection);
        connectionDropped?.Invoke();
        Shutdown();   // because we're in a two person game
    }
}
