using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : TileTypes
{
    public Spikes() {
        bIsAlive = true;
        bIsSafeToWalk = false;
        walkSpeed = 0.5f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs) {
        if (spikesNeighs >= 2) return new Spikes();
        else if (stoneNeighs >= 1) return new Stone();
        else if (grassNeighs >= 2) return new Grass();
        return this;
    }
}
