using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class ClientJoinCanvas : MonoBehaviour
{
    public Button joinButton;
    public Button cancelButton;

    public TextMeshProUGUI addressField;

    public void JoinServer()
    {
        NetworkManager.Singleton.StartClient();
        joinButton.interactable = false;
        cancelButton.interactable = true;
    }

    public void Cancel() {
        NetworkManager.Singleton.Shutdown();
        joinButton.interactable = true;
        cancelButton.interactable = false;
    }
}
