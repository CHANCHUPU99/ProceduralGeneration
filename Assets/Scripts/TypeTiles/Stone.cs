using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : TileTypes
{
    // Start is called before the first frame update
    public Stone() {
        bIsAlive = true;
        bIsSafeToWalk = false;
        walkSpeed = 0f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs) {
        if (stoneNeighs >= 2) return new Stone();
        else if (mudNeighs >= 1) return new Mud();
        else if (waterNeighs >= 2) return new Water();
        return this;
    }
}
