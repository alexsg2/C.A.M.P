using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    public TMP_Text canvasText;
    public TentTriggerZone tentTriggerZone;
    public FireTriggerZone fireTriggerZone;
    public MatchLighterTrigger matchLighterTrigger;

    // Start is called before the first frame update
    void Start()
    {
        canvasText.text =
            "Task 1: Build a Fire\n\tPut 3 twigs into the fire\n\tLight the match using the matchbox\n\tThrow the lit match into the firewood\n" +
            "Task 2: Assemble a Tent\n\tPlace two poles into the ground\n\tPut two tarps on the poles\n\tNail the tarp to the ground by placing and hammering nails in the corners\n\n\n" +
            "Task 3: Identify Animals\n\tSubtask 1\n\tSubtask 2\n\tSubtask 3\n";
    }

    // Update is called once per frame
    void Update()
    {
        // Check for task completion and update the task list accordingly
        UpdateTaskList();
    }

    void UpdateTaskList()
    {
        // Get the current text content of the task list
        string taskListText = canvasText.text;
        Debug.Log("TaskListText" + taskListText);

        // Update the formatting based on completion status
        taskListText = UpdateTaskCompletionStatus(taskListText, "Put 3 twigs into the fire", fireTriggerZone.checkSticks());
        taskListText = UpdateTaskCompletionStatus(taskListText, "Light the match using the matchbox", matchLighterTrigger.checkMatch());
        taskListText = UpdateTaskCompletionStatus(taskListText, "Throw the lit match into the firewood", fireTriggerZone.checkFire());
        taskListText = UpdateTaskCompletionStatus(taskListText, "Place two poles into the ground", tentTriggerZone.checkPole());
        taskListText = UpdateTaskCompletionStatus(taskListText, "Put two tarps on the poles", tentTriggerZone.checkTarp());
        taskListText = UpdateTaskCompletionStatus(taskListText, "Nail the tarp to the ground by placing and hammering nails in the corners", tentTriggerZone.checkNail());

        // Update the text content of the task list
        canvasText.text = taskListText;
        Debug.Log("canvasListText" + canvasText.text);
    }

    string UpdateTaskCompletionStatus(string text, string taskName, bool completed)
    {
        // Determine the formatting based on completion status
        string formatting = completed ? "<s>" : ""; // Strikethrough formatting if completed

        // Find the index of the task name in the text content
        int index = text.IndexOf(taskName);

        if (index != -1)
        {
            // Insert the formatting at the appropriate position
            return text.Insert(index, formatting);
        }
        Debug.Log("text: " + text);

        return text;
    }
}
