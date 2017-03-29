using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

//  ======================================================================================
//                                  Player 1 is Blue Guy
//  ======================================================================================

public class Player_1 : MonoBehaviour {

    // Script / Component variables
    Animator animator;
    Player_2 p2;
    HealthManager hm;
    HealthSystem health_bar;

    // Sound variables
    private UnityEngine.AudioSource source;
    public AudioClip hitSound;
    public AudioClip anotherSound;

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

    private void Awake()
    {
        source = GetComponent<UnityEngine.AudioSource>();
    }

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();

        p2 = GameObject.FindGameObjectWithTag("Player-2").GetComponent<Player_2>();

        hm = GameObject.FindGameObjectWithTag("Health").GetComponent<HealthManager>();

        health_bar = new HealthSystem(HealthBarDimens, VerticleHealthBar, HealthBubbleTexture, HealthTexture, HealthBubbleTextureRotation);

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
        else {
            canHit = false;
        }

    }// End update

    // Draw the health bar to the screen
    public void OnGUI()
    {
        health_bar.DrawBar();
    }

    // Detect collision with Red Guy
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.name == "RedGuy" && hm.isBlueAttacking == true)
        {

            if (canHit == true)
            {

                /* ================================================================================================================
                                                                     NOTE
                   Health bars were switched on screen so when blue guy hits he's actually taking health off himself
                   but on screen the health bars are placed over other player to give impresssion the other player is losing health
                   ================================================================================================================ */

                if (hm.isBlueBlocking == false)
                {

                    // Keeping track of blue guy health
                    hm.blueHealth -= hm.damage;

                    // Play animation that shows player being knocked back after gettng hit
                    p2.takenHit();

                    // Play hit sound
                    source.PlayOneShot(hitSound, 1);
                    //source.PlayOneShot(anotherSound, 1); Slap Sound Effect

                    // Deplete health bar 
                    health_bar.IncrimentBar(hm.damage);

                    // Update the health bar
                    health_bar.Update();

                    // Reset is attacking value
                    hm.isBlueAttacking = false;

                }

                if (hm.isRedBlocking == true)
                {
                    // Start coroutine that delays isRedBlocking variable
                    StartCoroutine(BlockCoroutine());
                }

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

    public void takenHit()
    {
        animator.Play("TakenHit");
    }

    IEnumerator BlockCoroutine()
    {
        yield return new WaitForSeconds(1f);
        hm.isRedBlocking = false;
    }
}// End class Player_1
