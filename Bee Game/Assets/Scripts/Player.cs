using UnityEngine;

public class Player : MonoBehaviour
{
    public MapSettings settings;
    public Point mapPosition;
    Point movementAmount;
    MapRender mapRender;
    Vector3 targetPosition;
    Vector3 targetAngle;

    private const float ZLAYER = -1.01f;

    bool resetButtonDown = false;
    float resetCounter = 0f;

    void Awake() {
        mapRender = GameObject.Find("MapRenderer").GetComponent<MapRender>();

        //Player starts at (0,0), regardless of the level
        mapPosition = new Point(0, 0);

        // Initial Player Orientation
        targetPosition = new Vector3(0, 0, ZLAYER);
        targetAngle = Vector3.zero;
    }

    void Update()
    {
        if(Input.anyKeyDown) {
            if(settings.type == MapSettings.HexType.Flat) {
                movementFlat();
            } else if (settings.type == MapSettings.HexType.Pointy) {
                movementPointy();
            }

            resetButtonDown = (Input.GetKeyDown(KeyCode.R)) ? true : resetButtonDown = false;

            mapRender.checkWin(mapPosition);
        }

        if(resetButtonDown) {
            resetCounter += Time.deltaTime;
        } else {
            resetCounter = 0;
        }

        if (resetCounter > 1) {
            mapRender.reloadLevel();
            this.mapPosition = new Point(0 ,0);
            resetCounter = Mathf.NegativeInfinity;
        }

        // Constantly check if the player's mapPosition does not correspond to the mapPosition and
        // if the rotation is not the same as the target rotation
        targetPosition = calculateScreenPosition(mapPosition);

        smoothMove();
        smoothRotate();
    }

    Vector3 calculateScreenPosition(Point nextPosition) {
        // Takes in a map coordinate, returns the vector3 location that the player should be at for the corresponding coordinate

        /*
        For some reason I have to double the position of the player so that it renders
        in the center of the hexagon
        */
        if (settings.type == MapSettings.HexType.Flat) {
            return new Vector3(nextPosition.x * 3 / 2f * settings.hexagonSize, (nextPosition.x * Mathf.Sqrt(3) / 2f + nextPosition.y * Mathf.Sqrt(3)) * settings.hexagonSize, ZLAYER);
        } else if (settings.type == MapSettings.HexType.Pointy) {
            return new Vector3(nextPosition.x * Mathf.Sqrt(3) * settings.hexagonSize + nextPosition.y * Mathf.Sqrt(3) / 2f * settings.hexagonSize, nextPosition.y * 3 / 2f * settings.hexagonSize, ZLAYER);
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
            Z is undo, holding down R is reset
        */

        // Cache the current location. Create a vector that determines the direction the player moves in
        // Check to see if moving the player by the vector would move onto an obstacle; if true, move the obstacle instead
        // of the player by the vector
        // Check to see if the player would be moving outside of the map and make sure the player is actually moving before
        // recording the movement
        movementAmount = new Point(0, 0);
        Point previousMapPosition = new Point(mapPosition);

        if(Input.GetKeyDown(KeyCode.Z)) {
            undoMove();
        } else {
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
            mapPosition.Add(movementAmount);
            // The player "moves" anytime there is any input, but that movement can be (0, 0)
            // The default is to move the player; only when there is an obstacle or when 
            // the target tile isn't active is when the player doesn't move.

                // Check to see if movement would end up in an active obstacle.
                // If so, move the obstacle and keep the player still.
                // If the obstacle cannot move, don't save the movement
            if (mapRender.getPushables().ContainsKey(mapPosition) && mapRender.getPushables()[mapPosition].isActive) {
                Point temp = mapRender.moveObstacle(mapPosition, movementAmount);
                // Debug.Log("Obstacle moved from " + mapPosition + " to " + temp);
                if (!temp.Equals(mapPosition))
                    mapRender.logCache(mapPosition, temp, MovementCache.movedObject.Obstacle);
                mapPosition.Sub(movementAmount);
            }
            else if (mapRender.getFlowers().ContainsKey(mapPosition)) {
                mapRender.logCache(mapPosition.Sub(movementAmount), mapPosition.Add(movementAmount), MovementCache.movedObject.Player);
                mapRender.touchFlower(mapPosition);
            }
                //Don't move the player if they are outside the map
            else if(!inMap(mapPosition)) {
                mapPosition.Sub(movementAmount);
            } 
            else {
                mapRender.logCache(mapPosition.Sub(movementAmount), mapPosition.Add(movementAmount), MovementCache.movedObject.Player);
            }
        }
    }

    void undoMove() {
        mapRender.undo();
    }
    // Pretty much the same as movementFlat, just different vector directions and different keycodes
    void movementPointy() {
        /*  Logic for moving around the grid using qweasd and rotating player
            Q moves the character up-left
            W moves the character up-right
            A moves the character left
            S moves the character right
            Z moves the character down-left
            X moves the character down-right
            C is undo, holding down R is reset
        */

        movementAmount = new Point(0, 0);
        Point previousMapPosition = new Point(mapPosition);

        if(Input.GetKeyDown(KeyCode.C)) {
            undoMove();
        } else {
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
            mapPosition.Add(movementAmount);
            // The player "moves" anytime there is any input, but that movement can be (0, 0)
            // The default is to move the player; only when there is an obstacle or when 
            // the target tile isn't active is when the player doesn't move.

                // Check to see if movement would end up in an active obstacle.
                // If so, move the obstacle and keep the player still.
                // If the obstacle cannot move, don't save the movement
            if (mapRender.getPushables().ContainsKey(mapPosition) && mapRender.getPushables()[mapPosition].isActive) {
                Point temp = mapRender.moveObstacle(mapPosition, movementAmount);
                // Debug.Log("Obstacle moved from " + mapPosition + " to " + temp);
                if (!temp.Equals(mapPosition))
                    mapRender.logCache(mapPosition, temp, MovementCache.movedObject.Obstacle);
                mapPosition.Sub(movementAmount);
            }
            else if (mapRender.getFlowers().ContainsKey(mapPosition)) {
                mapRender.logCache(mapPosition.Sub(movementAmount), mapPosition.Add(movementAmount), MovementCache.movedObject.Player);
                mapRender.touchFlower(mapPosition);
            }
                //Don't move the player if they are outside the map
            else if(!inMap(mapPosition)) {
                mapPosition.Sub(movementAmount);
            } 
            else {
                mapRender.logCache(mapPosition.Sub(movementAmount), mapPosition.Add(movementAmount), MovementCache.movedObject.Player);
            }
        }
    }

    void smoothMove() {
        // Animation through code; take the direction from the current location to the target and move toward it.

        Vector3 dir = targetPosition - transform.position;

        transform.position += dir * Time.deltaTime * settings.moveSpeed;
        
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.001f) {
            transform.position = targetPosition;
        }
    }

    void smoothRotate() {
        //Rotates player to face in recently moved direction

        float rotationalDifference = targetAngle.z - transform.eulerAngles.z;


        // Weird conditions to make it rotate in the correct direction
        if(rotationalDifference > 180 ){
            rotationalDifference = -(360 - rotationalDifference);
        }
        if(rotationalDifference < -180) {
            rotationalDifference = 360 + rotationalDifference;
        }

        transform.eulerAngles += new Vector3(0, 0, rotationalDifference * Time.deltaTime * settings.turnSpeed);

        if (Vector3.Magnitude(transform.eulerAngles - targetAngle) < 0.3f) {
            transform.eulerAngles = targetAngle;
        }
    }

    bool inMap(Point point) {
        return mapRender.getCoordinates().Contains(point);
    }
}
