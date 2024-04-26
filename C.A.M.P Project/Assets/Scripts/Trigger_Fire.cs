using UnityEngine;

public class FireTriggerZone : MonoBehaviour
{
    public GameObject logstack; // Logs to toggle visibility
    public GameObject fire;
    public GameObject fireLight;
    private int twigsCount = 0;
    private bool twigCountReached = false;
    private bool matchCountReached = false;
    private int matchCount = 0;
    private bool fireMade = false;
    private bool logStackThere = false;
    private int twigsRequired = 12; // Number of twigs required
    private int matchesRequired = 3; // Number of matches required

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Firewood") && !twigCountReached)
        {
            twigsCount++;
            Debug.Log("Twigs count: " + twigsCount);
            CheckRequirements();
        }
        else if (other.CompareTag("Match") && logStackThere && !matchCountReached)
        {
            matchCount++;
            Debug.Log("Match count: " + matchCount);
            if (twigCountReached && (matchCount == matchesRequired))
            {
                Debug.Log("Match count reached: " + matchCount);
                matchCountReached = true;
                MakeFire();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Firewood") && !twigCountReached)
        {
            twigsCount--;
            Debug.Log("Twigs count: " + twigsCount);
            CheckRequirements();
        }
    }

    private void CheckRequirements()
    {
        if (twigsCount >= twigsRequired && !twigCountReached)
        {
            Debug.Log("Enough firewood. Campfire activated.");
            twigCountReached = true;

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
        else if (twigsCount < twigsRequired && twigCountReached)
        {
            Debug.Log("Not enough firewood. Campfire deactivated.");
            twigCountReached = false;

            // Set the campfire prefab inactive
            logstack.SetActive(false);
        }
    }

    private void MakeFire()
    {
        fire.SetActive(true);
        fireLight.SetActive(true);
        fireMade = true;

        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Match"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    public bool checkSticks()
    {
        return twigCountReached;
    }

    public bool checkFire()
    {
        return fireMade;
    }
}