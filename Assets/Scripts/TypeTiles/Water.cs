using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : TileTypes
{
    // Start is called before the first frame update
    public Water() {
        //bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 0.0f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.Log($"Water: deadNeighs={deadNeighs}, grassNeighs={grassNeighs}, mudNeighs={mudNeighs}, waterNeighs={waterNeighs}, stoneNeighs={stoneNeighs}, spikesNeighs={spikesNeighs}");
        if (deadNeighs > 0) {
            return new Water();
        } else if (waterNeighs >= 2) {
            return new Mud();
        } else if (grassNeighs >= 1) {
            return new Spikes();
        } else if (stoneNeighs >= 1) {
            return new Mud();
        } else {
            return new Grass(); 
        }
    }

    /// <summary>
    /// si mas de cero muertos genera water
    /// si tiene dos o mas de water genera mud
    /// si tiene uno o mas de grass genera spikes
    /// si tiene uno o mas de stone genera mud
    /// si no se cumple ninguna genera grass
    /// </summary>
}
