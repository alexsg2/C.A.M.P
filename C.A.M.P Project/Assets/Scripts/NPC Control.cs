using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class NPCControl : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject toActivate;

    private Transform avatar;


    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            await Task.Delay(50);

            // enable the diolog
            toActivate.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Recover()
    {
        // avatar.GetComponent<PlayerInput>().enabled = true;
        toActivate.SetActive(false);
    }
}
