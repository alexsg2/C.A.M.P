using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Throw_From : MonoBehaviour
{
    private bool playerinside = false;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinside = false;
        }
    }

    public bool checkPlayerInside()
    {
        return playerinside;
    }
}
