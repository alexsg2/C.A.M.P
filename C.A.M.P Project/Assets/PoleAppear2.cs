using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleAppear2 : MonoBehaviour
{
    public GameObject pole2; // reference to standing pole1

    public TentTriggerZone tentTriggerZone; // reference to TentTriggerZone script
    public TaskList taskList; // reference to TaskList script

    private bool triggered = false; // flag to track if trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pole") && !triggered && taskList.checkTask1())
        {
            Destroy(other.gameObject);
            pole2.SetActive(true);
            triggered = true; // set the flag to true

            // Call the PoleScriptExecuted method of TentTriggerZone script
            tentTriggerZone.PoleScript2Executed();
        }
    }
}