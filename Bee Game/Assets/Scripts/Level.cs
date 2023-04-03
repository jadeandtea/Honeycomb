using System.Collections.Generic;

public class Level {
    List<Point> obstacles;
    List<Point> map;
    List<Point> flowers;
    
    public int initialFlightLength;
    public int levelNumber;

    public Level(List<Point> obstacles, List<Point> map, List<Point> flowers, int levelNumber) {
        this.obstacles = obstacles;
        this.map = map;
        this.flowers = flowers;
        this.levelNumber = levelNumber;
    }

    public Level() {
        //Don't remove yet, used to get rid of compile erros
    }

    public List<Point> getObstacles() {
        return obstacles;
    }

    public List<Point> getMap() {
        return map;
    }

    public List<Point> getFlowers() {
        return flowers;
    }

    public override bool Equals(object obj) {
        return obj is Level level && EqualityComparer<int>.Default.Equals(levelNumber, level.levelNumber);
    }

    public bool Equals(int levelNumber){
        return this.levelNumber == levelNumber;
    }
    //Unecessary
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    //Unecessary
    public override string ToString()
    {
        return "Level #" + levelNumber;
    }
}
