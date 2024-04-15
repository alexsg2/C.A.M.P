using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleAppear2 : MonoBehaviour
{
    public GameObject pole2; // reference to standing pole1

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pole"))
        {
            Destroy(other.gameObject);
            pole2.SetActive(true);
        }
    }
}
