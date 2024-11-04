using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : TileTypes
{
    // Start is called before the first frame update
    public Stone() {
        //bIsAlive = true;
        bIsSafeToWalk = false;
        walkSpeed = 0f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        if(waterNeighs >= 3) {
            return new Water();
        } else if(spikesNeighs >= 1) {
            return new Spikes();
        } else {
            return this; 
        }
    }
}
