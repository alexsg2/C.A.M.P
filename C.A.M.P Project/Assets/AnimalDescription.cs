using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

public class AnimalDescription : MonoBehaviour
{
    public Text canvasText;

    private void Start()
    {
        var interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
        interactable.selectEntered.AddListener(OnSelectEnter);
        interactable.selectExited.AddListener(OnSelectExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        // Debug.Log("OnHover" + args.interactable.gameObject);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        // Debug.Log("ExitHover" + args.interactable.gameObject);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        // Debug.Log("OnSelect" + args.interactable.gameObject);
        canvasText.text = GetAnimalName();

    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        // Debug.Log("ExitSelect" + args.interactable.gameObject);
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        canvasText.text = "";
    }
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
        else if (animalName.Contains("Deer"))
        {
            return "White-Tailed Deer";
        }
        else if (animalName.Contains("Turtle"))
        {
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