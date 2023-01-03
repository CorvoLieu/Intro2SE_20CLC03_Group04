using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinGame : MonoBehaviour
{
    public TextMeshProUGUI m_title;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rematch()
    {
        SceneManager.LoadScene("Customize Game Menu");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Menu");
        //Disconnect with client
    }

    public void ChangeTitle(string newTitle)
    {
        m_title.text = newTitle;
    }
}
