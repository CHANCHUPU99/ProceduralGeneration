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
        if(deadNeighs > 0) {
            return new Grass();
        } else if (waterNeighs >= 2) {
            return new Water();
        } else if (stoneNeighs >= 1) {
            return new Stone();
        } else {
            return this;
        }
    }

     /// <summary>
    /// si tiene mas de un tile muerto saca el random y decide aleatoriamente un nuevo tile
    /// </summary>
}

