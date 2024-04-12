using UnityEngine;

public class MatchLighter : MonoBehaviour
{
    public GameObject litMatchPrefab; // Reference to the lit match prefab
    public Transform handTransform; // Reference to the player's hand transform (optional)

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a match GameObject tagged as "UnlitMatch"
        if (other.CompareTag("UnlitMatch"))
        {
            // Light up the match

            // Implement your match lighting logic here
            Debug.Log("Match is lit!");
            // Destroy the unlit match GameObject
            Destroy(other.gameObject);

            // Capture the position and rotation of the match
            Vector3 matchPosition = other.transform.position;
            // Quaternion matchRotation = other.transform.rotation;

            // Instantiate the lit match GameObject at the captured position and rotation
            // GameObject litMatch = Instantiate(litMatchPrefab, matchPosition, matchRotation);
            litMatchPrefab.SetActive(true);
            litMatchPrefab.transform.position = matchPosition;

            // If the player's hand transform is specified (optional), make the lit match a child of the hand
            if (handTransform != null)
            {
                litMatchPrefab.transform.SetParent(handTransform);
            }
        }
    }
}