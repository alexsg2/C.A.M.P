using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject toActivate;

    [SerializeField] private GameObject Testing;

    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // enable the diolog
            toActivate.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Recover()
    {
        toActivate.SetActive(false);
    }
}
