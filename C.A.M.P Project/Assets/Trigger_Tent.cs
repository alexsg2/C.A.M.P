using UnityEngine;

public class TentTriggerZone : MonoBehaviour
{
    public int nailsRequired = 4; // Number of nails required
    public int polesRequired = 2; // Number of poles required
    public int tarpRequired = 1; // Number of tarps required
    public GameObject tentPrefab; // Tent prefab to toggle visibility

    private int nailCount = 0;
    private int poleCount = 0;
    private int tarpCount = 0;
    private bool requirementsMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nail") && !requirementsMet)
        {
            nailCount++;
            Debug.Log("Nail count: " + nailCount);
            CheckRequirements();
        }
        else if (other.CompareTag("Pole") && !requirementsMet)
        {
            poleCount++;
            Debug.Log("Pole count: " + poleCount);
            CheckRequirements();
        }
        else if (other.CompareTag("Tarp") && !requirementsMet)
        {
            tarpCount++;
            Debug.Log("Tarp count: " + tarpCount);
            CheckRequirements();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Nail") && !requirementsMet)
        {
            nailCount--;
            Debug.Log("Nail count: " + nailCount);
            CheckRequirements();
        }
        else if (other.CompareTag("Pole") && !requirementsMet)
        {
            poleCount--;
            Debug.Log("Pole count: " + poleCount);
            CheckRequirements();
        }
        else if (other.CompareTag("Tarp") && !requirementsMet)
        {
            tarpCount--;
            Debug.Log("Tarp count: " + tarpCount);
            CheckRequirements();
        }
    }

    private void CheckRequirements()
    {
        if (nailCount >= nailsRequired && poleCount >= polesRequired && tarpCount >= tarpRequired && !requirementsMet)
        {
            Debug.Log("Enough materials. Tent built.");
            requirementsMet = true;

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
            Debug.Log("Not enough materials. Tent not built.");
            requirementsMet = false;

            // Set the tent prefab inactive
            tentPrefab.SetActive(false);
        }
    }
}