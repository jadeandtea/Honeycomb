using UnityEngine;

public class Tile
{
    private const float ZLAYER = 0f;
    MapSettings settings;

    GameObject host;

    HexagonMesh mesh;
    //Used for Level Creator
    MeshCollider collider;

    Point mapCoord;

    public bool isActive;
    public bool isWeak;

    public Tile(MapSettings settings, Point mapCoord, Transform parent, bool weak = false, bool editMode = false) {
        // A tile object consists of the hexagon mesh and the game object that holds the mesh.

        this.settings = settings;
        this.mapCoord = mapCoord;
        this.isWeak = weak;

        int s = -mapCoord.x - mapCoord.y;
        host = new GameObject(mapCoord.x + "," + mapCoord.y + "," + s);
        host.transform.position = Vector3.zero;

        // The collider is used for the level creator system
        host.layer = 3;
        collider = new MeshCollider();

        mesh = new HexagonMesh(settings, host, MapSettings.MeshType.Tile, mapCoord, ZLAYER);
        mesh.setColor(settings.tileOuterColor, settings.tileCenterColor, settings.centerColorWeight);
        mesh.setTargetLocation(mapCoord);

        this.isActive = !editMode;

        host.transform.SetParent(parent);
    }

    public void setActiveLocation(Point mapCoord) {
        // Instantly places tile at mapCoord
        mesh.setActiveLocation(mapCoord);
    }

    public void updateTile(Point mapCoord) {
        this.mapCoord = mapCoord;
        mesh.active(isActive);
        mesh.setTargetLocation(mapCoord);
        mesh.updateHex();
    }

    public void updateMesh() {
        mesh.recalculateMesh();
    }

    public void setTileMesh(Texture texture) {
        mesh.setTileMesh(texture);
    }

    public void setColor(Color outer, Color center, float t) {
        mesh.setColor(outer, center, t);
    }

    public void Destroy() {
        GameObject.Destroy(host);
    }
}
