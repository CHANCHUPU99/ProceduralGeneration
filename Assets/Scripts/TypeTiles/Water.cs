using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : TileTypes
{
    // Start is called before the first frame update
    public Water() {
        //bIsAlive = true;
        bIsSafeToWalk = false;
        walkSpeed = 0.0f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs) {
        if(grassNeighs >= 3) {
            return new Grass();
        } else if(stoneNeighs >= 2) {
            return new Stone();
        } else {
            return this; 
        }
    }
}
