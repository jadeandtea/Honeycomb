using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelCreatorManager : MonoBehaviour
{
    public MapSettings settings;
    Dictionary<Point, TileType> pointList;
    int layerMask;

    enum TileType{
        Hidden, Tile, Obstacle, Flower
    }

    void Awake() {
        layerMask = LayerMask.GetMask("Tiles");
        pointList = new Dictionary<Point, TileType>();
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

                MapManager tempManager = hit.transform.gameObject.GetComponentInParent<MapRender>().mapManager;
                switch (pointList[point]){
                    case TileType.Hidden:
                        tempManager.deactivateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Tile:
                        tempManager.activateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Obstacle:
                        tempManager.activateTile(point);
                        tempManager.activateObstacle(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Flower:
                        tempManager.activateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.activateFlower(point);
                        break;
                }
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

                MapManager tempManager = hit.transform.gameObject.GetComponentInParent<MapRender>().mapManager;
                switch (pointList[point]){
                    case TileType.Hidden:
                        tempManager.deactivateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Tile:
                        tempManager.activateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Obstacle:
                        tempManager.activateTile(point);
                        tempManager.activateObstacle(point);
                        tempManager.deactivateFlower(point);
                        break;
                    case TileType.Flower:
                        tempManager.activateTile(point);
                        tempManager.deactivateObstacle(point);
                        tempManager.activateFlower(point);
                        break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            saveLayout();
        }
    }

    void saveLayout() {
        //TODO Make sure this works properly, maybe format it better so i can just copy and paste into a level?
        List<Point> tiles = new List<Point>();
        List<Point> obstacles = new List<Point>();
        List<Point> flowers = new List<Point>();
        
        foreach(KeyValuePair<Point, TileType> valuePair in pointList) {
            if (valuePair.Value == TileType.Tile) {
                tiles.Add(valuePair.Key);
            } else if (valuePair.Value == TileType.Obstacle) {
                obstacles.Add(valuePair.Key);
            } else if (valuePair.Value == TileType.Flower) {
                flowers.Add(valuePair.Key);
            }
        }
        string path = "Assets/Resources/potentialLevel.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write("[\n");
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
        writer.Write("]\n[\n");
        foreach(Point obs in obstacles) {
            writer.Write("new Point");
            writer.Write(obs.ToString());
            writer.Write(", \n");
        }
        writer.Write("]\n[\n");
        foreach(Point flower in flowers) {
            writer.Write("new Point");
            writer.Write(flower.ToString());
            writer.Write(", \n");
        }
        writer.Write("]\n");
        writer.Close();

        Debug.Log("Saved Level.");
    }
}
