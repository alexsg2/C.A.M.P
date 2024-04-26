using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{

    private Animator animator;

    // Animates animal when talking and leaving
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Grounded");
            animator.SetTrigger("Wave");
        }
    }

    public void Close()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Wave");
    }
}
