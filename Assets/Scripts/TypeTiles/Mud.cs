using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : TileTypes
{
    public Mud() {
        bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 0.8f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs) {
        if (mudNeighs >= 2) return new Mud();
        else if (grassNeighs >= 3) return new Grass();
        else if (spikesNeighs >= 1) return new Spikes();
        return this;
    }
}
