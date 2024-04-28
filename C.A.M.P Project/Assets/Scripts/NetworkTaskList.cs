using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;

public class NetworkTaskList : NetworkBehaviour
{

    // Text rows on task board
    //t1 header and subheaders/tasksprivate const string T1H = "Task 1: Build a Fire\n\n";
    private const string T1H = "Task 1: Build a Fire\n\n";
    private const string T1HDone = "<s>Task 1: Build a Fire</s>\n\n";
    private const string T1s1 = "[] Put 12 twigs into the \n   firepit\n\n";
    private const string T1s1Done = "[x] <s>Put 12 twigs into the</s>\n     <s>firepit</s>\n\n";
    private const string T1s2 = "[] Light 3 Matches using \n   a matchbox and put\n   them in the firepit\n\n";
    private const string T1s2Done = "[x] <s>Light 3 Matches using</s> \n     <s>a matchbox and put</s>\n     <s>them in the firepit</s>\n\n";
    // t2 header and subheaders/tasks
    private const string T2H = "Task 2: Assemble a Tent\n\n";
    private const string T2HDone = "<s>Task 2: Assemble a Tent</s>\n\n";
    private const string T2s1 = "[] Place 2 poles into the\n   ground\n\n";
    private const string T2s1Done = "[x] <s>Place 2 poles into the</s>\n     <s>ground</s>\n\n";
    private const string T2s2 = "[] Put 2 tarps on the poles\n\n";
    private const string T2s2Done = "[x] <s>Put 2 tarps on the poles</s>\n\n";
    private const string T2s3 = "[] Place stakes in the tent \n   corners and hammer\n   them twice";
    private const string T2s3Done = "[x] <s>Place stakes in the tent</s>\n     <s>corners and hammer</s>\n     <s>them twice</s>\n";

    // t3 header and subheaders/tasks
    private const string T3H = "Task 3: Identify Animals\n\n";
    private const string T3HDone = "<s>Task 3: Identify Animals</s>\n\n";
    private const string T3s1 = "[] Identify 1 Land Animal\n\n";
    private const string T3s1Done = "[x] <s>Identify 1 Land Animal\n\n</s>";
    private const string T3s2 = "[] Identify 1 Bird Type";
    private const string T3s2Done = "[x] <s>Identify 1 Bird Type</s>";

    private const string Done = "     All Tasks Complete\n\n     Nice job campers!";

    // Board Text
    public TMP_Text canvasText;

    public GameObject boardIndicator; // enable this when starting new task

    // Task scripts
    public NetworkFireTask fireTask;
    public NetworkTentTask tentTask;
    public NetworkAnimalTask animalTask;

    enum tasks {
        Wait,
        Task1,
        Task2,
        Task3,
        Done
    }

    // curr task we are displaying / completing
    private NetworkVariable<tasks> curr_task = new NetworkVariable<tasks>(
        tasks.Wait, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

        // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // TODO: fast forward for 1st task ish
        if (IsClient && curr_task.Value != tasks.Wait) {
            curr_task.OnValueChanged += OnCurrTaskChange;
            OnCurrTaskChange(tasks.Wait, curr_task.Value);
            return;
        }
        curr_task.OnValueChanged += OnCurrTaskChange;
        boardIndicator.SetActive(true);
    }

    // update task status after ending prev task and player
    // enters collider
    private void OnTriggerEnter(Collider other) {
        if (IsClient) {
            return;
        }
        // Debug.Log("Server Task List: something entered task board trigger");
        // if we're "in between" tasks, move on to next task
        if (other.CompareTag("Player") && boardIndicator.activeSelf) {
            // Debug.Log("Server Task List: player entered trigger and board indic active, moving on to next task");
            curr_task.Value += 1;  // increment curr task
            //curr_task.Value = tasks.Task3; // uncomment this and set to desired task for debugging purposes
            // Debug.Log($"NetworkTaskList: moved on to task {curr_task.Value}");
            // boardIndicator.SetActive(false);
        }
    }

    private void SetFirepitText(){
        switch (fireTask.TaskStatus.Value) {
            case (FireTaskStatus.Done):
                canvasText.text = T1HDone + T1s1Done + T1s2Done;
                break;
            case (FireTaskStatus.TwigsDone):
                canvasText.text = T1H + T1s1Done + T1s2;
                break;
            case (FireTaskStatus.Start):
                canvasText.text = T1H + T1s1 + T1s2;
                break;
        }
    }

    private void SetTentText() {
        switch (tentTask.taskStatus.Value) {
            case (TentTaskStatus.Done):
                canvasText.text = T2HDone + T2s1Done + T2s2Done + T2s3Done;
                break;
            case (TentTaskStatus.TarpsDone):
                canvasText.text = T2H + T2s1Done + T2s2Done + T2s3;
                break;
            case (TentTaskStatus.PolesDone):
                canvasText.text = T2H + T2s1Done + T2s2 + T2s3;
                break;
            case (TentTaskStatus.Start):
                canvasText.text = T2H + T2s1 + T2s2 + T2s3;
                break;
        }
    }

    private void SetAnimalText() {
        string header = T3H;
        string sub1 = T3s1;
        string sub2 = T3s2;

        TwoBools status = animalTask.taskStatus.Value;
        if (status.left && status.right) {
            header = T3HDone;
            sub1 = T3s1Done;
            sub2 = T3s2Done;
        }
        else if (status.left) {
            sub1 = T3s1Done;
        }
        else if (status.right) {
            sub2 = T3s2Done;
        }

        canvasText.text = header + sub1 + sub2;
    }

    // Update canvas text based on curr task
    private void SetBoardText() {
        switch (curr_task.Value) {
            case tasks.Task1:
                // Debug.Log($"NetworkTaskList: Set firepit text");
                SetFirepitText();
                break;
            case tasks.Task2:
                // Debug.Log($"NetworkTaskList: Set tent text");
                SetTentText();
                break;
            case tasks.Task3:
                // Debug.Log($"NetworkTaskList: Set animal text");
                SetAnimalText();
                break;
            case tasks.Done:
                // Debug.Log($"NetworkTaskList: set done text");
                canvasText.text = Done;
                break;
        }
    }

    // Update board to fit new task we are currently on
    private void OnCurrTaskChange(tasks old, tasks updated) {
        switch (updated) {
            case (tasks.Task1):
                fireTask.TaskStatus.OnValueChanged += OnTask1StatusChange;
                // init task 1
                if (IsServer) {
                    // Debug.Log($"NetworkTaskList: stared task 1");
                    fireTask.TaskStatus.Value = FireTaskStatus.Start;
                }
                break;
            case (tasks.Task2):
                tentTask.taskStatus.OnValueChanged += OnTask2StatusChange;
                // fireTask.TaskStatus.OnValueChanged -= OnTask1StatusChange;
                if (IsServer) {
                    // Debug.Log("NetworkTaskList: started task 2");
                    tentTask.taskStatus.Value = TentTaskStatus.Start;
                }
                break;
            case (tasks.Task3):
                // tentTask.taskStatus.OnValueChanged -= OnTask2StatusChange;
                // animalTask.taskStatus.OnValueChanged += OnTask3StatusChange;
                animalTask.taskStatus.OnValueChanged += OnTask3StatusChange;
                animalTask.StartTask(); // clients and server invoke this
                // if (IsServer) {
                //     Debug.Log("NetworkTaskList: started task 3");
                //     // animalTask.taskStatus.Value = AnimalTaskStatus.Start;
                // }
                break;
            case (tasks.Done):
                // clean up
                // animalTask.taskStatus.OnValueChanged -= OnTask3StatusChange;
                break;
        }
        boardIndicator.SetActive(false);
        SetBoardText(); // update text
    }

    // public void FastForward() {
    //     // TODO: fast forward state to match server if new client joins
    // }

    // Called when T1 is not complete and its state
    // changes.
    public void OnTask1StatusChange(FireTaskStatus prev, FireTaskStatus updated){
        SetBoardText();

        if (updated == FireTaskStatus.Done) {
            // Debug.Log($"NetworkTaskList: fire task done, waiting for player to enter indicator");
            boardIndicator.SetActive(true);
            fireTask.TaskStatus.OnValueChanged -= OnTask1StatusChange;
        }
    }

    public void OnTask2StatusChange(TentTaskStatus prev, 
        TentTaskStatus updated) {
        SetBoardText();

        if (updated == TentTaskStatus.Done) {
            // Debug.Log("NetworkTaskList: tent task done, waiting for player to enter indicator");
            boardIndicator.SetActive(true);
            tentTask.taskStatus.OnValueChanged -= OnTask2StatusChange;
        }       
    }

    public void OnTask3StatusChange(TwoBools prev, TwoBools updated){
        SetBoardText();

        // Are both the land and bird animal tasks done?
        if (updated.left && updated.right) {
            // Debug.Log($"NetworkTaskList: animal task done, waiting for player to enter indicator");
            boardIndicator.SetActive(true);
            animalTask.taskStatus.OnValueChanged -= OnTask3StatusChange;
        }
    }
}