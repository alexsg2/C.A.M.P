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
    private string T3sub1;
    private string T3sub2;
    private bool task1Done = false;
    private bool T1sub1Completed = false;
    private bool T1sub2Completed = false;
    private bool T1sub3Completed = false;
    private bool task2Done = false;
    private bool T2sub1Completed = false;
    private bool T2sub2Completed = false;
    private bool T2sub3Completed = false;
    private bool task3Done = false;
    private bool T3sub1Completed = false;
    private bool T3sub2Completed = false;
    private bool allTasksDone = false;


    // Start is called before the first frame update
    void Start()
    {
        task1 = "Task 1: Build a Fire\n\n";
        T1sub1 = "[] Put 12 twigs into the fire\n\n";
        T1sub2 = "[] Light 3 Matches using \n   a matchbox\n\n";
        T1sub3 = "[] Throw the lit Matches \n   into the firewood";
        task2 = "Task 2: Assemble a Tent\n\n";
        T2sub1 = "[] Place 2 poles into the\n   ground\n\n";
        T2sub2 = "[] Put 2 tarps on the poles\n\n";
        T2sub3 = "[] Nail the tarp to the ground by placing and hammering nails in the corners\n";
        task3 = "Task 3: Identify Animals\n\n";
        T3sub1 = "[] Identify 1 Land Animal\n\n";
        T3sub2 = "[] Identify 1 Bird Type";

        canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allTasksDone)
        {
            if (fireTriggerZone.checkSticks() && !T1sub1Completed)
            {
                T1sub1 = "[x] <s>Put 12 twigs into the fire</s>\n\n";
                T1sub1Completed = true;
                FirePitIndicator.SetActive(false);
                MatchIndicator.SetActive(true);

                canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3;
            }
            if (matchLighterTrigger.checkMatch() && !T1sub2Completed)
            {
                T1sub2 = "[x] <s>Light 3 Matches using</s> \n\t<s>a matchbox</s>\n\n";
                T1sub2Completed = true;
                FirePitIndicator.SetActive(true);
                MatchIndicator.SetActive(false);

                canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3;
            }
            if (fireTriggerZone.checkFire() && !T1sub3Completed)
            {
                T1sub3 = "[x] <s>Light 3 Matches using</s> \n     <s>a matchbox\n\n</s>";
                T1sub3Completed = true;
                FirePitIndicator.SetActive(false);
                TentIndicator.SetActive(true);
                PolesIndicator.SetActive(true);
                Pole1PlaceIndicator.SetActive(true);
                Pole2PlaceIndicator.SetActive(true);

                canvasText.text = task1 + T1sub1 + T1sub2 + T1sub3;
            }
            if (T1sub1Completed && T1sub2Completed && T1sub3Completed && !task1Done)
            {
                task1 = "<s>Task 1: Build a Fire</s>\n";
                task1Done = true;
                canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
            }
            if (tentTriggerZone.checkPole() && !T2sub1Completed)
            {
                Debug.Log("Got in");
                T2sub1 = "[x] <s>Place 2 poles into the</s>\n     <s>ground</s>\n\n";
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

                canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
            }
            if (tentTriggerZone.checkTarp() && !T2sub2Completed)
            {
                T2sub2 = "[x] <s>Put 2 tarps on the poles</s>\n\n";
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

                canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
            }
            if (tentTriggerZone.checkNail() && !T2sub3Completed)
            {
                T2sub3 = "[x] <s>Nail the tarp to the ground by placing and hammering nails in the corners</s>\n";
                T2sub3Completed = true;
                NailIndicator.SetActive(false);

                canvasText.text = task2 + T2sub1 + T2sub2 + T2sub3;
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

                canvasText.text = task3 + T3sub1 + T3sub2;
            }
            if (T3sub1Completed && !task3Done && task2Done)
            {
                T3sub1 = "[x] <s>Identify 1 Land Animal\n\n</s>";
                canvasText.text = task3 + T3sub1 + T3sub2;
            }
            if (T3sub2Completed && !task3Done && task2Done)
            {
                T3sub2 = "[x] <s>Identify 1 Bird Type</s>";
                canvasText.text = task3 + T3sub1 + T3sub2;
            }
            if (T3sub1Completed && T3sub2Completed && !task3Done && task2Done)
            {
                task3 = "<s>Task 3: Identify Animals</s>\n";
                task3Done = true;
            }
            if (task1Done && task2Done && task3Done)
            {
                allTasksDone = true;
                canvasText.text = "\n\n\n\tAll tasks done.\n\n\n  Go explore and have fun!";
            }
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

    public void updateT3Sub1()
    {
        T3sub1Completed = true;
    }
    public void updateT3Sub2()
    {
        T3sub2Completed = true;
    }
}