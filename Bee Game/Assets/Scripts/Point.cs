using UnityEngine;

public class Point {
    public int x, y;

    public Point(int x, int y) {this.x = x; this.y = y;}

    public void Set(int x, int y) {this.x = x; this.y = y;}
    public void Set(float x, float y) {this.x = (int) x; this.y = (int) y;}
}