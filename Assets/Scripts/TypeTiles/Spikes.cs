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
        Debug.Log($"Spikes: deadNeighs={deadNeighs}, grassNeighs={grassNeighs}, mudNeighs={mudNeighs}, waterNeighs={waterNeighs}, stoneNeighs={stoneNeighs}, spikesNeighs={spikesNeighs}");
        if (deadNeighs > 0) {
            return new Spikes();
        } else if (spikesNeighs >= 2) {
            return new Mud();
        } else if (grassNeighs >= 1) {
            return new Water();
        } else if (stoneNeighs >= 1) {
            return new Grass();
        } else {
            return new Stone();

        }
    }
        /// <summary>
        /// si tiene mas de un tile muerto saca un spike
        /// si tiene mas dos o mas de spikes genera mud
        /// si tiene uno o mas de grass genera water
        /// si tiene uno o mas de stone grass
        /// si no se cumple ninguna genera Stone
        /// </summary>
}
