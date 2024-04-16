using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskList : MonoBehaviour
{
    public TMP_Text canvasText;

    // Start is called before the first frame update
    void Start()
    {
        canvasText.text =
            "Task 1: Build a Fire\n\tPut 3 twigs into the fire\n\tLight the match using the matchbox\n\tThrow the lit match into the firewood\n" +
            "Task 2: Assemble a Tent\n\tSubtask 1\n\tSubtask 2\n\tSubtask 3\n\n\n" +
            "Task 3: Identify Animals\n\tSubtask 1\n\tSubtask 2\n\tSubtask 3\n";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
