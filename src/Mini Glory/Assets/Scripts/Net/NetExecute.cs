using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using UnityEngine.SceneManagement;
using System;

public class NetExecute : MonoBehaviour
{
    public Server server;
    public Client client;


    // multi logic
    private int playerCount = -1;
    private int currentTeam = -1;
    private bool localGame = true;
    private bool[] playerRematch = new bool[2];
    [HideInInspector] public int[] playerID = new int[2];



    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        RegisterEvents();
    }


    #region
    private void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
        NetUtility.S_REMATCH += OnRematchServer;
        NetUtility.S_ID += OnIDServer;
        NetUtility.S_DISCONNECT += OnDisconnectServer;

        NetUtility.C_WELCOME += OnWelcomeClient;
        NetUtility.C_START_GAME += OnStartGameClient;
        NetUtility.C_MAKE_MOVE += OnMakeMoveClient;
        NetUtility.C_REMATCH += OnRematchClient;
        NetUtility.C_DISCONNECT += OnDisconnectClient;
    }

    private void UnRegisterEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
        NetUtility.S_REMATCH -= OnRematchServer;
        NetUtility.S_ID -= OnIDServer;
        NetUtility.S_DISCONNECT -= OnDisconnectServer;

        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGameClient;
        NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;
        NetUtility.C_REMATCH -= OnRematchClient;
        NetUtility.C_DISCONNECT -= OnDisconnectClient;
    }





    // Server
    private void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn)
    {
        NetMakeMove mm = msg as NetMakeMove;
        // Receive, and just broadcast it back
        Server.Instance.Broadcast(msg);
    }
    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        NetWelcome nw = msg as NetWelcome;
        nw.AssignedTeam = ++playerCount;
        Server.Instance.SendToClient(cnn, nw);

        // if there are enough 2 connections, start the game
        if (playerCount == 1)
            Server.Instance.Broadcast(new NetStartGame());
    }
    private void OnRematchServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }
    private void OnIDServer(NetMessage msg, NetworkConnection cnn)
    {
        NetID ni = msg as NetID;
        Debug.Log("[SERVER] Receive ID " + ni.PlayerID.ToString());
        Server.Instance.Broadcast(msg);

        playerID[playerCount] = ni.PlayerID;
        Debug.Log($"[SERVER] Player ID {playerID[playerCount]} saved as player {playerCount}");
    }

    private void OnDisconnectServer(NetMessage msg, NetworkConnection cnn)
    {
        Debug.Log("[SERVER] Shutting down server");
        Server.Instance.Broadcast(msg as NetDisconnect);
        Server.Instance.Shutdown();
    }



    // Client
    private void OnWelcomeClient(NetMessage msg)
    {
        NetWelcome nw = msg as NetWelcome;
        currentTeam = nw.AssignedTeam;

        Debug.Log($"[CLIENT] My assigned team is {nw.AssignedTeam}");
    }
    private void OnStartGameClient(NetMessage msg)
    {
        // We just need to go to the game scene
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }
    private void OnMakeMoveClient(NetMessage msg)
    {
        NetMakeMove mm = msg as NetMakeMove;

        Debug.Log($"MM: {mm.teamId} : {mm.originalX} {mm.originalY} -> {mm.destinationX} {mm.destinationY}");

        if (mm.teamId != currentTeam)
        {
            // MoveTo function
        }
    }
    private void OnRematchClient(NetMessage msg)
    {
        NetRematch rm = msg as NetRematch;

        playerRematch[rm.teamId] = rm.wantRematch == 1;

        // activate the piece of UI

        // if both players want to rematch
    }
    private void OnDisconnectClient(NetMessage msg)
    {
        Debug.Log("[CLIENT] Shutting down client");
        Client.Instance.Shutdown();
        SceneManager.LoadScene(5);
    }
    #endregion
}
