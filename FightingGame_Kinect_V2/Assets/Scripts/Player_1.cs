using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class Player_1 : MonoBehaviour {

  
    Animator animator;

 

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }
	// Update is called once per frame
	void Update () 
    {


    }

    // Detect collision with Red Guy
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "RedGuy")
        {
            Debug.Log("Hit red guy");
        }
    }

    public void straight_right_punch()
    {
        animator.Play("Straight_Right_Punch");
    }

    public void straight_left_punch()
    {

    }

    public void block()
    {

    }
}
