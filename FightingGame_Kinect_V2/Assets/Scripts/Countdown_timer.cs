using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown_timer : MonoBehaviour {

    float timeLeft = 90.0f;

    public Text text;

    void Update()
    {

        timeLeft -= Time.deltaTime;
        text.text =  Mathf.Round(timeLeft).ToString();
       
    }
}
