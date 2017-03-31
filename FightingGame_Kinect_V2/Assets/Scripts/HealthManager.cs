using UnityEngine;

public class HealthManager : MonoBehaviour {

    // Health bar variables
    public Rect HealthBarDimens;
    public bool VerticleHealthBar;
    public Texture HealthBubbleTexture;
    public Texture HealthTexture;
    public float HealthBubbleTextureRotation;

    // Scripts
    HealthSystem health_bar;
    KinectManager km;

    // Health
    private const int maxHealth = 100;

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
    public bool hasWinGesture { get; set; }

    // Use this for initialization
    void Start () {

        // Starting health
        blueHealth = 0;
        redHealth = 0;

        // Attack damage
        leftPunch = -7;
        rightPunch = -5;
        leftKick = -12;
        rightKick = -3;

        // Attacking 
        isBlueAttacking = false;
        isRedAttacking = false;

        // Blocking
        isBlueBlocking = false;
        isRedBlocking = false;

        // Lost state
        hasRedLost = false;
        hasBlueLost = false;

        // Win gesture state
        hasWinGesture = false;

        km = GameObject.FindGameObjectWithTag("Kinect").GetComponent<KinectManager>();
        health_bar = new HealthSystem(HealthBarDimens, VerticleHealthBar, HealthBubbleTexture, HealthTexture, HealthBubbleTextureRotation);

    }// End Start
	
	// Update is called once per frame
	void Update () {

        // If Blues Health goes above a certain value red loses
        // This is due to the health bars being switched to speed up development
        if (blueHealth >= maxHealth)
        {
            hasRedLost = true;
            km.isGameOver = true;
            redHealth = 0;
            health_bar.Update();
        }

        // If Red Health goes above a certain value blue loses
        // This is due to the health bars being switched to speed up development
        if (redHealth >= maxHealth)
        {
            hasBlueLost = true;
            km.isGameOver = true;
            blueHealth = 0;
            health_bar.Update();
        }

    }// End Update

}// End class HealthManager
