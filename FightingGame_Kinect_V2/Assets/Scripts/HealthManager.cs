using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    private const int maxHealth = 20;

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

    public bool isBlueBlocking { get; set; }
    public bool isRedBlocking { get; set; }

    public bool hasRedLost { get; set; }
    public bool hasBlueLost { get; set; }

    // Use this for initialization
    void Start () {

        // Starting health
        blueHealth = 0;
        redHealth = 0;

        // Attack damage
        leftPunch = -5;
        rightPunch = -10;
        leftKick = -5;
        rightKick = -5;

        isBlueAttacking = false;
        isRedAttacking = false;

        isBlueBlocking = false;
        isRedBlocking = false;

        hasRedLost = false;
        hasBlueLost = false;

    }// End Start
	
	// Update is called once per frame
	void Update () {

        // If Blues Health goes above a certain value red loses
        // This is due to the health bars being switched to speed up development
        if (blueHealth >= maxHealth)
        {
            hasRedLost = true;
        }

        // If Red Health goes above a certain value blue loses
        // This is due to the health bars being switched to speed up development
        if (redHealth >= maxHealth)
        {
            hasBlueLost = true;
        }

    }// End Update

}// End class HealthManager
