using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;
using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

/// <summary>
/// Control UI elements and
/// hosting logic on Server build.
/// </summary>
public class ServerControl : MonoBehaviour
{
    public GameObject startButton;
    public GameObject stopButton;
    public GameObject ipDisplay;

    private string address;
    private ushort port;

    /// <summary>
    /// Don't automatically detect ipv4.
    /// </summary>
    public bool use_defaults;

    public void Start() {
        // Register callbacks
        NetworkManager.Singleton.OnServerStarted += OnServerStart;
        NetworkManager.Singleton.OnServerStopped += OnServerStop;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        // Get address / port we will host on
        UnityTransport tp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        address = tp.ConnectionData.Address;
        port = tp.ConnectionData.Port; // use port on network manager
        
        if (!use_defaults) {
            string lan_address = GetLocalWifiIPV4();

            if (lan_address != "") {
                address = lan_address;
            }
            tp.SetConnectionData(address, port);
        }

        Debug.Log($"Will host on {address}:{port}");

        ipDisplay.gameObject.GetComponent<TextMeshProUGUI>().text = $"Hosted on {address}:{port}";

        // Update UI
        stopButton.SetActive(false);
        startButton.SetActive(true);
    }

    /// <summary>
    /// Start the server.
    /// </summary>
    public void StartServer() {
        NetworkManager.Singleton.StartServer();
    }

    /// <summary>
    /// Shut down the server.
    /// </summary>
    public void StopServer() {
        NetworkManager.Singleton.Shutdown();
    }

    /// <summary>
    /// Called when server is started. Update UI and debug log.
    /// </summary>
    private void OnServerStart() {
        Debug.Log($"Server started on {address}:{port}");
        if (startButton != null) {
            startButton.SetActive(false);
        }

        if (stopButton != null) {
            stopButton.SetActive(true);
        }
    }

    /// <summary>
    /// Called when server is stopped. Update UI and debug log.
    /// </summary>
    /// <param name="b"></param>
    private void OnServerStop(bool b) {
        Debug.Log("Server stopped");
        // TODO: uh what is bool param for?
        if (startButton != null) {
            startButton.SetActive(true);
        }

        if (stopButton != null) {
            stopButton.SetActive(false);
        }
    }

    /// <summary>
    /// Debug print for when a new client connects to the server.
    /// </summary>
    /// <param name="clientId"></param>
    public void OnClientConnect(ulong clientId) {
        Debug.Log($"New client with id {clientId} connected");
    }

    public void OnClientDisconnect(ulong clientId) {
        Debug.Log($"Client with id {clientId} disconnected");
    }

    /// <summary>
    /// Get the LAN wireless IPv4 address of this machine.
    /// 
    /// Returns "" if we can't find it.
    /// </summary>
    private string GetLocalWifiIPV4() {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces().Where(x => 
            x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && 
            x.OperationalStatus == OperationalStatus.Up)
            .ToArray();

        foreach (NetworkInterface i in interfaces) {
            foreach (UnicastIPAddressInformation ip in i.GetIPProperties().UnicastAddresses) {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.Address.ToString();
                }
            }
        }

        return "";

    }
}
