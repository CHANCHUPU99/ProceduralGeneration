using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : TileTypes
{
    public Mud() {
        //bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 0.8f;
        weight = 3;
    }
    public Mud(Vector2Int _pos) {
        pos = _pos;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        if (deadNeighs > 0) {
            return new Water();
        } else if (mudNeighs >= 2) {
            return new Spikes();
        } else if (grassNeighs >= 1) {
            return new Stone();
        } else if (stoneNeighs >= 1) {
            return new Grass();
        } else {
            return new Water(); 
        }
    }

}
        /// <summary>
        /// si tiene mas de cero muertos genera water
        /// si tiene dos o mas de mud genera spikes
        /// si tiene uno o mas de grass genera stone
        /// si tiene mas o unoi de stone genera grass
        /// si no se cumple ninguna genera water
        /// </summary>
