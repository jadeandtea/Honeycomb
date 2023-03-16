using System;

[Serializable]
public class Point : IEquatable<Point>{
    public int x, y;

    public Point(int x, int y) {this.x = x; this.y = y;}
    public Point(Point point) {this.x = point.x; this.y = point.y;}
    public Point() {this.x = 0; this.y = 0;}

    public void Set(int x, int y) {this.x = x; this.y = y;}
    public void Copy(Point point) {this.x = point.x; this.y = point.y;}

    public bool Equals(Point point) {
        if (this.x == point.x && this.y == point.y) {
            return true;
        }
        return false;
    }
}