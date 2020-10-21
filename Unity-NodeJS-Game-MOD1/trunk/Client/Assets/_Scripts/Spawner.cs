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

    public Material defaultColor;
    public Material redColor;
    public Material blueColor;
    public Material yellowColor;

    // TODO: Add color parameter to be passed to AddPlayer().
    public GameObject SpawnPlayer(string id, int color)
    {
        var player = Instantiate(networkPlayer, Vector3.zero, Quaternion.identity) as GameObject;
        player.GetComponent<ClickToFollow>().currentPlayer = currentPlayer;
        player.GetComponent<NetworkEntity>().id = id;
        AddPlayer(id, player, color);
        return player;
    }

    public GameObject GetPlayer(string id)
    {
        return players[id];
    }

    // TODO: Add color parameter to replace Login.skinColor to support setting other avatar's color.
    public void AddPlayer(string id, GameObject player, int color)
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
            // Change the color of the Avatar based on Login.skinColor.
            // try this first. Change namePlate color. works.
            switch (color)
            {
                case 0:     // default Orange
                    textMesh.color = new Color(0.96f, 0.5f, 0.05f, 1.0f);
                    break;

                case 1:     // Blue
                    textMesh.color = Color.blue;
                    break;

                case 2:     // Yellow
                    textMesh.color = Color.yellow;
                    break;

                case 3:     // Red
                    textMesh.color = Color.red;
                    break;

                default:     // default Brown
                    textMesh.color = new Color(0.96f, 0.5f, 0.05f, 1.0f);
                    break;
            }
            // Made new Materials for colors.
            switch (color)
            {
                case 0:         // default. no change.
                    break;

                case 1:         // use Blue
                    player.GetComponentInChildren<SkinnedMeshRenderer>().material = blueColor;
                    break;

                case 2:         // use Yellow
                    player.GetComponentInChildren<SkinnedMeshRenderer>().material = yellowColor;
                    break;

                case 3:         // use Red
                    player.GetComponentInChildren<SkinnedMeshRenderer>().material = redColor;
                    break;

                default:
                    break;
            }
        }
        // Now set the new Player object as its parent.
        namePlate.transform.parent = player.transform;

        // Add Player to the Dictionary list of players indexed by their network ID.
        players.Add(id, player);
    }

    public void Remove(string id)
    {
        var player = players[id];
        Destroy(player);
        players.Remove(id);
    }
}
