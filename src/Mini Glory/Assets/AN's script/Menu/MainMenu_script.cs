using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Networking.Transport;




public class MainMenu_script : MonoBehaviour
{
    [SerializeField] private TMP_InputField addressInput;

    public Server server;
    public Client client;


    // multi logic
    private int playerCount = -1;
    private int currentTeam = -1;
    private bool localGame = true;


    //Start is called before the first frame update



    public void Start()
    {
        RegisterEvents();
    }



    // MAIN MENU
    public void CreateRoom()
    {
        server.Init(8008);
        client.Init("127.0.0.1", 8008);
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
        client.Init(addressInput.text, 8008);
    }
    public void LocalGame()
    {
        server.Init(8008);
        client.Init("127.0.0.1", 8008);
    }
    public void BackButton_JoinGame()
    {
        // Only switch scene
        // Done outside
    }









    // HOST GAME
    public void BackButton_HostGame()
    {
        server.Shutdown();
        client.Shutdown();
    }



    #region
    private void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;

        NetUtility.C_WELCOME += OnWelcomeClient;
        NetUtility.C_START_GAME += OnStartGameClient;
    }
    private void UnRegisterEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;

        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGameClient;
    }


    // Server
    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        NetWelcome nw = msg as NetWelcome;
        nw.AssignedTeam = ++playerCount;
        Server.Instance.SendToClient(cnn, nw);

        // if there are enough 2 connections, start the game
        if (playerCount == 1)
            Server.Instance.Broadcast(new NetStartGame());
    }


    // Client
    private void OnWelcomeClient(NetMessage msg)
    {
        NetWelcome nw = msg as NetWelcome;
        currentTeam = nw.AssignedTeam;

        Debug.Log($"My assigned team is {nw.AssignedTeam}");
    }
    private void OnStartGameClient(NetMessage msg)
    {
        // We just need to go to the game scene
        SceneManager.LoadScene(1);
    }
    #endregion

}
