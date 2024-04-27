using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;

public class NetworkTaskList : NetworkBehaviour
{

    // Text rows on task board
    //t1 header and subheaders/tasks
    private string task1;
    private string T1sub1;
    private string T1sub2;

    // t2 header and subheaders/tasks
    private string task2;
    private string T2sub1;
    private string T2sub2;
    private string T2sub3;

    // t3 header and subheaders/tasks
    private string task3;
    private string T3sub1;
    private string T3sub2;


    // private bool task1Done = false;

    // Board Text
    public TMP_Text canvasText;

    public GameObject boardIndicator;

    // Task scripts
    public NetworkFireTask fireTask;
    public NetworkTentTask tentTask;

    enum tasks {
        Task1,
        Task2,
        Task3,
        Done
    }

    // curr task we are displaying / completing
    private NetworkVariable<tasks> curr_task = new NetworkVariable<tasks>(
        tasks.Task1, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    // Update canvas text based on curr task
    private void SetBoardText() {
        switch (curr_task.Value) {
            case tasks.Task1:
                canvasText.text = task1 + T1sub1 + T1sub2;
                break;
            case tasks.Task2:
                canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
                break;
            case tasks.Task3:
                canvasText.text = task3 + T3sub1 + T3sub2;
                break;
            case tasks.Done:
                canvasText.text = "Nice job!";
                break;
        }

    }

    // Initialize text for tasks
    private void InitBoardText() {
        task1 = "Task 1: Build a Fire\n\n";
        T1sub1 = "[] Put 12 twigs into the firepit\n\n";
        T1sub2 = "[] Light 3 Matches using \n   a matchbox\n\n and put them in the firepit";
        task2 = "Task 2: Assemble a Tent\n\n";
        T2sub1 = "[] Place 2 poles into the\n   ground\n\n";
        T2sub2 = "[] Put 2 tarps on the poles\n\n";
        T2sub3 = "[] Nail the tarp to the ground by placing and hammering nails in the corners\n";
        task3 = "Task 3: Identify Animals\n\n";
        T3sub1 = "[] Identify 1 Land Animal\n\n";
        T3sub2 = "[] Identify 1 Bird Type";

        SetBoardText();
    }

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InitBoardText();
        // TODO: fast forward
        fireTask.TaskStatus.OnValueChanged += OnTask1StatusChange;
        curr_task.OnValueChanged += OnCurrTaskChange;
        // TODO: handle fast forward for clients that join late
        // PLAN THIS CLASS OUT OK!!! BE CAREFULLL
        // TODO: listen for changes to curr_task
    }

    // Update board to fit new task we are currently on
    private void OnCurrTaskChange(tasks old, tasks updated) {
        // switch (updated) {
        //     case tasks.Task1:
        //         SetBoardText();
        //         break;
        //     case tasks.Task2:
        //         SetBoardText();
        //         break;
        //     case tasks.Task3:
        //         SetBoardText();
        //         break;
        //     case tasks.Done:
        //         SetBoardText();
        //         break;
            
        // }
        SetBoardText();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        fireTask.TaskStatus.OnValueChanged -= OnTask1StatusChange;
    }

    public void FastForward() {
        // TODO: fast forward state to match server if new client joins
    }

    // Called when T1 is not complete and its state
    // changes.
    public void OnTask1StatusChange(int prev, int updated){
        switch (updated) {
            case 0:
                break;
            case 1:
                // update board
                T1sub1 = "[x] <s>Put 12 twigs into the fire</s>\n\n";
                SetBoardText();
                break;
            case 2:
                // update board, move to next task (register listener for that)
                T1sub2 = "[x] <s>Light 3 Matches using</s> \n\t<s>a matchbox</s>\n\n";
                fireTask.TaskStatus.OnValueChanged -= OnTask1StatusChange;
                SetBoardText();
                // TODO: uncomment when done testing tent task
                if (IsServer) {
                //     tentTask.taskStatus.Value = TentTaskStatus.Start;
                    // curr_task.Value =  tasks.Task2;
                    // TODO: add listeners, update task2's status to start
                    DelayUpdateCurrTask(tasks.Task2); // let text remain for a bit, then advance
                }
                // Debug.Log("NetworkTaskList: Task 2 has begun");
                // tentTask.taskStatus.OnValueChanged += OnTask2StatusChange;

                break;
        }

    }

    private IEnumerator DelayUpdateCurrTask(tasks newTask)
    {
        yield return new WaitForSeconds(10);
        curr_task.Value = newTask;
        Debug.Log($"NetworkTaskList: advanced to new task {newTask}");
    }

    public void OnTask2StatusChange(TentTaskStatus prev, 
        TentTaskStatus updated) {

        Debug.Log("NetworkTaskList: Task 2 completion status updated");

        switch (updated) {
            case TentTaskStatus.PolesDone:
                // TODO
                break;
            case TentTaskStatus.TarpsDone:
                // TODO
                break;
            case TentTaskStatus.Done:
                // TODO
                break;
        }
                
    }
}