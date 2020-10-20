using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField hostIP;
    static public string IPaddr;
    static public int skinColor = 0;      // default color..treat as orange.


    // Other InputFields can be added to set Player options like Name, Color, Avatar, etc.

    public void Start()
    {
        //Adds a listener that invokes the "LockInput" method when the player finishes editing the main input field.
        //Passes the main input field into the method when "LockInput" is invoked
        hostIP.onEndEdit.AddListener(delegate { SetHostIP(hostIP); });
    }

    // Set the static string then load the Main scene.
    void SetHostIP(InputField ip)
    {
        if( ip.text == "")
        {
            IPaddr = "127.0.0.1";
            ip.text = IPaddr;
        }
        else
        {
            IPaddr = ip.text;
        }
    }

    // This function will be called when the Button is Clicked. (see link in Inspector for Button)
    public void JoinGame()
    {
        // SocketIOComponent needs to read the static string to set the Host url.
        SceneManager.LoadScene("main");
    }

    // This function updates the Skin color public variable to set of the Avatar color.
    // This is called from each CanvasUI->Panel_Background->Toggle_<color> Toggle object.
    public void SetSkinColor(int color)
    {
        skinColor = color;
    }
}
