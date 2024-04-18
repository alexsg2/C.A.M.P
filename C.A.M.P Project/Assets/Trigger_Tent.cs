using System.Collections.Generic;
using UnityEngine;

public class TentTriggerZone : MonoBehaviour
{
    public GameObject tentPrefab; // Tent prefab to toggle visibility
    private bool requirementsMet = false;
    private bool poleScript1Executed = false; // Flag to track if PoleAppear1 script has executed
    private bool poleScript2Executed = false; // Flag to track if PoleAppear2 script has executed
    private bool nail1Executed = false; // Flag to track if Nail1 script has executed
    private bool nail2Executed = false; // Flag to track if Nail2 script has executed
    private bool nail3Executed = false; // Flag to track if Nail3 script has executed
    private bool nail4Executed = false; // Flag to track if Nail4 script has executed
    bool allNailsExecuted;
    private bool tarp1Executed = false; // Flag to track if Tarp1 script has executed
    private bool tarp2Executed = false; // Flag to track if Tarp2 script has executed
    private bool tentBuilt = false; // Track if the tent has been built

    private List<Collider> collidersInZone = new List<Collider>();


    // private void OnTriggerEnter(Collider other)
    // {
    //     if (!tentBuilt)
    //     {
    //         CheckRequirements();
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (!tentBuilt)
    //     {
    //         CheckRequirements();
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        // Add the collider to the list if it's not already in it
        // if (!collidersInZone.Contains(other))
        // {
            collidersInZone.Add(other);
            foreach (Collider collider in collidersInZone)
            {
                Debug.Log(collider.tag);
            }
            // CheckRequirements();
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the collider from the list when it exits the trigger zone
        // if (collidersInZone.Contains(other))
        // {
            collidersInZone.Remove(other);
            // CheckRequirements();
        // }
    }

    private void CheckRequirements()
    {
        bool allPolesExecuted = poleScript1Executed && poleScript2Executed;
        bool allNailsExecuted = nail1Executed && nail2Executed && nail3Executed && nail4Executed;
        bool allTarpsExecuted = tarp1Executed && tarp2Executed;

        // if (allPolesExecuted && allNailsExecuted && allTarpsExecuted && !requirementsMet)
        if (allPolesExecuted && allNailsExecuted && allTarpsExecuted)
        {
            // Debug.Log("Enough materials. Tent built.");
            requirementsMet = true;
            tentBuilt = true;

            // Set the tent prefab active
            tentPrefab.SetActive(true);

            // Destroy specified material objects in the trigger zone
            // Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale);
            // Debug.Log("Number of colliders found: " + colliders.Length);
            // foreach (Collider collider in colliders)
            // {
            //     Debug.Log($"Collider tag: {collider.tag}");
            //     colliderTags += "tag: " + collider.tag + "  ";
                // if (collider.CompareTag("Nail") || collider.CompareTag("Pole") || collider.CompareTag("Tarp"))
                // {
                //     Destroy(collider.gameObject);
                //     Debug.Log("Destroyed: " + collider.gameObject);
                // }
            // }
            foreach (Collider collider in collidersInZone)
            {
                Debug.Log(collider.tag);
                bool check1 = (collider.CompareTag("Nail") || collider.CompareTag("Pole") || collider.CompareTag("Tarp"));
                Debug.Log("Check" + check1);
                if (collider.CompareTag("Nail") || collider.CompareTag("Pole") || collider.CompareTag("Tarp"))
                {
                    Debug.Log("Inside: " + collider.tag);
                    Destroy(collider.gameObject);
                    Debug.Log("Destroyed: " + collider.gameObject);
                }
            }
        }
        else
        {
            // Debug.Log("Not all requirements done. Tent not built.");
            requirementsMet = false;

            // Set the tent prefab inactive
            tentPrefab.SetActive(false);
        }
    }

    // Method to be called when the PoleAppear1 script executes
    public void PoleScript1Executed()
    {
        poleScript1Executed = true;
    }

    public void PoleScript2Executed()
    {
        poleScript2Executed = true;
    }

    public void Nail1Executed()
    {
        nail1Executed = true;
        bool allNailsExecuted = nail1Executed && nail2Executed && nail3Executed && nail4Executed;
        if (allNailsExecuted)
        {
            CheckRequirements();
        }
    }

    public void Nail2Executed()
    {
        nail2Executed = true;
        bool allNailsExecuted = nail1Executed && nail2Executed && nail3Executed && nail4Executed;
        if (allNailsExecuted)
        {
            CheckRequirements();
        }
    }

    public void Nail3Executed()
    {
        nail3Executed = true;
        bool allNailsExecuted = nail1Executed && nail2Executed && nail3Executed && nail4Executed;
        if (allNailsExecuted)
        {
            CheckRequirements();
        }
    }

    public void Nail4Executed()
    {
        nail4Executed = true;
        bool allNailsExecuted = nail1Executed && nail2Executed && nail3Executed && nail4Executed;
        if (allNailsExecuted)
        {
            CheckRequirements();
        }
    }

    public void Tarp1Executed()
    {
        tarp1Executed = true;
    }

    public void Tarp2Executed()
    {
        tarp2Executed = true;
    }

    public bool checkPole()
    {
        return (poleScript1Executed && poleScript2Executed);
    }

    public bool checkTarp()
    {
        return (tarp1Executed && tarp2Executed);
    }
}
