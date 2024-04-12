using UnityEngine;

public class MatchLighter : MonoBehaviour
{
    public string unlitMatchTag = "UnlitMatch"; // Tag of the match before it's lit
    public string litMatchTag = "Match"; // Tag of the match after it's lit

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the match GameObject
        if (other.CompareTag(unlitMatchTag))
        {
            Debug.Log("Box triggered collision with unlit match.");

            // Light up the match (replace this with your actual logic)
            LightMatch(other.gameObject);

            // Change the tag of the match to "Match"
            Debug.Log("Changing tag of match to 'Match'.");
            other.tag = litMatchTag;
        }
    }

    private void LightMatch(GameObject match)
    {
        // Implement your match lighting logic here
        Debug.Log("Match is lit!");
    }
}