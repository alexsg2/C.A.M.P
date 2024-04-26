using UnityEngine;

public class TentTriggerZone : MonoBehaviour
{
    public GameObject tentPrefab; // Tent prefab to toggle visibility
    public GameObject tarp1;
    public GameObject tarp2;
    public GameObject pole1;
    public GameObject pole2;
    public GameObject nail1;
    public GameObject nail2;
    public GameObject nail3;
    public GameObject nail4;
    private bool requirementsMet = false;
    private bool poleScript1Executed = false; // Flag to track if PoleAppear1 script has executed
    private bool poleScript2Executed = false; // Flag to track if PoleAppear2 script has executed
    private bool nail1Executed = false; // Flag to track if Nail1 script has executed
    private bool nail2Executed = false; // Flag to track if Nail2 script has executed
    private bool nail3Executed = false; // Flag to track if Nail3 script has executed
    private bool nail4Executed = false; // Flag to track if Nail4 script has executed
    private bool allNailsExecuted;
    private bool tarp1Executed = false; // Flag to track if Tarp1 script has executed
    private bool tarp2Executed = false; // Flag to track if Tarp2 script has executed
    private bool tentBuilt = false; // Track if the tent has been built

    private void CheckRequirements()
    {
        bool allPolesExecuted = poleScript1Executed && poleScript2Executed;
        allNailsExecuted = nail1Executed && nail2Executed && nail3Executed && nail4Executed;
        bool allTarpsExecuted = tarp1Executed && tarp2Executed;

        if (allPolesExecuted && allNailsExecuted && allTarpsExecuted)
        {
            tentBuilt = true;
            tentPrefab.SetActive(true);

            // Code to get all colliders in the trigger zone
            // Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale);
            // string colliderNames = "Collider names: ";
            // foreach (Collider collider in colliders)
            // {
            //     colliderNames += collider.name + ", "; // Concatenate the name and a comma to the colliderNames string
            // }
            // Debug.Log(colliderNames);

            Destroy(tarp1.gameObject);
            Destroy(tarp2.gameObject);
            Destroy(pole1.gameObject);
            Destroy(pole2.gameObject);
            Destroy(nail1.gameObject);
            Destroy(nail2.gameObject);
            Destroy(nail3.gameObject);
            Destroy(nail4.gameObject);
        }
        else
        {
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

    public bool checkNail()
    {
        return (nail1Executed && nail2Executed && nail3Executed && nail4Executed);
    }
}
