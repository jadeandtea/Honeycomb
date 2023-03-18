using UnityEngine;

public class FlowerRender
{
    Point mapCoordinate;
    Sprite main;
    float hexagonSize;
    float zLayer = -0.1f;
    MapRender.HexType type;

    public FlowerRender(Sprite[] spriteArray, Point mapCoordinate, float hexagonSize, MapRender.HexType type) {
        this.mapCoordinate = mapCoordinate;
        this.hexagonSize = hexagonSize;

        if(spriteArray != null) {
            main = spriteArray[Random.Range(0, spriteArray.Length)];
        }
    }
}
