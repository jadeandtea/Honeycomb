using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{

    public enum HexType {
        Flat, 
        Pointy
    }

    public float hexagonSize = 1.0f;
    [Range (0.0f, 1.0f)]
    public float outlineSize = 0.05f;
    public HexType type;
    
    Color32 hexColor;
    Color32 obsColor;

    string[] flowerSpriteList;

    public MapManager mapManager;

    void Start(){
        
        hexColor = Color.yellow;
        obsColor = Color.black;
        
        mapManager = new MapManager();
        
        mapManager.loadMap(2, hexagonSize, 1 - outlineSize, hexColor, this.transform);

        mapManager.loadObstacles(Levels.lvl_1_Obs, hexagonSize, 1 - outlineSize, obsColor, this.transform);

        flowerSpriteList = new string[3]{"Assets/FlowerSpriteList/Black-eyed Susan.png", "Assets/FlowerSpriteList/Daisy.png", "Assets/FlowerSpriteList/Rose.png"};
        mapManager.loadFlowers(Levels.lvl_1_Flowers, hexagonSize, flowerSpriteList, type, this.transform);

        type = HexType.Flat;
    }

    void FixedUpdate() {  //For the purpose of modifying hexagons mid-game;
        foreach(Tile tile in mapManager.getTiles()) {
            tile.updateTile(hexagonSize, 1 - outlineSize, type);
        }
        foreach(Obstacle obstacle in mapManager.getObstacles()) {
            obstacle.updateObs(hexagonSize, hexagonSize/2, 1 - outlineSize, type);
        }
        foreach(Flower flower in mapManager.getFlowers()) {
            flower.update(hexagonSize, type);
        }
    }

    public void moveObstacle(int index, Point movementDir) {
        mapManager.moveObstacle(index, movementDir, hexagonSize, 1 - outlineSize, type, hexColor, this.transform);
    }
}
