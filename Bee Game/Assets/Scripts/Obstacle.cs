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
    public bool isPushable;

    public Obstacle(MapSettings settings, Point mapCoord, Transform parent, bool isPushable = false, bool editMode = true) {
        this.settings = settings;
        this.mapCoord = mapCoord;
        this.isPushable = isPushable;

        this.isActive = !editMode;

        int s = -mapCoord.x - mapCoord.y;
        host = new GameObject("(" + mapCoord.x + ", " + mapCoord.y + ", " + s + ")");
        host.transform.position = Vector3.zero;

        mesh = new HexagonMesh(settings, host, MapSettings.MeshType.Obs, mapCoord, ZLAYER);
        mesh.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        mesh.setTargetLocation(mapCoord);

        host.transform.SetParent(parent);
    }

    public void Destroy() {
        GameObject.Destroy(host);
    }

    public Point getCoord() {
        return mapCoord;
    }

    public void updateObs(Point mapCoord) {
        this.mapCoord = mapCoord;
        mesh.active(isActive);
        mesh.setTargetLocation(mapCoord);
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