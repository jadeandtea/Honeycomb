using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    string[] flowerSpriteList;
    public MapSettings settings;

    public MapManager mapManager;
    
    public LevelTextManager completeLevelText;
    public LevelTextManager keybindText;

    public bool editMode = false;

    void Awake(){
        reloadLevel();
    }

    void FixedUpdate() {  
        // Update and "Render" all hexagons
        //
        // TODO Potential slowdowns when on a large map (level editor)
        // Find some way to only update tiles when they need to move
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
            pushable.setColor(settings.pushOuterColor, settings.pushCenterColor, settings.centerColorWeight);
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
        if(keybindText != null) {
            keybindText.FadeTextToFullAlpha();
        }
        
        Level.loadLevel();

        mapManager = new MapManager(settings, this.transform, editMode);
        
        mapManager.loadMap(Level.map);
        mapManager.loadObstacles(Level.obstacles);
        mapManager.loadPushables(Level.pushables);

        flowerSpriteList = new string[3]{"Assets/FlowerSpriteList/Black-eyed Susan.png", "Assets/FlowerSpriteList/Daisy.png", "Assets/FlowerSpriteList/Rose.png"};
        mapManager.loadFlowers(Level.flowers, flowerSpriteList);

        mapManager.updateCoordinates();
    }

    public Point moveObstacle(Point point, Point movementDir) {
        Point temp = mapManager.moveObstacle(point, movementDir);
        mapManager.updateCoordinates();
        return temp;
    }

    public void logCache(Point newPoint, Point previousPoint, MovementCache.movedObject obj) {
        mapManager.logCache(newPoint, previousPoint, obj);
        if(keybindText != null) {
            keybindText.FadeTextToZeroAlpha();
        }
    }

    public void undo() {
        mapManager.undoMove();
    }

    public void touchFlower(Point point) {
        mapManager.touchFlower(point);
    }
    
    public void checkWin(Point playerPosition) {
        if (mapManager.checkWin(playerPosition)) {
            if(completeLevelText != null) {
                completeLevelText.animate();
            }
            LevelManager.levelComplete();
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
