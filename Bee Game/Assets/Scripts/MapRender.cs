using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    string[] flowerSpriteList;
    public MapSettings settings;

    public MapManager mapManager;

    void Start(){
        

        mapManager = new MapManager(settings, true);
        
        mapManager.loadMap(5, this.transform);

        // flowerSpriteList = new string[3]{"Assets/FlowerSpriteList/Black-eyed Susan.png", "Assets/FlowerSpriteList/Daisy.png", "Assets/FlowerSpriteList/Rose.png"};

        mapManager.updateCoordinates();
    }

    void FixedUpdate() {  
        // Update and "Render" all hexagons
        foreach(KeyValuePair<Point, Tile> pair in mapManager.getTiles()) {
            Tile tile = pair.Value;
            tile.updateTile(settings.hexagonSize, settings.outlineSize, settings.type);
            tile.setColor(settings.tileOuterColor, settings.tileCenterColor, settings.centerColorWeight);
        }
        foreach(KeyValuePair<Point, Obstacle> pair in mapManager.getObstacles()) {
            Obstacle obstacle = pair.Value;
            obstacle.updateObs();
            obstacle.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        }
        foreach(Flower flower in mapManager.getFlowers()) {
            flower.update();
        }
        mapManager.updateCoordinates();
    }

    public void moveObstacle(Point point, Point movementDir) {
        mapManager.moveObstacle(point, movementDir, this.transform);
        mapManager.updateCoordinates();
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

    public List<Flower> getFlowers() {
        return mapManager.getFlowers();
    }
}
