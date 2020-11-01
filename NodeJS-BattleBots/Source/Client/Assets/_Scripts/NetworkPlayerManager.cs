using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For network operations
using SocketIO;

public class NetworkPlayerManager : MonoBehaviour
{
    public Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    public GameObject networkPlayer;        // this is the prefab for a network player.

    // Spawn a Network Player onto the Client playing field.
    // Data packet uses the Server:Player data struct.
    public void SpawnPlayer(SocketIOEvent obj)
    {
        Debug.Log("Spawn this player: " + obj.data);

        GameObject newPlayer = Instantiate<GameObject>(networkPlayer, Vector3.zero, Quaternion.identity);
        // Initialize new remote player data.
        newPlayer.GetComponent<RemotePlayer>().id = obj.data["id"].str;
        newPlayer.GetComponent<RemotePlayer>().color = (int)obj.data["color"].n;
        newPlayer.transform.position = new Vector3(obj.data["position"]["x"].n, obj.data["position"]["y"].n, obj.data["position"]["z"].n);
        newPlayer.GetComponent<RemotePlayer>().speed = obj.data["speed"].n;
        newPlayer.GetComponent<RemotePlayer>().SetVelocity(new Vector3(obj.data["velocity"]["x"].n, obj.data["velocity"]["y"].n, obj.data["velocity"]["z"].n));

        AddPlayer(newPlayer.GetComponent<RemotePlayer>().id, newPlayer);
    }

    // Adds either a Network Player or the Local Player.
    public void AddPlayer(string id, GameObject player)
    {
        players.Add(id, player);
    }

    // Removes a Network Player.
    public void RemovePlayer(string id)
    {
        var player = players[id];

        Debug.Log("Removing Player " + id);

        players.Remove(id);                  // remove player from list.
        GameObject.Destroy(player);          // remove avatar.
    }

    // Update the Network Player's avatar on this Client.
    // The data packet uses the Client player data package structure.
    public void UpdateMotion(SocketIOEvent obj)
    {
        GameObject remotePlayer = players[obj.data["id"].str];
        Vector3 newPosition = new Vector3(obj.data["p"]["x"].n, obj.data["p"]["y"].n, obj.data["p"]["z"].n);
        remotePlayer.transform.position = newPosition;
    }

    // Update the Network Player's avatar position on this Client.
    // The data packet uses the Client player data package structure.
    public void UpdatePosition(SocketIOEvent obj)
    {
        GameObject remotePlayer = players[obj.data["id"].str];
        Vector3 newPosition = new Vector3(obj.data["p"]["x"].n, obj.data["p"]["y"].n, obj.data["p"]["z"].n);
        remotePlayer.transform.position = newPosition;
    }
}
