using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : TileTypes
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
