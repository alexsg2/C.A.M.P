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
            // Capture the position and rotation of the match
            Vector3 matchPosition = other.transform.position;
            Quaternion matchRotation = other.transform.rotation;

            // Light up the match
            LightMatch();

            // Destroy the unlit match GameObject
            Destroy(other.gameObject);

            // Instantiate the lit match GameObject at the captured position and rotation
            GameObject litMatch = Instantiate(litMatchPrefab, matchPosition, matchRotation);

            // If the player's hand transform is specified (optional), make the lit match a child of the hand
            if (handTransform != null)
            {
                litMatch.transform.SetParent(handTransform);
            }
        }
    }

    private void LightMatch()
    {
        // Implement your match lighting logic here
        Debug.Log("Match is lit!");
    }
}