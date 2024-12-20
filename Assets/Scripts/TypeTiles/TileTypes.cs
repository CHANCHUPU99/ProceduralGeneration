using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypes 
{
    public bool bIsAlive;
    public bool bIsSafeToWalk;
    public float walkSpeed;
    public bool isEditable = true;
    public bool isChanging;
    public int weight;
    public TileTypes previousTile;
    public float cosst = float.MaxValue;
    public Vector2Int pos;
    //public tileTypes tileTypesEnum;
    public TileTypes() {
        bIsAlive = false;
        isEditable = true;
        isChanging = true;
        weight = 0;
    }
    public TileTypes(bool _isAlive, bool _isSafeToWalk, float _walkSpeed, int _weight, Vector2Int _pos) {
        bIsAlive = _isAlive;
        bIsSafeToWalk = _isSafeToWalk;
        walkSpeed = _walkSpeed;
        weight = _weight;
        isEditable = true; 
        isChanging = true;
        pos = _pos;
    }
    public TileTypes(Vector2Int _pos) {
        pos = _pos;
    }

    public virtual TileTypes neighsTypeCount(int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs,int deadNeighs) {
        return null;
    }
    public void setIsSafeToWalk(bool _isSafeToWalk) {
        bIsSafeToWalk = _isSafeToWalk;
    }

    public bool getIsSafeToWalk() {
        return bIsSafeToWalk;
    }

    public void setWalkSpeed(float _walkSpeed) {
        walkSpeed = _walkSpeed;
    }

    public float getWalkSpeed() {
        return walkSpeed;
    }

    //public void setTileType(tileTypes _tileTypesEnum) {
    //    tileTypesEnum = _tileTypesEnum;
    //}

    //public tileTypes GetTileType() {
    //    return tileTypesEnum;
    //}

}
