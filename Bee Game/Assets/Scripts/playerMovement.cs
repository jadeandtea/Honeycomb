using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float hexagonSize;
    //Location of player in Hex grid
    [SerializeField]
    Vector2 mapPosition = Vector2.zero;
    MapRender parentScript;

    Vector3 targetPosition = new Vector3(0, 0, -1);
    Vector3 targetAngle = Vector3.zero;

    public int moveSpeed = 7;
    public int turnSpeed = 5;
    void Start() {
        parentScript = this.transform.parent.GetComponent<MapRender>();
        this.hexagonSize = parentScript.hexagonSize;
        float vOffset = hexagonSize * Mathf.Sqrt(3) / 2;

        //Player starts at (0,0)
        transform.position = targetPosition;
        transform.eulerAngles = targetAngle;
    }

    void Update()
    {
        if(Input.anyKeyDown){
            movement();
        }
        
        //Calculations for rendering player in correct Hex

        /*
        For some reason I have to double the position of the player so that it renders
        in the center of the hexagon; not sure why 
        For position calculations, all odd negative values are shifted up one
        */
        
        this.hexagonSize = parentScript.hexagonSize;
        float hOffset = hexagonSize * 3 / 2;
        float vOffset = hexagonSize * Mathf.Sqrt(3);
            
        targetPosition = new Vector3(mapPosition.x * hOffset, mapPosition.y * vOffset + mapPosition.x % 2 * vOffset/2, transform.position.z);

        Vector3 dir = targetPosition - transform.position;
        Vector3 rot = targetAngle - transform.eulerAngles;

        transform.position += dir * Time.deltaTime * moveSpeed;
        transform.eulerAngles += rot * Time.deltaTime * turnSpeed;

        if (Vector3.Magnitude(transform.position - targetPosition) < 0.1f) {
            transform.position = targetPosition;
        }
        if (Vector3.Magnitude(transform.eulerAngles - targetAngle) < 0.1f) {
            transform.eulerAngles = targetAngle;
        }
    }

    void calculateScreenPosition(float hexagonSize, Vector2 mapPosition) {

    }

    void movement() {
        /*  Logic for moving around the grid using qweasd and rotating player
            Q moves the character up-left
            W moves the character up
            E moves the character up-right
            A moves the character down-left
            S moves the character down
            D moves the character down-right
        */

        if(Input.GetKeyDown(KeyCode.Q)){
            mapPosition.x--;
            if(mapPosition.x % 2 == 0){
                mapPosition.y++;
            }
            targetAngle = new Vector3(0, 0, 60);
        }
        if(Input.GetKeyDown(KeyCode.W)){
            mapPosition.y++;
            targetAngle = new Vector3(0, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            mapPosition.x++;
            if(mapPosition.x % 2 == 0){
                mapPosition.y++;
            }
            targetAngle = new Vector3(0, 0, 300);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            mapPosition.x--;
            if(mapPosition.x % 2 != 0){
                mapPosition.y--;
            }
            targetAngle = new Vector3(0, 0, 120);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            mapPosition.y--;
            targetAngle = new Vector3(0, 0, 180);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            mapPosition.x++;
            if(mapPosition.x % 2 != 0){
                mapPosition.y--;
            }
            targetAngle = new Vector3(0, 0, 240);
        }
    }
}
