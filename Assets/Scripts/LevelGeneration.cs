using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelGeneration : MonoBehaviour
{
    public TileTypes[,] theGrid;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile;

    public Tile grassTile;
    public Tile mudTile;
    public Tile spikesTile;
    public Tile stoneTile;
    public Tile waterTile;

    public int rows = 145;
    public int columns = 145;
    private bool simulationIsRunning;
    public Tile deadTile;

    //private void Start() {
    //    theGrid = new TileTypes[rows, columns];
    //    for(int i = 0; i < rows; i++) {
    //        for(int c = 0; c < columns; c++) {
    //            theGrid[i, c] = randomTile(); 
    //        }
    //    }
    //    updateVisualGrid(); 
    //}

    //para que inicie con el boton
    private void initializeLevelGeneration() {
        theGrid = new TileTypes[rows, columns];
        for(int i = 0;i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                theGrid[i, c] = randomTile();
            }
        }
        proceduralGenerationRules();
        updateVisualGrid();
    }


    private TileTypes randomTile() {
        float randomTileF = Random.value;
        if (randomTileF < 0.5f) {
            return new Grass();
        } else if (randomTileF < 0.7f) {
            return new Mud();
        } else if (randomTileF < 0.85f) {
            return new Water();
        } else if (randomTileF < 0.95f) {
            return new Spikes();
        } else {
            return new Stone();
        }
    }

    private void proceduralGenerationRules() {
        for(int i = 0;i < rows; ++i) {
            for(int c = 0; c < columns; c++) {
                TileTypes tile = theGrid[i, c];
                int grassNeighs = checkTypeNeighs(i, c, typeof(Grass));
                int waterNeighs = checkTypeNeighs(i, c, typeof(Water));
                int stoneNeighs = checkTypeNeighs(i, c, typeof(Stone));
                if (grassNeighs >= 3) {
                    theGrid[i, c] = new Grass();
                } else if (waterNeighs >= 2) {
                    theGrid[i, c] = new Water();
                } else if (stoneNeighs >= 1) {
                    theGrid[i, c] = new Stone();
                } else if (tile is Grass && Random.value < 0.2f) {
                    theGrid[i, c] = new Spikes();
                }
            }
        }
        updateVisualGrid(); 
    }

    private int checkTypeNeighs(int x, int y, System.Type tileType) {
        int typeCount = 0;

        for(int i = -1;i <= 1; i++) {
            for(int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue; 
                }
                int checkX = x + i;
                int checkY = y + j;
                if (checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns) {
                    if (theGrid[checkX, checkY].GetType() == tileType) {
                        typeCount++;
                    }
                }
            }
        }

        return typeCount;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 coursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = userTilemap.WorldToCell(coursorPos);
            if (tilePos.x >= 0 && tilePos.x < rows && tilePos.y >= 0 && tilePos.y < columns) {
                updateGrid(tilePos);
            }
        }
        if (simulationIsRunning) {
            StartCoroutine(generationsInterval());
        }
    }

    public void runGameOfLife() {
        TileTypes[,] tempGrid = new TileTypes[rows, columns];
        for(int i = 0; i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                tempGrid[i, c] = new TileTypes();
            }
        }
        for(int i = 0; i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                int currentNeighs = checkNeighCells(i, c);
                if(theGrid[i, c].bIsAlive) {
                    tempGrid[i, c].bIsAlive = currentNeighs >= 2 && currentNeighs <= 3;
                } else {
                    tempGrid[i, c].bIsAlive = currentNeighs == 3;
                }
            }
        }

        theGrid = tempGrid;
        updateVisualGrid();
    }

    private void updateGrid(Vector3Int tilePos) {
        TileBase currentTile = tilemap.GetTile(tilePos);
        if (currentTile == null) {
            tilemap.SetTile(tilePos, drawedTile);
            userTilemap.SetTile(tilePos, drawedTile);
            theGrid[tilePos.x, tilePos.y].bIsAlive = true;
        } else {
            tilemap.SetTile(tilePos, null);
            userTilemap.SetTile(tilePos, null);
            theGrid[tilePos.x, tilePos.y].bIsAlive = false;
        }
    }

    private void updateVisualGrid() {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c, 0);
                TileTypes tile = theGrid[i, c];
                if (tile is Grass) {
                    tilemap.SetTile(currentGridPos, grassTile);
                } else if (tile is Mud) {
                    tilemap.SetTile(currentGridPos, mudTile);
                } else if (tile is Water) {
                    tilemap.SetTile(currentGridPos, waterTile);
                } else if (tile is Spikes) {
                    tilemap.SetTile(currentGridPos, spikesTile);
                } else if (tile is Stone) {
                    tilemap.SetTile(currentGridPos, stoneTile);
                }
            }
        }
    }

    public void clearTheGrid() {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c, 0);
                theGrid[i, c].bIsAlive = false;
                tilemap.SetTile(currentGridPos, null);
                userTilemap.SetTile(currentGridPos, null);
            }
        }
    }

    int checkNeighCells(int x, int y) {
        int aliveNeighs = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) continue;
                int checkX = x + i;
                int checkY = y + j;
                if (checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns) {
                    if (theGrid[checkX, checkY].bIsAlive) aliveNeighs++;
                }
            }
        }
        return aliveNeighs;
    }

    IEnumerator generationsInterval() {
        while (simulationIsRunning) {
            yield return new WaitForSeconds(0.3f);
            runGameOfLife();
        }
    }

    public void startGameOfLife() {
        initializeLevelGeneration();
        simulationIsRunning = true;
        StartCoroutine(generationsInterval());
    }

    public void stopGameOfLife() {
        simulationIsRunning = false;
    }

    //function to print 
    void printGridState() {
        string gridState = "";
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                gridState += theGrid[i, c].bIsAlive ? "1 " : "0 ";
            }
            gridState += "\n";
        }
        Debug.Log($"Estado actual de theGrid:\n{gridState}");
    }

    //void threeFirstTiles() {
    //    int randomTile = 3;
    //    int randomRow;
    //    int randomCol;
    //    TileTypes.tileTypes[] tileTypeeeessss = {
    //    TileTypes.tileTypes.Grass,
    //    TileTypes.tileTypes.Mud,
    //    TileTypes.tileTypes.Spikes,
    //    TileTypes.tileTypes.Stone,
    //    TileTypes.tileTypes.Water
    //};

    //    for (int i= 0; i < randomTile; i++) {
    //        randomRow = Random.Range(0, rows);
    //        randomCol = Random.Range(0, columns);
    //        TileTypes.tileTypes randomTileType = tileTypeeeessss[Random.Range(0,tileTypeeeessss.Length)];
    //        theGrid[randomRow, randomCol] = newRandomTile;
    //    }
    //}
}
