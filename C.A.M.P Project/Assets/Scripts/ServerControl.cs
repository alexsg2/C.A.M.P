using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Control UI Elements on server build.
/// </summary>
public class ServerControl : MonoBehaviour
{
    public GameObject startButton;
    public GameObject stopButton;

    public void Start() {
        NetworkManager.Singleton.OnServerStarted += OnServerStart;
        NetworkManager.Singleton.OnServerStopped += OnServerStop;
    }

    public void StartServer() {
        NetworkManager.Singleton.StartServer();
    }

    public void StopServer() {
        NetworkManager.Singleton.Shutdown();
    }

    // Disable startbutton enable stopbutton
    public void OnServerStart() {
        if (startButton != null) {
            startButton.SetActive(false);
        }

        if (stopButton != null) {
            stopButton.SetActive(true);
        }
    }

    // Disable stopbutton enable startbutton
    public void OnServerStop(bool b) {
        // TODO: uh what is bool param for?
        if (startButton != null) {
            startButton.SetActive(true);
        }

        if (stopButton != null) {
            stopButton.SetActive(false);
        }
    }
}
