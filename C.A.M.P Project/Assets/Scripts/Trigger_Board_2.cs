using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Board_2 : MonoBehaviour
{
    private bool board = false;
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            board = true;
            triggered = true;
        }

    }

    public bool checkBoard()
    {
        return board;
    }
}
