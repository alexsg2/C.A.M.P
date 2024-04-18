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

/// <summary>
/// Control UI elements and
/// hosting logic on Server build.
/// </summary>
public class ServerControl : MonoBehaviour
{
    public GameObject startButton;
    public GameObject stopButton;
    public GameObject ipDisplay;

    public void Start() {
        // Register
        NetworkManager.Singleton.OnServerStarted += OnServerStart;
        NetworkManager.Singleton.OnServerStopped += OnServerStop;

        UnityTransport tp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        string lan_address = GetLocalWifiIPV4();
        string address = tp.ConnectionData.Address;
        ushort port = tp.ConnectionData.Port; // use port on network manager

        if (lan_address != "") {
            address = lan_address;
        }

        tp.SetConnectionData(address, port);

        Debug.Log($"Will host on {address}:{port}");

        ipDisplay.gameObject.GetComponent<TextMeshProUGUI>().text = $"Hosted on {address}:{port}";
        stopButton.SetActive(false);
        startButton.SetActive(true);
    }

    public void StartServer() {
        NetworkManager.Singleton.StartServer();
    }

    public void StopServer() {
        NetworkManager.Singleton.Shutdown();
    }

    // Disable startbutton enable stopbutton
    private void OnServerStart() {
        if (startButton != null) {
            startButton.SetActive(false);
        }

        if (stopButton != null) {
            stopButton.SetActive(true);
        }
    }

    // Disable stopbutton enable startbutton
    private void OnServerStop(bool b) {
        // TODO: uh what is bool param for?
        if (startButton != null) {
            startButton.SetActive(true);
        }

        if (stopButton != null) {
            stopButton.SetActive(false);
        }
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
