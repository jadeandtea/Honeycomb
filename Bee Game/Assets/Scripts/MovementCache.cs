using System.Collections.Generic;

public class MovementCache 
{
    Point playerLocation;
    List<Obstacle> obstacleLocations;
    List<Tile> tileLocations;
    List<int> flowersTouched;

    public MovementCache(Point player, List<Obstacle> obstacles, List<Tile> map, List<int> flowersTouched) {
        this.playerLocation = player;
        this.obstacleLocations = obstacles;
        this.tileLocations = map;
        this.flowersTouched = flowersTouched;
    }

    public Point getPlayerLocation() {
        return playerLocation;
    }

    public List<Obstacle> getObstacles() {
        return obstacleLocations;
    }

    public List<Tile> getTiles() {
        return tileLocations;
    }

    public List<int> getFlowers() {
        return flowersTouched;
    }
}
