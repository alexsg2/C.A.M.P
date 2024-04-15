using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
// using UnityEngine.SceneManagement;

public class JoinServerButton : MonoBehaviour
{
    public void JoinServer()
    {
        NetworkManager.Singleton.StartClient();
        // SceneManager.LoadScene("C.A.M.P Environment", LoadSceneMode.Single);
    }
}
