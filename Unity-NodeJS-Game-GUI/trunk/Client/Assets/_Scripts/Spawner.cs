using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;
using UnityEngine.UI; // Required when Using UI elements.

public class Spawner : MonoBehaviour
{

    public GameObject networkPlayer;
    public GameObject currentPlayer;
    public SocketIOComponent socket;
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    private GameObject namePlate;
    public Text nameBox;

    public GameObject SpawnPlayer(string id)
    {
        var player = Instantiate(networkPlayer, Vector3.zero, Quaternion.identity) as GameObject;
        player.GetComponent<ClickToFollow>().currentPlayer = currentPlayer;
        player.GetComponent<NetworkEntity>().id = id;
        AddPlayer(id, player);
        return player;
    }

    public GameObject GetPlayer(string id)
    {
        return players[id];
    }

    public void AddPlayer(string id, GameObject player)
    {
        // Add namePlate here after instaniation.
        namePlate = new GameObject("NamePlate");
        namePlate.AddComponent<TextMesh>();            // To display name.
        // Adjust position and settings.
        TextMesh textMesh = namePlate.GetComponent<TextMesh>();
        if (textMesh != null)
        {
            Debug.Log("Adding Nameplate");
            textMesh.transform.position = player.transform.position + new Vector3(0, 1.3f, 0);  // elevate y
            textMesh.transform.rotation = player.transform.rotation * new Quaternion(0, 180.0f, 0, 0);  // turn it around
            textMesh.characterSize = 0.2f;
            textMesh.fontSize = 10;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = Color.white;
            textMesh.text = id;     // Set the name text to the network id string.
            // Set UI name also
            nameBox.text = id;
        }
        // Now set the new Player object as its parent.
        namePlate.transform.parent = player.transform;

        players.Add(id, player);
    }
    public void Remove(string id)
    {
        var player = players[id];
        Destroy(player);
        players.Remove(id);
    }
}
