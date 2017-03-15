using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using System;

public class KinectManager : MonoBehaviour {

    // public Text GestureTextGameObject;
    //public Text ConfidenceTextGameObject;
    //public GameObject Player;
    //private Turning turnScript;

    // Kinect 
    private KinectSensor _Sensor;

    // color frame and data 
    private ColorFrameReader colorFrameReader;
    private byte[] colorData;
    private Texture2D colorTexture;

    private BodyFrameReader bodyFrameReader;
    private int bodyCount;
    private Body[] bodies;

    //private string leanLeftGestureName = "Lean_Left";
    //private string leanRightGestureName = "Lean_Right";
    private string straightPunch = "Right_Straight_Punch_Right";

    // GUI output
    //private UnityEngine.Color[] bodyColors;
    //private string[] bodyText;

    /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
    private List<GestureDetector> gestureDetectorList = null;

    // Use this for initialization
    void Start()
    {

        // Get the sensor
        this._Sensor = KinectSensor.GetDefault();

        // If sensor is working
        if (this._Sensor != null)
        {

            bodyFrameReader = this._Sensor.BodyFrameSource.OpenReader();

            this.bodyCount = this._Sensor.BodyFrameSource.BodyCount;

            this.bodies = new Body[this.bodyCount];

            /*if (!this._Sensor.IsOpen)
            {
                this._Sensor.Open();
               
            }*/

            this.gestureDetectorList = new List<GestureDetector>();

            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                this.gestureDetectorList.Add(new GestureDetector(this._Sensor));
               
            }

            // Open the sensor
            this._Sensor.Open();
            Debug.Log("Kinect is open");

        }// End if

     }// End Start

    // Update is called once per frame
    void Update()
    {

        // process bodies
        bool newBodyData = false;

        using (BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame())
        {
            if (bodyFrame != null)
            {
                bodyFrame.GetAndRefreshBodyData(this.bodies);
                newBodyData = true;

                //Debug.Log("you Andrejs");
            }
        }

        if (newBodyData)
        {
            //Debug.Log("New Body Data " + newBodyData);

            // update gesture detectors with the correct tracking id
            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                var body = this.bodies[bodyIndex];
                if (body != null)
                {
                    var trackingId = body.TrackingId;

                    //Debug.Log(trackingId);

                    // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                    if (trackingId != this.gestureDetectorList[bodyIndex].TrackingId)
                    {
                        //GestureTextGameObject.text = "none";
                        //this.bodyText[bodyIndex] = "none";
                        this.gestureDetectorList[bodyIndex].TrackingId = trackingId;

                        // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                        // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                        this.gestureDetectorList[bodyIndex].IsPaused = (trackingId == 0);
                        //Debug.Log("Above");
                        this.gestureDetectorList[bodyIndex].OnGestureDetected += CreateOnGestureHandler(bodyIndex);
                        //Debug.Log("Below");

                    }
                }
            }
        }


       

    }// End Update

    private EventHandler<GestureEventArgs> CreateOnGestureHandler(int bodyIndex)
    {
        return (object sender, GestureEventArgs e) => OnGestureDetected(sender, e, bodyIndex);
    }

    private void OnGestureDetected(object sender, GestureEventArgs e, int bodyIndex)
    {
        
        var isDetected = e.IsBodyTrackingIdValid && e.IsGestureDetected;

        //Debug.Log(e.GestureID);

        if (e.GestureID == straightPunch)
        {

            //NEW UI FOR GESTURE DETECTed
            //GestureTextGameObject.text = "Gesture Detected: " + isDetected;
            //StringBuilder text = new StringBuilder(string.Format("Gesture Detected? {0}\n", isDetected));
           // ConfidenceTextGameObject.text = "Confidence: " + e.DetectionConfidence;
            //text.Append(string.Format("Confidence: {0}\n", e.DetectionConfidence));
            if (e.DetectionConfidence > 0.65f)
            {
                // turnScript.turnLeft = true;
                Debug.Log("Punch Punch Punch");
            }
            else
            {
                //turnScript.turnLeft = false;
            }
        }

        /*if (e.GestureID == leanRightGestureName)
        {
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
        }*/

        //this.bodyText[bodyIndex] = text.ToString();
    }

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
    }

    // bodies = new Body[this.kinectSensor.BodyFrameSource.BodyCount];

    /* this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

     var id = this.kinectSensor.UniqueKinectId;

     Debug.Log("id is " + id);

     Debug.Log("Body count is " + this.bodyCount);*/

    /*if (this.kinectSensor != null)
    {
        this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

        // color reader
        this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

        // create buffer from RGBA frame description
        var desc = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);


        // body data
        this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

        // body frame to use
        this.bodies = new Body[this.bodyCount];

        // initialize the gesture detection objects for our gestures
        //this.gestureDetectorList = new List<GestureDetector>();
        for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
        {
            //PUT UPDATED UI STUFF HERE FOR NO GESTURE
            //GestureTextGameObject.text = "none";
            //this.bodyText[bodyIndex] = "none";
            //this.gestureDetectorList.Add(new GestureDetector(this.kinectSensor));
        }

        // start getting data from runtime
        this.kinectSensor.Open();
    }
    else
    {
        //kinect sensor not connected
    }*/



    // ========================  Put in update  ================================

    /*
        // process bodies
        bool newBodyData = false;

        using (BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame())
        {
            if (bodyFrame != null)
            {
                bodyFrame.GetAndRefreshBodyData(this.bodies);
                newBodyData = true;

                Debug.Log("you Andrejs");
            }
        }

        if (newBodyData)
        {
            // update gesture detectors with the correct tracking id
            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                var body = this.bodies[bodyIndex];
                if (body != null)
                {
                    var trackingId = body.TrackingId;

                    Debug.Log(trackingId);

                    // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                    if (trackingId != this.gestureDetectorList[bodyIndex].TrackingId)
                    {
                        //GestureTextGameObject.text = "none";
                        //this.bodyText[bodyIndex] = "none";
                        this.gestureDetectorList[bodyIndex].TrackingId = trackingId;

                        // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                        // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                        this.gestureDetectorList[bodyIndex].IsPaused = (trackingId == 0);
                        this.gestureDetectorList[bodyIndex].OnGestureDetected += CreateOnGestureHandler(bodyIndex);
                    }
                }
            }
        }
        */

}




