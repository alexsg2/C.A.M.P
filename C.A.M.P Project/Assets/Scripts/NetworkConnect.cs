using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("C.A.M.P Environment", LoadSceneMode.Single);
    }

    public void StartServer() {
        NetworkManager.Singleton.StartServer();
        if (startButtons != null) {
            startButtons.SetActive(false);
        }

        if (stopButton != null) {
            stopButton.SetActive(true);
        }
    }

    public void StopServer() {
        NetworkManager.Singleton.Shutdown();
        if (startButtons != null) {
            startButtons.SetActive(true);
        }

        if (stopButton != null) {
            stopButton.SetActive(false);
        }
    }
}
