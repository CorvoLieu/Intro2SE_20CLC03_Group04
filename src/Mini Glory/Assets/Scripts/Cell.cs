using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    Material m_material;
    // Color m_defaut_color;

    Color m_black = new Vector4(0.2196f,0.2196f,0.2196f);
    Color m_white = new Vector4(0.8773f,0.8773f,0.8773f);
    Color m_red = new Vector4(1f,0.3245f,0.3245f);
    Color m_green = new Vector4(0.5496f,1f,0.3716f);
    // Start is called before the first frame update
    void Start()
    {
        // m_material = gameObject.GetComponent<MeshRenderer>().material;
        // m_material.color = m_white;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //0-white, 1-black, 2-red, other-green
    public void ChangeColor(int col)    
    {
        m_material = gameObject.GetComponent<MeshRenderer>().material;
        if (col == 0)
            m_material.color = m_white;
        else if (col == 1)
            m_material.color = m_black;
        else if (col == 2)
            m_material.color = m_red;
        else
            m_material.color = m_green;
    }

    // public void ResetColor()
    // {
    //     m_material.color = m_defaut_color;
    // }

    // //0-white, other-black
    // public void set_default_color(int col)
    // {
    //     if (col == 0)
    //     {
    //         m_defaut_color = m_white;
    //         // m_material.color = m_white;
    //     }
    //     else
    //     {
    //         m_defaut_color = m_black;
    //         // m_material.color = m_black;
    //     }
    // }
}
