using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For network operations
using SocketIO;


public class Network : MonoBehaviour
{
    static SocketIOComponent socket;


    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();

        // Register all of the message handlers (callbacks)
        socket.On("open", OnConnected);
        socket.On("register", OnRegister);
        socket.On("spawn", OnSpawn);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
        socket.On("disconnected", OnDisconnected);
    }

    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }

    private void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("registered id = " + obj.data);
    }

    private void OnSpawn(SocketIOEvent obj)
    {
        Debug.Log("Spawn " + obj.data);
    }

    private void OnUpdatePosition(SocketIOEvent obj)
    {
        Debug.Log("update position " + obj.data);
    }

    private void OnRequestPosition(SocketIOEvent obj)
    {
        Debug.Log("request position " + obj.data);
    }

    private void OnDisconnected(SocketIOEvent obj)
    {
        Debug.Log("disconnet " + obj.data);
    }
}
