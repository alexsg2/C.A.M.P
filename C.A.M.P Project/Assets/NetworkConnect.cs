using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkConnect : MonoBehaviour
{
    public GameObject startButtons;
    public GameObject stopButton;
    public void CreateHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
        startButtons.SetActive(false);
    }

    public void StartServer() {
        NetworkManager.Singleton.StartServer();
        startButtons.SetActive(false);
        stopButton.SetActive(true);
    }

    public void StopServer() {
        NetworkManager.Singleton.Shutdown();
        startButtons.SetActive(true);
        stopButton.SetActive(false);
    }
}
