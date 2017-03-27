using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    // Variables for health and attack damage
    public int blueHealth { get; set; }
    public int redHealth { get; set; }

    public int leftPunch { get; set; }
    public int rightPunch { get; set; }
    public int leftKick { get; set; }
    public int rightKick { get; set; }

    public int damage { get; set; }

    public bool isBlueAttacking { get; set; }
    public bool isRedAttacking { get; set; }

    // Use this for initialization
    void Start () {

        // Starting health
        blueHealth = 100;
        redHealth = 100;

        // Attack damage
        leftPunch = -5;
        rightPunch = -10;
        leftKick = -5;
        rightKick = -5;

        isBlueAttacking = false;
        isRedAttacking = false;

    }// End Start
	
	// Update is called once per frame
	void Update () {
		
	}

}// End class HealthManager
