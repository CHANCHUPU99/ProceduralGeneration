using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : TileTypes
{
    // Start is called before the first frame update
    public Water() {
        bIsAlive = true;
        bIsSafeToWalk = false;
        walkSpeed = 0.0f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs) {
        if (waterNeighs >= 2) return new Water();
        else if (grassNeighs >= 1) return new Grass();
        else if (stoneNeighs >= 2) return new Stone();
        return this;
    }
}
