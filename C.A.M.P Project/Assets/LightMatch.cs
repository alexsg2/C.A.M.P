using UnityEngine;

public class MatchLighterTrigger : MonoBehaviour
{
    private bool MatchLit = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a match GameObject tagged as "UnlitMatch"
        if (other.CompareTag("UnlitMatch"))
        {
            // Capture the unlit match GameObject
            GameObject unlitMatch = other.gameObject;
            if (unlitMatch != null)
            {
                // Find the fire GameObject which is a child of the unlit match
                GameObject fire = FindChildWithTag(unlitMatch.transform, "Fire");
                if (fire != null)
                {
                    // Set the fire GameObject active
                    fire.SetActive(true);
                    MatchLit = true;
                }

                // Change the tag of the unlit match GameObject to "Match"
                unlitMatch.tag = "Match";
            }
        }
    }

    // Helper function to find a child GameObject with a specific tag
    private GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null; // Not found
    }

    public bool checkMatch()
    {
        return MatchLit;
    }
}
