using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTile : TileTypes
{
    public FinishTile() {
        bIsSafeToWalk = true;  // Puedes marcarlo como seguro para caminar
        // Aqu� puedes agregar m�s propiedades si necesitas que el FinalTile tenga alg�n efecto especial
    }
}
