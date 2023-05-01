using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    string[] flowerSpriteList;
    public MapSettings settings;

    public MapManager mapManager;
    
    public LevelTextManager textManager;

    public bool editMode = false;

    void Start(){
        reloadLevel();
    }

    void FixedUpdate() {  
        // Update and "Render" all hexagons
        foreach(KeyValuePair<Point, Tile> pair in mapManager.getTiles()) {
            Tile tile = pair.Value;
            Point mapCoord = pair.Key;
            tile.updateTile(mapCoord);
            tile.setColor(settings.tileOuterColor, settings.tileCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Obstacle> pair in mapManager.getObstacles()) {
            Obstacle obstacle = pair.Value;
            Point mapCoord = pair.Key;
            obstacle.updateObs(mapCoord);
            obstacle.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Obstacle> pair in mapManager.getPushables()) {
            Obstacle pushable = pair.Value;
            Point mapCoord = pair.Key;
            pushable.updateObs(mapCoord);
            pushable.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Flower> pair in mapManager.getFlowers()) {
            Flower flower = pair.Value;
            flower.update();
        }
        mapManager.updateCoordinates();
    }

    public void reloadLevel() {
        if(mapManager != null) {
            mapManager.Destroy();
        }
        
        Level.loadLevel();

        mapManager = new MapManager(settings, textManager, editMode);
        
        mapManager.loadMap(Level.map, this.transform);

        mapManager.loadObstacles(Level.obstacles, this.transform);

        mapManager.loadPushables(Level.pushables, this.transform);

        flowerSpriteList = new string[3]{"Assets/FlowerSpriteList/Black-eyed Susan.png", "Assets/FlowerSpriteList/Daisy.png", "Assets/FlowerSpriteList/Rose.png"};
        mapManager.loadFlowers(Level.flowers, flowerSpriteList, this.transform);

        mapManager.updateCoordinates();
    }

    public void moveObstacle(Point point, Point movementDir) {
        mapManager.moveObstacle(point, movementDir, this.transform);
        mapManager.updateCoordinates();
    }

    public void touchFlower(Point point) {
        mapManager.touchFlower(point);
    }
    
    public void checkWin(Point playerPosition) {
        if (mapManager.checkWin(playerPosition)) {
            if(textManager != null) {
                textManager.animate();
            }
        }
    }

    public List<Point> getCoordinates() {
        return mapManager.getCoordinates();
    }

    public Dictionary<Point, Tile> getTiles() {
        return mapManager.getTiles();
    }

    public Dictionary<Point, Obstacle> getObstacles() {
        return mapManager.getObstacles();
    }

    public Dictionary<Point, Obstacle> getPushables() {
        return mapManager.getPushables();
    }

    public Dictionary<Point, Flower> getFlowers() {
        return mapManager.getFlowers();
    }
}
