using UnityEngine;

public class MapRender : MonoBehaviour
{
    public float hexagonSize = 1.0f;
    public float playerSize = 1.0f;
    [Range (0.0f, 1.0f)]
    public float outlineSize = 0.05f;
    private int mapSize = 1;

    float vOffset;
    float hOffset;

    HexagonMesh[,] map;
    void Start(){
        
        map = new HexagonMesh[2 * mapSize, 2 * mapSize];

        Vector2 mapCoord = new Vector2(0, 0);
        // Using Offset Coordinates and Flat top
        for (int i = 0; i < 2 * mapSize; i++) {
            for (int j = 0; j < 2 * mapSize; j++) {
                mapCoord.Set(i - mapSize, j - mapSize);
                if(mapCoord.x < 0 && mapCoord.x % 2 != 0) mapCoord.y++;

                GameObject tempHexRef = new GameObject("(" + mapCoord.x + ", " + mapCoord.y + ")");
                vOffset = hexagonSize * Mathf.Sqrt(3) / 2;
                hOffset = hexagonSize / 2;
                
                tempHexRef.transform.position = new Vector3(mapCoord.x * hexagonSize * 3 / 4, mapCoord.y * vOffset + mapCoord.x % 2 * vOffset / 2, 0);

                HexagonMesh point = new HexagonMesh(tempHexRef, hexagonSize, 1 - outlineSize);
                map[i, j] = point;

                tempHexRef.transform.SetParent(this.transform);
            }
        } 

        for (int x = 0; x < 2 * mapSize; x++){
            for (int y = 0; y < 2 * mapSize; y++){
                mapCoord.Set(x, y);
            }
        }
    }

    // void FixedUpdate() {  //For the purpose of changing hexagon sizes mid-game
    //     vOffset = hexagonSize * Mathf.Sqrt(3) / 2;
    //     hOffset = hexagonSize / 2;
    //     for (int i = 0; i < 2 * mapSize; i++) {
    //         for (int j = 0; j < 2 * mapSize; j++) {
    //             map[i, j].update(hexagonSize);
    //             map[i, j].point.transform.position = new Vector3((i - mapSize) * hexagonSize * 3 / 4, (j - mapSize) * vOffset + (i - mapSize) % 2 * vOffset / 2, 0);
    //         }
    //     } 
    // }
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

        public HexagonMesh(GameObject point, float hexagonSize, float outlineSize) {
            //Given the location of where the hex is to be placed, render 6 triangles to form
            //a hexagon centered around the given point.

            this.point = point;
            this.hexagonSize = hexagonSize;
            this.outlineSize = outlineSize;

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

        public void update(float hexagonSize) {
            this.hexagonSize = hexagonSize;

            mesh = calculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer.material = new Material(Shader.Find("Standard"));
        }

        Mesh calculateMesh(Mesh mesh) {
            mesh.Clear();

            float positionX = point.transform.position.x;
            float positionY = point.transform.position.y;

            /* Calculations using Trigonometry */

            vertices = new Vector3[7];
            for (int i = -3; i < 3; i++) {
                var angle_rad = -Mathf.PI / 3 * i;
                vertices[i + 4] = new Vector3(positionX + hexagonSize * Mathf.Cos(angle_rad) * outlineSize, positionY + hexagonSize * Mathf.Sin(angle_rad) * outlineSize, zLayer);
            }
            vertices[0] = point.transform.position;

            /* Calculations using offsets */

            // float vOffset = hexagonSize * Mathf.Sqrt(3) / 2 - outlineSize;
            // float hOffset = hexagonSize / 2 - outlineSize;

            // vertices = new Vector3[7]{new Vector3(positionX, positionY, zLayer), new Vector3(positionX - hexagonSize, positionY, zLayer), new Vector3(positionX - hOffset, positionY + vOffset, zLayer), new Vector3(positionX + hOffset, positionY + vOffset, zLayer),new Vector3(positionX + hexagonSize, positionY, zLayer), new Vector3(positionX + hOffset, positionY - vOffset, zLayer), new Vector3(positionX - hOffset, positionY - vOffset, zLayer)};

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
