using UnityEngine;
using UnityEngine.UI;

public class Skip_Tutorial : MonoBehaviour
{
    private bool skipped = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button button = GetComponent<Button>();

        // Add a listener for when the button is clicked and call the OnButtonClicked method
        button.onClick.AddListener(OnButtonClicked);
    }

    // Method called when the button is clicked
    void OnButtonClicked()
    {
        Debug.Log("Clicked");
        skipped = true;
    }

    // Method to check if the tutorial has been skipped
    public bool IsSkipped()
    {
        return skipped;
    }
}
