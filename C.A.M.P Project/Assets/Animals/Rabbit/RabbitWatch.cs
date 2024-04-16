// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RabbitWatch : MonoBehaviour
// {
//     private bool isHighlighted = false;
//     private Renderer animalRenderer;

//     // Start is called before the first frame update
//     void Start()
//     {
//         animalRenderer = GetComponent<Renderer>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Check if the user clicks the trigger on the QUEST controller
//         if (Input.GetButtonDown("Fire1")) // Assuming "Fire1" is the input for the trigger button
//         {
//             // Check if the animal is currently highlighted
//             if (isHighlighted)
//             {
//                 // Remove the green border
//                 animalRenderer.material.SetColor("_OutlineColor", Color.clear);
//                 isHighlighted = false;
//             }
//             else
//             {
//                 // Add the green border
//                 animalRenderer.material.SetColor("_OutlineColor", Color.green);
//                 isHighlighted = true;
//             }
//         }
//     }
// }
