using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : TileTypes {
    // Start is called before the first frame update
    public Grass() {
        //bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 1.0f;
    }

    public override TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        if (deadNeighs > 0) {
            return new Grass();
        } else if (grassNeighs >= 2) {
            return new Stone();
        } else if (mudNeighs >= 1) {
            return new Water();
        } else if (spikesNeighs >= 1) {
            return new Mud();
        } else {
            return new Spikes();
        }
    }

    /// <summary>
    /// si tiene mas de un tile muerto saca un grass
    /// si tiene mas dos o mas de grass genera stone
    /// si tiene uno o mas de mud genera water
    /// si tiene uno o mas de spikes genera mud
    /// si no se cumple ninguna genera spike
    /// </summary>
}

