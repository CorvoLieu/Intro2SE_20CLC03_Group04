using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}

public class ChatLog : MonoBehaviour
{
    [SerializeField]
    List<Message> messageList = new List<Message>();
    public int maxMess;

    public GameObject ChatPanel;
    public GameObject TextObject;
    public InputField chatBox;
    public static string inComeText = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inComeText != "")
        {
            SendMessageToChat(inComeText);
            inComeText = "";
        }

        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var nc = new NetChat();
                nc.msg = chatBox.text;
                Client.Instance.SendToServer(nc);
                chatBox.text = "";
            }
        }
        else 
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
            }
        }
    }

    //Gui message len chat
    public void SendMessageToChat(string text)
    {
        if (messageList.Count >= maxMess)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMess = new Message();
        newMess.text = text;
        GameObject newText = Instantiate(TextObject, ChatPanel.transform);
        newMess.textObject = newText.GetComponent<Text>();
        newMess.textObject.text = newMess.text;
        messageList.Add(newMess);
    }

}
