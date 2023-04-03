using System;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : IEquatable<Obstacle>
{
    const float ZLAYER = -0.1f;

    GameObject host;

    HexagonMesh mesh;

    Point mapCoord;
    List<Point> history;

    public Obstacle(Point mapCoord, float hexagonSize, float outlineSize, MapRender.HexType type, Color hexColor, Transform parent) {

        this.mapCoord = mapCoord;

        int s = -mapCoord.x - mapCoord.y;
        host = new GameObject("(" + mapCoord.x + ", " + mapCoord.y + ", " + s + ")");
        host.transform.position = Vector3.zero;

        mesh = new HexagonMesh(host, hexagonSize, outlineSize, type, hexColor, mapCoord, ZLAYER);

        host.transform.SetParent(parent);

    }

    //For comparison purposes only; DO NOT actually use this to construct an Obstacle
    public Obstacle(Point mapCoord){
        this.mapCoord = mapCoord;
    }

    public void moveObstacle(Point dir){
        mapCoord.Add(dir);
    }

    public Point getCoord() {
        return mapCoord;
    }

    public void updateObs(float hexagonSizeMap, float hexagonSizeRender, float outlineSize, MapRender.HexType type) {
        mesh.updateHex(hexagonSizeMap, hexagonSizeRender, outlineSize, type);
    }

    public bool Equals(Obstacle obstacle){
        return mapCoord.Equals(obstacle.getCoord());
    }

    public void Destroy() {
        GameObject.Destroy(host);
    }
}