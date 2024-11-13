using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class PathFIndingAStar : MonoBehaviour
{
    private TileTypes[,] logicGrid;
    [SerializeField]private int rows = 10;
    [SerializeField]private int columns = 10;
    public Tilemap tilemap;
    public Tile grassTile, mudTile, waterTile, spikesTile, stoneTile, finishTile, deadTile, pathTile;
    private Vector2Int playerPosition;
    private Vector2Int finishPosition;

    public void findBestPath() {
        NewLevelGeneration levelGeneration = FindObjectOfType<NewLevelGeneration>();
        logicGrid = levelGeneration.logicGrid;

        playerPosition = new Vector2Int(playerPosition.x, playerPosition.y);
        finishPosition = new Vector2Int(playerPosition.x, playerPosition.y);
        Vector2Int currentPos = playerPosition;
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(currentPos);
        while (currentPos != finishPosition) {
            List<Vector2Int> neighbors = getNeighbors(currentPos);
            Vector2Int nextPos = currentPos;
            float minDistance = 1000f;
            foreach (Vector2Int neighbor in neighbors) {
                if (logicGrid[neighbor.x, neighbor.y] is Spikes || logicGrid[neighbor.x, neighbor.y] is Stone) {
                    continue;
                }
                float distance = Vector2Int.Distance(neighbor, finishPosition);
                if (distance < minDistance) {
                    minDistance = distance;
                    nextPos = neighbor;
                }
            }
            if (nextPos == currentPos) {
                Debug.Log("no se encontró un camino a finishTile");
                return;
            }

            path.Add(nextPos);
            currentPos = nextPos;
        }
        foreach (Vector2Int pos in path) {
            logicGrid[pos.x, pos.y] = new PathTile();  
        }
        applyLogicToVisualGrid(path);
        Debug.Log("camino encontrado!!!!!!!!!");
    }

    private List<Vector2Int> getNeighbors(Vector2Int position) {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for(int rows = -1; rows <= 1; rows++) {
            for(int columns = -1; columns <= 1; columns++) {
                if (rows == 0 && columns == 0) continue;

                int neighX = position.x + rows;
                int neighY = position.y + columns;

                if (neighX >= 0 && neighX < rows && neighY >= 0 && neighY < columns) {
                    neighbors.Add(new Vector2Int(neighX, neighY));
                }
            }
        }
        return neighbors;
    }

    private void applyLogicToVisualGrid(List<Vector2Int> tilesFromListPath) {
        foreach(Vector2Int localTile in tilesFromListPath) {
            Vector3Int worldPos = new Vector3Int(localTile.x, localTile.y, 0);
            TileTypes tile = logicGrid[localTile.x, localTile.y];
            if(tile is PathTile) {
                tilemap.SetTile(worldPos, pathTile);
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
        } else {
            tilemap.SetTile(gridPos, deadTile);
        }
    }
}
