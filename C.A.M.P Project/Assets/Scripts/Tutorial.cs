using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    // Declaring variables for various game objects and triggers
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
    public GameObject Board;

    // References to trigger scripts to check completion status

    public Trigger_Walk walkStatus;
    public Trigger_Box grabStatus;
    public Trigger_Throw_To throwToStatus;
    public Trigger_Throw_From throwFromStatus;
    public Skip_Tutorial skipped_Tutorial;

    // Flags to track completion status
    private bool walkDone = false;
    private bool pickupDone = false;
    private bool throwDone = false;
    private bool allTasksDone = false;
    private bool skipped = true;
    private string script = "";

    // Target position for TourGuide movement
    private Vector3 targetPosition = new Vector3(0f, 0f, 0f);

    // Animator for TourGuide animation
    public Animator animator;

    // Typing speed for text animation
    private float typingSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial target position for TourGuide
        targetPosition = new Vector3(-4.22f, 0f, 2.6f);

        // Start TourGuide movement and text animation coroutine
        animator.SetFloat("MoveSpeed", 1f);
        StartCoroutine(MoveTourGuide(targetPosition));
        StartCoroutine(TypeOutTextIntro());
    }

    // Coroutine to move TourGuide to a specified position
    private IEnumerator MoveTourGuide(Vector3 targetPosition)
    {
        // Define movement duration and initialize elapsed time
        float duration = 3f;
        float elapsedTime = 0f;
        Vector3 startingPosition = TourGuide.transform.position;

        // Move TourGuide using Lerp until duration is reached
        while (elapsedTime < duration)
        {
            TourGuide.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches exactly the target position
        TourGuide.transform.position = targetPosition;
    }

    // Coroutine to type out introductory text
    private IEnumerator TypeOutTextIntro()
    {
        // Wait for a few seconds before starting animation
        yield return new WaitForSeconds(3);
        animator.SetTrigger("Wave");
        animator.SetFloat("MoveSpeed", 0f);

        // Activate canvas for text display
        Canvas.SetActive(true);

        // Text animation for introduction
        script = "Hello, and welcome to CAMP, the Collaborative Ambient Multiplayer Park. My name is Mr. Foxy, and I am going to be your camp counselor!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        // More text animation for tutorial instructions
        script = "To get the most out of your camping experience, I am going to take you through a tutorial to teach you all of the necessary controls.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
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

        WalkTrigger.SetActive(true);

        // Move TourGuide to the next position and deactivate canvas
        targetPosition = new Vector3(1.26f, 0f, 2.6f);
        animator.SetFloat("MoveSpeed", -0.3f);
        StartCoroutine(MoveTourGuide(targetPosition));
        Canvas.SetActive(false);
        yield return new WaitForSeconds(3);
        animator.SetFloat("MoveSpeed", 0f);
    }

    // Coroutine to type out instructions for the grabbing task
    private IEnumerator TypeOutTextWalk()
    {
        // Deactivate indicators and triggers related to walking task
        WalkIndicator.SetActive(false);
        WalkTrigger.SetActive(false);

        // Activate canvas for text display
        Canvas.SetActive(true);

        // Text animation for walking instructions
        script = "Nice work! Now let’s figure out how to interact with items. In a second a cube will appear.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        // More text animation for grabbing instructions
        script = "Aim your controller towards the cube. Once the line turns white, pick up the item by using the side button on the left or right controller.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(5);
        canvasText.text = "";

        // Activate indicators and triggers related to grabbing task
        BoxIndicator.SetActive(true);
        Cube.SetActive(true);
        GrabTrigger.SetActive(true);

        // Deactivate canvas
        Canvas.SetActive(false);
    }

    // Coroutine to type out instructions for the grabbing task
    private IEnumerator TypeOutTextPickUp()
    {
        // Deactivate indicators and triggers related to grabbing task
        BoxIndicator.SetActive(false);
        GrabTrigger.SetActive(false);
        Canvas.SetActive(true);

        // Text animation for grabbing completion message
        script = "Awesome job! Continue holding the side button on the left or right controller to keep holding onto the item.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        // More text animation for throwing instructions
        script = "Let's throw the cube next. Lean back, swing forward, release the side button. Keep in the zone and throw the yellow cube straight to me!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(5);
        canvasText.text = "";

        // Activate indicators and triggers related to throwing task
        ThrowToIndicator.SetActive(true);
        ThrowFromIndicator.SetActive(true);
        ThrowToTrigger.SetActive(true);
        ThrowFromTrigger.SetActive(true);

        // Deactivate canvas
        Canvas.SetActive(false);
    }

    // Coroutine to type out instructions for the throwing task
    private IEnumerator TypeOutTextThrow()
    {
        // Deactivate cube and indicators related to throwing task
        Cube.SetActive(false);
        ThrowToIndicator.SetActive(false);
        ThrowFromIndicator.SetActive(false);
        ThrowToTrigger.SetActive(false);
        ThrowFromTrigger.SetActive(false);

        // Activate canvas for final instructions
        Canvas.SetActive(true);

        // Text animation for completion message
        script = "Amazing throw! You've learned all of the basic controls. If you have any further questions, just ask me!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        // More text animation for additional instructions
        script = "While at the campsite, walk up to me with any questions about what to do next or how to tackle your tasks. Just click the mic icon and ask away.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        // Activate to-do board
        Board.SetActive(true);

        // More text animation for to-do board explanation
        script = "Your tasks and subtasks will also be shown on this to-do board! If you ever forget what you’re doing or want to see your progress, check this board out.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(5);
        canvasText.text = "";

        // Deactivate to-do board
        Board.SetActive(false);

        // Final instructions for completing the tutorial
        script = "You're all set for camping! Walk to the car and hit the join button. I'll meet you there!\nHappy camping!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Trigger wave animation and wait before clearing text
        animator.SetTrigger("Wave");
        yield return new WaitForSeconds(5);
        canvasText.text = "";
        Canvas.SetActive(false);

        // Deactivate canvas, set allTasksDone flag to true, and hide TourGuide while showing Car
        allTasksDone = true;
        TourGuide.SetActive(false);
        Car.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if all tasks are not done yet
        if (!allTasksDone)
        {
            // Check if tutorial is skipped
            if (skipped_Tutorial.IsSkipped())
            {
                // Skip tutorial and show Car
                Cube.SetActive(false);
                WalkIndicator.SetActive(false);
                BoxIndicator.SetActive(false);
                ThrowToIndicator.SetActive(false);
                ThrowFromIndicator.SetActive(false);
                WalkTrigger.SetActive(false);
                GrabTrigger.SetActive(false);
                ThrowFromTrigger.SetActive(false);
                ThrowToTrigger.SetActive(false);
                Board.SetActive(false);
                TourGuide.SetActive(false);

                Car.SetActive(true);
            }
            // Check if walking task is completed
            else if (!walkDone && walkStatus.checkWalk())
            {
                walkDone = true;
                StartCoroutine(TypeOutTextWalk());
            }
            // Check if grabbing task is completed
            else if (!pickupDone && walkDone && grabStatus.checkGrab())
            {
                StartCoroutine(TypeOutTextPickUp());
                pickupDone = true;
            }
            // Check if throwing task is completed
            else if (!throwDone && walkDone && pickupDone && throwToStatus.checkBoxInside() && throwFromStatus.checkPlayerInside())
            {
                throwDone = true;
                animator.SetTrigger("Pickup");
                StartCoroutine(TypeOutTextThrow());
            }
        }
    }
}
