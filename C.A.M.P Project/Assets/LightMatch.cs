using UnityEngine;

public class MatchLighter : MonoBehaviour
{
    public GameObject litMatchPrefab; // Reference to the lit match prefab

    private Transform handTransform; // Reference to the hand transform where the unlit match was held

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a match GameObject tagged as "UnlitMatch"
        if (other.CompareTag("UnlitMatch"))
        {
            // Light up the match
            // Implement your match lighting logic here
            Debug.Log("Match is lit!");

            // Capture the position and rotation of the match
            Vector3 matchPosition = other.transform.position;

            // Get the parent of the unlit match to determine which hand it was in
            if (other.transform.parent != null)
            {
                handTransform = other.transform.parent;
            }
            else
            {
                Debug.LogWarning("Unlit match was not held by a hand.");
                handTransform = null;
            }

            // Destroy the unlit match GameObject
            Destroy(other.gameObject);

            // Instantiate the lit match GameObject
            GameObject litMatch;

            if (handTransform != null)
            {
                // If the unlit match was held by a hand, instantiate the lit match as a child of that hand
                litMatch = Instantiate(litMatchPrefab, handTransform);
            }
            else
            {
                // If the unlit match wasn't held by a hand, instantiate the lit match at the unlit match's position
                litMatch = Instantiate(litMatchPrefab, matchPosition, Quaternion.identity);
            }

            // Set the instantiated lit match to active
            litMatch.SetActive(true);
        }
    }
}