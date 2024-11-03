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

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs) {
        if(grassNeighs >= 3) {
            return new Grass();
        } else if(waterNeighs >= 2) {
            return new Water();
        } else {
            return this; 
        }
    }
}
