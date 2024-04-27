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
    private const string T1H = "Task 1: Build a Fire\n\n";
    private const string T1HDone = "<s>Task 1: Build a Fire</s>\n";
    private const string T1s1 = "[] Put 12 twigs into the firepit\n\n";
    private const string T1s1Done = "[x] <s>Put 12 twigs into the firepit</s>\n\n";
    private const string T1s2 =  "[] Light 3 Matches using \n   a matchbox\n\n and put them in the firepit\n\n";
    private const string T1s2Done =  "[x] <s>Light 3 Matches using \n   a matchbox\n\n and put them in the firepit</s>\n\n";
    // t2 header and subheaders/tasks
    private const string T2H = "Task 2: Assemble a Tent\n\n";
    private const string T2HDone = "<s>Task 2: Assemble a Tent</s>\n";
    private const string T2s1 = "[] Place 2 poles into the\n   ground\n\n";
    private const string T2s1Done = "[x] <s>Place 2 poles into the</s>\n     <s>ground</s>\n\n";
    private const string T2s2 = "[] Put 2 tarps on the poles\n\n";
    private const string T2s2Done = "[x] <s>Put 2 tarps on the poles</s>\n\n";
    private const string T2s3 = "[] Stake the tarp to the ground by placing and hammering stakes in the corners\n";
    private const string T2s3Done = "[x] <s>Stakes the tarp to the ground by placing and hammering stakes in the corners</s>\n";

    // t3 header and subheaders/tasks
    private const string T3H = "Task 3: Identify Animals\n\n";
    private const string T3HDone = "<s>Task 3: Identify Animals</s>\n";
    private const string T3s1 = "[] Identify 1 Land Animal\n\n";
    private const string T3s1Done = "[x] <s>Identify 1 Land Animal\n\n</s>";
    private const string T3s2 = "[] Identify 1 Bird Type";
    private const string T3s2Done = "[x] <s>Identify 1 Bird Type</s>";

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
        // if we're "in between" tasks, move on to next task
        if (other.CompareTag("Player") && boardIndicator.activeSelf) {
            curr_task.Value += 1; // increment curr task
            Debug.Log($"NetworkTaskList: moved on to task {curr_task.Value}");
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
        switch (animalTask.taskStatus.Value) {
            case AnimalTaskStatus.Start:
                canvasText.text = T3H + T3s1 + T3s2;
                break;
            case AnimalTaskStatus.AnimalsDone:
            canvasText.text = T3H + T3s1Done + T3s2;
                break;
            case AnimalTaskStatus.Done:
                canvasText.text = T3HDone + T3s1Done + T3s2Done;
                break;
        }
    }

    // Update canvas text based on curr task
    private void SetBoardText() {
        switch (curr_task.Value) {
            case tasks.Task1:
                Debug.Log($"NetworkTaskList: Set firepit text");
                SetFirepitText();
                break;
            case tasks.Task2:
                Debug.Log($"NetworkTaskList: Set tent text");
                SetTentText();
                break;
            case tasks.Task3:
                Debug.Log($"NetworkTaskList: Set animal text");
                SetAnimalText();
                break;
            case tasks.Done:
                Debug.Log($"NetworkTaskList: set done text");
                canvasText.text = "All tasks done!";
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
                    Debug.Log($"NetworkTaskList: stared task 1");
                    fireTask.TaskStatus.Value = FireTaskStatus.Start;
                }
                break;
            case (tasks.Task2):
                tentTask.taskStatus.OnValueChanged += OnTask2StatusChange;
                fireTask.TaskStatus.OnValueChanged -= OnTask1StatusChange;
                if (IsServer) {
                    Debug.Log("NetworkTaskList: started task 2");
                    tentTask.taskStatus.Value = TentTaskStatus.Start;
                }
                // init task2
                // register new listener
                break;
            case (tasks.Task3):
                tentTask.taskStatus.OnValueChanged -= OnTask2StatusChange;
                animalTask.taskStatus.OnValueChanged += OnTask3StatusChange;
                if (IsServer) {
                    Debug.Log("NetworkTaskList: started task 3");
                    animalTask.taskStatus.Value = AnimalTaskStatus.Start;
                }
                break;
            case (tasks.Done):
                // clean up
                animalTask.taskStatus.OnValueChanged -= OnTask3StatusChange;
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
            Debug.Log($"NetworkTaskList: fire task done, waiting for player to enter indicator");
            boardIndicator.SetActive(true);
            // fireTask.TaskStatus.OnValueChanged -= OnTask1StatusChange;
        }
    }

    public void OnTask3StatusChange(AnimalTaskStatus prev, AnimalTaskStatus updated){
        SetBoardText();

        if (updated == AnimalTaskStatus.Done) {
            Debug.Log($"NetworkTaskList: animal task done, waiting for player to enter indicator");
            boardIndicator.SetActive(true);
            // animalTask.TaskStatus.OnValueChanged -= OnTask1StatusChange;
        }
    }

    // private IEnumerator DelayUpdateCurrTask(tasks newTask)
    // {
    //     yield return new WaitForSeconds(10);
    //     curr_task.Value = newTask;
    //     Debug.Log($"NetworkTaskList: advanced to new task {newTask}");
    // }

    public void OnTask2StatusChange(TentTaskStatus prev, 
        TentTaskStatus updated) {
        SetBoardText();

        if (updated == TentTaskStatus.Done) {
            Debug.Log("NetworkTaskList: tent task done, waiting for player to enter indicator");
            boardIndicator.SetActive(true);
            // tentTask.taskStatus.OnValueChanged -= OnTask2StatusChange;
        }       
    }
}