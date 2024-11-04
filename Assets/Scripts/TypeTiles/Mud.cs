using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : TileTypes
{
    public Mud() {
        //bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 0.8f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        if (deadNeighs > 0) {
            return new Mud();
        } else if (grassNeighs >= 3) {
            return new Grass();
        } else if (stoneNeighs >= 2) {
            return new Stone();
        } else {
            return this;
        }
    }
}
