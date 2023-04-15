using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public MapSettings settings;
    float hexagonSize;
    public Point mapPosition;
    [SerializeField]
    Point movementAmount;
    MapRender mapRender;
    Vector3 targetPosition;
    Vector3 targetAngle;

    public int moveSpeed = 7;
    public float turnSpeed = 5f;
    private const float ZLAYER = -1.01f;
    MapSettings.HexType type;

    void Awake() {
        mapRender = GameObject.Find("MapRenderer").GetComponent<MapRender>();

        hexagonSize = settings.hexagonSize;

        //Player starts at (0,0)
        mapPosition = new Point(0, 0);

        targetPosition = new Vector3(0, 0, ZLAYER);
        targetAngle = Vector3.zero;
    }

    void Update()
    {
        this.hexagonSize = settings.hexagonSize;
        this.type = settings.type;

        if(Input.anyKeyDown) {
            if(type == MapSettings.HexType.Flat) {
                movementFlat();
            } else if (type == MapSettings.HexType.Pointy) {
                movementPointy();
            }
        }

        targetPosition = calculateScreenPosition(mapPosition);

        smoothMove();
        smoothRotate();
    }

    Vector3 calculateScreenPosition(Point nextPosition) {
        //Calculations for rendering player in correct Hex

        /*
        For some reason I have to double the position of the player so that it renders
        in the center of the hexagon; not sure why 
        */
        if (type == MapSettings.HexType.Flat) {
            return new Vector3(nextPosition.x * 3 / 2f * hexagonSize, (nextPosition.x * Mathf.Sqrt(3) / 2f + nextPosition.y * Mathf.Sqrt(3)) * hexagonSize, ZLAYER);
        } else if (type == MapSettings.HexType.Pointy) {
            return new Vector3(nextPosition.x * Mathf.Sqrt(3) * hexagonSize + nextPosition.y * Mathf.Sqrt(3) / 2f * hexagonSize, nextPosition.y * 3 / 2f * hexagonSize, ZLAYER);
        }
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

        // Cache the current location. Create a vector that determines the direction the player moves in
        // Check to see if moving the player by the vector would move onto an obstacle; if true, move the obstacle instead
        // of the player by the vector
        // Check to see if the player would be moving outside of the map and make sure the player is actually moving before
        // adding the value to past locations
        movementAmount = new Point();
        Point previousMapPosition = new Point(mapPosition);

        //TODO implement undo function

        // if(Input.GetKeyDown(KeyCode.Z)) {
        //     undoMove();
        // } else {
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
            // mapRender.cache(mapPosition);
            // The player "moves" anytime there is any input, but that movement can be (0, 0)
            // The default is to move the player; only when there is an obstacle or when 
            // the target tile isn't active is when the player doesn't move.
            mapPosition.Add(movementAmount);

                //Check to see if movement would end up in an active obstacle.
                //If so, move the obstacle and keep the player still.
            if (mapRender.getObstacles().ContainsKey(mapPosition) && mapRender.getObstacles()[mapPosition].isActive) {
                mapRender.moveObstacle(mapPosition, movementAmount);
                mapPosition.Sub(movementAmount);
            }
                //Don't move the player if they are outside the map
            else if(!inMap(mapPosition)) {
                mapPosition.Sub(movementAmount);
            } 
        // }
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

        // Point previousMapPosition = new Point();
        // Point movementAmount = new Point();
        // previousMapPosition.Copy(mapPosition.Sub(mapRender.mapManager.peekPlayerHistory()));

        // if(Input.GetKeyDown(KeyCode.Q)){
        //     movementAmount.Set(-1, 1);
        //     targetAngle = new Vector3(0, 0, 30);
        // }
        // if(Input.GetKeyDown(KeyCode.W)){
        //     movementAmount.Set(0, 1);
        //     targetAngle = new Vector3(0, 0, -30);
        // }
        // if(Input.GetKeyDown(KeyCode.A)){
        //     movementAmount.Set(-1, 0);
        //     targetAngle = new Vector3(0, 0, 90);
        // }
        // if(Input.GetKeyDown(KeyCode.S)){
        //     movementAmount.Set(1, 0);
        //     targetAngle = new Vector3(0, 0, -90);
        // }
        // if(Input.GetKeyDown(KeyCode.Z)){
        //     movementAmount.Set(0, -1);
        //     targetAngle = new Vector3(0, 0, 150);
        // }
        // if(Input.GetKeyDown(KeyCode.X)){
        //     movementAmount.Set(1, -1);
        //     targetAngle = new Vector3(0, 0, -150);
        // }

        // mapPosition.Add(movementAmount);
        // int index = mapRender.mapManager.getObstacles().IndexOf(new Obstacle(mapPosition));

        // if(Input.GetKeyDown(KeyCode.Z)) {
        //     undoMove();
        // }
        // else if (index >= 0) {
        //     mapRender.moveObstacle(index, movementAmount);
        // }

        // //Move player if the new point is within map boundaries
        // else if(inMap(mapPosition) && !previousMapPosition.Equals(mapPosition)) {
        //     mapRender.mapManager.pushPlayerHistory(new Point(movementAmount.flip()));
        // } else {
        //     mapPosition.Sub(movementAmount);
        // }
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
        return mapRender.getCoordinates().Contains(point);
    }
}
