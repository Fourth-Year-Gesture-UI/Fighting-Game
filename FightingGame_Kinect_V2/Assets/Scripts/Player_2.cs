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

    public void straight_right_punch()
    {
        animator.Play("Straight_Right_Punch");
    }
}
