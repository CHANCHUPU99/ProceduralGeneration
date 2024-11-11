using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesRUles 
{
    public static TileTypes ApplyRules(TileTypes tileActual, int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        switch (tileActual) {
            case Grass _:
                return grassRules(grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);

            case Mud _:
                return mudRules(grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);

            case Water _:
                return waterRules(grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);

            case Spikes _:
                return spikesRules(grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);

            case Stone _:
                return stoneRules(grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);

            default:
                return new DeadTile();
        }
    }

    private static TileTypes grassRules(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.LogWarning("reglas en grass");
        if (deadNeighs > 0) return new Grass();
        if (grassNeighs >= 2) return new Stone();
        if (mudNeighs >= 1) return new Water();
        if (spikesNeighs >= 1) return new Mud();
        return new Spikes();
    }

    private static TileTypes mudRules(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.LogWarning("reglas en mud");
        if (deadNeighs > 0) return new Water();
        if (mudNeighs >= 2) return new Spikes();
        if (grassNeighs >= 1) return new Stone();
        if (stoneNeighs >= 1) return new Grass();
        return new Water();
    }

    private static TileTypes waterRules(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.LogWarning("reglas en water");
        if (deadNeighs > 0) return new Water();
        if (waterNeighs >= 2) return new Mud();
        if (grassNeighs >= 1) return new Spikes();
        if (stoneNeighs >= 1) return new Mud();
        return new Grass();
    }

    private static TileTypes spikesRules(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.LogWarning("reglas en spikes");
        if (deadNeighs > 0) return new Spikes();
        if (spikesNeighs >= 2) return new Mud();
        if (grassNeighs >= 1) return new Water();
        if (stoneNeighs >= 1) return new Grass();
        return new Stone();
    }

    private static TileTypes stoneRules(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.LogWarning("reglas en stone");
        if (deadNeighs > 0) return new Stone();
        if (stoneNeighs >= 2) return new Grass();
        if (grassNeighs >= 1) return new Mud();
        if (waterNeighs >= 1) return new Spikes();
        return new Water();
    }
}
