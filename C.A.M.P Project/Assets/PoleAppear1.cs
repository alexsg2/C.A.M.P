using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleAppear1 : MonoBehaviour
{
    public GameObject pole1; // reference to standing pole1

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pole"))
        {
            Destroy(other.gameObject);
            pole1.SetActive(true);
        }
    }
}
