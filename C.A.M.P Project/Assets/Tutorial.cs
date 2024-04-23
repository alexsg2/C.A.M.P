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
    public GameObject Board;
    public Trigger_Walk walkStatus;
    public Trigger_Box grabStatus;
    public Trigger_Throw_To throwToStatus;
    public Trigger_Throw_From throwFromStatus;
    public Skip_Tutorial skipped_Tutorial;
    private bool walkDone = false;
    private bool pickupDone = false;
    private bool throwDone = false;
    private bool allTasksDone = false;
    private bool skipped = true;
    private string script = "";
    private Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public Animator animator;


    private float typingSpeed = 0.01f;
    // private float typingSpeed = 0.00000001f;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3(-4.22f, 0f, 2.6f);
        animator.SetFloat("MoveSpeed", 1f);
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
        animator.SetTrigger("Wave");
        animator.SetFloat("MoveSpeed", 0f);

        Canvas.SetActive(true);

        script = "Hello, and welcome to CAMP, the Collaborative Ambient Multiplayer Park. My name is Mr. Foxy, and I am going to be your camp counselor!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

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

        targetPosition = new Vector3(1.26f, 0f, 2.6f);
        animator.SetFloat("MoveSpeed", -0.3f);
        StartCoroutine(MoveTourGuide(targetPosition));
        Canvas.SetActive(false);
        yield return new WaitForSeconds(3);
        animator.SetFloat("MoveSpeed", 0f);
    }

    private IEnumerator TypeOutTextWalk()
    {
        WalkIndicator.SetActive(false);
        WalkTrigger.SetActive(false);

        Canvas.SetActive(true);

        script = "Nice work! Now let’s figure out how to interact with items. In a second a cube will appear.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        script = "Aim your controller towards the cube. Once the line turns white, pick up the item by using the side button on the left or right controller.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(5);
        canvasText.text = "";

        BoxIndicator.SetActive(true);
        Cube.SetActive(true);
        GrabTrigger.SetActive(true);

        Canvas.SetActive(false);
    }

    private IEnumerator TypeOutTextPickUp()
    {
        BoxIndicator.SetActive(false);
        GrabTrigger.SetActive(false);
        Canvas.SetActive(true);

        script = "Awesome job! Continue holding the side button on the left or right controller to keep holding onto the item.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        script = "Let's throw the cube next. Lean back, swing forward, release the side button. Keep in the zone and throw the yellow cube straight to me!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(5);
        canvasText.text = "";

        ThrowToIndicator.SetActive(true);
        ThrowFromIndicator.SetActive(true);
        ThrowToTrigger.SetActive(true);
        ThrowFromTrigger.SetActive(true);

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
        script = "Amazing throw! You've learned all of the basic controls. If you have any further questions, just ask me!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        script = "While at the campsite, walk up to me with any questions about what to do next or how to tackle your tasks. Just click the mic icon and ask away.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3);
        canvasText.text = "";

        Board.SetActive(true);

        script = "Your tasks and subtasks will also be shown on this to-do board! If you ever forget what you’re doing or want to see your progress, check this board out.";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        // Delay after typing each text
        yield return new WaitForSeconds(5);
        canvasText.text = "";

        Board.SetActive(false);

        script = "You're all set for camping! Walk to the car and hit the join button. I'll meet you there!\nHappy camping!";
        foreach (char letter in script)
        {
            canvasText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        animator.SetTrigger("Wave");
        yield return new WaitForSeconds(5);
        canvasText.text = "";
        Canvas.SetActive(false);

        allTasksDone = true;
        TourGuide.SetActive(false);
        Car.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!allTasksDone)
        {
            if (skipped_Tutorial.IsSkipped())
            {
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
            else if (!walkDone && walkStatus.checkWalk())
            {
                Debug.Log("Walk done");
                walkDone = true;
                StartCoroutine(TypeOutTextWalk());
            }
            else if (!pickupDone && walkDone && grabStatus.checkGrab())
            {
                Debug.Log("Pickup done");
                StartCoroutine(TypeOutTextPickUp());
                pickupDone = true;
            }
            else if (!throwDone && walkDone && pickupDone && throwToStatus.checkBoxInside() && throwFromStatus.checkPlayerInside())
            {
                Debug.Log("Throw done");
                throwDone = true;
                animator.SetTrigger("Pickup");
                StartCoroutine(TypeOutTextThrow());
            }
        }
    }
}
