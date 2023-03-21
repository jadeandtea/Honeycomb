using System.Collections.Generic;

public class Level {
    List<Point> obstacles;
    List<Point> map;
    List<Point> flowers;
    
    public int initialFlightLength;
    public int levelNumber;

    public Level(List<Point> obstacles, List<Point> map, List<Point> flowers, int flightLength, int levelNumber) {
        this.obstacles = obstacles;
        this.map = map;
        this.flowers = flowers;
        this.initialFlightLength = flightLength;
        this.levelNumber = levelNumber;
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

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "Level #" + levelNumber;
    }
}
