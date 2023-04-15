using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    playerMovement player;
    MapSettings settings;
    List<Point> openCoordinates;
    Dictionary<Point, Tile> tiles;
    Dictionary<Point, Obstacle> obstacles;
    List<Flower> flowers;
    Stack<MovementCache> history;
    List<int> flowersTouched;

    bool editMode;

    public MapManager(MapSettings settings, bool editMode = false) {
        this.editMode = editMode;
        this.settings = settings;

        openCoordinates = new List<Point>();

        tiles = new Dictionary<Point, Tile>();
        obstacles = new Dictionary<Point, Obstacle>();
        flowers = new List<Flower>();
        flowersTouched = new List<int>();


        history = new Stack<MovementCache>();
    }

    public void loadMap(int mapSize, Transform parent) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize

        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Tile tempTileRef = new Tile(settings, new Point(x, y), parent, editMode);
                    
                    tiles.Add(new Point(x, y), tempTileRef);
                }
            }
        }
    }

    public void loadMap(List<Point> levelMap, Transform parent) {
        //Loads a map given a list of points

        foreach(Point coord in levelMap) {
            Tile tempTileRef = new Tile(settings, coord, parent);
            tiles.Add(coord, tempTileRef);
        }
    }

    public void loadObstacles(List<Point> obstacleList, Transform parent) {
        //Creates Obstacle Game Objects

        foreach(Point coord in obstacleList) {
            int s = -coord.x - coord.y;
            Obstacle tempObsRef = new Obstacle(settings, coord, parent);

            obstacles.Add(coord, tempObsRef);
        }
    }

    public void loadFlowers(List<Point> flowerList, string[] flowerSpriteList, MapSettings.HexType type, Transform parent) {
        //Creates Flower Game Objects

        foreach(Point coord in flowerList) {
            int s = -coord.x - coord.y;
            
            int flowerIndex = Random.Range(0, flowerSpriteList.Length);
            GameObject tempFlowerRef = new GameObject("(" + coord.x + ", " + coord.y + ", " + s + ")");
            tempFlowerRef.transform.position = Vector3.zero;

            Flower flower = new Flower(settings, tempFlowerRef, flowerSpriteList[flowerIndex], coord);

            flowers.Add(flower);

            tempFlowerRef.transform.SetParent(parent);
        }
    }

    public void moveObstacle(Point point, Point movementDir, Transform parent) {
        //TODO allow obstacles to push each other in a line

        // Add previous obstacle position to possible movement tiles,
        // shift the hexagon's map coordinate, then remove the new position
        // from possible movement tiles.

        //Move obstacle to new position
        obstacles[point].push(movementDir);

        //If the new obstacle position isn't on another tile 
        if (!openCoordinates.Remove(point)) {
            //Make it into a new tile
            openCoordinates.Add(point);
            Tile tempTileRef = new Tile(settings, point, parent);
            tiles.Add(point, tempTileRef);

            //Disable the obstacle
            Obstacle tempRef = obstacles[point];
            tempRef.isActive = false;
        } else {
            obstacles[point].isActive = true;
        }
    }

    //TODO make obstacles undoable
    public void undoObstacleMove(Point point, Point dir, Transform parent) {
        Obstacle obsRef = obstacles[point];
        if(!obsRef.isActive) {
            //Means that the obstacle has been pushed off the map;
            //Reactivate the obstacle, remove the tile placed
            obsRef.isActive = true;
            openCoordinates.Remove(point);
            tiles.Remove(point);
        }
        //Move the obstacle to the right place
        obsRef.push(dir);
        openCoordinates.Remove(point);
    }

    public void activateTile(Point point) {
        tiles[point].isActive = true;
    }

    public void deactivateTile(Point point) {
        tiles[point].isActive = false;
    }

    private void updateCoordinates(List<Point> coordinates) {
        openCoordinates = coordinates;
    }

    public void updateCoordinates() {
        List<Point> tempCoordinates = new List<Point>();
        foreach(KeyValuePair<Point, Tile> valuePair in tiles) {
            Tile tile = valuePair.Value;
            if(tile.isActive) 
                tempCoordinates.Add(tile.getPoint());
        }
        foreach(KeyValuePair<Point, Obstacle> valuePair in obstacles) {
            Point point = valuePair.Key;
            tempCoordinates.Remove(point);
        }
        updateCoordinates(tempCoordinates);
    }

    public List<Point> getCoordinates() {
        return this.openCoordinates;
    }

    public Dictionary<Point, Tile> getTiles() {
        return this.tiles;
    }

    public Dictionary<Point, Obstacle> getObstacles() {
        return this.obstacles;
    }

    public List<Flower> getFlowers() {
        return this.flowers;
    }
}
