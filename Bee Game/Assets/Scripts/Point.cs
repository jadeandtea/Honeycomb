using System;
using UnityEngine;

[Serializable]
public class Point : IEquatable<Point>{
    //Simple Point class to hold coordinates for anything on the hexagon grid.
    public int x, y;

    public Point(int x, int y) {this.x = x; this.y = y;}
    public Point(Point point) {this.x = point.x; this.y = point.y;}
    public Point() {this.x = 0; this.y = 0;}
    public Point(string point) 
    // Write the string as "x,y" without any other characters
    // FIX Make the input string more flexible
    {
        string[] numbers = point.Split(',');
        int x = int.Parse(numbers[0]);
        int y = int.Parse(numbers[1]);
        this.x = x;
        this.y = y;
    } 

    public void Set(int x, int y) {this.x = x; this.y = y;}
    public Point Add(Point point) {this.x += point.x; this.y += point.y; return this;}
    public Point Add(int x, int y) {this.x += x; this.y += y; return this;}
    public Point Sub(Point point) {this.x -= point.x; this.y -= point.y; return this;}
    public Point Sub(int x, int y) {this.x -= x; this.y -= y; return this;}
    public Point flip() {return new Point(-this.x, -this.y);}

    public bool Equals(Point point) {
        // Returns true if the point values are the same.
        return this.x == point.x && this.y == point.y;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Point);
    }

    public bool Equals(int x, int y) {
        return this.x == x && this.y == y;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() + y.GetHashCode();
    }

    public override string ToString(){
        //Formatted as "(x, y)"
        string tempString = "(";
        tempString += x + ", " + y + ")";
        return tempString;
    }
}