using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class FlowerRender
{
    Point mapCoordinate;
    Sprite main;
    float hexagonSize;
    const float ZLAYER = -0.1f;
    MapRender.HexType type;
    Sprite[] spriteAry;
    SpriteRenderer spriteRenderer;
    GameObject host;
    AsyncOperationHandle<Sprite[]> spriteHandle;

    public string key;

    public FlowerRender(GameObject host, string key, Point mapCoordinate, float hexagonSize, MapRender.HexType type) {
        this.mapCoordinate = mapCoordinate;
        this.hexagonSize = hexagonSize;
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
            host.transform.position = new Vector3(mapCoordinate.x * 3 / 2f * hexagonSize, (mapCoordinate.x * Mathf.Sqrt(3) / 2f + mapCoordinate.y * Mathf.Sqrt(3)) * hexagonSize, ZLAYER);

            spriteRenderer.sprite = spriteAry[0];
        }
    }

    public void update(float hexagonSize, MapRender.HexType type) {
        this.hexagonSize = hexagonSize;
        this.type = type;

        if(type == MapRender.HexType.Flat) {
            host.transform.position = new Vector3(mapCoordinate.x * 3 / 2f * hexagonSize, (mapCoordinate.x * Mathf.Sqrt(3) / 2f + mapCoordinate.y * Mathf.Sqrt(3)) * hexagonSize, ZLAYER);
        } else if (type == MapRender.HexType.Pointy) {
            host.transform.position = new Vector3(mapCoordinate.x * Mathf.Sqrt(3) * hexagonSize + mapCoordinate.y * Mathf.Sqrt(3) / 2 * hexagonSize, mapCoordinate.y * 3 / 2f * hexagonSize, ZLAYER);
        }
    }

    void OnDestroy() {
        Addressables.Release(spriteHandle);
    }
}
