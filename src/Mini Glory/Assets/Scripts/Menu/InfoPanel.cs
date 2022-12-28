using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public PowerMenu menu;
    private TMPro.TextMeshProUGUI output;
    private int currentlySelect;
    public int CurrentlySelect
    {
        get
        {
            return currentlySelect;
        }
        set
        {
            currentlySelect = value;
            changeText();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        output = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if(output == null)
            Debug.Log("Unable to get Text Mesh");
        else   
            Debug.Log(output.GetType());
        
    }

    private void changeText()
    {
        switch (currentlySelect)
        {
            case 1:
            {
                output.text = 
                "Passive:\n" + 
                " - Eleminate obstacle as a normal kill.\n" + 
                " - I have 2 lives.\n" + 
                "Active:\n" + 
                " - Refill my lives and gain +1 life for 2 turns.\n" + 
                "Ulticharge: 4";
                break;
            }
            case 2:
            {
                output.text =  
                "Passive:\n" + 
                " - I can only eleminate an enemy if that enemy is behind another enemy.\n" + 
                "Active:\n" + 
                " - Jump to an enemy behind another enemy and kill any enemies directly in front, back, left and right of me.\n" + 
                "Ulticharge: 5";
                break;
            }
            case 3:
            {
                output.text = 
                "Passive:\n" + 
                " - I can not kill enemies.\n" +
                "Active:\n" + 
                " - I stun enemies around me for 2 turns.\n" + 
                "Ulticharge: 3";;
                break;
            }
            default:
            {
                break;
            }
        }
    }
}
