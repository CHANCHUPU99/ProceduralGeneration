using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : TileTypes {
    public Grass() {
        //bIsAlive = true;
        bIsSafeToWalk = true;
        walkSpeed = 1.0f;
        weight = 1;
    }
    public Grass(Vector2Int _pos) {
        pos = _pos;
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

}

