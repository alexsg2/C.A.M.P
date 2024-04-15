using UnityEngine;

public class MatchLighter : MonoBehaviour
{
    public GameObject litMatchPrefab; // Reference to the lit match prefab

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

            // Destroy the unlit match GameObject
            Destroy(other.gameObject);

            // Instantiate the lit match GameObject
            GameObject litMatch = Instantiate(litMatchPrefab, matchPosition, Quaternion.identity);

            // Set the instantiated lit match to active
            litMatch.SetActive(true);
        }
    }
}