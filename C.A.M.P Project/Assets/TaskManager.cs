using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Task
{
    public string taskName;

    public bool isCompleted;

    public List<string> subtasks;
}

public class TaskManager : MonoBehaviour
{
    public GameObject mainTaskPrefab;

    public GameObject subtaskPrefab;

    public Transform tasksParent;

    public List<Task> tasks;

    void Start()
    {
        // Example tasks with subtasks
        tasks =
            new List<Task> {
                new Task {
                    taskName = "Build a campfire",
                    subtasks =
                        new List<string> {
                            "Gather firewood",
                            "Find rocks for the fire ring",
                            "Collect tinder and kindling"
                        }
                },
                new Task {
                    taskName = "Set up tent",
                    subtasks =
                        new List<string> {
                            "Lay out tent footprint",
                            "Assemble tent poles",
                            "Attach rainfly"
                        }
                },
                new Task {
                    taskName = "Gather food",
                    subtasks =
                        new List<string> {
                            "Fishing at the lake",
                            "Foraging for berries",
                            "Hunting for game"
                        }
                }
            };

        // Instantiate tasks and subtasks
        foreach (Task task in tasks)
        {
            GameObject mainTaskObject =
                Instantiate(mainTaskPrefab, tasksParent);
            Text mainTaskText = mainTaskObject.GetComponent<Text>();
            mainTaskText.text = task.taskName;

            foreach (string subtask in task.subtasks)
            {
                GameObject subtaskObject =
                    Instantiate(subtaskPrefab, tasksParent);
                Text subtaskText = subtaskObject.GetComponent<Text>();
                subtaskText.text = "- " + subtask;
            }
        }
    }
}
