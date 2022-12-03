using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;




public class MainMenu_script : MonoBehaviour
{
    [SerializeField] private TMP_InputField addressInput;

    public Server server;
    public Client client;






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

}
