using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCinfo : MonoBehaviour
{
    [SerializeField] private string npcName = "Mr.Foxy";
    [SerializeField] private string npcInfo = "I can help you will all the camping activities in this environment";
    [SerializeField] private string npcPersonality = "nice, energetic, goofy and funny";

    public string getPrompt() 
    {
        return "";
    }
}
