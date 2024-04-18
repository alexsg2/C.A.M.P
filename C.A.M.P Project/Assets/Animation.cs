using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update 
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
        // avatar.GetComponent<PlayerInput>().enabled = true;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Wave");
        
    }
}
