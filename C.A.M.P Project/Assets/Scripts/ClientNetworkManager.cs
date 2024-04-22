using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

// Attach to NeworkManager in join scene. Makes sure
// We don't have more than one manager when reloading
// join scene. Also handles disconnect.
public class ClientNetworkManager : MonoBehaviour
{
    private int joinSceneBuildIndex;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: hope this is only called once, not when we go across scenes
        NetworkManager mine = GetComponent<NetworkManager>();

        if (mine != NetworkManager.Singleton) {
            Debug.Log("Destroying NetworkManager clone");
            Destroy(gameObject);
        }
        else {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            joinSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        }
    }

    public void OnClientDisconnect(ulong clientId) {
        if (clientId != NetworkManager.Singleton.LocalClientId) {
            return;
        }
        Debug.Log("Handling server disconnect");
        // shutdown client
        NetworkManager.Singleton.Shutdown();
        // load join scene
        SceneManager.LoadScene(joinSceneBuildIndex, LoadSceneMode.Single);
    }
}
