using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleAppear1 : MonoBehaviour
{
    public GameObject pole1; // reference to standing pole1
    private bool triggered = false; // flag to track if trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pole") && !triggered)
        {
            Destroy(other.gameObject);
            pole1.SetActive(true);
            triggered = true; // set the flag to true
        }
    }
}
