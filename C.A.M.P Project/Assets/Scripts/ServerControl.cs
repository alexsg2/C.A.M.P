using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerControl : MonoBehaviour
{
    public GameObject startButton;
    public GameObject stopButton;

    public void StartServer() {
        NetworkManager.Singleton.StartServer();
        if (startButton != null) {
            startButton.SetActive(false);
        }

        if (stopButton != null) {
            stopButton.SetActive(true);
        }
    }

    public void StopServer() {
        NetworkManager.Singleton.Shutdown();
        if (startButton != null) {
            startButton.SetActive(true);
        }

        if (stopButton != null) {
            stopButton.SetActive(false);
        }
    }
}
