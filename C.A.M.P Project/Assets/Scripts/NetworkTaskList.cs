using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;

public class NetworkTaskList : NetworkBehaviour
{
    // public TentTriggerZone tentTriggerZone;
    // public NetworkFireTriggerZone fireTriggerZone;
    // public MatchLighterTrigger matchLighterTrigger;
    // public Trigger_Board_1 board1;
    // public Trigger_Board_2 board2;
    // public Trigger_Board_3 board3;
    // public GameObject FirePitIndicator;
    // public GameObject MatchIndicator;
    // public GameObject TentIndicator;
    // public GameObject PolesIndicator;
    // public GameObject Tarp1Indicator;
    // public GameObject Tarp2Indicator;
    // public GameObject NailIndicator;
    // public GameObject Pole1PlaceIndicator;
    // public GameObject Pole2PlaceIndicator;
    // public GameObject Tarp1PlaceIndicator;
    // public GameObject Tarp2PlaceIndicator;
    // public GameObject Nail1PlaceIndicator;
    // public GameObject Nail2PlaceIndicator;
    // public GameObject Nail3PlaceIndicator;
    // public GameObject Nail4PlaceIndicator;
    // public GameObject BoardIndicator;
    // public GameObject BoardTrigger2;
    // public GameObject BoardTrigger3;

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

    // Task scripts
    public NetworkFireTask fireTask;
    public NetworkTentTask tentTask;
    // track completion of task 1 with an int: 1 = sub1 done, 2 = sub2 done, 3 = sub3 done all done yeah
    // public NetworkVariable<int> Task1Status = new NetworkVariable<int>(
    //     0,  
    //     NetworkVariableReadPermission.Everyone, 
    //     NetworkVariableWritePermission.Server); 
    // private bool task1Done = false;
    // private bool T1sub1Completed = false;
    // private bool T1sub2Completed = false;
    // private bool T1sub3Completed = false;
    // private bool task2Done = false;
    // private bool T2sub1Completed = false;
    // private bool T2sub2Completed = false;
    // private bool T2sub3Completed = false;
    // private bool task3Done = false;
    // private bool T3sub1Completed = false;
    // private bool T3sub2Completed = false;
    // private bool allTasksDone = false;
    // private bool checkBoard1 = true;
    // private bool checkBoard2 = true;
    // private bool checkBoard3 = true;

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
        fireTask.TaskStatus.OnValueChanged += OnTask1StatusChange;
        // TODO: handle fast forward for clients that join late
        // PLAN THIS CLASS OUT OK!!! BE CAREFULLL
        // TODO: listen for changes to curr_task
        // if (IsServer || IsHost) {

        // }
        // else {

        // }
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
                // task is done
                // move to tent task
                tentTask.InitTask();
                Debug.Log("NetworkTaskList: Task 2 has begun");
                tentTask.TaskStatus.OnValueChanged += OnTask2StatusChange;

                break;
        }

    }

    public void OnTask2StatusChange(NetworkTentTask.TentTaskStatus prev, 
        NetworkTentTask.TentTaskStatus updated) {

        Debug.Log("NetworkTaskList: Task 2 completion status updated");

        switch (updated) {
            case NetworkTentTask.TentTaskStatus.PolesDone:
                // TODO
                break;
            case NetworkTentTask.TentTaskStatus.TarpsDone:
                // TODO
                break;
            case NetworkTentTask.TentTaskStatus.Done:
                // TODO
                break;
        }
                
    }

    // TODO: handle indicators and board text

    // Update is called once per frame
    // void Update()
    // {
    //     if (!allTasksDone)
    //     {
    //         if (board1.checkBoard() && checkBoard1)
    //         {
    //             BoardIndicator.SetActive(false);
    //             FirePitIndicator.SetActive(true);
    //             checkBoard1 = false;
    //         }
    //         if (fireTriggerZone.checkSticks() && !T1sub1Completed)
    //         {
    //             T1sub1 = "[x] <s>Put 12 twigs into the fire</s>\n\n";
    //             T1sub1Completed = true;
    //             FirePitIndicator.SetActive(false);
    //             MatchIndicator.SetActive(true);

    //             canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3;
    //         }
    //         if (matchLighterTrigger.checkMatch() && !T1sub2Completed)
    //         {
    //             T1sub2 = "[x] <s>Light 3 Matches using</s> \n\t<s>a matchbox</s>\n\n";
    //             T1sub2Completed = true;
    //             FirePitIndicator.SetActive(true);
    //             MatchIndicator.SetActive(false);

    //             canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3;
    //         }
    //         if (fireTriggerZone.checkFire() && !T1sub3Completed)
    //         {
    //             T1sub3 = "[x] <s>Light 3 Matches using</s> \n     <s>a matchbox\n\n</s>";
    //             T1sub3Completed = true;
    //             FirePitIndicator.SetActive(false);
    //             BoardTrigger2.SetActive(true);
    //             BoardIndicator.SetActive(true);

    //             canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3;
    //         }
    //         if (T1sub1Completed && T1sub2Completed && T1sub3Completed && !task1Done)
    //         {
    //             task1 = "<s>Task 1: Build a Fire</s>\n";
    //             task1Done = true;
    //             canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
    //         }
    //         if (board2.checkBoard() && task1Done && checkBoard2)
    //         {
    //             BoardIndicator.SetActive(false);
    //             TentIndicator.SetActive(true);
    //             PolesIndicator.SetActive(true);
    //             Pole1PlaceIndicator.SetActive(true);
    //             Pole2PlaceIndicator.SetActive(true);
    //             checkBoard2 = false;
    //         }
    //         if (tentTriggerZone.checkPole() && !T2sub1Completed)
    //         {
    //             Debug.Log("Got in");
    //             T2sub1 = "[x] <s>Place 2 poles into the</s>\n     <s>ground</s>\n\n";
    //             T2sub1Completed = true;
    //             PolesIndicator.SetActive(false);
    //             Debug.Log("Poles Active: " + PolesIndicator.activeSelf);
    //             Pole1PlaceIndicator.SetActive(false);
    //             Debug.Log("Pole1 Active: " + Pole1PlaceIndicator.activeSelf);
    //             Pole2PlaceIndicator.SetActive(false);
    //             Debug.Log("Pole2 Active: " + Pole2PlaceIndicator.activeSelf);
    //             Tarp1Indicator.SetActive(true);
    //             Tarp2Indicator.SetActive(true);
    //             Tarp1PlaceIndicator.SetActive(true);
    //             Tarp2PlaceIndicator.SetActive(true);

    //             canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
    //         }
    //         if (tentTriggerZone.checkTarp() && !T2sub2Completed)
    //         {
    //             T2sub2 = "[x] <s>Put 2 tarps on the poles</s>\n\n";
    //             T2sub2Completed = true;
    //             Tarp1Indicator.SetActive(false);
    //             Tarp2Indicator.SetActive(false);
    //             Tarp1PlaceIndicator.SetActive(false);
    //             Tarp2PlaceIndicator.SetActive(false);
    //             NailIndicator.SetActive(true);
    //             Nail1PlaceIndicator.SetActive(true);
    //             Nail2PlaceIndicator.SetActive(true);
    //             Nail3PlaceIndicator.SetActive(true);
    //             Nail4PlaceIndicator.SetActive(true);

    //             canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
    //         }
    //         if (tentTriggerZone.checkNail() && !T2sub3Completed)
    //         {
    //             T2sub3 = "[x] <s>Nail the tarp to the ground by placing and hammering nails in the corners</s>\n";
    //             T2sub3Completed = true;
    //             NailIndicator.SetActive(false);

    //             canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
    //         }
    //         if (T2sub1Completed && T2sub2Completed && T2sub3Completed && !task2Done)
    //         {
    //             task2 = "<s>Task 2: Assemble a Tent</s>\n";
    //             task2Done = true;
    //             Nail1PlaceIndicator.SetActive(false);
    //             Nail2PlaceIndicator.SetActive(false);
    //             Nail3PlaceIndicator.SetActive(false);
    //             Nail4PlaceIndicator.SetActive(false);
    //             TentIndicator.SetActive(false);
    //             BoardTrigger3.SetActive(true);
    //             BoardIndicator.SetActive(true);

    //             canvasText.text = task3 + T3sub1 + T3sub2;
    //         }
    //         if (board3.checkBoard() && task2Done && checkBoard3)
    //         {
    //             BoardIndicator.SetActive(false);
    //             checkBoard2 = false;
    //         }
    //         if (T3sub1Completed && !task3Done && task2Done)
    //         {
    //             T3sub1 = "[x] <s>Identify 1 Land Animal\n\n</s>";
    //             canvasText.text = task3 + T3sub1 + T3sub2;
    //         }
    //         if (T3sub2Completed && !task3Done && task2Done)
    //         {
    //             T3sub2 = "[x] <s>Identify 1 Bird Type</s>";
    //             canvasText.text = task3 + T3sub1 + T3sub2;
    //         }
    //         if (T3sub1Completed && T3sub2Completed && !task3Done && task2Done)
    //         {
    //             task3 = "<s>Task 3: Identify Animals</s>\n";
    //             task3Done = true;
    //         }
    //         if (task1Done && task2Done && task3Done)
    //         {
    //             allTasksDone = true;
    //             canvasText.text = "\n\n\n\tAll tasks done.\n\n\n  Go explore and have fun!";
    //         }
    //     }
    // }

    // public bool checkTask1()
    // {
    //     return task1Done;
    // }

    // public bool checkTask2()
    // {
    //     return task2Done;
    // }

    // public void updateT3Sub1()
    // {
    //     T3sub1Completed = true;
    // }
    // public void updateT3Sub2()
    // {
    //     T3sub2Completed = true;
    // }
}