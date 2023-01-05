using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public GameController controller;

    public void LeaveGame()
    {
        Debug.Log(Client.Instance.ToString());
        Client.Instance.SendToServer(new NetDisconnect());
        SceneManager.LoadScene(0);
    }

    public void SaveGame()
    {
        controller.SaveGame();
    }

    public void LoadGame()
    {
        controller.LoadGame();
    }
}
