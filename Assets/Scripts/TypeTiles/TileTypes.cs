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
    //public tileTypes tileTypesEnum;
    public TileTypes() {
        bIsAlive = false;
        isEditable = true;
        isChanging = true;
    }
    public TileTypes(bool _isAlive, bool _isSafeToWalk, float _walkSpeed) {
        bIsAlive = _isAlive;
        bIsSafeToWalk = _isSafeToWalk;
        walkSpeed = _walkSpeed;
        isEditable = true; 
        isChanging = true;
    }

    public virtual void tileValues() {

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
