using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelCreatorManager : MonoBehaviour
{
    public MapSettings settings;
    Camera m_Camera;
    Dictionary<Point, int> pointList;
    void Awake() {
        m_Camera = Camera.main;
        pointList = new Dictionary<Point, int>();
    }

    void Update()
    {
        // 0 is leftMouseButton, 1 is rightMouseButton
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Point point = new Point(hit.transform.gameObject.name);
                if (!pointList.ContainsKey(point))
                    pointList.Add(point, 0);
                if(pointList[point] < 3)
                    pointList[point]++;

                MapManager tempManager = hit.transform.gameObject.GetComponentInParent<MapRender>().mapManager;
                tempManager.activateTile(point);
            }
        } 
        else if (Input.GetMouseButton(1)) 
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Point point = new Point(hit.transform.gameObject.name);
                if (!pointList.ContainsKey(point))
                    pointList.Add(point, 0);
                if(pointList[point] > 0) 
                    pointList[point]--;

                MapManager tempManager = hit.transform.gameObject.GetComponentInParent<MapRender>().mapManager;
                tempManager.deactivateTile(point);
            }
        }
    }

    void saveLayout() {
        //TODO Make sure this works properly, maybe format it better so i can just copy and paste into a level?
        List<Point> tiles = new List<Point>();
        List<Point> obstacles = new List<Point>();
        List<Point> flowers = new List<Point>();
        foreach(KeyValuePair<Point, int> valuePair in pointList) {
            if (valuePair.Value == 1) {
                tiles.Add(valuePair.Key);
            } else if (valuePair.Value == 2) {
                obstacles.Add(valuePair.Key);
            } else if (valuePair.Value == 3) {
                flowers.Add(valuePair.Key);
            }
        }
        string path = "Assets/Resources/potentialLevel.txt";
        StreamWriter writer = new StreamWriter(path, true);
        foreach(Point tile in tiles) {
            writer.Write(tile.ToString());
        }
        foreach(Point obs in obstacles) {
            writer.Write(obs.ToString());
        }
        foreach(Point flower in flowers) {
            writer.Write(flower.ToString());
        }
        writer.Close();
    }
}
