using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Throw_To : MonoBehaviour
{
    private bool boxinside = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.CompareTag("Box"))
        {
            boxinside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        if (other.CompareTag("Box"))
        {
            boxinside = false;
        }
    }

    public bool checkBoxInside()
    {
        return boxinside;
    }
}
