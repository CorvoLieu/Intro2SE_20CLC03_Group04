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



    // MAIN MENU
    public void CreateRoom()
    {
        net_execute.server.Init(8008);
        net_execute.client.Init("127.0.0.1", 8008);
        NotifCenter.notif.Enqueue("Create a Room");
    }
    public void JoinRoom()
    {
        NotifCenter.notif.Enqueue("Join a Room");
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
        if(addressInput.text != "")
            net_execute.client.Init(addressInput.text, 8008);
        else
            NotifCenter.notif.Enqueue("Input field must not be empty");
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


}
