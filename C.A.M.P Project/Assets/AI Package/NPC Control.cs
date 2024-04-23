using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class NPCControl : MonoBehaviour
{
    [SerializeField] private GameObject toActivate;
    [SerializeField] private Moving npcMovementScript;  // Reference to the NPC's Moving script

    private Transform playerTransform;  // To store the player's transform

    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;  // Store the player's transform for rotation

            await Task.Delay(50);

            // enable the diolog
            toActivate.SetActive(true);

            // Stop NPC and make it face the player
            npcMovementScript.StopAndFacePlayer(playerTransform.position);

            // Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Recover()
    {
        // avatar.GetComponent<PlayerInput>().enabled = true;
        toActivate.SetActive(false);
        
        // Optionally, let the NPC resume normal behavior
        npcMovementScript.ResumeNormalBehavior();
    }
}
