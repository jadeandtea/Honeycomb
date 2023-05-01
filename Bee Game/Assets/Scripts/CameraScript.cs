using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject mapRenderer;
    Transform player;
    MapRender mapRender;
    int moveSpeed;
    float playerWeight;

    public bool fixedCamera = false;
    void Start() {

        moveSpeed = 7;

        player = GameObject.Find("Player").transform;

        mapRenderer = GameObject.Find("MapRenderer");
        mapRender = mapRenderer.GetComponent<MapRender>();

        playerWeight = mapRender.GetComponentInChildren<Transform>().childCount * 3;
    }
    
    void Update()
    {
        float xSum = 0, ySum = 0;
        float xAvg = 0, yAvg = 0;

        if(mapRender != null && mapRender.GetComponentInChildren<Transform>().childCount > 0 && !fixedCamera) {
            foreach(Transform child in mapRender.GetComponentInChildren<Transform>()) {
                xSum += child.position.x;
                ySum += child.position.y;
            }
            
            xSum += player.transform.position.x * playerWeight;
            ySum += player.transform.position.y * playerWeight;
            

            xAvg = (Mathf.Abs(xSum) > 0.1) ? xSum / (mapRender.mapManager.getCoordinates().Count + playerWeight) : xAvg = 0;
            yAvg = (Mathf.Abs(ySum) > 0.1) ? ySum / (mapRender.mapManager.getCoordinates().Count + playerWeight) : yAvg = 0;
        }
        
        
        smoothMove(new Vector3(xAvg, yAvg, transform.position.z));
        handleZoom();

        // this.transform.LookAt(Vector3.Lerp(Vector3.zero, player.position, 0.2f), Vector3.up);
    }

    void smoothMove(Vector3 targetPosition) {

        Vector3 dir = targetPosition - transform.position;

        transform.position += dir * Time.deltaTime * moveSpeed;
        
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.001f) {
            transform.position = targetPosition;
        }
    }

    void handleZoom() {
        if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0) {
            transform.position += new Vector3(0, 0, Input.mouseScrollDelta.y * Time.deltaTime * 10f);
        } 
    }
}
