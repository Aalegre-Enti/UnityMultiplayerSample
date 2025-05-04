using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.VersionControl;
using Unity.Services.Lobbies.Models;
using System;

public class CharacterMover : NetworkBehaviour
{
    public GameObject spawnObject;
    public float speed;
    float horizontal;
    float vertical;
    private void Start()
    {
        if (!IsOwner)
        {
            this.enabled = false;
            //Destroy(GetComponent<Rigidbody>());
        }
        RelayManager.players.Add(this);
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal * Time.deltaTime * speed, 0, vertical * Time.deltaTime * speed);
        transform.position += dir;

        if (Input.GetButtonDown("Jump"))
        {
            SpawnObjectServerRPC();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            string message = "Test";
            Debug.Log("You sent: " + message);
            SendMessageServerRpc(message, OwnerClientId);
        }
    }

    [ServerRpc]
    void SpawnObjectServerRPC()
    {
        GameObject go = Instantiate(spawnObject, transform.position, transform.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc]
    void SendMessageServerRpc(string message, ulong sender, long target = -1)
    {
        SendMessageClientRpc(message, sender, target);
    }
    [ClientRpc]
    void SendMessageClientRpc(string message, ulong sender, long target = -1)
    {
        if (target < 0)
        {
            Message(sender, message);
        }
        else
        {
            foreach (CharacterMover player in RelayManager.players)
            {
                if (player.OwnerClientId == (ulong)target)
                    player.Message(sender, "(private) " + message);
            }
        }
    }

    public void Message(ulong from, string message)
    {
        Debug.Log("Player " + from + ": " + message);
    }
}
