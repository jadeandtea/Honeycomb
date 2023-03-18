using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float hexagonSize;
    Point newMapPosition;
    Stack<Point> previousMoves = new Stack<Point>();
    MapRender parentScript;
    Vector3 targetPosition;
    Vector3 targetAngle;

    public int moveSpeed = 7;
    public float turnSpeed = 5f;
    private const int Z = -1;
    MapRender.HexType type;

    void Start() {
        parentScript = this.transform.parent.GetComponent<MapRender>();
        hexagonSize = parentScript.hexagonSize;

        //Player starts at (0,0)
        previousMoves.Push(new Point(0, 0));
        
        newMapPosition = new Point();
        newMapPosition.Copy(previousMoves.Peek());

        targetPosition = new Vector3(0, 0, Z);
        targetAngle = Vector3.zero;
    }

    void Update()
    {
        this.hexagonSize = parentScript.hexagonSize;
        this.type = parentScript.type;

        if(Input.anyKeyDown) {
            movementFlat();
        }

        newMapPosition.Copy(previousMoves.Peek());

        calculateScreenPosition();

        smoothMove();
        smoothRotate();
    }

    void calculateScreenPosition() {
        //Calculations for rendering player in correct Hex

        /*
        For some reason I have to double the position of the player so that it renders
        in the center of the hexagon; not sure why 
        */
        Point nextPosition = previousMoves.Peek();
        if (type == MapRender.HexType.Flat) {
            targetPosition = new Vector3(nextPosition.x * 3 / 2f * hexagonSize, (nextPosition.x * Mathf.Sqrt(3) / 2f + nextPosition.y * Mathf.Sqrt(3)) * hexagonSize, Z);
        } else if (type == MapRender.HexType.Pointy) {
            targetPosition = new Vector3(nextPosition.x * Mathf.Sqrt(3) * hexagonSize + nextPosition.y * Mathf.Sqrt(3) / 2f * hexagonSize, nextPosition.y * 3 / 2f * hexagonSize, Z);
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

        Point previousMapPosition = new Point();
        previousMapPosition.Copy(previousMoves.Peek());

        if(Input.GetKeyDown(KeyCode.Q)){
            newMapPosition.x--;
            newMapPosition.y++;
            targetAngle = new Vector3(0, 0, 60);
        } else if(Input.GetKeyDown(KeyCode.W)){
            newMapPosition.y++;
            targetAngle = new Vector3(0, 0, 0);
        } else if(Input.GetKeyDown(KeyCode.E)){
            newMapPosition.x++;
            targetAngle = new Vector3(0, 0, 300);
        } else if(Input.GetKeyDown(KeyCode.A)){
            newMapPosition.x--;
            targetAngle = new Vector3(0, 0, 120);
        } else if(Input.GetKeyDown(KeyCode.S)){
            newMapPosition.y--;
            targetAngle = new Vector3(0, 0, 180);
        } else if(Input.GetKeyDown(KeyCode.D)){
            newMapPosition.x++;
            newMapPosition.y--;
            targetAngle = new Vector3(0, 0, 240);
        }
        if(Input.GetKeyDown(KeyCode.Z)) {
            undoMove();
        }

        //Move player if the new point is within map boundaries
        else if(inMap(newMapPosition) && !previousMapPosition.Equals(newMapPosition)) {
            previousMoves.Push(new Point(newMapPosition));
        }
    }

    void undoMove() {
        if(previousMoves.Count > 1){
            newMapPosition = previousMoves.Pop();
        }
    }

    void smoothMove() {

        Vector3 dir = targetPosition - transform.position;

        transform.position += dir * Time.deltaTime * moveSpeed;
        
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.001f) {
            transform.position = targetPosition;
        }
    }

    void smoothRotate() {
        //Rotates player to face in recently moved direction

        float rotationalDifference = targetAngle.z - transform.eulerAngles.z;

        if(rotationalDifference > 180 ){
            rotationalDifference = -(360 - rotationalDifference);
        }
        if(rotationalDifference < -180) {
            rotationalDifference = 360 + rotationalDifference;
        }

        transform.eulerAngles += new Vector3(0, 0, rotationalDifference * Time.deltaTime * turnSpeed);

        if (Vector3.Magnitude(transform.eulerAngles - targetAngle) < 0.3f) {
            transform.eulerAngles = targetAngle;
        }
    }

    bool inMap(Point point) {
        if(parentScript.openCoordinates.Contains(point)) {
            return true;
        }
        return false;
    }
}
