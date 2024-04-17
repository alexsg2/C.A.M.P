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
    private bool tarp1Executed = false; // Flag to track if Tarp1 script has executed
    private bool tarp2Executed = false; // Flag to track if Tarp2 script has executed

    private bool tentBuilt = false; // Track if the tent has been built


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

    private void CheckRequirements()
    {
        bool allPolesExecuted = poleScript1Executed && poleScript2Executed;
        bool allNailsExecuted = nail1Executed && nail2Executed && nail3Executed && nail4Executed;
        bool allTarpsExecuted = tarp1Executed && tarp2Executed;

        Debug.Log("Poles: " + allPolesExecuted);
        Debug.Log("Nails: " + allNailsExecuted);
        Debug.Log("Tarps: " + allTarpsExecuted);
        Debug.Log("Requirements: " + requirementsMet);
        if (allPolesExecuted && allNailsExecuted && allTarpsExecuted && !requirementsMet)
        {
            Debug.Log("Enough materials. Tent built.");
            requirementsMet = true;
            tentBuilt = true;

            // Set the tent prefab active
            tentPrefab.SetActive(true);

            // Destroy specified material objects in the trigger zone
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Nail") || collider.CompareTag("Pole") || collider.CompareTag("Tarp"))
                {
                    Destroy(collider.gameObject);
                    Debug.Log("Destroyed: " + collider.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("Not all requirements done. Tent not built.");
            requirementsMet = false;

            // Set the tent prefab inactive
            tentPrefab.SetActive(false);
        }
        if (allNailsExecuted)
        {
            Debug.Log("Nails Good.");
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
        CheckRequirements();
    }

    public void Nail2Executed()
    {
        Debug.Log("Nail2 Good.");
        nail2Executed = true;
        CheckRequirements();
    }

    public void Nail3Executed()
    {
        Debug.Log("Nail3 Good.");
        nail3Executed = true;
        CheckRequirements();
    }

    public void Nail4Executed()
    {
        Debug.Log("Nail4 Good.");
        nail4Executed = true;
        CheckRequirements();
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
