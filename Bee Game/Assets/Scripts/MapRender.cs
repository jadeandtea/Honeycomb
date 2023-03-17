using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{

    public enum Type {
        Flat, 
        Pointy
    }

    public float hexagonSize = 1.0f;
    [Range (0.0f, 1.0f)]
    public float outlineSize = 0.05f;
    public Type type;
    Color32 hexColor;
    Color32 obsColor;

    List<HexagonMesh> map;
    //public so that player script can access
    public List<Point> currentMapCoordinateList;
    List<HexagonMesh> obstacles;
    
    List<Point> lvl_1_Obs = new List<Point>{new Point(-1, 0)};
    List<Point> lvl_1_Map = new List<Point>{new Point(-1, 0), new Point(-1, 1), new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4), new Point(1, 2)};

    int MAXMAPSIZE = 10;

    void Start(){
        
        map = new List<HexagonMesh>();
        
        currentMapCoordinateList = new List<Point>();
        loadMap(lvl_1_Map);
        obstacles = new List<HexagonMesh>();
        loadObstacles(lvl_1_Obs);

        type = Type.Flat;
        //Makes a honey-like color after lerping with yellow, not going to touch :)
        hexColor = new Color32(255, 35, 0, 255);
        obsColor = Color.black;
    }

    void FixedUpdate() {  //For the purpose of modifying hexagons mid-game;
        foreach(HexagonMesh mesh in map) {
            mesh.updateColor(hexColor);
            mesh.updateSize(hexagonSize, 1 - outlineSize, type);
            // Hex Positons are different based on if they are flat or pointy
            if(type == Type.Flat){
                mesh.updatePos(hexagonSize);
            } else if (type == Type.Pointy) {
                mesh.updatePos(hexagonSize);
            }
        }
        foreach(HexagonMesh mesh in obstacles) {
            mesh.updateColor(Color.black);
            mesh.updateSize(hexagonSize/2, 1 - outlineSize, type);
            if(type == Type.Flat){
                mesh.updatePos(hexagonSize);
            } else if (type == Type.Pointy) {
                mesh.updatePos(hexagonSize);
            }
        }
    }

    void initMaxMap() {
        //In a loop, create the list of game objects

        Point tempMapCoord = new Point();
        for (int x = 0; x < 2 * MAXMAPSIZE; x++) {
            for (int y = 0; y < 2 * MAXMAPSIZE; y++) {
                tempMapCoord.Set(x - MAXMAPSIZE, y - MAXMAPSIZE);
                int s = - tempMapCoord.x - tempMapCoord.y;

                GameObject tempHexRef = new GameObject("(" + tempMapCoord.x + ", " + tempMapCoord.y + ", " + s + ")");
                tempHexRef.transform.position = Vector3.zero;

                HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize, Type.Flat, hexColor, new Point(x, y));
                map.Add(point);

                currentMapCoordinateList.Add(new Point(x, y));
                
                tempHexRef.transform.SetParent(this.transform);
            }
        }
    }

    void loadMap(List<Point> levelMap) {
        //Loading a map creates the game objects

        foreach(Point coord in levelMap) {
            int s = -coord.x - coord.y;

            GameObject tempHexRef = new GameObject("(" + coord.x + ", " + coord.y + ", " + s + ")");
            tempHexRef.transform.position = Vector3.zero;

            HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize, Type.Flat, hexColor, new Point(coord.x, coord.y));
            map.Add(point);

            currentMapCoordinateList.Add(coord);
            
            tempHexRef.transform.SetParent(this.transform);
        }
    }

    void loadObstacles(List<Point> obstacleList) {
        //Creates Obstacle Game Objects

        foreach(Point coord in obstacleList) {
            int s = -coord.x - coord.y;

            GameObject tempHexRef = new GameObject("(" + coord.x + ", " + coord.y + ", " + s + ")");
            tempHexRef.transform.position = Vector3.zero;

            HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize / 2, 1 - outlineSize, Type.Flat, obsColor, new Point(coord.x, coord.y), -0.1f);

            obstacles.Add(point);
            currentMapCoordinateList.Remove(coord);
            
            tempHexRef.transform.SetParent(this.transform);
        }
    }


    private class HexagonMesh
    {
        float hexagonSize;
        float outlineSize;

        MeshRenderer meshRenderer;
        Material test;
        MeshFilter meshFilter;
        Mesh mesh;

        Vector3[] vertices;
        Vector3[] normals;
        Color32[] colors;
        int[] triangles;

        GameObject center;
        Point mapCoord;
        float zLayer = 0;

        Type type;

        public HexagonMesh(GameObject point, float hexagonSize, float outlineSize, Type type, Color color, Point mapCoord, float zLayer = 0) {
            //Given the location of where the hex is to be placed, render 6 triangles to form
            //a hexagon centered around the given point.

            this.center = point;
            this.hexagonSize = hexagonSize;
            this.outlineSize = outlineSize;
            this.mapCoord = mapCoord;
            this.type = type;
            this.zLayer = zLayer;

            meshFilter = point.AddComponent<MeshFilter>();
            mesh = new Mesh();

            //Triangles render from left clockwise
            //IDK what normals do, they are required or something
            normals = new Vector3[7]{-Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward};
            triangles = new int[18]{0, 1, 2,  0, 2, 3,  0, 3, 4,  0, 4, 5,  0, 5, 6,  0, 6, 1};

            mesh = calculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer = point.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        public void updateSize(float hexagonSize, float outlineSize, Type type) {

            this.hexagonSize = hexagonSize;
            this.outlineSize = outlineSize;
            this.type = type;

            mesh = calculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        public void updatePos(float newX, float newY) {

            center.transform.position = new Vector3(newX, newY, zLayer);

        }

        public void updatePos(float hexagonSize){

            center.transform.position = new Vector3(mapCoord.x * 3 / 4f * hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 4f + mapCoord.y * Mathf.Sqrt(3) / 2) * hexagonSize);

        }

        public void updateColor(Color color) {
            color = Color32.Lerp(Color.yellow, color, 0.3f);
            colors = new Color32[7]{Color32.Lerp(Color.yellow, color, 0.3f), color, color, color, color, color, color};
        }

        Mesh calculateMesh(Mesh mesh) {
            mesh.Clear();

            //Central point coordinates
            float posX = center.transform.position.x;
            float posY = center.transform.position.y;

            vertices = new Vector3[7];

            if(type == Type.Flat) {
                //Flat Top
                // The angles for each point from the origin to it are 0°, 60°, 120°, 180°, 240°, 300°
                // The for loop looks weird but it works;
                // It starts at Pi (which is 0°) and moves around counterclockwise
                for (int i = -3; i < 3; i++) {
                    var angle_rad = -Mathf.PI / 3 * i;
                    vertices[i + 4] = new Vector3(posX + hexagonSize * Mathf.Cos(angle_rad) * outlineSize, posY + hexagonSize * Mathf.Sin(angle_rad) * outlineSize, zLayer);
                }
                vertices[0] = center.transform.position;
            } else if (type == Type.Pointy) {
                //Pointy Top
                // Only difference from the last is this starts at 30°, so + Pi/6
                vertices = new Vector3[7];
                for (int i = -3; i < 3; i++) {
                    var angle_rad = (-Mathf.PI / 3 * i) + Mathf.PI / 6;
                    vertices[i + 4] = new Vector3(posX + hexagonSize * Mathf.Cos(angle_rad) * outlineSize, posY + hexagonSize * Mathf.Sin(angle_rad) * outlineSize, zLayer);
                }
                vertices[0] = center.transform.position;
            }
            
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            mesh.colors32 = colors;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
