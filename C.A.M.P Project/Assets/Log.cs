using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Log : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Singleton.IsConnectedClient) {
            NetworkLog.LogInfoServer("HELLO!!!!");
        }
    }
}
