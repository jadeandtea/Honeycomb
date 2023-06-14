using UnityEngine;

public class Flower
{
    MapSettings settings;
    //Used in MapRender to handle addressable flowers and associated Game Objects
    Point mapCoord;

    Vector3 targetPosition;

    const float ZLAYER = -0.1f;

    SpriteRenderer spriteRenderer;
    GameObject host;


    public bool isActive;

    public Flower(MapSettings settings, GameObject host, Sprite spriteAry, Point mapCoordinate, bool editMode = false) {
        this.settings = settings;
        this.mapCoord = mapCoordinate;
        this.host = host;

        host.transform.position = Vector3.zero;

        this.isActive = !editMode;

        spriteRenderer = host.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteAry;
    }

    public void update() {
        if(settings.type == MapSettings.HexType.Flat) {
            targetPosition = new Vector3(mapCoord.x * 3 / 2f * settings.hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 2f + mapCoord.y * Mathf.Sqrt(3)) * settings.hexagonSize, ZLAYER);
        } else if (settings.type == MapSettings.HexType.Pointy) {
            targetPosition = new Vector3(mapCoord.x * Mathf.Sqrt(3) * settings.hexagonSize + mapCoord.y * Mathf.Sqrt(3) / 2 * settings.hexagonSize, mapCoord.y * 3 / 2f * settings.hexagonSize, ZLAYER);
        }

        smoothMove();
        spriteRenderer.enabled = isActive;
    }

    void smoothMove() {
        Vector3 dir = targetPosition - host.transform.position;

        if(dir != Vector3.zero){
            host.transform.position += dir * Time.deltaTime * settings.moveSpeed;
        
            if (Vector3.Magnitude(host.transform.position - targetPosition) < 0.001f) {
                host.transform.position = targetPosition;
            }
        }
    }

    public void setActiveLocation(Point mapCoord) {
        // Instantly places tile at mapCoord
        if(settings.type == MapSettings.HexType.Flat) {
            targetPosition = new Vector3(mapCoord.x * 3 / 2f * settings.hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 2f + mapCoord.y * Mathf.Sqrt(3)) * settings.hexagonSize, ZLAYER);
        } else if (settings.type == MapSettings.HexType.Pointy) {
            targetPosition = new Vector3(mapCoord.x * Mathf.Sqrt(3) * settings.hexagonSize + mapCoord.y * Mathf.Sqrt(3) / 2 * settings.hexagonSize, mapCoord.y * 3 / 2f * settings.hexagonSize, ZLAYER);
        }

        host.transform.position = targetPosition;
    }

    public void Destroy() {
        GameObject.Destroy(host);
    }

    public void DestroyImmediate() {
        GameObject.DestroyImmediate(host);
    }
    

    public override string ToString()
    {
        return mapCoord.ToString();
    }

    public bool Equals(Flower flower) {
        return mapCoord.Equals(flower.mapCoord);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Flower);
    }

    public override int GetHashCode()
    {
        return mapCoord.GetHashCode();
    }
}
