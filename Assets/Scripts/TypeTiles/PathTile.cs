using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : TileTypes
{
    public PathTile() {
        //bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 1.0f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.Log($"Grass: deadNeighs={deadNeighs}, grassNeighs={grassNeighs}, mudNeighs={mudNeighs}, waterNeighs={waterNeighs}, stoneNeighs={stoneNeighs}, spikesNeighs={spikesNeighs}");
        if (deadNeighs > 0) {
            return new Grass();
        } else if (grassNeighs >= 2) {
            return new Stone();
        } else if (mudNeighs >= 1) {
            return new Water();
        } else if (spikesNeighs >= 1) {
            return new Mud();
        } else {
            return new Spikes();
        }
    }
}
