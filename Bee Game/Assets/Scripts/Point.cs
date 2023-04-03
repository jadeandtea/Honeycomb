using System;

[Serializable]
public class Point : IEquatable<Point>{
    //Simple Point class to hold Axial coordinates for anything on the hexagon grid.
    public int x, y;

    public Point(int x, int y) {this.x = x; this.y = y;}
    public Point(Point point) {this.x = point.x; this.y = point.y;}
    public Point() {this.x = 0; this.y = 0;}

    public void Set(int x, int y) {this.x = x; this.y = y;}
    public void Copy(Point point) {this.x = point.x; this.y = point.y;}
    public void Add(Point point) {this.x += point.x; this.y += point.y;}
    public void Add(int x, int y) {this.x += x; this.y += y;}

    public bool Equals(Point point) {
        // Returns true if the point values are the same.
        return this.x == point.x && this.y == point.y;
    }
}