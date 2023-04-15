using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class Flower
{
    MapSettings settings;
    //Used in MapRender to handle addressable flowers and associated Game Objects
    Point mapCoord;
    Sprite main;
    const float ZLAYER = -0.1f;
    Sprite[] spriteAry;
    SpriteRenderer spriteRenderer;
    GameObject host;
    AsyncOperationHandle<Sprite[]> spriteHandle;

    public string key;

    public Flower(MapSettings settings, GameObject host, string key, Point mapCoordinate) {
        this.settings = settings;
        this.mapCoord = mapCoordinate;
        this.host = host;
        this.key = key;

        spriteAry = new Sprite[1];
        spriteRenderer = host.AddComponent<SpriteRenderer>();

        loadSprites();
    }

    void loadSprites() {
        spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(key);
        spriteHandle.Completed += LoadSpritesWhenReady;
    }

    void LoadSpritesWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck) { 
        if(handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteAry = handleToCheck.Result;
            host.transform.position = new Vector3(mapCoord.x * 3 / 2f * settings.hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 2f + mapCoord.y * Mathf.Sqrt(3)) * settings.hexagonSize, ZLAYER);

            spriteRenderer.sprite = spriteAry[0];
        }
    }

    public void update() {
        if(settings.type == MapSettings.HexType.Flat) {
            host.transform.position = new Vector3(mapCoord.x * 3 / 2f * settings.hexagonSize, (mapCoord.x * Mathf.Sqrt(3) / 2f + mapCoord.y * Mathf.Sqrt(3)) * settings.hexagonSize, ZLAYER);
        } else if (settings.type == MapSettings.HexType.Pointy) {
            host.transform.position = new Vector3(mapCoord.x * Mathf.Sqrt(3) * settings.hexagonSize + mapCoord.y * Mathf.Sqrt(3) / 2 * settings.hexagonSize, mapCoord.y * 3 / 2f * settings.hexagonSize, ZLAYER);
        }
    }

    void OnDestroy() {
        Addressables.Release(spriteHandle);
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
