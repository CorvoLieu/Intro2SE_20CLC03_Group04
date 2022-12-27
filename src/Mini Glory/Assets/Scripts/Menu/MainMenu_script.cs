using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Networking.Transport;
using System;

public class MainMenu_script : MonoBehaviour
{
    [SerializeField] private TMP_InputField addressInput;

    public NetExecute net_execute;



    //public Server server;
    //public Client client;


    //// multi logic
    //private int playerCount = -1;
    //private int currentTeam = -1;
    //private bool localGame = true;
    //private bool[] playerRematch = new bool[2];




    //Start is called before the first frame update



    //public void Start()
    //{
    //    RegisterEvents();
    //}



    // MAIN MENU
    public void CreateRoom()
    {
        net_execute.server.Init(8008);
        net_execute.client.Init("127.0.0.1", 8008);
    }
    public void JoinRoom()
    {
        // Only switch scene
        // Done outside
    }
    public void OptionButton()
    {
        // Only switch scene
        // Done outside
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }






    // OPTION MENU
    public void BackButton_OptionMenu()
    {
        // Only switch scene
        // Done outside
    }








    // JOIN GAME
    public void JoinButton()
    {
        net_execute.client.Init(addressInput.text, 8008);
    }
    public void LocalGame()
    {
        net_execute.server.Init(8008);
        net_execute.client.Init("127.0.0.1", 8008);
    }
    public void BackButton_JoinGame()
    {
        // Only switch scene
        // Done outside
    }









    // HOST GAME
    public void BackButton_HostGame()
    {
        net_execute.server.Shutdown();
        net_execute.client.Shutdown();
    }



    //#region
    //private void RegisterEvents()
    //{
    //    NetUtility.S_WELCOME += OnWelcomeServer;
    //    NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
    //    NetUtility.S_REMATCH += OnRematchServer;

    //    NetUtility.C_WELCOME += OnWelcomeClient;
    //    NetUtility.C_START_GAME += OnStartGameClient;
    //    NetUtility.C_MAKE_MOVE += OnMakeMoveClient;
    //    NetUtility.C_REMATCH += OnRematchClient;
    //}
    //private void UnRegisterEvents()
    //{
    //    NetUtility.S_WELCOME -= OnWelcomeServer;
    //    NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
    //    NetUtility.S_REMATCH -= OnRematchServer;

    //    NetUtility.C_WELCOME -= OnWelcomeClient;
    //    NetUtility.C_START_GAME -= OnStartGameClient;
    //    NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;
    //    NetUtility.C_REMATCH -= OnRematchClient;
    //}





    //// Server
    //private void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn)
    //{
    //    NetMakeMove mm = msg as NetMakeMove;
    //    // Receive, and just broadcast it back
    //    Server.Instance.Broadcast(msg);
    //}
    //private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    //{
    //    NetWelcome nw = msg as NetWelcome;
    //    nw.AssignedTeam = ++playerCount;
    //    Server.Instance.SendToClient(cnn, nw);

    //    // if there are enough 2 connections, start the game
    //    if (playerCount == 1)
    //        Server.Instance.Broadcast(new NetStartGame());
    //}
    //private void OnRematchServer(NetMessage msg, NetworkConnection cnn)
    //{
    //    Server.Instance.Broadcast(msg);
    //}



    //// Client
    //private void OnWelcomeClient(NetMessage msg)
    //{
    //    NetWelcome nw = msg as NetWelcome;
    //    currentTeam = nw.AssignedTeam;

    //    Debug.Log($"My assigned team is {nw.AssignedTeam}");
    //}
    //private void OnStartGameClient(NetMessage msg)
    //{
    //    // We just need to go to the game scene
    //    SceneManager.LoadScene(1);
    //}
    //private void OnMakeMoveClient(NetMessage msg)
    //{
    //    NetMakeMove mm = msg as NetMakeMove;

    //    Debug.Log($"MM: {mm.teamId} : {mm.originalX} {mm.originalY} -> {mm.destinationX} {mm.destinationY}");

    //    if (mm.teamId != currentTeam)
    //    {
    //        // MoveTo function
    //    }
    //}
    //private void OnRematchClient(NetMessage msg)
    //{
    //    NetRematch rm = msg as NetRematch;

    //    playerRematch[rm.teamId] = rm.wantRematch == 1;

    //    // activate the piece of UI

    //    // if both players want to rematch
    //}
    //#endregion

}
