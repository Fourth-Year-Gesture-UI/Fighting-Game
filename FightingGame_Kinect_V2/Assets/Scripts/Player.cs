using Windows.Kinect;

// Player class is representing player that is in the player queue.
// Properties for the player is number, tracking id and character
// that the current player is controlling.

public class Player
{
    // properties
    public int playerNumber { get; set; }
    public Body body { get; set; }
    public char playerControl { get; set; }
    public ulong trackingId { get; set; }

    // Constructions
    public Player(int pn, Body b, ulong tId) // parameterized
    {
        playerNumber = pn;
        body = b;
        trackingId = tId;
    }
}