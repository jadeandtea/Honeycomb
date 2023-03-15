using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float hexagonSize;
    //Location of player in Hex grid
    [SerializeField]
    //Any Coordinates are stored in Axial
    Point mapPosition;
    [SerializeField]
    Stack<Point> previousMoves = new Stack<Point>();
    MapRender parentScript;
    Vector3 targetPosition = new Vector3(0, 0, -1);
    Vector3 targetAngle = Vector3.zero;

    public int moveSpeed = 7;
    public float turnSpeed = 5f;
    MapRender.Type type;

    void Start() {
        parentScript = this.transform.parent.GetComponent<MapRender>();
        hexagonSize = parentScript.hexagonSize;
        mapPosition = new Point(0, 0);

        //Player starts at (0,0)
        transform.position = targetPosition;
        transform.eulerAngles = targetAngle;
    }

    void Update()
    {
        this.hexagonSize = parentScript.hexagonSize;
        this.type = parentScript.type;

        if(type == MapRender.Type.Flat){
            movementFlat();
        } else if (type == MapRender.Type.Pointy) {
            movementPointy();
        }
        if(!inMap()) {
            mapPosition = previousMoves.Pop();
        }

        calculateScreenPosition();

        smoothMove();
    }

    void calculateScreenPosition() {
        //Calculations for rendering player in correct Hex

        /*
        For some reason I have to double the position of the player so that it renders
        in the center of the hexagon; not sure why 
        */
        if (type == MapRender.Type.Flat) {
            targetPosition = new Vector3(mapPosition.x * 3 / 2f * hexagonSize, (mapPosition.x * Mathf.Sqrt(3) / 2f + mapPosition.y * Mathf.Sqrt(3)) * hexagonSize, transform.position.z);
        } else if (type == MapRender.Type.Pointy) {
            targetPosition = new Vector3(mapPosition.x * Mathf.Sqrt(3) * hexagonSize + mapPosition.y * Mathf.Sqrt(3) / 2f * hexagonSize, mapPosition.y * 3 / 2f * hexagonSize, transform.position.z);
        }
    }

    void movementFlat() {
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
            mapPosition.y++;
            targetAngle = new Vector3(0, 0, 60);
        }
        if(Input.GetKeyDown(KeyCode.W)){
            mapPosition.y++;
            targetAngle = new Vector3(0, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            mapPosition.x++;
            targetAngle = new Vector3(0, 0, -60);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            mapPosition.x--;
            targetAngle = new Vector3(0, 0, 120);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            mapPosition.y--;
            targetAngle = new Vector3(0, 0, 180);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            mapPosition.x++;
            mapPosition.y--;
            targetAngle = new Vector3(0, 0, -120);
        }

        //After moving, add the position to the stack
        previousMoves.Push(new Point(mapPosition));
    }

    void movementPointy() {
        /*  Logic for moving around the grid using qweasd and rotating player
            Q moves the character up-left
            W moves the character up-right
            A moves the character left
            S moves the character right
            Z moves the character down-left
            X moves the character down-right
        */
        if(Input.GetKeyDown(KeyCode.Q)){
            mapPosition.x--;
            mapPosition.y++;
            targetAngle = new Vector3(0, 0, 60);
        }
        if(Input.GetKeyDown(KeyCode.W)){
            mapPosition.y++;
            targetAngle = new Vector3(0, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            mapPosition.x--;
            targetAngle = new Vector3(0, 0, 300);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            mapPosition.x++;
            targetAngle = new Vector3(0, 0, 120);
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            mapPosition.y--;
            targetAngle = new Vector3(0, 0, 180);
        }
        if(Input.GetKeyDown(KeyCode.X)){
            mapPosition.x++;
            mapPosition.y--;
            targetAngle = new Vector3(0, 0, 240);
        }
    }

    void smoothMove() {
        //TODO Fix rotation
        Vector3 dir = targetPosition - transform.position;
        Vector3 rot;
        if (targetAngle.z < 0) {
            rot = targetAngle - (transform.eulerAngles - new Vector3(0, 0, 360));
        } else {
            rot = targetAngle - transform.eulerAngles;
        }

        transform.position += dir * Time.deltaTime * moveSpeed;
        transform.eulerAngles += rot * Time.deltaTime * turnSpeed;
        
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.1f) {
            transform.position = targetPosition;
        }
        if (Vector3.Magnitude(transform.eulerAngles - targetAngle) < 0.1f) {
            transform.eulerAngles = targetAngle;
        }
    }

    bool inMap() {
        if(parentScript.CurrentCoordinateList.Contains(mapPosition)) {
            return true;
        }
        return false;
    }
}
