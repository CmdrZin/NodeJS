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
        socket.On("updateMotion", OnUpdateMotion);
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
        player.GetComponent<PlayerController>().id = obj.data["id"].str;

        JSONObject playerData = new JSONObject(JSONObject.Type.OBJECT);
        playerData.AddField("color", player.GetComponent<PlayerController>().color);
        playerData.AddField("p", VectorToJson(player.transform.position));
        playerData.AddField("speed", player.GetComponent<PlayerController>().speed);
        Vector3 v3 = player.GetComponent<PlayerController>().GetVelocity();
        playerData.AddField("v", VectorToJson(v3));

        string id = player.GetComponent<PlayerController>().id;
        networkPlayerManager.AddPlayer(id, player);
        Debug.Log("Local Player " + id + " added");

        socket.Emit("newPlayerData", playerData);
    }

    private void OnSpawn(SocketIOEvent obj)
    {
        Debug.Log("Spawn " + obj.data);

        networkPlayerManager.SpawnPlayer(obj);
    }

    // Update this Remote Player's id, velocity, speed, and position.
    private void OnUpdateMotion(SocketIOEvent obj)
    {
        Debug.Log("update motion " + obj.data);
        networkPlayerManager.UpdateMotion(obj);
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
        networkPlayerManager.RemovePlayer(obj.data["id"].str);
    }

    // Create a data packet for updateMotion message.
    public void SendUpdateMotion(Vector3 velocity, float speed, Vector3 position)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("id", player.GetComponent<PlayerController>().id);
        jsonObject.AddField("v", VectorToJson(velocity));
        jsonObject.AddField("speed", speed);
        jsonObject.AddField("p", VectorToJson(position));

        socket.Emit("updateMotion", jsonObject);
    }

    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("x", vector.x);
        jsonObject.AddField("y", vector.y);
        jsonObject.AddField("z", vector.z);

        return jsonObject;
    }

    public static JSONObject Vector2ToJson(Vector2 vector)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("x", vector.x);
        jsonObject.AddField("y", vector.y);

        return jsonObject;
    }

}
