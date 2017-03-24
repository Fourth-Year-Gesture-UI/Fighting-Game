using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class Player_2 : MonoBehaviour {

    Animator animator;
   
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Detect collision with Red Guy
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "BlueGuy")
        {
            Debug.Log("Hit blue guy");
        }
    }

    public void straight_right_punch()
    {
        animator.Play("Straight_Right_Punch");
    }

    public void straight_left_punch()
    {
        animator.Play("Straight_Left_Punch");
    }

    public void block()
    {
        animator.Play("Block");
    }

    public void left_kick()
    {
        animator.Play("Left_Kick");
    }

    public void right_kick()
    {
        animator.Play("Right_Kick");
    }
}
