using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public KinectSensor _Sensor;
    private ColorFrameReader colorFrameReader;
    private BodyFrameReader bodyFrameReader;
    private int bodyCount;
    private Body[] bodies;
    private List<Body> trackedBodies;

    // Use this for initialization
    void Start () {

        // Get the sensor
        this._Sensor = KinectSensor.GetDefault();

        bodyFrameReader = this._Sensor.BodyFrameSource.OpenReader();
        this.bodyCount = this._Sensor.BodyFrameSource.BodyCount;
        this.bodies = new Body[this.bodyCount];
        this.trackedBodies = new List<Body> { };

        // Open the sensor
        this._Sensor.Open();

    }
	
	// Update is called once per frame
	void Update () {

        if (this._Sensor.IsOpen)
        {

            //Debug.Log("Connect is open");

        }

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

            if (newBodyData)
            {

                // update gesture detectors with the correct tracking id
                for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
                {
                    var body = this.bodies[bodyIndex];

                    if (body != null)
                    {

                        if (body.IsTracked)
                        {

                            if (body.HandRightState == HandState.Open)
                            {

                               // _Sensor.Close();

                                /*if (!this._Sensor.IsOpen)
                                {

                                    Debug.Log("Gfnjfdjskfi");

                                }*/

                                SceneManager.LoadScene("Main");

                            }

                        }
                    }
                }
            }

        }// End using

    }// End Update 

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
}
