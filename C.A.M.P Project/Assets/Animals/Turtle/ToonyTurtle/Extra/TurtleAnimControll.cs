using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAnimControll : MonoBehaviour {

    public Transform platformTF;
    public Animator myAnim;
    public bool hide, walk, eat, dead, run, rest;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (platformTF)
            platformTF.eulerAngles += new Vector3(0, (Input.GetAxis("Horizontal") * 40) * Time.deltaTime, 0);
    }

		public void ButtonHide()
        {
            dead = false;
            myAnim.SetBool("Dead", dead);
            walk = false;
            myAnim.SetBool("Walk", walk);
            rest = false;
            myAnim.SetBool("Rest", rest);

        hide = !hide;
            myAnim.SetBool("Hide", hide);
        }

    public void ButtonWalk()
    {
            dead = false;
            myAnim.SetBool("Dead", dead);
            hide = false;
            myAnim.SetBool("Hide", hide);
        rest = false;
        myAnim.SetBool("Rest", rest);

        walk = !walk;
            myAnim.SetBool("Walk", walk);
        }

    public void ButtonEat()
    {
            dead = false;
            myAnim.SetBool("Dead", dead);
            hide = false;
            myAnim.SetBool("Hide", hide);
            walk = false;
            myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        myAnim.SetTrigger("Eat");
        }

    public void ButtonAttack1()
    {
            dead = false;
            myAnim.SetBool("Dead", dead);
            hide = false;
            myAnim.SetBool("Hide", hide);
            walk = false;
            myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        myAnim.SetTrigger("Attack1");
        }

    public void ButtonGetHitR()
    {
            dead = false;
            myAnim.SetBool("Dead", dead);
            hide = false;
            myAnim.SetBool("Hide", hide);
            walk = false;
            myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        myAnim.SetTrigger("GetHitR");
        }

    public void ButtonGetHitD()
    {
            dead = false;
            myAnim.SetBool("Dead", dead);
            hide = false;
            myAnim.SetBool("Hide", hide);
            walk = false;
            myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        myAnim.SetTrigger("GetHitD");
        }

    public void ButtonGetHitL()
    {
        dead = false;
        myAnim.SetBool("Dead", dead);
        hide = false;
        myAnim.SetBool("Hide", hide);
        walk = false;
        myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        myAnim.SetTrigger("GetHitL");
    }

    public void ButtonGetHitU()
    {
        dead = false;
        myAnim.SetBool("Dead", dead);
        hide = false;
        myAnim.SetBool("Hide", hide);
        walk = false;
        myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        myAnim.SetTrigger("GetHitU");
    }

    public void ButtonGetHitB()
    {
        dead = false;
        myAnim.SetBool("Dead", dead);
        hide = false;
        myAnim.SetBool("Hide", hide);
        walk = false;
        myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        myAnim.SetTrigger("GetHitB");
    }

    public void ButtonDead()
    {
            hide = false;
            myAnim.SetBool("Hide", hide);
            walk = false;
            myAnim.SetBool("Walk", walk);
        rest = false;
        myAnim.SetBool("Rest", rest);

        if (!dead)
                myAnim.SetTrigger("DeadT");
            dead = !dead;
            myAnim.SetBool("Dead", dead);
        }

    public void ButtonRest()
    {
        dead = false;
        myAnim.SetBool("Dead", dead);
        walk = false;
        myAnim.SetBool("Walk", walk);
        hide = false;
        myAnim.SetBool("Hide", hide);

        rest = !rest;
        myAnim.SetBool("Rest", rest);
    }

    public void ButtonWalkRun()
    {
        run = !run;
        myAnim.SetBool("Run", run);
    }
}
