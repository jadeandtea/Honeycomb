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

        MapSettings settings;
        MapSettings.MeshType meshType;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider collider;
        Mesh mesh;

        Vector3[] vertices;
        Vector3[] normals;
        Vector2[] uvs;
        Color32[] colors;
        int[] triangles;

        GameObject gameObject;
        Point mapCoord;
        float zLayer = 0;

        Vector3 targetPosition;
        int moveSpeed = 7;

        MapSettings.HexType type;

        bool isActive;

        public HexagonMesh(MapSettings settings, GameObject gameObject, MapSettings.MeshType meshType, Point mapCoord, float zLayer = 0) {
            //Given the location of where the hex is to be placed, render 6 triangles to form
            //a hexagon centered around the given point.

            this.settings = settings;
            this.gameObject = gameObject;
            this.meshType = meshType;
            
            this.mapCoord = mapCoord;
            this.type = settings.type;
            this.zLayer = zLayer;

            this.isActive = true;

            this.targetPosition = gameObject.transform.position;

            meshFilter = gameObject.AddComponent<MeshFilter>();
            mesh = new Mesh();

            collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;

            recalculateVertices();
            normals = new Vector3[7]{-Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward};
            triangles = new int[18]{0, 1, 2,  0, 2, 3,  0, 3, 4,  0, 4, 5,  0, 5, 6,  0, 6, 1};
            uvs = new Vector2[7] {new Vector2(0.5f, 0.5f), new Vector2(1, 0.5f), new Vector2(0.75f, 1), new Vector2(0.25f, 1), new Vector2(0, 0.5f), new Vector2(0.25f, 0), new Vector2(0.75f, 1)};
            if (meshType == MapSettings.MeshType.Tile) {
                setColor(settings.tileOuterColor, settings.tileCenterColor, settings.centerColorWeight);
            } else if (meshType == MapSettings.MeshType.Obs){
                setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
            } else {
                Debug.Log("MapSettings not initialized?");
            }
            
            mesh = recalculateMesh(mesh);

            meshFilter.mesh = mesh;

            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        public void updateHex() {

            this.type = settings.type;
            recalculateVertices();

            if(type == MapSettings.HexType.Flat) {
                targetPosition = new Vector3(mapCoord.x * 3 / 2f * settings.hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 2f + mapCoord.y * Mathf.Sqrt(3)) * settings.hexagonSize, zLayer);
            } else if (type == MapSettings.HexType.Pointy) {
                targetPosition = new Vector3(mapCoord.x * Mathf.Sqrt(3) * settings.hexagonSize + mapCoord.y * Mathf.Sqrt(3) / 2 * settings.hexagonSize, mapCoord.y * 3 / 2f * settings.hexagonSize, zLayer);
            }
            smoothMove();

            // mesh = recalculateMesh(mesh);

            meshFilter.mesh = mesh;
            collider.sharedMesh = mesh;
            meshRenderer.enabled = isActive;
        }

        public void setTargetLocation(Point mapCoord) {
            this.mapCoord = mapCoord;
        }

        public void setActiveLocation(Point mapCoord) {
            if(type == MapSettings.HexType.Flat) {
                targetPosition = new Vector3(mapCoord.x * 3 / 2f * settings.hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 2f + mapCoord.y * Mathf.Sqrt(3)) * settings.hexagonSize, zLayer);
            } else if (type == MapSettings.HexType.Pointy) {
                targetPosition = new Vector3(mapCoord.x * Mathf.Sqrt(3) * settings.hexagonSize + mapCoord.y * Mathf.Sqrt(3) / 2 * settings.hexagonSize, mapCoord.y * 3 / 2f * settings.hexagonSize, zLayer);
            }
            gameObject.transform.position = targetPosition;
        }

        public void setColor(Color mainColor, Color centerColor, float t) {
            colors = new Color32[7]{Color.Lerp(mainColor, centerColor, t), mainColor, mainColor, mainColor, mainColor, mainColor, mainColor};
        }

        public void setTileMesh(Texture texture) {
            meshRenderer.material.SetTexture("_BaseMap", texture);
        }

        // Colliders are for the level editor
        // On a mouse input, the engine sends a raycast straight from the camera, and it only hits a gameObject if 
        // it has a collider of some sort

        public void recalculateMesh() {
            mesh = recalculateMesh(mesh);
        }

        void recalculateVertices() {
            vertices = new Vector3[7];

            float hexagonSizeRender = 0;
            if (meshType == MapSettings.MeshType.Tile) {
                hexagonSizeRender = settings.hexagonSize;
            } else if (meshType == MapSettings.MeshType.Obs){
                hexagonSizeRender = settings.hexagonSize / 2;
            } else {
                Debug.Log("MapSettings not initialized");
            }

            if(type == MapSettings.HexType.Flat) {
                // The angles for each point from the origin to it are 0°, 60°, 120°, 180°, 240°, 300°
                // The loop starts at Pi (which is 0°) and moves around counterclockwise
                for (int i = -3; i < 3; i++) {
                    var angle_rad = -Mathf.PI / 3 * i;
                    vertices[i + 4] = new Vector3(hexagonSizeRender * Mathf.Cos(angle_rad) * settings.outlineSize, hexagonSizeRender * Mathf.Sin(angle_rad) * settings.outlineSize, zLayer);
                }
                vertices[0] = Vector3.zero;
            } else if (type == MapSettings.HexType.Pointy) {
                // Only difference from the last is this starts at 30°, so + Pi/6
                vertices = new Vector3[7];
                for (int i = -3; i < 3; i++) {
                    var angle_rad = (-Mathf.PI / 3 * i) + Mathf.PI / 6;
                    vertices[i + 4] = new Vector3(hexagonSizeRender * Mathf.Cos(angle_rad) * settings.outlineSize, hexagonSizeRender * Mathf.Sin(angle_rad) * settings.outlineSize, zLayer);
                }
                vertices[0] = Vector3.zero;
            }
        }

        Mesh recalculateMesh(Mesh mesh) {
            mesh.Clear();
            
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            // mesh.colors32 = colors;
            mesh.uv = uvs;
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

        public void active(bool isActive) {
            this.isActive = isActive;
        }
    }