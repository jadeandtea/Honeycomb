using System.Collections.Generic;

public class Level {
    public static List<Point> map;
    public static List<Point> obstacles;
    public static List<Point> flowers;
    public static List<Point> pushables;

    public static void loadLevel() {
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
            default:
                map = lvl1_Tile;
                obstacles = lvl1_Obs;
                pushables = lvl1_Push;
                flowers = lvl1_Flower;
                break;
        }
    }

    public static void loadLevel(int levelNumber) {
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
            default:
                map = lvl1_Tile;
                obstacles = lvl1_Obs;
                pushables = lvl1_Push;
                flowers = lvl1_Flower;
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
        new Point(-1, 1), 
        new Point(1, -1), 
        new Point(4, -3), 
        new Point(1, 0), 
        new Point(-1, 0), 
        new Point(0, -1), 
    };

    static List<Point> lvl2_Obs = new List<Point>{
    };

    static List<Point> lvl2_Push = new List<Point>{
        new Point(1, 0), 
        new Point(-1, 0), 
        new Point(0, -1)
    };


    static List<Point> lvl2_Flower = new List<Point>{
        new Point(4, -3)
    };

    static List<Point> lvl3_Tile = new List<Point>{
    };

    static List<Point> lvl3_Obs = new List<Point>{
    };

    static List<Point> lvl3_Push = new List<Point>{
    };


    static List<Point> lvl3_Flower = new List<Point>{
    };
}
