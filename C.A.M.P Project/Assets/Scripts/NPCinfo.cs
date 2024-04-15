using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCinfo : MonoBehaviour
{
    [SerializeField] private string npcName = "Mr.Foxy";
    [SerializeField] private string npcInfo = "nice, energetic, goofy and funny";
    [SerializeField] private string npcPersonality = "";

    public string getPrompt() 
    {
        return "";
    }
}
