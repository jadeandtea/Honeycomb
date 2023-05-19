using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject mapRenderer;
    Transform player;
    MapParent mapRender;
    int moveSpeed;

    public bool fixedCamera = false;
    void Start() {

        moveSpeed = 7;

        player = GameObject.Find("Player").transform;

        mapRenderer = GameObject.Find("MapRenderer");
        mapRender = mapRenderer.GetComponent<MapParent>();
    }
    
    void Update()
    {
        float xSum = 0, ySum = 0;
        float xAvg = 0, yAvg = 0;

        Vector2 directionToPlayer = Vector2.zero;

        if(mapRender != null && mapRender.GetComponentInChildren<Transform>().childCount > 0 && !fixedCamera) {
            foreach(Transform child in mapRender.GetComponentInChildren<Transform>()) {
                if(child.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh) && mesh.enabled == true){
                    xSum += child.position.x;
                    ySum += child.position.y;
                }
            }

            xAvg = (Mathf.Abs(xSum) > 0.1) ? xSum / (mapRender.mapManager.getCoordinates().Count): xAvg = 0;
            yAvg = (Mathf.Abs(ySum) > 0.1) ? ySum / (mapRender.mapManager.getCoordinates().Count): yAvg = 0;

            directionToPlayer = new Vector2(xAvg + player.transform.position.x, yAvg + player.transform.position.y) / 3;
        }
        
        
        smoothMove(directionToPlayer);
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

    void smoothMove(Vector2 targetPosition) {
        smoothMove(new Vector3(targetPosition.x, targetPosition.y, transform.position.z));
    }

    void handleZoom() {
        if (Input.mouseScrollDelta.y > 0 && Camera.main.transform.position.z < -2) {
            transform.position += new Vector3(0, 0, Input.mouseScrollDelta.y * Time.deltaTime * 10f);
        } else if (Input.mouseScrollDelta.y < 0 && Camera.main.transform.position.z > -23) {
            transform.position += new Vector3(0, 0, Input.mouseScrollDelta.y * Time.deltaTime * 10f);
        }
    }
}
