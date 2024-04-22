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
    public GameObject WalkTrigger;
    public GameObject GrabTrigger;
    public GameObject ThrowFromTrigger;
    public GameObject ThrowToTrigger;
    public Trigger_Walk walkStatus;
    public Trigger_Box grabStatus;
    public Trigger_Throw_To throwToStatus;
    public Trigger_Throw_From throwFromStatus;
    private bool walkDone = false;
    private bool pickupDone = false;
    private bool throwDone = false;
    private bool allTasksDone = false;
    private string script = "";
    private Vector3 targetPosition = new Vector3(0f, 0f, 0f);


    private float typingSpeed = 0.001f;
    // private float typingSpeed = 0.00000001f;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3(-4.22f, 0f, 2.6f);
        StartCoroutine(MoveTourGuide(targetPosition));
        StartCoroutine(TypeOutTextIntro());
    }

    private IEnumerator MoveTourGuide(Vector3 targetPosition)
    {
        float duration = 3f; // Adjust the duration as needed
        float elapsedTime = 0f;
        Vector3 startingPosition = TourGuide.transform.position;

        while (elapsedTime < duration)
        {
            TourGuide.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches exactly the target position
        TourGuide.transform.position = targetPosition;
    }

    private IEnumerator TypeOutTextIntro()
    {
        yield return new WaitForSeconds(3);
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
        WalkTrigger.SetActive(true);
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
        targetPosition = new Vector3(1.26f, 0f, 2.6f);
        StartCoroutine(MoveTourGuide(targetPosition));
        Canvas.SetActive(false);
    }

    private IEnumerator TypeOutTextWalk()
    {
        WalkIndicator.SetActive(false);
        WalkTrigger.SetActive(false);
        BoxIndicator.SetActive(true);
        Cube.SetActive(true);
        GrabTrigger.SetActive(true);
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
        GrabTrigger.SetActive(false);
        ThrowToIndicator.SetActive(true);
        ThrowFromIndicator.SetActive(true);
        ThrowToTrigger.SetActive(true);
        ThrowFromTrigger.SetActive(true);
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
        ThrowToTrigger.SetActive(false);
        ThrowFromTrigger.SetActive(false);
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
            if (!walkDone && walkStatus.checkWalk())
            {
                Debug.Log("Walk done");
                walkDone = true;
                StartCoroutine(TypeOutTextWalk());
            }
            if (!pickupDone && walkDone && grabStatus.checkGrab())
            {
                Debug.Log("Pickup done");
                StartCoroutine(TypeOutTextPickUp());
                pickupDone = true;
            }
            if (!throwDone && walkDone && pickupDone && throwToStatus.checkBoxInside() && throwFromStatus.checkPlayerInside())
            {
                Debug.Log("Throw done");
                throwDone = true;
                StartCoroutine(TypeOutTextThrow());
                allTasksDone = true;
            }
        }
    }
}
