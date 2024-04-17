using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarpAppear2 : MonoBehaviour
{
    public GameObject tarp2; // reference to standing pole1
    private bool triggered = false; // flag to track if trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tarp") && !triggered)
        {
            Destroy(other.gameObject);
            tarp2.SetActive(true);
            triggered = true; // set the flag to true
        }
    }
}
