using System.Collections.Generic;

public static class Level {
    public static List<Point> map;
    public static List<Point> obstacles;
    public static List<Point> flowers;
    public static List<Point> pushables;

    public static void loadLevel() {
        // Not very scalable, but works for what I have
        int levelNumber = LevelManager.currentLevelNumber;
        switch (levelNumber){
            case 1:
                map = lvl1_Tile;
                obstacles = lvl1_Obs;
                pushables = lvl1_Push;
                flowers = lvl1_Flower;
                break;
            case 2:
                map = lvl2_Tile;
                obstacles = lvl2_Obs;
                pushables = lvl2_Push;
                flowers = lvl2_Flower;
                break;
            case 3:
                map = lvl3_Tile;
                obstacles = lvl3_Obs;
                pushables = lvl3_Push;
                flowers = lvl3_Flower;
                break;
            case 4:
                map = lvl4_Tile;
                obstacles = lvl4_Obs;
                pushables = lvl4_Push;
                flowers = lvl4_Flower;
                break;
            case 5:
                map = lvl5_Tile;
                obstacles = lvl5_Obs;
                pushables = lvl5_Push;
                flowers = lvl5_Flower;
                break;
            case 6:
                map = lvl6_Tile;
                obstacles = lvl6_Obs;
                pushables = lvl6_Push;
                flowers = lvl6_Flower;
                break;
            case 7:
                map = lvl7_Tile;
                obstacles = lvl7_Obs;
                pushables = lvl7_Push;
                flowers = lvl7_Flower;
                break;
            case 8:
                map = lvl8_Tile;
                obstacles = lvl8_Obs;
                pushables = lvl8_Push;
                flowers = lvl8_Flower;
                break;
            case 9:
                map = lvl9_Tile;
                obstacles = lvl9_Obs;
                pushables = lvl9_Push;
                flowers = lvl9_Flower;
                break;
            case 10:
                map = lvl10_Tile;
                obstacles = lvl10_Obs;
                pushables = lvl10_Push;
                flowers = lvl10_Flower;
                break;
            default:
                map = new List<Point>{new Point(0, 0)};
                obstacles = new List<Point>();
                pushables = new List<Point>();
                flowers = new List<Point>();
                break;
        }
    }

    static List<Point> lvl1_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(0, 1), 
        new Point(1, 0), 
        new Point(-1, 1), 
        new Point(1, 1), 
        new Point(-1, 2), 
        new Point(-2, 2), 
        new Point(2, 0), 
        new Point(-1, 3), 
        new Point(-2, 3), 
        new Point(2, 1), 
        new Point(1, 2), 
        new Point(2, 2), 
        new Point(1, 3), 
        new Point(0, 4), 
        new Point(-1, 4), 
        new Point(-2, 4), 
        new Point(0, 3)
    };
    static List<Point> lvl1_Obs = new List<Point>{
    };
    static List<Point> lvl1_Push = new List<Point>{
    };
    static List<Point> lvl1_Flower = new List<Point>{
        new Point(0, 3)
    };
    
    static List<Point> lvl2_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(0, 1), 
        new Point(0, -1), 
        new Point(3, 0), 
        new Point(-3, 0), 
        new Point(1, 0), 
        new Point(-1, 0), 
        new Point(1, 1), 
        new Point(-1, -1), 
    };
    static List<Point> lvl2_Obs = new List<Point>{
        new Point(1, 0), 
        new Point(-1, 0),  
    };
    static List<Point> lvl2_Push = new List<Point>{
        new Point(1, 1), 
        new Point(-1, -1),
    };
    static List<Point> lvl2_Flower = new List<Point>{
        new Point(3, 0), 
        new Point(-3, 0), 
    };


    static List<Point> lvl3_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(0, 1), 
        new Point(-1, 1), 
        new Point(1, -1), 
        new Point(4, -3), 
        new Point(1, 0), 
        new Point(-1, 0), 
        new Point(0, -1), 
    };
    static List<Point> lvl3_Obs = new List<Point>{
    };
    static List<Point> lvl3_Push = new List<Point>{
        new Point(1, 0), 
        new Point(-1, 0), 
        new Point(0, -1)
    };
    static List<Point> lvl3_Flower = new List<Point>{
        new Point(4, -3)
    };


    static List<Point> lvl4_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(7, -4), 
        new Point(0, 1), 
        new Point(-1, 1), 
        new Point(-1, 0), 
        new Point(0, -1), 
        new Point(1, -1), 
        new Point(1, 0), 
        new Point(1, 1), 
        new Point(-1, 2), 
        new Point(-1, -1), 
        new Point(-2, 1), 
        new Point(2, -1), 
        new Point(1, -2),
    };
    static List<Point> lvl4_Obs = new List<Point>{
    };
    static List<Point> lvl4_Push = new List<Point>{
        new Point(0, 1), 
        new Point(-1, 1), 
        new Point(-1, 0), 
        new Point(0, -1), 
        new Point(1, -1), 
        new Point(1, 0), 
        new Point(1, 1), 
        new Point(-1, 2), 
        new Point(-1, -1), 
        new Point(-2, 1), 
        new Point(2, -1), 
        new Point(1, -2), 
    };
    static List<Point> lvl4_Flower = new List<Point>{
        new Point(7, -4), 
    };


    static List<Point> lvl5_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(-1, 1), 
        new Point(-2, 0), 
        new Point(-1, 0), 
        new Point(-3, 0), 
        new Point(-3, 4), 
        new Point(-1, 3), 
        new Point(-2, 1), 
        new Point(-3, 1), 
        new Point(-1, 2),
    };
    static List<Point> lvl5_Obs = new List<Point>{
        
    };
    static List<Point> lvl5_Push = new List<Point>{
        new Point(-2, 1), 
        new Point(-3, 1), 
        new Point(-1, 2), 
    };
    static List<Point> lvl5_Flower = new List<Point>{
        new Point(-3, 4), 
        new Point(-1, 3), 
    };

    static List<Point> lvl6_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(-2, 2), 
        new Point(0, 1), 
        new Point(1, 0), 
        new Point(-1, 1), 
        new Point(-2, 3), 
        new Point(-1, 3), 
        new Point(5, 0), 
        new Point(5, -3), 
        new Point(0, 3), 
        new Point(0, 2), 
        new Point(1, 1), 
        new Point(2, 0), 
        new Point(1, 2), 
        new Point(2, 1),  
    };
    static List<Point> lvl6_Obs = new List<Point>{
        new Point(0, 3), 
    };
    static List<Point> lvl6_Push = new List<Point>{
        new Point(0, 2), 
        new Point(1, 1), 
        new Point(2, 0), 
        new Point(1, 2), 
        new Point(2, 1), 
    };
    static List<Point> lvl6_Flower = new List<Point>{
        new Point(5, 0), 
        new Point(5, -3), 
    };

    static List<Point> lvl7_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(-2, 2), 
        new Point(-3, 3), 
        new Point(2, -2), 
        new Point(3, -3), 
        new Point(-2, 0), 
        new Point(-3, 0), 
        new Point(2, 0), 
        new Point(3, 0), 
        new Point(-4, 2), 
        new Point(4, -2), 
        new Point(0, 1), 
        new Point(0, -1), 
        new Point(-1, 1), 
        new Point(1, -1), 
        new Point(-1, 0), 
        new Point(1, 0),
    };
    static List<Point> lvl7_Obs = new List<Point>{
    };
    static List<Point> lvl7_Push = new List<Point>{
        new Point(0, 1), 
        new Point(0, -1), 
        new Point(-1, 1), 
        new Point(1, -1), 
        new Point(-1, 0), 
        new Point(1, 0),
    };
    static List<Point> lvl7_Flower = new List<Point>{
        new Point(-4, 2), 
        new Point(4, -2),
    };

    static List<Point> lvl8_Tile = new List<Point>{
        new Point(0, 0), 
        new Point(1, 0), 
        new Point(-1, 2), 
        new Point(0, 4), 
        new Point(-3, 4), 
        new Point(0, 3), 
        new Point(2, 0), 
        new Point(-2, 3), 
        new Point(-1, 1), 
        new Point(0, 1), 
        new Point(1, 1), 
        new Point(0, 2), 
        new Point(-2, 2),
    };
    static List<Point> lvl8_Obs = new List<Point>{
        new Point(0, 3), 
        new Point(2, 0), 
        new Point(-2, 3), 
    };
    static List<Point> lvl8_Push = new List<Point>{
        new Point(-1, 1), 
        new Point(0, 1), 
        new Point(1, 1), 
        new Point(0, 2), 
        new Point(-2, 2),
    };
    static List<Point> lvl8_Flower = new List<Point>{
        new Point(0, 4), 
        new Point(-3, 4),
    };

    static List<Point> lvl9_Tile = new List<Point>{

    };
    static List<Point> lvl9_Obs = new List<Point>{

    };
    static List<Point> lvl9_Push = new List<Point>{

    };
    static List<Point> lvl9_Flower = new List<Point>{

    };

    static List<Point> lvl10_Tile = new List<Point>{

    };
    static List<Point> lvl10_Obs = new List<Point>{

    };
    static List<Point> lvl10_Push = new List<Point>{

    };
    static List<Point> lvl10_Flower = new List<Point>{

    };
}
