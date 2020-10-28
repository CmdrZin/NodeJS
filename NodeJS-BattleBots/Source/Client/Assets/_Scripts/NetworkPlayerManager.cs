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
    public void SpawnPlayer(SocketIOEvent obj)
    {
        Debug.Log("Spawn this player: " + obj.data);

        GameObject newPlayer = Instantiate<GameObject>(networkPlayer, Vector3.zero, Quaternion.identity);
        // Initialize new remote player data.
        newPlayer.GetComponent<RemotePlayer>().id = obj.data["id"].str;
        newPlayer.GetComponent<RemotePlayer>().color = (int)obj.data["color"].n;
        newPlayer.transform.position = new Vector3(obj.data["position"]["x"].n, 0.0f, obj.data["position"]["y"].n);
        newPlayer.GetComponent<RemotePlayer>().SetForce(new Vector2(obj.data["position"]["x"].n, obj.data["position"]["y"].n));
        newPlayer.GetComponent<RemotePlayer>().speed = obj.data["speed"].n;

        AddPlayer(newPlayer);
    }

    // Adds either a Network Player or the Local Player.
    public void AddPlayer(GameObject player)
    {
        players.Add(player.GetComponent<RemotePlayer>().id, player);
        Debug.Log("Player " + player.GetComponent<RemotePlayer>().id + " added");
    }

}
