using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    MapSettings settings;
    List<Point> openCoordinates;
    Dictionary<Point, Tile> tiles;
    Dictionary<Point, Obstacle> obstacles;
    Dictionary<Point, Obstacle> pushables;
    Dictionary<Point, Flower> flowers;
    Dictionary<Point, bool> touchedFlowers;
    Stack<MovementCache> history;
    LevelTextManager textManager;

    bool editMode;
    
    public MapManager(MapSettings settings, LevelTextManager textManager, bool editMode = false) {
        this.editMode = editMode;
        this.settings = settings;
        this.textManager = textManager;

        openCoordinates = new List<Point>();

        tiles = new Dictionary<Point, Tile>();
        obstacles = new Dictionary<Point, Obstacle>();
        pushables = new Dictionary<Point, Obstacle>();
        flowers = new Dictionary<Point, Flower>();

        touchedFlowers = new Dictionary<Point, bool>();

        history = new Stack<MovementCache>();
    }

    public void loadMap(int mapSize, Transform parent) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize
        
        tiles = new Dictionary<Point, Tile>();
        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Tile tempTileRef = new Tile(settings, new Point(x, y), parent, editMode);
                    
                    tiles[new Point(x, y)] = tempTileRef;
                }
            }
        }
    }

    public void loadMap(List<Point> levelMap, Transform parent) {
        //Loads a map given a list of points
        if(editMode) {
            loadMap(10, parent);
        } else {
            tiles = new Dictionary<Point, Tile>();

            foreach(Point coord in levelMap) {
                Tile tempTileRef = new Tile(settings, coord, parent);
                tiles[coord] = tempTileRef;
            }
        }
    }

    public void loadObstacles(int mapSize, Transform parent) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize
        
        obstacles = new Dictionary<Point, Obstacle>();
        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Obstacle tempObsRef = new Obstacle(settings, new Point(x, y), parent, false, editMode);
                    
                    obstacles[new Point(x, y)] = tempObsRef;
                }
            }
        }
    }

    public void loadObstacles(List<Point> obstacleList, Transform parent) {
        //Creates Obstacle Game Objects that cannot be pushed

        if(editMode) {
            loadObstacles(10, parent);
        } else {
            obstacles = new Dictionary<Point, Obstacle>();

            foreach(Point coord in obstacleList) {
                int s = -coord.x - coord.y;
                Obstacle tempObsRef = new Obstacle(settings, coord, parent, false, editMode);

                obstacles[coord] = tempObsRef;
            }
        }
    }

    public void loadPushables(int mapSize, Transform parent) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize
        
        pushables = new Dictionary<Point, Obstacle>();
        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Obstacle tempObsRef = new Obstacle(settings, new Point(x, y), parent, true, editMode);
                    
                    pushables[new Point(x, y)] = tempObsRef;
                }
            }
        }
    }

    public void loadPushables(List<Point> pushableList, Transform parent) {
        //Creates Obstacle Game Objects that can be pushed

        if (editMode) {
            loadPushables(10, parent);
        } else {
            pushables = new Dictionary<Point, Obstacle>();

            foreach(Point coord in pushableList) {
                int s = -coord.x - coord.y;
                Obstacle tempObsRef = new Obstacle(settings, coord, parent, true, editMode);

                pushables[coord] = tempObsRef;
            }
        }
    }

    public void loadFlowers(int mapSize, string[] flowerSpriteList, Transform parent) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize
        
        flowers = new Dictionary<Point, Flower>();
        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    int flowerIndex = Random.Range(0, flowerSpriteList.Length);

                    GameObject tempFlowerRef = new GameObject("(" + x + ", " + y + ", " + s + ")");
                    tempFlowerRef.transform.position = Vector3.zero;

                    Flower flower = new Flower(settings, tempFlowerRef, flowerSpriteList[flowerIndex], new Point(x, y), editMode);
                    
                    flowers[new Point(x, y)] = flower;

                    tempFlowerRef.transform.SetParent(parent);

                    touchedFlowers[new Point(x, y)] = false;
                }
            }
        }
    }

    public void loadFlowers(List<Point> flowerList, string[] flowerSpriteList, Transform parent) {
        //Creates Flower Game Objects

        if (editMode) {
            loadFlowers(10, flowerSpriteList, parent);
        } else {

            flowers = new Dictionary<Point, Flower>();

            foreach(Point coord in flowerList) {
                int s = -coord.x - coord.y;
                
                int flowerIndex = Random.Range(0, flowerSpriteList.Length);
                GameObject tempFlowerRef = new GameObject("(" + coord.x + ", " + coord.y + ", " + s + ")");
                tempFlowerRef.transform.position = Vector3.zero;

                Flower flower = new Flower(settings, tempFlowerRef, flowerSpriteList[flowerIndex], coord);

                flowers[coord] = flower;

                tempFlowerRef.transform.SetParent(parent);

                touchedFlowers[coord] = false;
            }
        }
    }

    public void moveObstacle(Point point, Point movementDir, Transform parent) {
        //TODO allow obstacles to push each other in a line

        // Add previous obstacle position to possible movement tiles,
        // shift the hexagon's map coordinate, then remove the new position
        // from possible movement tiles.

        // Change the obstacle reference key and move the obstacle to the next point
        Obstacle pushRef = pushables[point];

        pushables.Remove(point);
        point.Add(movementDir);
        pushables[point] = pushRef;

        if(!tiles.ContainsKey(point)) {
            pushables[point].isActive = false;
            try{
                tiles[point].isActive = true;
            } catch {
                tiles[point] = new Tile(settings, point, parent, editMode);
                tiles[point].setActiveLocation(point.Sub(movementDir));
                updateCoordinates();
            }
        }
    }

    //TODO make obstacles undoable
    public void undoObstacleMove(Point point, Point dir, Transform parent) {
        
    }

    public void touchFlower(Point point) {
        touchedFlowers[point] = true;
    }

    public bool checkWin(Point playerPosition) {
        return !touchedFlowers.ContainsValue(false) && playerPosition.Equals(0, 0);
    }

    public void activateTile(Point point) {
        tiles[point].isActive = true;
    }

    public void deactivateTile(Point point) {
        tiles[point].isActive = false;
    }
    
    public void activateObstacle(Point point) {
        obstacles[point].isActive = true;
    }

    public void deactivateObstacle(Point point) {
        obstacles[point].isActive = false;
    }
    
    public void activateFlower(Point point) {
        flowers[point].isActive = true;
    }

    public void deactivateFlower(Point point) {
        flowers[point].isActive = false;
    }

    private void updateCoordinates(List<Point> coordinates) {
        openCoordinates = coordinates;
    }

    public void updateCoordinates() {
        List<Point> tempCoordinates = new List<Point>();
        foreach(KeyValuePair<Point, Tile> valuePair in tiles) {
            Tile tile = valuePair.Value;
            if(tile.isActive) 
                tempCoordinates.Add(valuePair.Key);
        }
        foreach(KeyValuePair<Point, Obstacle> valuePair in obstacles) {
            if(valuePair.Value.isActive) {
                Point point = valuePair.Key;
                tempCoordinates.Remove(point);
            }
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

    public Dictionary<Point, Obstacle> getPushables() {
        return this.pushables;
    }

    public Dictionary<Point, Flower> getFlowers() {
        return this.flowers;
    }

    public void Destroy() {
        foreach(KeyValuePair<Point, Tile> valuePair in tiles) {
            valuePair.Value.Destroy();
        }
        foreach(KeyValuePair<Point, Obstacle> valuePair in obstacles) {
            valuePair.Value.Destroy();
        }
        foreach(KeyValuePair<Point, Flower> valuePair in flowers) {
            valuePair.Value.Destroy();
        }
    }
}
