using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypes : MonoBehaviour
{
    public bool bIsAlive;
    public bool bIsSafeToWalk;
    public float walkSpeed;
    //public tileTypes tileTypesEnum;
    public TileTypes() {
        bIsAlive = false;
    }
    public TileTypes(bool _isAlive, bool _isSafeToWalk, float _walkSpeed) {
        bIsAlive = _isAlive;
        bIsSafeToWalk = _isSafeToWalk;
        walkSpeed = _walkSpeed;
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
