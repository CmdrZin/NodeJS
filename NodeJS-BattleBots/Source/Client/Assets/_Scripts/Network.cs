using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For network operations
using SocketIO;


public class Network : MonoBehaviour
{
    static SocketIOComponent socket;

    public GameObject player;

    public NetworkPlayerManager networkPlayerManager;

    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        // +++ TEST CODE
//        Debug.Log("Socket.id = " + socket.sid);
        // ---

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
        Debug.Log("conected - open");
    }

    private void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("registered id = " + obj.data);
        // Save Player ID
        player.GetComponent<PlayerController>().id = obj.data.str;

        JSONObject playerData = new JSONObject(JSONObject.Type.OBJECT);
        playerData.AddField("color", player.GetComponent<PlayerController>().color);
        playerData.AddField("position", VectorToJson(player.transform.position));
        Vector2 v2 = player.GetComponent<PlayerController>().GetForce();
        Vector3 v3 = new Vector3(v2.x, 0.0f, v2.y);
        playerData.AddField("force", VectorToJson(v3));
        playerData.AddField("speed", player.GetComponent<PlayerController>().speed);

        socket.Emit("newPlayerData", playerData);
    }

    private void OnSpawn(SocketIOEvent obj)
    {
        Debug.Log("Spawn " + obj.data);

        networkPlayerManager.SpawnPlayer(obj);
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

    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("x", vector.x);
        jsonObject.AddField("y", vector.z);

        return jsonObject;
    }

}
