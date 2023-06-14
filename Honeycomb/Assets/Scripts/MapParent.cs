using System.Collections.Generic;
using UnityEngine;

public class MapParent : MonoBehaviour
{
    public MapSettings settings;

    public MapManager mapManager;
    
    public LevelTextManager completeLevelText;
    public LevelTextManager keybindText;

    [SerializeField]
    public Texture texture;

    public bool editMode = false;
    public bool loadLevel = false;
    public int level = 0;

    [ContextMenu("Load Level")]
    private void reload() 
    {
        if(loadLevel)
            LevelManager.currentLevelNumber = level;
        Level.loadLevel();
        reloadLevel();
        InstantUpdate();
    }

    [ContextMenu("Clear Level")]
    private void clear() 
    {
        while(transform.childCount > 0) {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void Awake(){
        if(loadLevel)
            LevelManager.currentLevelNumber = level;
        Level.loadLevel();

        clear();
        reloadLevel();        
    }

    public void FixedUpdate() {  
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

    private void InstantUpdate() {
        //Moves all the tiles to where they want to go instantly
        foreach(KeyValuePair<Point, Tile> pair in mapManager.getTiles()) {
            Tile tile = pair.Value;
            Point mapCoord = pair.Key;
            tile.setActiveLocation(mapCoord);
            tile.setColor(settings.tileOuterColor, settings.tileCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Obstacle> pair in mapManager.getObstacles()) {
            Obstacle obstacle = pair.Value;
            Point mapCoord = pair.Key;
            obstacle.setActiveLocation(mapCoord);
            obstacle.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Obstacle> pair in mapManager.getPushables()) {
            Obstacle pushable = pair.Value;
            Point mapCoord = pair.Key;
            pushable.setActiveLocation(mapCoord);
            pushable.setColor(settings.pushOuterColor, settings.pushCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Flower> pair in mapManager.getFlowers()) {
            Flower flower = pair.Value;
            Point mapCoord = pair.Key;
            flower.setActiveLocation(mapCoord);
        }
        mapManager.updateCoordinates();
    }

    public void reloadLevel() {
        if(mapManager != null) {
            mapManager.DestroyImmediate();
        }

        if(keybindText != null) {
            keybindText.FadeTextToFullAlpha();
        }
        
        Level.loadLevel();

        mapManager = new MapManager(settings, this.transform, editMode);
        
        mapManager.loadMap(Level.map);
        mapManager.loadObstacles(Level.obstacles);
        mapManager.loadPushables(Level.pushables);

        mapManager.loadFlowers(Level.flowers, settings.flowerSpriteList);

        mapManager.updateCoordinates();
    }

    public Point movePushable(Point point, Point movementDir) {
        Point temp = mapManager.movePushable(point, movementDir);
        mapManager.updateCoordinates();
        return temp;
    }

    public void logCache(Point newPoint, Point previousPoint, MovementCache.movedObject obj) {
        mapManager.logCache(newPoint, previousPoint, obj);
        if(keybindText != null && LevelManager.currentLevelNumber != 1) {
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

    public void updateMeshes() {
        foreach(KeyValuePair<Point, Tile> pair in mapManager.getTiles()) {
            Tile tile = pair.Value;
            Point mapCoord = pair.Key;
            tile.updateMesh();
            // tile.setTileMesh(texture);
            tile.setColor(settings.tileOuterColor, settings.tileCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Obstacle> pair in mapManager.getObstacles()) {
            Obstacle obstacle = pair.Value;
            Point mapCoord = pair.Key;
            obstacle.updateMesh();
            obstacle.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Obstacle> pair in mapManager.getPushables()) {
            Obstacle pushable = pair.Value;
            Point mapCoord = pair.Key;
            pushable.updateMesh();
            pushable.setColor(settings.pushOuterColor, settings.pushCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Flower> pair in mapManager.getFlowers()) {
            Flower flower = pair.Value;
            flower.update();
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

    public Texture getTexture() {
        return texture;
    }
}
