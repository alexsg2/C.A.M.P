using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

public class AnimalDescription : MonoBehaviour
{
    public Text canvasText;
    public TaskList taskList;
    public GameObject AnimalText;
    private bool selectedLandAnimal = false;
    private bool selectedBird = false;


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
        AnimalText.SetActive(true);

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
        AnimalText.SetActive(false);
    }
    private string GetAnimalName()
    {
        string animalName = gameObject.name;

        if (animalName.Contains("Rabbit"))
        {
            taskList.updateT3Sub1();
            string animalColor = GetAnimalColor();
            if (animalColor.Contains("Brown"))
            {
                return "The Desert Cottontail:\n\nA small rabbit species native to North America, particularly found in arid and semi-arid regions such as deserts, scrublands, and grasslands. These rabbits are characterized by their soft, sandy-brown fur with white fluffy tails resembling balls of cotton, hence the name 'cottontail.'";
            }
            else if (animalColor.Contains("White"))
            {
                return "The California rabbit:\n\nAlso known as the California white rabbit, is a breed of domestic rabbit developed in the United States. These rabbits are known for their distinctive appearance, characterized by their pure white fur and bright red eyes. They have a medium to large size with a muscular build, making them popular for meat production.";
            }
            else
            {
                return "American Sable:\n\nA docile breed with a body similar to a chinchilla's but with different coat colors. American Sables have dark sepia on their head, back, ears, feet, and the top of their tail, and lighter tan on the rest of their body. They typically weigh 7–15 pounds.";
            }
        }
        else if (animalName.Contains("Frog"))
        {
            taskList.updateT3Sub1();
            string animalColor = GetAnimalColor();
            if (animalColor.Contains("Purple"))
            {
                return "The Inyo toad, Black toad:\n\nAlso known as the Deep Springs toad, is a small amphibian native to California's Eastern Sierra Nevada region. These toads have olive-green to brown warty skin, and golden irises, and measure about 2 to 3 inches long. They are nocturnal, sheltering during the day and emerging at night to forage. Breeding occurs in spring and summer, with females laying eggs in shallow water";
            }
            else if (animalColor.Contains("Yellow"))
            {
                return "Mountain Yellow-legged Frogs:\n\nA species of amphibians native to the mountainous regions of California's Sierra Nevada and southern California's Transverse Ranges. They are characterized by their striking yellow or orange markings on their underside and legs, contrasting with dark or olive-colored backs.";
            }
            else
            {
                return "The California Red-legged Frog:\n\nA striking amphibian native to California's coastal regions, known for its distinct red coloring on its legs. Found in various aquatic habitats like ponds and streams, they feed on insects and small vertebrates. Breeding occurs in spring and summer, with females laying eggs in shallow water. Once widespread, habitat loss, pollution, and introduced predators have led to their decline. They're now a threatened species, vital for ecosystem health, requiring conservation efforts for their survival.";
            }
        }
        else if (animalName.Contains("Deer"))
        {
            taskList.updateT3Sub1();
            return "White-Tailed Deer:\n\nGraceful and elegant mammals. They are found inhabiting various environments such as forests, grasslands, and even suburban areas. Deer are known for their slender bodies, long legs, and distinct antlers (in most species, but not all) found on males. Their fur can vary in color depending on the species and the season, ranging from reddish-brown to grayish-brown.";
        }
        else if (animalName.Contains("Turtle"))
        {
            taskList.updateT3Sub1();
            return "Box turtles:\nFascinating reptiles found in North America. They belong to the genus Terrapene and are characterized by their dome-shaped carapace (top shell) and hinged plastron (bottom shell) which allows them to completely close up like a box, hence their name.";
        }
        else if (animalName.Contains("blueJay"))
        {
            Debug.Log("Blue Jay");
            taskList.updateT3Sub2();
            return "Blue Jay";
        }
        else if (animalName.Contains("cardinal"))
        {
            Debug.Log("Cardinal");
            taskList.updateT3Sub2();
            return "Cardinal";
        }
        else if (animalName.Contains("chickadee"))
        {
            Debug.Log("Chickadee");
            taskList.updateT3Sub2();
            return "Chickadee";
        }
        else if (animalName.Contains("crow"))
        {
            Debug.Log("Crow");
            taskList.updateT3Sub2();
            return "Crow";
        }
        else if (animalName.Contains("goldFinch"))
        {
            Debug.Log("Gold Finch");
            taskList.updateT3Sub2();
            return "Gold Finch";
        }
        else if (animalName.Contains("robin"))
        {
            Debug.Log("Robin");
            taskList.updateT3Sub2();
            return "Robin";
        }
        else if (animalName.Contains("sparrow"))
        {
            Debug.Log("Sparrow");
            taskList.updateT3Sub2();
            return "Sparrow";
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