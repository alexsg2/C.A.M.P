using UnityEngine;

public class FireTriggerZone : MonoBehaviour
{
    public int twigsRequired = 3; // Number of twigs required
    public GameObject logstack; // Logs to toggle visibility
    public GameObject fire;
    public GameObject fireLight;

    private int twigsCount = 0;
    private bool requirementsMet = false;
    private bool logStackThere = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Firewood") && !requirementsMet)
        {
            twigsCount++;
            Debug.Log("Twigs count: " + twigsCount);
            CheckRequirements();
        }
        else if (other.CompareTag("Match") && logStackThere)
        {
            MakeFire();
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
            logstack.SetActive(true);
            logStackThere = true;

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
            logstack.SetActive(false);
        }
    }

    private void MakeFire()
    {
        fire.SetActive(true);
        fireLight.SetActive(true);

        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Match"))
            {
                Destroy(collider.gameObject);
            }
        }
    }
}