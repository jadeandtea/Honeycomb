using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelCreatorManager : MonoBehaviour
{
    public MapSettings settings;
    static Dictionary<Point, TileType> pointList;
    int layerMask;

    public int level = 1;
    public bool loadLevel = false;

    enum TileType{
        Hidden, Tile, Obstacle, Pushable, Flower
    }

    void Awake() {
        layerMask = LayerMask.GetMask("Tiles");
        pointList = new Dictionary<Point, TileType>();

        if(loadLevel)
            LevelManager.currentLevelNumber = level;
        Level.loadLevel();
        loadPoints(Level.map, Level.obstacles, Level.pushables, Level.flowers);
    }

    void loadPoints(List<Point> Tiles, List<Point> Obstacles, List<Point> Pushables, List<Point> Flowers) {
        foreach(Point point in Tiles) {
            pointList[point] = TileType.Tile;
        }
        foreach(Point point in Obstacles) {
            pointList[point] = TileType.Obstacle;
        }
        foreach(Point point in Pushables) {
            pointList[point] = TileType.Pushable;
        }
        foreach(Point point in Flowers) {
            pointList[point] = TileType.Flower;
        }
    }

    void Update()
    {
        // 0 is leftMouseButton, 1 is rightMouseButton
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Point point = new Point(hit.transform.gameObject.name);
                if (!pointList.ContainsKey(point))
                    pointList[point] = TileType.Hidden;
                if(pointList[point] != TileType.Flower)
                    pointList[point]++;

                activateTiles(hit, point);
            }
        } 
        else if (Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Point point = new Point(hit.transform.gameObject.name);
                if (!pointList.ContainsKey(point))
                    pointList[point] = TileType.Hidden;
                if(pointList[point] != TileType.Hidden) 
                    pointList[point]--;

                activateTiles(hit, point);
            }
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            saveLayout();
        }
    }

    void activateTiles(RaycastHit hit, Point point) {
        MapManager tempManager = hit.transform.gameObject.GetComponentInParent<MapRender>().mapManager;
                switch (pointList[point]){
                    case TileType.Hidden:
                        tempManager.deactivateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.deactivatePushable(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Tile:
                        tempManager.activateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.deactivatePushable(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Obstacle:
                        tempManager.activateTile(point);
                        tempManager.activateObstacle(point);
                        tempManager.deactivatePushable(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Pushable:
                        tempManager.activateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.activatePushable(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Flower:
                        tempManager.activateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.deactivatePushable(point);
                        tempManager.activateFlower(point);
                        break;
                }
    }

    void saveLayout() {
        //TODO Make sure this works properly, maybe format it better so i can just copy and paste into a level?
        List<Point> tiles = new List<Point>();
        List<Point> obstacles = new List<Point>();
        List<Point> pushables = new List<Point>();
        List<Point> flowers = new List<Point>();
        
        foreach(KeyValuePair<Point, TileType> valuePair in pointList) {
            if (valuePair.Value == TileType.Tile) {
                tiles.Add(valuePair.Key);
            } else if (valuePair.Value == TileType.Obstacle) {
                obstacles.Add(valuePair.Key);
            } else if (valuePair.Value == TileType.Pushable) {
                pushables.Add(valuePair.Key);
            } else if (valuePair.Value == TileType.Flower) {
                flowers.Add(valuePair.Key);
            }
        }
        string path = "Assets/Resources/potentialLevel.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write("[ Tiles: \n");
        foreach(Point tile in tiles) {
            writer.Write("new Point");
            writer.Write(tile.ToString());
            writer.Write(", \n");
        }
        foreach(Point flower in flowers) {
            writer.Write("new Point");
            writer.Write(flower.ToString());
            writer.Write(", \n");
        }
        foreach(Point obs in obstacles) {
            writer.Write("new Point");
            writer.Write(obs.ToString());
            writer.Write(", \n");
        }
        foreach(Point push in pushables) {
            writer.Write("new Point");
            writer.Write(push.ToString());
            writer.Write(", \n");
        }
        writer.Write("]\n[ Obstacles:\n");
        foreach(Point obs in obstacles) {
            writer.Write("new Point");
            writer.Write(obs.ToString());
            writer.Write(", \n");
        }
        writer.Write("]\n[ Pushables:\n");
        foreach(Point push in pushables) {
            writer.Write("new Point");
            writer.Write(push.ToString());
            writer.Write(", \n");
        }
        writer.Write("]\n[ Flowers:\n");
        foreach(Point flower in flowers) {
            writer.Write("new Point");
            writer.Write(flower.ToString());
            writer.Write(", \n");
        }
        writer.Write("]\n");
        writer.Close();

        Debug.Log("Saved Level.");
    }

    public static void reset() {
        pointList = new Dictionary<Point, TileType>();
    }
}
