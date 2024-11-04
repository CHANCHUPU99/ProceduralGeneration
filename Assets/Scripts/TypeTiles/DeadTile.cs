using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTile : TileTypes
{
    public DeadTile() {
        //bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 0.8f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        if (grassNeighs > 0) {
            return new Grass();
        } else if (mudNeighs > 0) {
            return new Mud();
        } else if (waterNeighs > 0) {
            return new Water();
        } else if (stoneNeighs > 0) {
            return new Stone();
        } else if (spikesNeighs > 0) {
            return new Spikes();
        } else {
            return this;
        }
    }
}
