using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using System;
using UnityEngine.SceneManagement;

public class KinectManager : MonoBehaviour {

    // Kinect 
    public KinectSensor _Sensor;

    // color frame and data 
    private ColorFrameReader colorFrameReader;
    private byte[] colorData;
    private Texture2D colorTexture;

    private BodyFrameReader bodyFrameReader;
    private int bodyCount;
    private Body[] bodies;
    private List<Body> trackedBodies;
    private string straightPunch = "Right_Straight_Punch_Right";
    private string left_punch = "Left_Punch_Left";
    private string block = "Block";
    private string left_kick = "Left_Kick_Left";
    private string right_kick = "Right_Kick_Right";
    private string win = "Win";

    /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
    private List<GestureDetector> gestureDetectorList = null;

    private float rightPunch;
    private int count;

    // Used to assign trackind id's so that two players can be differentiated while in game
    private ulong player_1;
    private ulong player_2;

    private bool isPlayer1;
    private bool isPlayer2;

    public bool isGameOver { get; set; }

    private int bi;
   
    // Player scripts
    Player_1 p1;
    Player_2 p2;
    HealthManager hm;

    private void Awake()
    {
        // Get the sensor
        this._Sensor = KinectSensor.GetDefault();

        // If sensor is working
        if (this._Sensor != null)
        {

            bodyFrameReader = this._Sensor.BodyFrameSource.OpenReader();
            this.bodyCount = this._Sensor.BodyFrameSource.BodyCount;
            this.bodies = new Body[this.bodyCount];
            this.trackedBodies = new List<Body> { };
            this.gestureDetectorList = new List<GestureDetector>();
            this.bi = 0;

            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                this.gestureDetectorList.Add(new GestureDetector(this._Sensor));
            }

            // Open the sensor
            this._Sensor.Open();

            if (this._Sensor.IsOpen)
            {
                Debug.Log("Kinect is open");
            }

            p1 = GameObject.FindGameObjectWithTag("Player-1").GetComponent<Player_1>();
            p2 = GameObject.FindGameObjectWithTag("Player-2").GetComponent<Player_2>();
            hm = GameObject.FindGameObjectWithTag("Health").GetComponent<HealthManager>();

            isGameOver = false;

        }// End if

        isPlayer1 = false;
        isPlayer2 = false;
    }

    // Initialization
    void Start()
    {
        
    }// End Start

    // Update is called once per frame
    void Update()
    {
        // process bodies
        bool newBodyData = false;

        // Get the latest frame
        using (BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame())
        {

            if (bodyFrame != null)
            {
                bodyFrame.GetAndRefreshBodyData(this.bodies);

                newBodyData = true;

            }// End if

        }// End using

        // If the frame is not null new a new body is found
        if (newBodyData)
        {

            // update gesture detectors with the correct tracking id
            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {

                // Store body from array
                var body = this.bodies[bodyIndex];

                // If there is somebody in camera view
                if (body != null)
                {

                    ulong trackingId = 0;

                    // if player is detected
                    if (bodies[bodyIndex].IsTracked)
                    {

                        // Check if game is over
                        if (isGameOver)
                        {

                            // Enable player to use gesture to go back to main menu
                            if (body.HandRightState == HandState.Lasso && hm.hasWinGesture == true)
                            {
                                OnApplicationQuit();
                                SceneManager.LoadScene("MainMenu");
                            }

                        }// End game over if
                       
                        // Adding bodies to the list
                        if (trackedBodies.Count < 2) // if list is empty
                        {
                            // if no players in the list:
                            if(trackedBodies.Count == 0)
                            {
                                // then add player 1
                                trackedBodies.Add(body);
                            }
                            // if there player 1 in the list:
                            if(trackedBodies.Count == 1)
                            {
                                // make sure that it is not player 1
                                if(trackedBodies[0].TrackingId != bodies[bodyIndex].TrackingId)
                                {
                                    // and add player 2 to the list
                                    trackedBodies.Add(body);
                                }
                            }
                        }

                        foreach (Body b in trackedBodies)
                        {
                            //Debug.Log("body tracked id" + b.TrackingId);
                        }

                        // Assign tracking id 
                        trackingId = body.TrackingId;

                        int index = bodyIndex;
                    }

                    this.gestureDetectorList[bodyIndex].OnGestureDetected += CreateOnGestureHandler(trackedBodies, trackingId);

                    // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                    if (trackingId != this.gestureDetectorList[bodyIndex].TrackingId)
                    {
                        //GestureTextGameObject.text = "none";
                        //this.bodyText[bodyIndex] = "none";
                        this.gestureDetectorList[bodyIndex].TrackingId = trackingId;

                       // player_1 = checkTrackingId(this.gestureDetectorList[bodyIndex].TrackingId, player_1);
                        //player_2 = checkTrackingId(this.gestureDetectorList[bodyIndex].TrackingId, player_2);

                        // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                        // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                        this.gestureDetectorList[bodyIndex].IsPaused = (trackingId == 0);
                       
                        this.gestureDetectorList[bodyIndex].OnGestureDetected += CreateOnGestureHandler(trackedBodies, trackingId);
                       
                    }
                }
            }
        }

    }// End Update

    private EventHandler<GestureEventArgs> CreateOnGestureHandler(List<Body> players, ulong trackingId)
    {
        return (object sender, GestureEventArgs e) => OnGestureDetected(sender, e, players, trackingId);
    }

    private void OnGestureDetected(object sender, GestureEventArgs e, List<Body> players, ulong trackingId)
    {
        
        var isDetected = e.IsBodyTrackingIdValid && e.IsGestureDetected;

        // Right Punch
        if (e.GestureID == straightPunch)
        {

            if (e.DetectionConfidence > 0.4)
            {

                for (int i = 0; i < players.Count; i++)
                {
                    Body b = players[i];
                    if (b.TrackingId == trackingId)
                    {
                        if (i == 0)
                        {

                            if (hm.hasRedLost == false)
                            {
                                p1.straight_right_punch();
                                hm.isBlueAttacking = true;
                                hm.damage = hm.rightPunch;
                            }
                           
                        }
                        else
                        {
                            if (hm.hasBlueLost == false)
                            {
                                p2.straight_right_punch();
                                hm.isRedAttacking = true;
                                hm.damage = hm.rightPunch;
                            }
                           
                        }// End if /else

                    }// End if

                }// End for

            }// End if

        }// End outer if

        // Left Punch Gesture
        if (e.GestureID == left_punch)
        {

            if (e.DetectionConfidence > 0.4)
            {

                for (int i = 0; i < players.Count; i++)
                {

                    Body b = players[i];
                    if (b.TrackingId == trackingId)
                    {
                        if (i == 0)
                        {

                            if (hm.hasRedLost == false)
                            {
                                p1.straight_left_punch();
                                hm.isBlueAttacking = true;
                                hm.damage = hm.leftPunch;
                            }   
                        }
                        else
                        {
                            if (hm.hasBlueLost == false)
                            {
                                p2.straight_left_punch();
                                hm.isRedAttacking = true;
                                hm.damage = hm.leftPunch;
                            }
                           
                        }// End if / else

                    }//End if

                }// End for

            }// End if

        }// End outer if

        // Block Gesture
        if (e.GestureID == block)
        {

            if (e.DetectionConfidence > 0.7)
            {

                for (int i = 0; i < players.Count; i++)
                {

                    Body b = players[i];
                    if (b.TrackingId == trackingId)
                    {
                        if (i == 0)
                        {
                            p1.block();
                            hm.isBlueBlocking = true;
                        }
                        else
                        {
                            p2.block();
                            hm.isRedBlocking = true;
                        }// End if / else

                    }//End if

                }// End for

            }// End if

        }// End outer if

        // Left Kick Gesture
        if (e.GestureID == left_kick)
        {

            if (e.DetectionConfidence > 0.3)
            {

                for (int i = 0; i < players.Count; i++)
                {

                    Body b = players[i];
                    if (b.TrackingId == trackingId)
                    {
                        if (i == 0)
                        {
                            if (hm.hasRedLost == false)
                            {
                                p1.left_kick();
                                hm.isBlueAttacking = true;
                                hm.damage = hm.leftKick;
                            }
                               
                        }
                        else
                        {
                            if (hm.hasBlueLost == false)
                            {
                                p2.left_kick();
                                hm.isRedAttacking = true;
                                hm.damage = hm.leftKick;
                            }
                                
                        }// End if / else

                    }//End if

                }// End for

            }// End if

        }// End outer if

        // Right Kick Gesture
        if (e.GestureID == right_kick)
        {

            if (e.DetectionConfidence > 0.3)
            {

                for (int i = 0; i < players.Count; i++)
                {

                    Body b = players[i];
                    if (b.TrackingId == trackingId)
                    {
                        if (i == 0)
                        {
                            if (hm.hasRedLost == false)
                            {
                                p1.right_kick();
                                hm.isBlueAttacking = true;
                                hm.damage = hm.rightKick;
                            }
                                
                        }
                        else
                        {
                            if (hm.hasBlueLost == true)
                            {
                                p2.right_kick();
                                hm.isRedAttacking = true;
                                hm.damage = hm.rightKick;
                            }
                           
                        }// End if / else

                    }//End if

                }// End for

            }// End if

        }// End outer if

        // Win Gesture
        if (e.GestureID == win)
        {

            if (e.DetectionConfidence > 0.4)
            {

                for (int i = 0; i < players.Count; i++)
                {
                    Body b = players[i];
                    if (b.TrackingId == trackingId)
                    {
                        if (i == 0)
                        {
                            if (hm.hasRedLost == true)
                            {
                                p1.win();
                            }
                                
                        }
                        else
                        {
                            if (hm.hasBlueLost == true)
                            {
                                p2.win();
                            }
           
                        }// End if /else

                    }// End if

                }// End for

            }// End if

        }// End outer if

    }// End OnGestureDetected

    private void OnRightLeanGestureDetected(object sender, GestureEventArgs e, int bodyIndex)
    {
        var isDetected = e.IsBodyTrackingIdValid && e.IsGestureDetected;

        //NEW UI FOR GESTURE DETECTed
        //GestureTextGameObject.text = "Gesture Detected: " + isDetected;
        //StringBuilder text = new StringBuilder(string.Format("Gesture Detected? {0}\n", isDetected));
        //ConfidenceTextGameObject.text = "Confidence: " + e.DetectionConfidence;
        //text.Append(string.Format("Confidence: {0}\n", e.DetectionConfidence));
        if (e.DetectionConfidence > 0.65f)
        {
           // turnScript.turnRight = true;
        }
        else
        {
           // turnScript.turnRight = false;
        }

        //this.bodyText[bodyIndex] = text.ToString();
    }

    void OnApplicationQuit()
    {
        if (this.colorFrameReader != null)
        {
            this.colorFrameReader.Dispose();
            this.colorFrameReader = null;
        }

        if (this.bodyFrameReader != null)
        {
            this.bodyFrameReader.Dispose();
            this.bodyFrameReader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }

    }// End OnApplicationQuit

}// End class KinectManager




