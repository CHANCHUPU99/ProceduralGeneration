using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : TileTypes
{
    // Start is called before the first frame update
    void Start()
    {
        bIsAlive = true;
        bIsSafeToWalk = false;
        walkSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
