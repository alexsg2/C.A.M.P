using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TMP_Text canvasText;
    public GameObject WalkIndicator;
    public GameObject BoxIndicator;
    public GameObject ThrowToIndicator;
    public GameObject ThrowFromIndicator;
    private bool task1Done = false;
    private bool task2Done = false;


    // Start is called before the first frame update
    void Start()
    {
        script = "Hello, and welcome to CAMP, the Collaborative Ambient Multiplayer Park. My name is Mr. Foxy, and I am going to be your camp counselor!";
        canvasText.text = script;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allTasksDone)
        {
            if (!task1Done)
            {
                task1Done = true;
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
