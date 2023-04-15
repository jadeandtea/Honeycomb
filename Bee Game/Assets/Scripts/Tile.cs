using UnityEngine;

public class Tile
{
    MapSettings settings;
    const float ZLAYER = 0f;

    GameObject host;

    HexagonMesh mesh;
    //Used for Level Creator
    MeshCollider collider;

    Point mapCoord;

    public bool isActive;

    public Tile(MapSettings settings, Point mapCoord, Transform parent, bool editMode = false) {
        this.settings = settings;
        this.mapCoord = mapCoord;
        this.isActive = true;

        int s = -mapCoord.x - mapCoord.y;
        host = new GameObject(mapCoord.x + "," + mapCoord.y + "," + s);
        host.transform.position = Vector3.zero;

        host.layer = 3;
        collider = new MeshCollider();

        mesh = new HexagonMesh(settings, host, MapSettings.MeshType.Tile, mapCoord, ZLAYER);
        mesh.setColor(settings.tileOuterColor, settings.tileCenterColor, settings.centerColorWeight);
        mesh.setLocation(mapCoord);

        if (editMode) {
            mesh.addCollider();
            isActive = false;
        }

        host.transform.SetParent(parent);
    }


    public void updateTile(float hexagonSize, float outlineSize, MapSettings.HexType type) {
        mesh.active(isActive);
        mesh.updateHex();
    }

    public void setColor(Color outer, Color center, float t) {
        mesh.setColor(outer, center, t);
    }

    public Point getPoint() {
        return mapCoord;
    }


    public override string ToString()
    {
        return mapCoord.ToString();
    }

    public bool Equals(Tile tile)
    {
        return mapCoord.Equals(tile.getPoint());
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Tile);
    }

    public override int GetHashCode()
    {
        return mapCoord.GetHashCode();
    }
}
