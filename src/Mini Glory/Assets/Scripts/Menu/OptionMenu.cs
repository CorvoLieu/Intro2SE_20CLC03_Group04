using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public void LeaveGame()
    {
        Debug.Log(Client.Instance.ToString());
        Client.Instance.SendToServer(new NetDisconnect());
    }
}
