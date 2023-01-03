using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public void LeaveGame()
    {
        Debug.Log(Client.Instance.ToString());
        Client.Instance.SendToServer(new NetDisconnect());
        SceneManager.LoadScene(0);
    }
}
