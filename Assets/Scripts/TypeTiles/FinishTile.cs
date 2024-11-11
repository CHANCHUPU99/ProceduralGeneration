using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTile : TileTypes
{
    public FinishTile() {
        bIsSafeToWalk = true;  // Puedes marcarlo como seguro para caminar
        // Aquí puedes agregar más propiedades si necesitas que el FinalTile tenga algún efecto especial
    }
}
