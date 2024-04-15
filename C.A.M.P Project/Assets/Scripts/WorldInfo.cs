using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInfo : MonoBehaviour
{
    [SerializeField, TextArea] private string gameWorld;
    [SerializeField, TextArea] private string gameStory;

    public string getPrompt()
    {
        return "";
    }


}
