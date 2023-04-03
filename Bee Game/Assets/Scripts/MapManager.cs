using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    List<Point> openCoordinates;
    List<Tile> tiles;
    List<Obstacle> obstacles;
    List<Flower> flowers;

    public MapManager() {
        openCoordinates = new List<Point>();
        tiles = new List<Tile>();
        obstacles = new List<Obstacle>();
        flowers = new List<Flower>();
    }

    public MapManager(List<Point> openCoordinates) {
        this.openCoordinates = openCoordinates;
    }

    public void loadMap(int mapSize, float hexagonSize, float outlineSize, Color hexColor, Transform parent) {
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Tile tempTileRef = new Tile(new Point(x, y), hexagonSize, 1 - outlineSize, MapRender.HexType.Flat, hexColor, parent);
                    
                    tiles.Add(tempTileRef);

                    Add(new Point(x, y));
                }
            }
        }
    }

    public void loadMap(List<Point> levelMap, float hexagonSize, float outlineSize, Color hexColor, Transform parent) {
        //Loading a map creates the game objects

        foreach(Point coord in levelMap) {
            Tile tempTileRef = new Tile(coord, hexagonSize, 1 - outlineSize, MapRender.HexType.Flat, hexColor, parent);
            tiles.Add(tempTileRef);

            Add(coord);
        }
    }

    public void loadObstacles(List<Point> obstacleList, float hexagonSize, float outlineSize, Color obsColor, Transform parent) {
        //Creates Obstacle Game Objects

        foreach(Point coord in obstacleList) {
            int s = -coord.x - coord.y;
            Obstacle tempObsRef = new Obstacle(coord, hexagonSize, 1 - outlineSize, MapRender.HexType.Flat, obsColor, parent);

            obstacles.Add(tempObsRef);
            Remove(coord);
        }
    }

    public void loadFlowers(List<Point> flowerList, float hexagonSize, string[] flowerSpriteList, MapRender.HexType type, Transform parent) {
        //Creates FlowerRender Game Objects

        foreach(Point coord in flowerList) {
            int s = -coord.x - coord.y;
            
            int flowerIndex = Random.Range(0, flowerSpriteList.Length);
            GameObject tempFlowerRef = new GameObject("(" + coord.x + ", " + coord.y + ", " + s + ")");
            tempFlowerRef.transform.position = Vector3.zero;

            Flower flower = new Flower(tempFlowerRef, flowerSpriteList[flowerIndex], coord, hexagonSize, type);

            flowers.Add(flower);

            tempFlowerRef.transform.SetParent(parent);
        }
    }

    public void moveObstacle(int index, Point movementDir, float hexagonSize, float outlineSize, MapRender.HexType type, Color hexColor, Transform parent) {
        // Add previous obstacle position to possible movement tiles,
        // shift the hexagon's map coordinate, then remove the new position
        // from possible movement tiles.

        //Make previous obstacle location free
        openCoordinates.Add(new Point(obstacles[index].getCoord()));

        //Move obstacle to new position
        obstacles[index].moveObstacle(movementDir);

        //If the new obstacle position isn't on another tile
        if (!openCoordinates.Remove(new Point(obstacles[index].getCoord()))) {
            Obstacle tempRef = obstacles[index];

            //Make it into a new tile
            openCoordinates.Add(new Point(tempRef.getCoord()));
            Tile tempTileRef = new Tile(tempRef.getCoord(), hexagonSize, 1 - outlineSize, type, hexColor, parent);
            tiles.Add(tempTileRef);

            //Remove the obstacle from the list of obstacles
            obstacles.Remove(new Obstacle(obstacles[index].getCoord()));
            tempRef.Destroy();
        }
    }

    private void Add(Point point) {
        openCoordinates.Add(point);
    }

    private bool Remove(Point point) {
        return openCoordinates.Remove(point);
    }

    public List<Point> getCoordinates() {
        return this.openCoordinates;
    }

    public List<Tile> getTiles() {
        return this.tiles;
    }

    public List<Obstacle> getObstacles() {
        return this.obstacles;
    }

    public List<Flower> getFlowers() {
        return this.flowers;
    }

    IEnumerable resizeHex() {
        yield return null;
    }
}
