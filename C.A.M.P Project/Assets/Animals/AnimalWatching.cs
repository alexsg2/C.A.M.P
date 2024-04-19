using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalWatching : MonoBehaviour
{
    // Set in the Unity Editor (in the end, should be set to 0 = not watching)
    [Header("Watching")]
    public float watching = 0f; // 0f = not watching, 1f = watching

    /*
     * Update is called once per frame (for now).
     * It will display the animal's name and color to the Debug console if
     * the watching value is 1f. It will then reset the watching value to 0f.
     */
    private void Update()
    {
        if (watching == 1f)
        {
            // Display the animal's name and color to the UI
            // For now, it prints to the debug log
            Debug.Log("Animal: " + GetAnimalName());
        }

        // Reset the watching value to 0f (not wathcing)
        watching = 0f;
    }

    /*
     * Get the full animal name:
     *
     * Rabbits:
     *   Desert Cottontail Rabbit
     *   California White Rabbit
     *   Gray Rabbit
     *
     * Frogs:
     *   Purple/Black Toad
     *   Mountain Yellow-Legged Frog
     *   California Red-Legged Frog
     *
     * Deer:
     *   White-Tailed Deer
     *
     * Turtle:
    *    Box Turtle
     */
    private string GetAnimalName()
    {
        string animalName = gameObject.name;
        
        if (animalName.Contains("Rabbit"))
        {
            string animalColor = GetAnimalColor();
            if (animalColor.Contains("Brown"))
            {
                return "Desert Cottontail Rabbit";
            }
            else if (animalColor.Contains("White"))
            {
                return "California White Rabbit";
            }
            else
            {
                return "Gray Rabbit";
            }
        }
        else if (animalName.Contains("Frog"))
        {
            string animalColor = GetAnimalColor();
            if (animalColor.Contains("Purple"))
            {
                return "Black Toad";
            }
            else if (animalColor.Contains("Yellow"))
            {
                return "Mountain Yellow-Legged Frog";
            }
            else
            {
                return "California Red-Legged Frog";
            }
        }
        else if (animalName.Contains("Deer")) {
            return "White-Tailed Deer";
        }
        else if (animalName.Contains("Turtle")) {
            return "Box Turtle";
        }
        
        return animalName;
    }

    /*
    * Get the color of the animal:
    *
    * Rabbits:
    *   Brown Rabbit: RGBA(0.686, 0.553, 0.392, 1.000)
    *   White Rabbit: RGBA(1.000, 1.000, 1.000, 1.000)
    *   Gray Rabbit:
    *
    * Frogs:
    *   Purple Frog: RGBA(0.492, 0.138, 0.623, 1.000)
    *   Yellow Frog: RGBA(1.000, 0.891, 0.306, 1.000)
    *   Red Frog: RGBA(1.000, 0.138, 0.000, 1.000)
    */
    private string GetAnimalColor()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            string color = renderer.material.color.ToString();
            if (color.Contains("RGBA(0.686, 0.553, 0.392, 1.000)"))
            {
                return "Brown";
            }
            else if (color.Contains("RGBA(1.000, 1.000, 1.000, 1.000)"))
            {
                return "White";
            }
            else if (color.Contains("RGBA(0.492, 0.138, 0.623, 1.000)"))
            {
                return "Purple";
            }
            else if (color.Contains("RGBA(1.000, 0.891, 0.306, 1.000)"))
            {
                return "Yellow";
            }
            else if (color.Contains("RGBA(1.000, 0.138, 0.000, 1.000)"))
            {
                return "Red";
            }
        }
        return "No Color";
    }
}