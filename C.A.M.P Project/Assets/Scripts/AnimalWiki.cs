using UnityEngine;

public class AnimalWiki : MonoBehaviour {
    //Checks animal type and gets a description 
    public string GetAnimalDesc()
    {
        string animalName = gameObject.name;

        if (animalName.Contains("Rabbit"))
        {
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
                return "The California Red-legged Frog:\n\nA striking amphibian native to California's coastal regions, known for its distinct red coloring on its legs. Found in various aquatic habitats like ponds and streams, they feed on insects and small vertebrates. Breeding occurs in spring and summer, with females laying eggs in shallow water. They're now a threatened species, vital for ecosystem health, requiring conservation efforts for their survival.";
            }
        }
        else if (animalName.Contains("Deer"))
        {
            return "White-Tailed Deer:\n\nGraceful and elegant mammals. They are found inhabiting various environments such as forests, grasslands, and even suburban areas. Deer are known for their slender bodies, long legs, and distinct antlers found on males. Their fur can vary in color depending on the species and the season.";
        }
        else if (animalName.Contains("Turtle"))
        {
            return "Box turtles:\nFascinating reptiles found in North America. They belong to the genus Terrapene and are characterized by their dome-shaped carapace (top shell) and hinged plastron (bottom shell) which allows them to completely close up like a box, hence their name.";
        }
        else if (animalName.Contains("blueJay"))
        {
            return "Blue Jay\nBlue jays are primarily blue and relatively small birds, with white undersides and chests. They have a black U-shaped collar around the neck. They are large, intelligent, and colorful songbirds. They can also live in various habitats and have complex social systems that involve tight family bonds.";
        }
        else if (animalName.Contains("cardinal"))
        {
            return "Cardinal\nCardinals are medium-sized thick-billed songbirds, many with crested heads and some bright red coloring. They have strong breaks for crushing seeds, long tails, and prominent crests. Cardinals tend to sit in a hunched over position with their tails pointing towards the ground.";
        }
        else if (animalName.Contains("chickadee"))
        {
            return "Chickadee\nChickadees are small, plump birds with round bodies and short tails. They have black heads and backs, with white cheeks and undersides. Chickadees are very agile and they are often seen hopping around on branches in search of food. They eat a variety of insects and seeds.";
        }
        else if (animalName.Contains("crow"))
        {
            return "Crow\nThere are a wide variety of crow species. Most crows have black-colored feathers with an iridescent shine to them. They often live together in large families and are known for their loud voices and intelligence. Crows are curious birds.";
        }
        else if (animalName.Contains("goldFinch"))
        {
            return "Gold Finch\nGoldfinches are songbirds with short, notched tails and lots of yellow coloring. They are migratory birds. All have rather delicate sharp-pointed bills for finches. Flocks of goldfinches feed in fields and gardens.";
        }
        else if (animalName.Contains("robin"))
        {
            return "Robin\nRobins are small to medium-sized birds with an unmistakable orange breast and face. They have gray plumage on either side of the orange breast. Robins have sturdy legs with muscles designed for running or hopping.";
        }
        else if (animalName.Contains("sparrow"))
        {
            return "Sparrow\nSparrows are small birds, usually brown and gray. They have short tails and small, strong beaks. Sparrows are social birds and they live in flocks. It's beak is short and thick to better crack open seeds.";
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