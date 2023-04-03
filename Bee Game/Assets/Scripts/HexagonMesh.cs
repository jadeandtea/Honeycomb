using UnityEngine;

public class HexagonMesh {
        /* 
        Custom class to handle all mesh creation and modifying during active
        time. Constructor takes in a GameObject to center around and attach 
        mesh to, size of the hexagon to be rendered, type of hexagon (flat/pointy),
        a color, its map coordinates as a Point, and a default zLayer of 0 
        (used to hexes if necessary).
        
        The updateHex function recalculates the details of the mesh, namely
        the size, positon, and vertices. Should only need to called when the 
        details of the mesh change, but I am unsure how to implement that.
        Currently, it updates at FixedUpdate() in MapRender 50 times a second;
        moving to only updating when necessary can help performance if it ever
        comes to it.
        */

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        Mesh mesh;

        Vector3[] vertices;
        Vector3[] normals;
        Color32[] colors;
        int[] triangles;

        GameObject gameObject;
        Point mapCoord;
        float zLayer = 0;

        float hexagonSizeMap;
        float hexagonSizeRender;
        float outlineSize;

        Vector3 targetPosition;
        int moveSpeed = 7;

        float targetSize;

        MapRender.HexType type;

        public HexagonMesh(GameObject gameObject, float hexagonSize, float outlineSize, MapRender.HexType type, Color color, Point mapCoord, float zLayer = 0) {
            //Given the location of where the hex is to be placed, render 6 triangles to form
            //a hexagon centered around the given point.

            this.gameObject = gameObject;
            this.hexagonSizeMap = hexagonSize;
            this.hexagonSizeRender = hexagonSize;
            this.outlineSize = outlineSize;
            this.mapCoord = mapCoord;
            this.type = type;
            this.zLayer = zLayer;

            this.targetPosition = gameObject.transform.position;
            this.targetSize = hexagonSize;

            meshFilter = gameObject.AddComponent<MeshFilter>();
            mesh = new Mesh();

            recalculateVertices();
            normals = new Vector3[7]{-Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward};
            triangles = new int[18]{0, 1, 2,  0, 2, 3,  0, 3, 4,  0, 4, 5,  0, 5, 6,  0, 6, 1};
            setColor(color);

            mesh = recalculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        public void updateHex(float hexagonSize, float outlineSize, MapRender.HexType type) {
            
            this.hexagonSizeMap = hexagonSize;
            this.hexagonSizeRender = hexagonSize;
            this.outlineSize = outlineSize;
            this.type = type;
            recalculateVertices();

            if(type == MapRender.HexType.Flat) {
                gameObject.transform.position = new Vector3(mapCoord.x * 3 / 4f * hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 4f + mapCoord.y * Mathf.Sqrt(3) / 2) * hexagonSize, zLayer);
            } else if (type == MapRender.HexType.Pointy) {
                gameObject.transform.position = new Vector3(mapCoord.x * Mathf.Sqrt(3) * hexagonSize / 2 + mapCoord.y * Mathf.Sqrt(3) / 4 * hexagonSize, mapCoord.y * 3 / 4f * hexagonSize, zLayer);
            }

            mesh = recalculateMesh(mesh);

            meshFilter.mesh = mesh;
        }

        public void updateHex(float hexagonSizeMap, float hexagonSizeRender, float outlineSize, MapRender.HexType type) {

            this.hexagonSizeMap = hexagonSizeMap;
            this.hexagonSizeRender = hexagonSizeRender;
            this.outlineSize = outlineSize;
            this.type = type;
            recalculateVertices();

            setTargetPos(mapCoord);
            smoothMove();
            
            mesh = recalculateMesh(mesh);

            meshFilter.mesh = mesh;
        } 

        public void setColor(Color color) {
            color = Color32.Lerp(Color.yellow, color, 0.3f);
            colors = new Color32[7]{Color32.Lerp(Color.yellow, color, 0.3f), color, color, color, color, color, color};
        }

        void recalculateVertices() {
            vertices = new Vector3[7];

            float posX = gameObject.transform.position.x;
            float posY = gameObject.transform.position.y;

            if(type == MapRender.HexType.Flat) {
                //Flat Top
                // The angles for each point from the origin to it are 0°, 60°, 120°, 180°, 240°, 300°
                // The for loop looks weird but it works;
                // It starts at Pi (which is 0°) and moves around counterclockwise
                for (int i = -3; i < 3; i++) {
                    var angle_rad = -Mathf.PI / 3 * i;
                    vertices[i + 4] = new Vector3(posX + hexagonSizeRender * Mathf.Cos(angle_rad) * outlineSize, posY + hexagonSizeRender * Mathf.Sin(angle_rad) * outlineSize, zLayer);
                }
                vertices[0] = gameObject.transform.position;
            } else if (type == MapRender.HexType.Pointy) {
                //Pointy Top
                // Only difference from the last is this starts at 30°, so + Pi/6
                vertices = new Vector3[7];
                for (int i = -3; i < 3; i++) {
                    var angle_rad = (-Mathf.PI / 3 * i) + Mathf.PI / 6;
                    vertices[i + 4] = new Vector3(posX + hexagonSizeRender * Mathf.Cos(angle_rad) * outlineSize, posY + hexagonSizeRender * Mathf.Sin(angle_rad) * outlineSize, zLayer);
                }
                vertices[0] = gameObject.transform.position;
            }
        }

        Mesh recalculateMesh(Mesh mesh) {
            mesh.Clear();
            
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            mesh.colors32 = colors;
            mesh.RecalculateNormals();

            return mesh;
        }

        void smoothMove() {
            Vector3 dir = targetPosition - gameObject.transform.position;

            if(dir != Vector3.zero){
                gameObject.transform.position += dir * Time.deltaTime * moveSpeed;
            
                if (Vector3.Magnitude(gameObject.transform.position - targetPosition) < 0.001f) {
                    gameObject.transform.position = targetPosition;
                }
            }
        }

        void smoothResize() {
            float intermediateSize = Mathf.Lerp(hexagonSizeMap, targetSize, 0.5f);

            if (intermediateSize != 0) {
                hexagonSizeRender = intermediateSize;

                if (intermediateSize < 0.001f) {
                    hexagonSizeRender = targetSize;
                }
            }
        }

        void setTargetPos(Point coord) {
            if(type == MapRender.HexType.Flat) {
                targetPosition = new Vector3(coord.x * 3 / 4f * hexagonSizeMap, (coord.x * Mathf.Sqrt(3) / 4f + coord.y * Mathf.Sqrt(3) / 2) * hexagonSizeMap, zLayer);
            } else if (type == MapRender.HexType.Pointy) {
                targetPosition = new Vector3(coord.x * Mathf.Sqrt(3) * hexagonSizeMap / 2 + coord.y * Mathf.Sqrt(3) / 4 * hexagonSizeMap, coord.y * 3 / 4f * hexagonSizeMap, zLayer);
            }
        }

        public Point getCoord() {
            return mapCoord;
        }
    }