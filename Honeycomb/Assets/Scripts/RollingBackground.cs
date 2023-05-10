using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBackground : MonoBehaviour
{
    public float scrollSpeed;

    new private Renderer renderer;
    private Vector2 savedOffset;
    float xMovement, yMovement;
    Vector2 direction;
    Vector2 offset;
    int flipTimingX, flipTimingY;
    float timeCounterX, timeCounterY;

    void Start () {
        renderer = GetComponent<Renderer> ();
        direction = Vector2.one;
        offset = Vector2.zero;
        flipTimingX = Random.Range(15, 30);
        flipTimingY = 0;
        timeCounterX = 0;
        timeCounterY = 0;
    }

    void FixedUpdate () {
        if(timeCounterX > flipTimingX) {
            flipX();
            timeCounterX = 0;
            flipTimingX = Random.Range(15, 30);
        }
        timeCounterX += Time.deltaTime;

        if(timeCounterY > flipTimingY) {
            flipY();
            timeCounterY = 0;
            flipTimingY = Random.Range(15, 30);
        }
        timeCounterY += Time.deltaTime;


        offset.x += Time.deltaTime * scrollSpeed * direction.x;
        offset.y += Time.deltaTime * scrollSpeed * direction.y;
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    void flipX() {
        direction.x *= -1;
    }

    void flipY() {
        direction.y *= -1;
    }
}
