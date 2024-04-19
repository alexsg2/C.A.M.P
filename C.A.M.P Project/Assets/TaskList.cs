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
    public GameObject FirePitIndicator;
    public GameObject MatchIndicator;
    public GameObject TentIndicator;
    public GameObject PolesIndicator;
    public GameObject Tarp1Indicator;
    public GameObject Tarp2Indicator;
    public GameObject NailIndicator;
    public GameObject Pole1PlaceIndicator;
    public GameObject Pole2PlaceIndicator;
    public GameObject Tarp1PlaceIndicator;
    public GameObject Tarp2PlaceIndicator;
    public GameObject Nail1PlaceIndicator;
    public GameObject Nail2PlaceIndicator;
    public GameObject Nail3PlaceIndicator;
    public GameObject Nail4PlaceIndicator;

    private string task1;
    private string T1sub1;
    private string T1sub2;
    private string T1sub3;
    private string task2;
    private string T2sub1;
    private string T2sub2;
    private string T2sub3;
    private string task3;
    private bool task1Done = false;
    private bool T1sub1Completed = false;
    private bool T1sub2Completed = false;
    private bool T1sub3Completed = false;
    private bool task2Done = false;
    private bool T2sub1Completed = false;
    private bool T2sub2Completed = false;
    private bool T2sub3Completed = false;
    private bool task3Done = false;
    private bool allTasksDone = false;


    // Start is called before the first frame update
    void Start()
    {
        task1 = "Task 1: Build a Fire\n";
        T1sub1 = "\tPut 3 twigs into the fire\n";
        T1sub2 = "\tLight the match using the matchbox\n";
        T1sub3 = "\tThrow the lit match into the firewood\n";
        task2 = "Task 2: Assemble a Tent\n";
        T2sub1 = "\tPlace two poles into the ground\n";
        T2sub2 = "\tPut two tarps on the poles\n";
        T2sub3 = "\tNail the tarp to the ground by placing and hammering nails in the corners\n";
        task3 = "Task 3: Identify Animals\n\tSubtask 1\n\tSubtask 2\n\tSubtask 3\n";
        
        canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3 + task2 + T2sub1 + T2sub2 + T2sub3 + task3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allTasksDone)
        {
            if (fireTriggerZone.checkSticks() && !T1sub1Completed)
            {
                T1sub1 = "\t<s>Put 3 twigs into the fire</s>\n";
                T1sub1Completed = true;
                FirePitIndicator.SetActive(false);
                MatchIndicator.SetActive(true);
            }
            if (matchLighterTrigger.checkMatch() && !T1sub2Completed)
            {
                T1sub2 = "\t<s>Light the match using the matchbox</s>\n";
                T1sub2Completed = true;
                FirePitIndicator.SetActive(true);
                MatchIndicator.SetActive(false);
            }
            if (fireTriggerZone.checkFire() && !T1sub3Completed)
            {
                T1sub3 = "\t<s>Throw the lit match into the firewood</s>\n";
                T1sub3Completed = true;
                FirePitIndicator.SetActive(false);
                TentIndicator.SetActive(true);
                PolesIndicator.SetActive(true);
                Pole1PlaceIndicator.SetActive(true);
                Pole2PlaceIndicator.SetActive(true);
            }
            if (T1sub1Completed && T1sub2Completed && T1sub3Completed && !task1Done)
            {
                task1 = "<s>Task 1: Build a Fire</s>\n";
                task1Done = true;
            }
            if (tentTriggerZone.checkPole() && !T2sub1Completed)
            {
                Debug.Log("Got in");
                T2sub1 = "\t<s>Place two poles into the ground</s>\n";
                T2sub1Completed = true;
                PolesIndicator.SetActive(false);
                Debug.Log("Poles Active: " + PolesIndicator.activeSelf);
                Pole1PlaceIndicator.SetActive(false);
                Debug.Log("Pole1 Active: " + Pole1PlaceIndicator.activeSelf);
                Pole2PlaceIndicator.SetActive(false);
                Debug.Log("Pole2 Active: " + Pole2PlaceIndicator.activeSelf);
                Tarp1Indicator.SetActive(true);
                Tarp2Indicator.SetActive(true);
                Tarp1PlaceIndicator.SetActive(true);
                Tarp2PlaceIndicator.SetActive(true);
            }
            if (tentTriggerZone.checkTarp() && !T2sub2Completed)
            {
                T2sub2 = "\t<s>Put two tarps on the poles</s>\n";
                T2sub2Completed = true;
                Tarp1Indicator.SetActive(false);
                Tarp2Indicator.SetActive(false);
                Tarp1PlaceIndicator.SetActive(false);
                Tarp2PlaceIndicator.SetActive(false);
                NailIndicator.SetActive(true);
                Nail1PlaceIndicator.SetActive(true);
                Nail2PlaceIndicator.SetActive(true);
                Nail3PlaceIndicator.SetActive(true);
                Nail4PlaceIndicator.SetActive(true);
            }
            if (tentTriggerZone.checkNail() && !T2sub3Completed)
            {
                T2sub3 = "\t<s>Nail the tarp to the ground by placing and hammering nails in the corners</s>\n";
                T2sub3Completed = true;
                NailIndicator.SetActive(false);
            }
            if (T2sub1Completed && T2sub2Completed && T2sub3Completed && !task2Done)
            {
                task2 = "<s>Task 2: Assemble a Tent</s>\n";
                task2Done = true;
                Nail1PlaceIndicator.SetActive(false);
                Nail2PlaceIndicator.SetActive(false);
                Nail3PlaceIndicator.SetActive(false);
                Nail4PlaceIndicator.SetActive(false);
                TentIndicator.SetActive(false);
            }
            if (task1Done && task2Done)
            {
                allTasksDone = true;
            }


            canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3 + task2 + T2sub1 + T2sub2 + T2sub3 + task3;
        }
    }

    public bool checkTask1()
    {
        return task1Done;
    }

    public bool checkTask2()
    {
        return task2Done;
    }
}
