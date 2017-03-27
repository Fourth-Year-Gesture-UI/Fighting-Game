using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class Player_2 : MonoBehaviour {

    // Script / Component variables
    Animator animator;
    HealthManager hm;
    HealthSystem health_bar;
    Player_1 p1;

    // Health bar variables
    public Rect HealthBarDimens;
    public bool VerticleHealthBar;
    public Texture HealthBubbleTexture;
    public Texture HealthTexture;
    public float HealthBubbleTextureRotation;

    // Hit timer variables
    float _hitTime = 1;
    float _hitTimer = 0;
    bool canHit = true;

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();

        hm = GameObject.FindGameObjectWithTag("Health").GetComponent<HealthManager>();

        health_bar = new HealthSystem(HealthBarDimens, VerticleHealthBar, HealthBubbleTexture, HealthTexture, HealthBubbleTextureRotation);

        p1 = GameObject.FindGameObjectWithTag("Player-1").GetComponent<Player_1>();

    }// End Start

    // Update is called once per frame
    void Update()
    {

        // Add to timer
        _hitTimer += Time.deltaTime;

        // Conditions that control whether player can hit or not
        if (_hitTimer > _hitTime)
        {
            canHit = true;
        }
        else
        {
            canHit = false;
        }

    }// End Update

    // Draw the health bar to the screen
    public void OnGUI()
    {
        health_bar.DrawBar();
    }

    // Detect collision with Red Guy
    void OnCollisionEnter(Collision col)
    {

        if (canHit == true)
        {

            if (col.gameObject.name == "BlueGuy" && hm.isRedAttacking == true)
            {

                // Keeping track of red guy health
                hm.redHealth -= hm.damage;

                // Deplete health bar 
                health_bar.IncrimentBar(hm.damage);

                // Update the health bar
                health_bar.Update();

                hm.isRedAttacking = false;

                // Reset timer
                _hitTimer = 0;
            }

        }// End outer if
           
    }// End OnCollisionEnter


    // ==============================
    //      Fighting Animations
    // ==============================
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

}// End class Player_2
