using UnityEngine;

[CreateAssetMenu]
public class  MapSettings : ScriptableObject {
    public enum HexType {
        Flat, Pointy
    }
    public enum MeshType {
        Tile, Obs
    }
    // Settings

    public HexType type = HexType.Flat;
    public float hexagonSize = 1;
    [Range(0, 1)]
    public float outlineSize = 0.05f;

    public Color tileOuterColor = new Color(204, 133, 0, 0);
    public Color tileCenterColor = Color.white;
    public Color obsOuterColor = Color.black;
    public Color obsCenterColor = new Color(153, 99, 0, 0);
    [Range(0,1)]
    public float centerColorWeight = 0.3f;
}