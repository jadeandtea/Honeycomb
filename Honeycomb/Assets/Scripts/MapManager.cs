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
    Dictionary<Point, int> touchedFlowers;
    Stack<MovementCache> history;
    Transform parent;

    bool editMode;
    
    public MapManager(MapSettings settings, Transform parent, bool editMode = false) {
        // Manages all things regarding the map; where tiles, obstacles, pushables, and flowers are; keeping track
        // of where the player has moved and how obstacles move; and checking if the player has won or not
        this.editMode = editMode;
        this.settings = settings;
        this.parent = parent;

        openCoordinates = new List<Point>();

        tiles = new Dictionary<Point, Tile>();
        obstacles = new Dictionary<Point, Obstacle>();
        pushables = new Dictionary<Point, Obstacle>();
        flowers = new Dictionary<Point, Flower>();

        touchedFlowers = new Dictionary<Point, int>();

        history = new Stack<MovementCache>();
    }

    public void loadMap(int mapSize) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize
        
        tiles = new Dictionary<Point, Tile>();
        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Tile tempTileRef = new Tile(settings, new Point(x, y), parent, false, editMode);
                    
                    tiles[new Point(x, y)] = tempTileRef;
                }
            }
        }
    }

    public void loadMap(List<Point> levelMap) {
        //Loads a map given a list of points
        if(editMode) {
            loadMap(10);
            foreach(Point coord in levelMap) {
                tiles[coord].isActive = true;
            }
        } else {
            tiles = new Dictionary<Point, Tile>();

            foreach(Point coord in levelMap) {
                Tile tempTileRef = new Tile(settings, coord, parent);
                tiles[coord] = tempTileRef;
            }
        }
    }

    public void loadObstacles(int mapSize) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize
        
        obstacles = new Dictionary<Point, Obstacle>();
        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Obstacle tempObsRef = new Obstacle(settings, new Point(x, y), parent, Obstacle.obstacleType.Obstacle, editMode);
                    
                    obstacles[new Point(x, y)] = tempObsRef;
                }
            }
        }
    }

    public void loadObstacles(List<Point> obstacleList) {
        //Creates Obstacle Game Objects that cannot be pushed

        if(editMode) {
            loadObstacles(10);
            foreach(Point coord in obstacleList) {
                obstacles[coord].isActive = true;
            }
        } else {
            obstacles = new Dictionary<Point, Obstacle>();

            foreach(Point coord in obstacleList) {
                int s = -coord.x - coord.y;
                Obstacle tempObsRef = new Obstacle(settings, coord, parent, Obstacle.obstacleType.Obstacle, editMode);

                obstacles[coord] = tempObsRef;
            }
        }
    }

    public void loadPushables(int mapSize) {
        //Creates a map of tiles centered on (0, 0) that extends equally in all directions by the given mapSize
        
        pushables = new Dictionary<Point, Obstacle>();
        //Weird x and y bounds, but it works
        for(int x = -mapSize; x < mapSize + 1; x++) {
            for (int y = -mapSize; y < mapSize + 1; y++) {
                int s = -x - y;
                if(Mathf.Abs(s) <= mapSize){
                    Obstacle tempObsRef = new Obstacle(settings, new Point(x, y), parent, Obstacle.obstacleType.Pushable, editMode);
                    
                    pushables[new Point(x, y)] = tempObsRef;
                }
            }
        }
    }

    public void loadPushables(List<Point> pushableList) {
        //Creates Obstacle Game Objects that can be pushed

        if (editMode) {
            loadPushables(10);
            foreach(Point coord in pushableList) {
                pushables[coord].isActive = true;
            }
        } else {
            pushables = new Dictionary<Point, Obstacle>();

            foreach(Point coord in pushableList) {
                int s = -coord.x - coord.y;
                Obstacle tempObsRef = new Obstacle(settings, coord, parent, Obstacle.obstacleType.Pushable, editMode);

                pushables[coord] = tempObsRef;
            }
        }
    }

    public void loadFlowers(int mapSize, string[] flowerSpriteList) {
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

                    touchedFlowers[new Point(x, y)] = 0;
                }
            }
        }
    }

    public void loadFlowers(List<Point> flowerList, string[] flowerSpriteList) {
        //Creates Flower Game Objects

        if (editMode) {
            loadFlowers(10, flowerSpriteList);
            foreach(Point coord in flowerList) {
                flowers[coord].isActive = true;
                touchedFlowers[coord] = 0;
            }
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

                touchedFlowers[coord] = 0;
            }
        }
    }

    public void logCache(Point previousPoint, Point newPoint, MovementCache.movedObject movedObject) {
        history.Push(new MovementCache(previousPoint, newPoint, movedObject));
    }

    public void undoMove(){
        MovementCache previousMove;
        history.TryPop(out previousMove);
        // Debug.Log("Undoing " + previousMove.mObject + " from " + previousMove.newPoint + " to " + previousMove.previousPoint);
        if (previousMove.mObject == MovementCache.movedObject.Player) {
            Player player = parent.gameObject.GetComponentInChildren<Player>();
            if (flowers.ContainsKey(player.mapPosition)) {
                undoTouchFlower(player.mapPosition);
            }
            player.mapPosition = previousMove.previousPoint;
        } else if (previousMove.mObject == MovementCache.movedObject.Obstacle) {
            try{
                Obstacle obstacleRef = pushables[previousMove.newPoint];
                if(obstacleRef.isActive) {
                    pushables.Remove(previousMove.newPoint);
                    pushables[previousMove.previousPoint] = obstacleRef;
                } else {
                    obstacleRef.isActive = true;
                    pushables.Remove(previousMove.newPoint);
                    pushables[previousMove.previousPoint] = obstacleRef;
                    tiles[previousMove.newPoint].isActive = false;
                }
            } catch{
                Obstacle obstacleRef = new Obstacle(settings, previousMove.newPoint, parent, Obstacle.obstacleType.Pushable, editMode);
                pushables[previousMove.previousPoint] = obstacleRef;
                tiles[previousMove.newPoint].isActive = false;
            }
        }
    }

    public Point movePushable(Point point, Point movementDir) {
        // Moving an pushable is not as easy as it seems! 
        //
        // First, get the pushable reference and set the point it will move to
        // If the position that it wants to move to has an active obstacle or pushable,
        // keep the point the pushable will move to the same (the pushable won't move)
        //
        // Set the pushable to move to the point, whether that be the new point or the same point
        // as where it started. 
        //
        // If the point the pushable moves to is not a tile, set the tile to active (or create a new tile),
        // then deactivate the pushable.
        //
        // Return the point the obstacle moves to store for undoing

        Obstacle pushRef = pushables[point];

        Point finalLocation;

        pushables.Remove(point);
        point.Add(movementDir);

        if(obstacles.ContainsKey(point) && obstacles[point].isActive) {
            point.Sub(movementDir);
        }
        else if(pushables.ContainsKey(point) && pushables[point].isActive) {
            point.Sub(movementDir);
        }
        else if(flowers.ContainsKey(point) && flowers[point].isActive) {
            point.Sub(movementDir);
        }
        else if(point.Equals(Point.zero)) {
            point.Sub(movementDir);
        }
        pushables[point] = pushRef;
        finalLocation = point;

        if(tiles.ContainsKey(point)) {
            if(!tiles[point].isActive) {
                tiles[point].isActive = true;
                pushables[point].isActive = false;
                tiles[point].setActiveLocation(point.Sub(movementDir));
            }
        } else {
            pushables[point].isActive = false;
            tiles[point] = new Tile(settings, point, parent, false, editMode);
            tiles[point].setActiveLocation(point.Sub(movementDir));
            updateCoordinates();
        }
        return finalLocation;
    }

    public void touchFlower(Point point) {
        touchedFlowers[point]++;
    }

    void undoTouchFlower(Point point) {
        touchedFlowers[point]--;
    }

    public bool checkWin(Point playerPosition) {
        return touchedFlowers.Count > 0 && !touchedFlowers.ContainsValue(0) && playerPosition.Equals(0, 0);
    }

    public void activateTile(Point point) {
        try{
            tiles[point].isActive = true;
        } catch {
            tiles[point] = new Tile(settings, point, parent, false, editMode);
        }
    }

    public void deactivateTile(Point point) {
        try{
            tiles[point].isActive = false;
        } catch {
            tiles[point] = new Tile(settings, point, parent, false, editMode);
            tiles[point].isActive = false;
        }
    }
    
    public void activateObstacle(Point point) {
        try{
            obstacles[point].isActive = true;
        } catch {
            obstacles[point] = new Obstacle(settings, point, parent, Obstacle.obstacleType.Obstacle, editMode);
        }
    }

    public void deactivateObstacle(Point point) {
        try{
            obstacles[point].isActive = false;
        } catch {
            obstacles[point] = new Obstacle(settings, point, parent, Obstacle.obstacleType.Obstacle, editMode);
            obstacles[point].isActive = false;
        }
    }

    public void activatePushable(Point point) {
        try{
            pushables[point].isActive = true;
        } catch {
            obstacles[point] = new Obstacle(settings, point, parent, Obstacle.obstacleType.Pushable, editMode);
        }
    }

    public void deactivatePushable(Point point) {
        try{
            pushables[point].isActive = false;
        } catch {
            pushables[point] = new Obstacle(settings, point, parent, Obstacle.obstacleType.Pushable, editMode);
            pushables[point].isActive = false;
        }
    }
    
    public void activateFlower(Point point) {
        try{
            flowers[point].isActive = true;
        } catch {
            
        }
    }

    public void deactivateFlower(Point point) {
        try{
            flowers[point].isActive = false;
        } catch {
            
        }
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
        openCoordinates = tempCoordinates;
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
        foreach(KeyValuePair<Point, Obstacle> valuePair in pushables) {
            valuePair.Value.Destroy();
        }
        foreach(KeyValuePair<Point, Flower> valuePair in flowers) {
            valuePair.Value.Destroy();
        }
    }
}
