using System.Collections.Generic;

public static class Levels {
    public static List<Point> lvl_1_Obs = new List<Point>{
        new Point(-1, 0)
    };
    public static List<Point> lvl_1_Map = new List<Point>{
        new Point(-1, 0), 
        new Point(-1, 1), 
        new Point(0, 0), 
        new Point(0, 1), 
        new Point(0, 2), 
        new Point(0, 3), 
        new Point(0, 4), 
        new Point(1, 2),
        new Point(2, 1)
    };
    public static List<Point> lvl_1_Flowers = new List<Point>{
        new Point(0, 4),
        new Point(2, 1)
    };
}
