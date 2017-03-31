using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown_timer : MonoBehaviour {

    float timeLeft = 10.0f;
    float startTime = 5.0f;

    public Text countdownTimerText;
    public Text startTimerText;

    private Player_1 p1;
    private Player_2 p2;
    private HealthManager hm;
    private KinectManager km;

    void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player-1").GetComponent<Player_1>();
        p2 = GameObject.FindGameObjectWithTag("Player-2").GetComponent<Player_2>();
        hm = GameObject.FindGameObjectWithTag("Health").GetComponent<HealthManager>();
        km = GameObject.FindGameObjectWithTag("Kinect").GetComponent<KinectManager>();
    }

    void Update()
    {

        // If timer is running
        if (startTime >= 0)
        {
            // When timer reaches 0 print the string "Fight" to screen
            if (Mathf.Round(startTime) == 0)
            {
                startTimerText.text = "Fight";

                // Disable scripts so players can not take damage during start up
                p1.enabled = false;
                p2.enabled = false;

                startTime -= Time.deltaTime;
            }
            else
            {
                startTime -= Time.deltaTime;
                startTimerText.text = Mathf.Round(startTime).ToString();

                p1.enabled = false;
                p2.enabled = false;
            }
        }
        else
        {
            startTimerText.text = "";
            timeLeft -= Time.deltaTime;
            countdownTimerText.text = Mathf.Round(timeLeft).ToString();
            p1.enabled = true;
            p2.enabled = true;
        }

        if (Mathf.Round(timeLeft) <= 0)
        {
            startTimerText.text = "Draw";
            countdownTimerText.text = "";
            p1.enabled = false;
            p2.enabled = false;
            km.isGameOver = true;
        }

    }// End Update

}// End class Countdown_timer
