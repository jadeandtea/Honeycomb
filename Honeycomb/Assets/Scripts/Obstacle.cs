using System;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle
{
    public enum obstacleType{
        Pushable, Obstacle
    }
    MapSettings settings;
    const float ZLAYER = -0.1f;

    GameObject host;

    HexagonMesh mesh;

    Point mapCoord;

    public bool isActive;

    public Obstacle(MapSettings settings, Point mapCoord, Transform parent, obstacleType type, bool editMode = false) {
        this.settings = settings;
        this.mapCoord = mapCoord;

        this.isActive = !editMode;

        int s = -mapCoord.x - mapCoord.y;
        host = new GameObject("(" + mapCoord.x + ", " + mapCoord.y + ", " + s + ")");
        host.transform.position = Vector3.zero;

        mesh = new HexagonMesh(settings, host, MapSettings.MeshType.Obs, mapCoord, ZLAYER);

        if (type == obstacleType.Pushable) {
            mesh.setColor(settings.pushOuterColor, settings.pushCenterColor, settings.centerColorWeight);
        } else if (type == obstacleType.Obstacle) {
            mesh.setColor(settings.obsOuterColor, settings.obsCenterColor, settings.centerColorWeight);
        }

        mesh.recalculateMesh();

        mesh.setTargetLocation(mapCoord);

        host.transform.SetParent(parent);
    }

    public void setActiveLocation(Point mapCoord) {
        // Instantly places tile at mapCoord
        mesh.setActiveLocation(mapCoord);
    }

    public void Destroy() {
        GameObject.Destroy(host);
    }

    public void DestroyImmediate() {
        GameObject.DestroyImmediate(host);
    }

    public void updateObs(Point mapCoord) {
        this.mapCoord = mapCoord;
        mesh.active(isActive);
        mesh.updateHex();
        mesh.setTargetLocation(mapCoord);
    }

    public void updateMesh() {
        mesh.recalculateMesh();
    }

    public void setColor(Color outer, Color center, float t){
        mesh.setColor(outer, center, t);
    }
}