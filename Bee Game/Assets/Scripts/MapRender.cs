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

    HexagonMesh[,] map;
    public List<Point> CurrentCoordinateList;
    List<Point> Level_1 = new List<Point>{new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4)};

    int MAXMAPSIZE = 10;

    void Start(){
        
        map = new HexagonMesh[2 * MAXMAPSIZE, 2 * MAXMAPSIZE];

        CurrentCoordinateList = new List<Point>();
        loadMap(Level_1);
        type = Type.Flat;
    }

    void FixedUpdate() {  //For the purpose of changing hexagon sizes mid-game;
        foreach(Point point in CurrentCoordinateList) {
            map[point.x, point.y].updateSize(hexagonSize, 1 - outlineSize, type);
            // Hex Positons are different based on if they are flat or pointy
            if(type == Type.Flat){
                map[point.x, point.y].updatePos(point.x * 3 / 4f * hexagonSize, (point.x * Mathf.Sqrt(3) / 4f + point.y * Mathf.Sqrt(3) / 2) * hexagonSize);
            } else if (type == Type.Pointy) {
                map[point.x, point.y].updatePos(point.x * Mathf.Sqrt(3) * hexagonSize / 2 + point.y * Mathf.Sqrt(3) / 4 * hexagonSize, point.y * 3 / 4f * hexagonSize);
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

                HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize, Type.Flat);
                map[x, y] = point;

                CurrentCoordinateList.Add(new Point(x, y));
                
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

            HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize, Type.Flat);
            map[coord.x, coord.y] = point;

            CurrentCoordinateList.Add(coord);
            
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
        int[] triangles;

        GameObject point;
        int zLayer = 0;

        Type type;

        public HexagonMesh(GameObject point, float hexagonSize, float outlineSize, Type type) {
            //Given the location of where the hex is to be placed, render 6 triangles to form
            //a hexagon centered around the given point.

            this.point = point;
            this.hexagonSize = hexagonSize;
            this.outlineSize = outlineSize;
            this.type = type;

            meshFilter = point.AddComponent<MeshFilter>();
            mesh = new Mesh();

            //Triangles render from left clockwise
            //IDK what normals do, they are required or something
            normals = new Vector3[7]{-Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward};
            triangles = new int[18]{0, 1, 2,  0, 2, 3,  0, 3, 4,  0, 4, 5,  0, 5, 6,  0, 6, 1};

            mesh = calculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer = point.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }

        public void updateSize(float hexagonSize, float outlineSize, Type type) {

            this.hexagonSize = hexagonSize;
            this.outlineSize = outlineSize;
            this.type = type;

            mesh = calculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer.material = new Material(Shader.Find("Standard"));
        }

        public void updatePos(float newX, float newY) {

            point.transform.position = new Vector3(newX, newY, zLayer);

        }

        Mesh calculateMesh(Mesh mesh) {
            mesh.Clear();

            //Central point coordinates
            float posX = point.transform.position.x;
            float posY = point.transform.position.y;

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
                vertices[0] = point.transform.position;
            } else if (type == Type.Pointy) {
                //Pointy Top
                // Only difference from the last is this starts at 30°, so + Pi/6
                vertices = new Vector3[7];
                for (int i = -3; i < 3; i++) {
                    var angle_rad = (-Mathf.PI / 3 * i) + Mathf.PI / 6;
                    vertices[i + 4] = new Vector3(posX + hexagonSize * Mathf.Cos(angle_rad) * outlineSize, posY + hexagonSize * Mathf.Sin(angle_rad) * outlineSize, zLayer);
                }
                vertices[0] = point.transform.position;
            }
            
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
