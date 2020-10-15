using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField hostIP;
    static public string IPaddr;


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
        IPaddr = hostIP.text;
        // SocketIOComponent needs to read the static string to set the Host url.
        SceneManager.LoadScene("main");
    }
}
