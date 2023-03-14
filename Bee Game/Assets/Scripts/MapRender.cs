using UnityEngine;

public class MapRender : MonoBehaviour
{

    enum Type {
        Flat, 
        Pointy
    }

    public float hexagonSize = 1.0f;
    [Range (0.0f, 1.0f)]
    public float outlineSize = 0.05f;
    private int mapSize = 2;
    private Point tempMapCoord = new Point(0, 0);

    float vOffset;
    float hOffset;

    HexagonMesh[,] map;
    void Start(){
        
        map = new HexagonMesh[2 * mapSize, 2 * mapSize];

        initAxialPointy();
    }

    void FixedUpdate() {  //For the purpose of changing hexagon sizes mid-game
        vOffset = hexagonSize * Mathf.Sqrt(3) / 2;
        hOffset = hexagonSize / 2;
        for (int i = 0; i < 2 * mapSize; i++) {
            for (int j = 0; j < 2 * mapSize; j++) {
                map[i, j].update(hexagonSize, 1 - outlineSize);
            }
        } 
    }

    void initAxialFlat() {
        //Using Axial Coordinates and Flat Top
        for (int x = 0; x < 2 * mapSize; x++) {
            for (int y = 0; y < 2 * mapSize; y++) {
                tempMapCoord.Set(x - mapSize, y - mapSize);
                int s = - tempMapCoord.x - tempMapCoord.y;

                GameObject tempHexRef = new GameObject("(" + tempMapCoord.x + ", " + tempMapCoord.y + ", " + s + ")");
                
                tempHexRef.transform.position = new Vector3(tempMapCoord.x * 3 / 4f * hexagonSize, (tempMapCoord.x * Mathf.Sqrt(3) / 4f + tempMapCoord.y * Mathf.Sqrt(3) / 2) * hexagonSize, 0);

                HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize, Type.Flat);
                map[x, y] = point;
                
                tempHexRef.transform.SetParent(this.transform);
            }
        }
    }

    void initAxialPointy() {
        for (int x = 0; x < 2 * mapSize; x++) {
            for (int y = 0; y < 2 * mapSize; y++) {
                tempMapCoord.Set(x - mapSize, y - mapSize);
                int s = - tempMapCoord.x - tempMapCoord.y;

                GameObject tempHexRef = new GameObject("(" + tempMapCoord.x + ", " + tempMapCoord.y + ", " + s + ")");
                
                tempHexRef.transform.position = new Vector3(tempMapCoord.x * Mathf.Sqrt(3) * hexagonSize / 2 + tempMapCoord.y * Mathf.Sqrt(3) / 4 * hexagonSize, tempMapCoord.y * 3 / 4f * hexagonSize, 0);

                HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize, Type.Pointy);
                map[x, y] = point;
                
                tempHexRef.transform.SetParent(this.transform);
            }
        }
    }

    void initOffsetFlat() {
        // Using Offset Coordinates and Flat top
        for (int i = 0; i < 2 * mapSize; i++) {
            for (int j = 0; j < 2 * mapSize; j++) {
                tempMapCoord.Set(i - mapSize, j - mapSize);
                if(tempMapCoord.x < 0 && tempMapCoord.x % 2 != 0) tempMapCoord.y++;

                GameObject tempHexRef = new GameObject("(" + tempMapCoord.x + ", " + tempMapCoord.y + ")");
                vOffset = hexagonSize * Mathf.Sqrt(3) / 2;
                hOffset = hexagonSize / 2;
                
                tempHexRef.transform.position = new Vector3(tempMapCoord.x * hexagonSize * 3 / 4, tempMapCoord.y * vOffset + tempMapCoord.x % 2 * vOffset / 2, 0);

                HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize, Type.Flat);
                map[i, j] = point;

                tempHexRef.transform.SetParent(this.transform);
            }
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

        public GameObject point;
        public int zLayer = 0;

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

        public void update(float hexagonSize, float outlineSize) {
            this.hexagonSize = hexagonSize;
            this.outlineSize = outlineSize;

            mesh = calculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer.material = new Material(Shader.Find("Standard"));
        }

        Mesh calculateMesh(Mesh mesh) {
            mesh.Clear();

            //Central point coordinates
            float posX = point.transform.position.x;
            float posY = point.transform.position.y;

            /* Calculations using Trigonometry */

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
            } else {
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
