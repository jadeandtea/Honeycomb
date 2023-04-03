using UnityEngine;

public class Tile
{
    const float ZLAYER = 0f;

    GameObject host;

    HexagonMesh mesh;

    Point mapCoord;

    public Tile(Point mapCoord, float hexagonSize, float outlineSize, MapRender.HexType type, Color hexColor, Transform parent) {
        int s = -mapCoord.x - mapCoord.y;
        host = new GameObject("(" + mapCoord.x + ", " + mapCoord.y + ", " + s + ")");
        host.transform.position = Vector3.zero;

        mesh = new HexagonMesh(host, hexagonSize, outlineSize, type, hexColor, mapCoord, ZLAYER);

        host.transform.SetParent(parent);
    }

    public void updateTile(float hexagonSize, float outlineSize, MapRender.HexType type) {
        mesh.updateHex(hexagonSize, outlineSize, type);
    }
}
