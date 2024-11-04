using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : TileTypes
{
    public Spikes() {
        //bIsAlive = true;
        bIsSafeToWalk = false;
        walkSpeed = 0.5f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        if (grassNeighs >= 3) {
            return new Grass();
        } else if (stoneNeighs >= 2) {
            return new Stone();
        } else {
            return this; 
        }
    }
}
