using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotifCenter : MonoBehaviour
{
    public TMP_Text text;
    public static Queue<string> notif = new Queue<string>();
    public float timeShow = 3.0f;
    private float timer = 0.0f;

    void Awake()
    {
        text.text = "";
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (text.text == "")
        {
            if (notif.Count != 0)
            {
                timer = 0.0f;
                text.text = notif.Dequeue();
            }
        }
        else if (timer >= timeShow)
        {
            text.text = "";
            timer = 0.0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
