using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TMP_Text canvasText;
    public GameObject Canvas;
    public GameObject Cube;
    public GameObject WalkIndicator;
    public GameObject BoxIndicator;
    public GameObject ThrowToIndicator;
    public GameObject ThrowFromIndicator;
    public GameObject TourGuide;
    public GameObject Car;
    private bool walkDone = false;
    private bool pickupDone = false;
    private bool throwDone = false;
    private bool allTasksDone = false;
    private string script = "";

    private float typingSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TypeOutTextIntro());
    }

    private IEnumerator TypeOutTextIntro()
    {
        Canvas.SetActive(true);
        script = "Hello, and welcome to CAMP, the Collaborative Ambient Multiplayer Park. My name is Mr. Foxy, and I am going to be your camp counselor!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = ""; // Clear the text after typing is complete
        script = "To get the most out of your camping experience, I am going to take you through a tutorial to teach you all of the necessary controls.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        WalkIndicator.SetActive(true);
        canvasText.text = "";
        script = "See the yellow zone in front of you? Try walking up to it. You can either actually walk or use the joystick on your left controller.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = "";
        Canvas.SetActive(false);
    }

    private IEnumerator TypeOutTextWalk()
    {
        WalkIndicator.SetActive(false);
        BoxIndicator.SetActive(true);
        Cube.SetActive(true);
        Canvas.SetActive(true);
        script = "Nice work! Now let’s figure out how to interact with items, in this case the yellow cube. First aim your controller towards it so that the controller line makes contact with it.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = ""; // Clear the text after typing is complete
        script = "Once the line turns white, pick up the item by using the side button on the left or right controller.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = "";
        Canvas.SetActive(false);
    }

    private IEnumerator TypeOutTextPickUp()
    {
        BoxIndicator.SetActive(false);
        ThrowToIndicator.SetActive(true);
        ThrowFromIndicator.SetActive(true);
        Canvas.SetActive(true);
        BoxIndicator.SetActive(false);
        script = "Awesome job! Continue holding the side button on the left controller to keep holding onto the item.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = ""; // Clear the text after typing is complete
        script = "The next thing we are going to do is throw the item. Just like a normal throw, lean your arm back and swing in a forward motion, releasing the side button. Stay in the zone and throw the yellow cube right to me!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = "";
        Canvas.SetActive(false);
    }

    private IEnumerator TypeOutTextThrow()
    {
        Cube.SetActive(false);
        ThrowToIndicator.SetActive(false);
        ThrowFromIndicator.SetActive(false);
        Canvas.SetActive(true);
        script = "Amazing throw! We’ve learned all of the basic controls. If you have any further questions, just ask me!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = ""; // Clear the text after typing is complete
        script = "I’ll be wandering around the campsite. You can ask me anything regarding what to do next or how to do your current tasks. To ask me a question, simply aim at me and press the side button on the right controller and speak your question out loud.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = "";
        script = "Your tasks and subtasks will also be shown on this to-do board! If you ever forget what you’re doing or want to see your progress, check this board out.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = "";
        script = "You’re ready to begin camping walk to the car and click the join button! I'll see you there! Happy camping!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(3);
        canvasText.text = "";
        Canvas.SetActive(false);
        TourGuide.SetActive(false);
        Car.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!allTasksDone)
        {
            if (!walkDone)
            {
                walkDone = true;
                // StartCoroutine(TypeOutTextWalk());
            }
            if (!pickupDone && walkDone)
            {
                pickupDone = true;
                // StartCoroutine(TypeOutTextPickUp());
            }
            if (!throwDone && walkDone && pickupDone)
            {
                throwDone = true;
                // StartCoroutine(TypeOutTextThrow());
                allTasksDone = true;
            }
        }
    }
}
