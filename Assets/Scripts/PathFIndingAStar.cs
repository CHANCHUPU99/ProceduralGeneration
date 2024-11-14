using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFIndingAStar : MonoBehaviour
{
    private TileTypes[,] logicGrid;
    [SerializeField]private int rows = 10;
    [SerializeField]private int columns = 10;
    public Tilemap tilemap;
    public Tile grassTile, mudTile, waterTile, spikesTile, stoneTile, finishTile, deadTile, pathTile;
    private Vector2Int playerPosition;
    private Vector2Int finishPosition;
    [SerializeField]int maxIt = 1000;
    [SerializeField] int itCount = 0;
    public void findBestPath() {
        NewLevelGeneration levelGeneration = FindObjectOfType<NewLevelGeneration>();
        logicGrid = levelGeneration.logicGrid;
        playerPosition = levelGeneration.playerStartPosition;
        finishPosition = levelGeneration.finishTilePosition;
        itCount = 0;
        for (int x = 0; x < rows; x++) {
            for (int y = 0; y < columns; y++) {
                logicGrid[x, y].cosst = 1000f;
                logicGrid[x, y].previousTile = null;
                logicGrid[x,y].pos = new Vector2Int(x, y);
            }
        }
        //de aqui incia el path
        logicGrid[playerPosition.x, playerPosition.y].cosst = 0; 
        PriorityQueue<Vector2Int> list = new PriorityQueue<Vector2Int>();
        //para guardar los tiles
        HashSet<Vector2Int> newList = new HashSet<Vector2Int>();
        list.enqueue(playerPosition, 0);
        //mientras que haya elementos en mi priority y no suepere mis its
        while(list.Count > 0 && itCount < maxIt) {
            Vector2Int currentPos = list.dequeue();
            if (currentPos == finishPosition) {
                //aqui se econtro mi path
                buildPath(currentPos);
                Debug.Log("camino encontrado");
                return;
            }
            newList.Add(currentPos);
            //checo vecinos
            foreach (Vector2Int neighbor in getNeighbors(currentPos)) {
                //si el neigh ya esta en mi newlist que checa tiles(nodos) sigo entonces para no visitarlo otra vez
                if (newList.Contains(neighbor)) {
                    continue;
                } 
                //reviso si mi neightile no es spike y tile porque isSafeToWalk = false
                TileTypes neighborTile = logicGrid[neighbor.x, neighbor.y];
                if (neighborTile is Spikes || neighborTile is Stone) {
                    continue;
                } 
                //aqui nada mas calculo costo del current neigh en mi lista que tengo de mi funcion getNeighs sumando el cost del tile acutal + el peso del neigh
                float newCost = logicGrid[currentPos.x, currentPos.y].cosst + neighborTile.weight;
                if (newCost < neighborTile.cosst) {
                    //si entro aqui es para decirle que mi nodo actual ahora es el previous porque el nuevo tiene mejro costr
                    neighborTile.cosst = newCost;
                    neighborTile.previousTile = logicGrid[currentPos.x, currentPos.y];
                    //saco mi dist  entre  neigh y mi finishTile y asi le doy prioridad en la queue 
                    float manhattanDis = newCost + Mathf.Abs(neighbor.x - finishPosition.x) + Mathf.Abs(neighbor.y - finishPosition.y);
                    list.enqueue(neighbor, manhattanDis);
                }
            }
            //evito que entre en infinitp
            itCount++;
        }
        Debug.LogWarning("no hse encontro camino");
    }

    private List<Vector2Int> getNeighbors(Vector2Int position) {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = {new Vector2Int(0, 1),new Vector2Int(1, 0),new Vector2Int(0, -1),new Vector2Int(-1, 0)};
        foreach (var dir in directions) {
            //aqui saco pos del neigh en x y Y
            int neighX = position.x + dir.x;
            int neighY = position.y + dir.y;
            //solo checo que esten dentro de thegrid
            if(neighX >= 0 && neighX < rows && neighY >= 0 && neighY < columns) {
                neighbors.Add(new Vector2Int(neighX, neighY));
            }
        }
        return neighbors;
    }

    private void buildPath(Vector2Int _pos) {
        //en esta agrego los que seran parte del path
        List<Vector2Int> path = new List<Vector2Int>();
        //aqui puedo sacar los previous y su cost
        TileTypes currentTile = logicGrid[_pos.x, _pos.y];
        //hasta que no hayan mas previousTiles
        while (currentTile != null) {
            //voy agregando los tiles a la lista path 
            path.Add(currentTile.pos);
            currentTile = currentTile.previousTile;
        }
        //solamente cambio el orden porque estaba checando los previous 
        path.Reverse();
        foreach(Vector2Int pos in path) {
            //les digo a mis tiles de path que ahora son de tipo Pathtile
            logicGrid[pos.x, pos.y] = new PathTile(pos);
        }
        applyLogicToVisualGrid(path);
    }

    private void applyLogicToVisualGrid(List<Vector2Int> tilesFromListPath) {
        foreach(Vector2Int localTile in tilesFromListPath) {
            Vector3Int worldPos = new Vector3Int(localTile.x, localTile.y, 0);
            //saco el tipo de tile que hay en esa pos
            TileTypes tile = logicGrid[localTile.x, localTile.y];
            if(tile is PathTile) {
                tilemap.SetTile(worldPos, pathTile);
            } else {
                updateVisualGrid(worldPos, tile);
            }
        }
    }

    private void updateVisualGrid(Vector3Int gridPos, TileTypes tile) {
        if(tile is Grass) {
            tilemap.SetTile(gridPos, grassTile);
        } else if(tile is Mud) {
            tilemap.SetTile(gridPos, mudTile);
        } else if(tile is Water) {
            tilemap.SetTile(gridPos, waterTile);
        } else if(tile is Spikes) {
            tilemap.SetTile(gridPos, spikesTile);
        } else if(tile is Stone) {
            tilemap.SetTile(gridPos, stoneTile);
        } else if(tile is FinishTile) {
            tilemap.SetTile(gridPos, finishTile);
        } else if(tile is PathTile) {
            tilemap.SetTile(gridPos, pathTile);
            Debug.Log("se pintoooo un pathTile");
        } else {
            tilemap.SetTile(gridPos, deadTile);
        }
    }
}
