﻿using System;
using System.IO;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine;

public class GestureEventArgs : EventArgs
{
    public bool IsBodyTrackingIdValid { get; private set; }

    public bool IsGestureDetected { get; private set; }

    public float DetectionConfidence { get; private set; }

    //my modification
    public string GestureID { get; private set; }

    // A mod
    public GestureEventArgs(bool isBodyTrackingIdValid, bool isGestureDetected, float detectionConfidence, string gestureID)
    {
        this.IsBodyTrackingIdValid = isBodyTrackingIdValid;
        this.IsGestureDetected = isGestureDetected;
        this.DetectionConfidence = detectionConfidence;
        this.GestureID = gestureID;
    }
}

/// <summary>
/// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
/// and calls the OnGestureDetected event handler when a gesture is detected.
/// </summary>
public class GestureDetector : IDisposable
{
    /// <summary> Path to the gesture database that was trained with VGB </summary>
    private readonly string straight_punchDB = "GestureDB\\Kinect_Gesture_Straight_Punch.gbd";
    private readonly string left_punchDB = "GestureDB\\Left_Punch.gbd";
    private readonly string blockDB = "GestureDB\\Block.gbd";
    private readonly string left_kickDB = "GestureDB\\Left_Kick.gbd";
    private readonly string right_kickDB = "GestureDB\\Right_Kick.gbd";
    private readonly string winDB = "GestureDB\\Win.gbd";

    /// <summary> Name of the discrete gesture in the database that we want to track </summary>
   // private readonly string leanLeftGestureName = "Lean_Left";
    //private readonly string leanRightGestureName = "Lean_Right";

    // ============================  Very Important  ============================
    // *****        Need to have same name as in gesture database          ******
    // ==========================================================================
    private readonly string straightPunch = "Right_Straight_Punch_Right";
    private readonly string left_punch = "Left_Punch_Left";
    private readonly string block = "Block";
    private readonly string left_kick = "Left_Kick_Left";
    private readonly string right_kick = "Right_Kick_Right";
    private readonly string win = "Win";

    /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
    private VisualGestureBuilderFrameSource vgbFrameSource = null;

    /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
    private VisualGestureBuilderFrameReader vgbFrameReader = null;

    public event EventHandler<GestureEventArgs> OnGestureDetected;

    /// <summary>
    /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
    /// </summary>
    /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
    public GestureDetector(KinectSensor kinectSensor)
    {
        if (kinectSensor == null)
        {
            throw new ArgumentNullException("kinectSensor");
        }

        // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
        this.vgbFrameSource = VisualGestureBuilderFrameSource.Create(kinectSensor, 0);
        this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

        // open the reader for the vgb frames
        this.vgbFrameReader = this.vgbFrameSource.OpenReader();
        if (this.vgbFrameReader != null)
        {
            this.vgbFrameReader.IsPaused = true;
            this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
        }

        // load the 'Seated' gesture from the gesture database
        var databasePath = Path.Combine(Application.streamingAssetsPath, this.straight_punchDB);
        var databasePathLeftPunch = Path.Combine(Application.streamingAssetsPath, this.left_punchDB);
        var databasePathBlock = Path.Combine(Application.streamingAssetsPath, this.blockDB);
        var databasePathLeftKick = Path.Combine(Application.streamingAssetsPath, this.left_kickDB);
        var databasePathRightKick = Path.Combine(Application.streamingAssetsPath, this.right_kickDB);
        var databasePathWin = Path.Combine(Application.streamingAssetsPath, this.winDB);

        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {
                if (gesture.Name.Equals(this.straightPunch))
                {
                    this.vgbFrameSource.AddGesture(gesture);
                }
            }
        }

        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePathBlock))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {
               
                if (gesture.Name.Equals(this.block))
                {
                    this.vgbFrameSource.AddGesture(gesture);
                }
            }
        }

        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePathLeftPunch))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {

                if (gesture.Name.Equals(this.left_punch))
                {
                    this.vgbFrameSource.AddGesture(gesture);
                }
            }
        }

        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePathLeftKick))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {

                if (gesture.Name.Equals(this.left_kick))
                {
                    this.vgbFrameSource.AddGesture(gesture);
                }
            }
        }

        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePathRightKick))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {

                if (gesture.Name.Equals(this.right_kick))
                {
                    this.vgbFrameSource.AddGesture(gesture);
                }
            }
        }

        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePathWin))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {

                if (gesture.Name.Equals(this.win))
                {
                    this.vgbFrameSource.AddGesture(gesture);
                }
            }
        }

    }

    /// <summary>
    /// Gets or sets the body tracking ID associated with the current detector
    /// The tracking ID can change whenever a body comes in/out of scope
    /// </summary>
    public ulong TrackingId
    {
        get
        {
            return this.vgbFrameSource.TrackingId;
        }

        set
        {
            if (this.vgbFrameSource.TrackingId != value)
            {
                this.vgbFrameSource.TrackingId = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the detector is currently paused
    /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
    /// </summary>
    public bool IsPaused
    {
        get
        {
            return this.vgbFrameReader.IsPaused;
        }

        set
        {
            if (this.vgbFrameReader.IsPaused != value)
            {
                this.vgbFrameReader.IsPaused = value;
            }
        }
    }

    /// <summary>
    /// Disposes all unmanaged resources for the class
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
    /// </summary>
    /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                this.vgbFrameReader.Dispose();
                this.vgbFrameReader = null;
            }

            if (this.vgbFrameSource != null)
            {
                this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                this.vgbFrameSource.Dispose();
                this.vgbFrameSource = null;
            }
        }
    }

    /// <summary>
    /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null)
            {
                // get the discrete gesture results which arrived with the latest frame
                var discreteResults = frame.DiscreteGestureResults;

                if (discreteResults != null)
                {
                    // we only have one gesture in this source object, but you can get multiple gestures
                    foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                    {

                        // Right Punch Gesture
                        if (gesture.Name.Equals(this.straightPunch) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null)
                            {
                                if (this.OnGestureDetected != null)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence, this.straightPunch));
                                }
                            }
                        }

                        // Left Punch Gesture
                        if (gesture.Name.Equals(this.left_punch) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null)
                            {
                                if (this.OnGestureDetected != null)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence, this.left_punch));
                                }
                            }
                        }

                        // Block Gesture
                        if (gesture.Name.Equals(this.block) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null)
                            {
                                if (this.OnGestureDetected != null)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence, this.block));
                                }
                            }
                        }

                        // Right Kick Gesture
                        if (gesture.Name.Equals(this.right_kick) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null)
                            {
                                if (this.OnGestureDetected != null)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence, this.right_kick));
                                }
                            }
                        }

                        // Left Kick Gesture
                        if (gesture.Name.Equals(this.left_kick) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null)
                            {
                                if (this.OnGestureDetected != null)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence, this.left_kick));
                                }
                            }
                        }

                        // Win Gesture
                        if (gesture.Name.Equals(this.win) && gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null)
                            {
                                if (this.OnGestureDetected != null)
                                {
                                    this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence, this.win));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
    {
        if (this.OnGestureDetected != null)
        {
            this.OnGestureDetected(this, new GestureEventArgs(false, false, 0.0f, "none"));
        }
    }

}// End GestureDetector

