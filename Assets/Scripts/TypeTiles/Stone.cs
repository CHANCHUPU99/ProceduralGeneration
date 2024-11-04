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
        if(deadNeighs > 0) {
            return new Stone();
        } else if (stoneNeighs >= 2) {
            return new Grass();
        } else if (grassNeighs >= 1) {
            return new Mud();
        } else if (waterNeighs >= 1) {
            return new Spikes();
        } else {
            return new Water();
        }
    }

    /// <summary>
    /// si mas de cero muertos genera stone
    /// si tiene dos o mas de stone genera grass
    /// si tiene uno o mas de grass genera mud
    /// si tiene uno o mas de water genera spikes
    /// si no se cumple ninguna genera water
    /// </summary>
}
