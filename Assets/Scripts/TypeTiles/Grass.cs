using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : TileTypes
{
    // Start is called before the first frame update
    void Start()
    {
        bIsAlive = true;        
        bIsSafeToWalk = true;    
        walkSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
