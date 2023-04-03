using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float hexagonSize;
    Point newMapPosition;
    Stack<Point> previousMoves = new Stack<Point>();
    MapRender mapRender;
    MapManager mapManager;
    Vector3 targetPosition;
    Vector3 targetAngle;

    public int moveSpeed = 7;
    public float turnSpeed = 5f;
    private const float ZLAYER = -1.01f;
    MapRender.HexType type;
    LevelManager levelManager;

    Transform home;

    void Start() {
        mapRender = this.transform.parent.GetComponent<MapRender>();
        home = this.transform.parent.GetChild(1);
        hexagonSize = mapRender.hexagonSize;

        //Player starts at (0,0)
        //TODO set starting point based on level
        previousMoves.Push(new Point(0, 0));
        home.transform.position = calculateScreenPosition(new Point());
        
        newMapPosition = new Point();
        newMapPosition.Copy(previousMoves.Peek());

        targetPosition = new Vector3(0, 0, ZLAYER);
        targetAngle = Vector3.zero;

        levelManager = new LevelManager(1);
    }

    void Update()
    {
        this.hexagonSize = mapRender.hexagonSize;
        this.type = mapRender.type;

        if(Input.anyKeyDown) {
            if(type == MapRender.HexType.Flat) {
                movementFlat();
            } else if (type == MapRender.HexType.Pointy) {
                movementPointy();
            }
            levelManager.onPlayerMove();
        }

        newMapPosition.Copy(previousMoves.Peek());

        targetPosition = calculateScreenPosition(previousMoves.Peek());

        smoothMove();
        smoothRotate();
    }

    Vector3 calculateScreenPosition(Point nextPosition) {
        //Calculations for rendering player in correct Hex

        /*
        For some reason I have to double the position of the player so that it renders
        in the center of the hexagon; not sure why 
        */
        if (type == MapRender.HexType.Flat) {
            return new Vector3(nextPosition.x * 3 / 2f * hexagonSize, (nextPosition.x * Mathf.Sqrt(3) / 2f + nextPosition.y * Mathf.Sqrt(3)) * hexagonSize, ZLAYER);
        } else if (type == MapRender.HexType.Pointy) {
            return new Vector3(nextPosition.x * Mathf.Sqrt(3) * hexagonSize + nextPosition.y * Mathf.Sqrt(3) / 2f * hexagonSize, nextPosition.y * 3 / 2f * hexagonSize, ZLAYER);
        }
        //Should never get here
        Debug.Log("playerMovement Error: Misdefined HexType");
        return Vector3.zero;
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
        Point movementAmount = new Point();
        previousMapPosition.Copy(previousMoves.Peek());

        if(Input.GetKeyDown(KeyCode.Q)){
            movementAmount.Set(-1, 1);
            targetAngle = new Vector3(0, 0, 60);
        } else if(Input.GetKeyDown(KeyCode.W)){
            movementAmount.Set(0, 1);
            targetAngle = new Vector3(0, 0, 0);
        } else if(Input.GetKeyDown(KeyCode.E)){
            movementAmount.Set(1, 0);
            targetAngle = new Vector3(0, 0, 300);
        } else if(Input.GetKeyDown(KeyCode.A)){
            movementAmount.Set(-1, 0);
            targetAngle = new Vector3(0, 0, 120);
        } else if(Input.GetKeyDown(KeyCode.S)){
            movementAmount.Set(0, -1);
            targetAngle = new Vector3(0, 0, 180);
        } else if(Input.GetKeyDown(KeyCode.D)){
            movementAmount.Set(1, -1);
            targetAngle = new Vector3(0, 0, 240);
        }

        newMapPosition.Add(movementAmount);
        int index = mapRender.mapManager.getObstacles().IndexOf(new Obstacle(newMapPosition));

        if(Input.GetKeyDown(KeyCode.Z)) {
            undoMove();
        }
        else if (index >= 0) {
            mapRender.moveObstacle(index, movementAmount);
        }

        //Move player if the new point is within map boundaries
        else if(inMap(newMapPosition) && !previousMapPosition.Equals(newMapPosition)) {
            previousMoves.Push(new Point(newMapPosition));
        }
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

        Point previousMapPosition = new Point();
        Point movementAmount = new Point();
        previousMapPosition.Copy(previousMoves.Peek());

        if(Input.GetKeyDown(KeyCode.Q)){
            movementAmount.Set(-1, 1);
            targetAngle = new Vector3(0, 0, 30);
        }
        if(Input.GetKeyDown(KeyCode.W)){
            movementAmount.Set(0, 1);
            targetAngle = new Vector3(0, 0, -30);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            movementAmount.Set(-1, 0);
            targetAngle = new Vector3(0, 0, 90);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            movementAmount.Set(1, 0);
            targetAngle = new Vector3(0, 0, -90);
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            movementAmount.Set(0, -1);
            targetAngle = new Vector3(0, 0, 150);
        }
        if(Input.GetKeyDown(KeyCode.X)){
            movementAmount.Set(1, -1);
            targetAngle = new Vector3(0, 0, -150);
        }

        newMapPosition.Add(movementAmount);
        int index = mapRender.mapManager.getObstacles().IndexOf(new Obstacle(newMapPosition));

        if(Input.GetKeyDown(KeyCode.Z)) {
            undoMove();
        }
        else if (index >= 0) {
            mapRender.moveObstacle(index, movementAmount);
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
        mapManager = mapRender.mapManager;
        return mapManager.getCoordinates().Contains(point);
    }

    public Point getLocation() {
        return new Point(previousMoves.Peek());
    }
}
