using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public int twigsRequired = 3; // Number of twigs required
    public GameObject campfirePrefab; // Campfire prefab to toggle visibility

    private int twigsCount = 0;
    private bool requirementsMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Firewood") && !requirementsMet)
        {
            twigsCount++;
            Debug.Log("Twigs count: " + twigsCount);
            CheckRequirements();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Firewood") && !requirementsMet)
        {
            twigsCount--;
            Debug.Log("Twigs count: " + twigsCount);
            CheckRequirements();
        }
    }

    private void CheckRequirements()
    {
        if (twigsCount >= twigsRequired && !requirementsMet)
        {
            Debug.Log("Enough firewood. Campfire activated.");
            requirementsMet = true;

            // Set the campfire prefab active
            campfirePrefab.SetActive(true);

            // Destroy all firewood objects in the trigger zone
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Firewood"))
                {
                    Destroy(collider.gameObject);
                }
            }
        }
        else if (twigsCount < twigsRequired && requirementsMet)
        {
            Debug.Log("Not enough firewood. Campfire deactivated.");
            requirementsMet = false;

            // Set the campfire prefab inactive
            campfirePrefab.SetActive(false);
        }
    }
}