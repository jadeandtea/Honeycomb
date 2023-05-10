using System.Collections.Generic;

public struct MovementCache 
{
    // Struct to help manage the undo system
    public enum movedObject {
        Player, Obstacle
    }

    public movedObject mObject;
    public Point previousPoint {get; set;}
    public Point newPoint {get; set;}

    public MovementCache(Point pPoint, Point nPoint, movedObject obj) {
        previousPoint = pPoint;
        newPoint = nPoint;
        mObject = obj;
    }
}
