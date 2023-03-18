using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject mapRenderer;
    MapRender mapRender;
    int moveSpeed;
    void Start() {

        moveSpeed = 7;

        mapRenderer = GameObject.Find("MapRenderer");
        mapRender = mapRenderer.GetComponent<MapRender>();
    }
    void Update()
    {
        float xSum = 0, ySum = 0;

        foreach(Transform child in mapRender.GetComponentInChildren<Transform>()) {
            xSum += child.position.x;
            ySum += child.position.y;
        }

        float xAvg = xSum / mapRender.openCoordinates.Count;
        float yAvg = ySum / mapRender.openCoordinates.Count;
        
        smoothMove(new Vector3(xAvg, yAvg, transform.position.z));
    }

    void smoothMove(Vector3 targetPosition) {

        Vector3 dir = targetPosition - transform.position;

        transform.position += dir * Time.deltaTime * moveSpeed;
        
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.001f) {
            transform.position = targetPosition;
        }
    }
}
