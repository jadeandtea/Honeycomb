using System;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : /*HexagonMesh,*/ IEquatable<Obstacle>
{
    MapSettings settings;
    const float ZLAYER = -0.1f;

    GameObject host;

    HexagonMesh mesh;

    Point mapCoord;

    public bool isActive;

    public Obstacle() {}

    public Obstacle(MapSettings settings, Point mapCoord, Transform parent) {
        this.settings = settings;
        this.mapCoord = mapCoord;

        int s = -mapCoord.x - mapCoord.y;
        host = new GameObject("(" + mapCoord.x + ", " + mapCoord.y + ", " + s + ")");
        host.transform.position = Vector3.zero;

        mesh = new HexagonMesh(settings, host, MapSettings.MeshType.Obs, mapCoord, ZLAYER);
        mesh.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        mesh.setLocation(mapCoord);

        host.transform.SetParent(parent);
    }

    public void push(Point dir){
        mapCoord.Add(dir);
    }

    public Point getCoord() {
        return mapCoord;
    }

    public void updateObs() {
        mesh.active(isActive);
        mesh.updateHex();
    }

    public void setColor(Color outer, Color center, float t){
        mesh.setColor(outer, center, t);
    }

    public bool Equals(Obstacle obstacle){
        return mapCoord.Equals(obstacle.getCoord());
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Obstacle);
    }

    public override int GetHashCode()
    {
        return mapCoord.GetHashCode();
    }

    public override string ToString()
    {
        return mapCoord.ToString();
    }
}