using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public int twigsRequired = 3; // Number of twigs required
    public int matchesRequired = 1; // Number of matches required
    public GameObject campfirePrefab; // Campfire prefab to instantiate

    private int twigsCount = 0;
    private int matchesCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Firewood"))
        {
            twigsCount++;
            Debug.Log("Twigs count: " + twigsCount);
            CheckFirewood();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Firewood"))
        {
            twigsCount--;
            Debug.Log("Twigs count: " + twigsCount);
        }
    }

    private void CheckFirewood()
    {
        if (twigsCount >= twigsRequired && matchesCount >= matchesRequired)
        {
            Debug.Log("Enough firewood and matches. Creating campfire.");
            // Destroy all firewood objects in the trigger zone
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Firewood"))
                {
                    Destroy(collider.gameObject);
                }
            }

            Vector3 newPosition = new Vector3(5.068001f, 0f, 6.679f);
            Quaternion noRotation = Quaternion.identity;
            campfirePrefab = Resources.Load<GameObject>("Imports/Low Poly Fire/Prefabs/Yellow-FireWood.prefab");

            Instantiate(campfirePrefab, newPosition, noRotation);

            // Reset counts
            twigsCount = 0;
            matchesCount = 0;
        }
        else
        {
            Debug.Log("Not enough firewood or matches yet.");
        }
    }
}
