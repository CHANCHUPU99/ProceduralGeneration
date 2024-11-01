using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{
    public TileTypes[,] theGrid;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile;
    public int rows = 350;
    public int columns = 350;
    private bool simulationIsRunning;
    public Tile deadTile;


    private void Start() {
        theGrid = new TileTypes[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                //esto es test
                //theGrid[i, c] = new TileTypes(false,);
            }
        }

    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 coursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = userTilemap.WorldToCell(coursorPos);
            if (tilePos.x >= 0 && tilePos.x < rows && tilePos.y >= 0 && tilePos.y < columns) {
                updateGrid(tilePos);
            }
        }
        if (simulationIsRunning) {
            StartCoroutine(generationsInterval());
            //runGameOfLife();
        }
    }

    public void runGameOfLife() {
        TileTypes[,] tempGrid = new TileTypes[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                tempGrid[i, c] = new TileTypes();
            }
        }
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                int currentNeighs = checkNeighCells(i, c);
                if (theGrid[i, c].bIsAlive) {
                    if (currentNeighs < 2 || currentNeighs > 3) {
                        tempGrid[i, c].bIsAlive = false;
                    } else {
                        tempGrid[i, c].bIsAlive = true;
                    }
                } else {
                    if (currentNeighs == 3) {
                        tempGrid[i, c].bIsAlive = true;
                    } else {
                        tempGrid[i, c].bIsAlive = false;
                    }
                }
            }
        }
        theGrid = tempGrid;
        //printGridState();
        updateVisualGrid();
    }

    private void updateGrid(Vector3Int tilePos) {
        TileBase currentTile = tilemap.GetTile(tilePos);
        if (currentTile == null) {
            tilemap.SetTile(tilePos, drawedTile);
            userTilemap.SetTile(tilePos, drawedTile);
            theGrid[tilePos.x, tilePos.y].bIsAlive = true;
        } else {
            //tilemap.SetTile(tilePos, deadTile);
            //userTilemap.SetTile(tilePos, deadTile);
            //theGrid[tilePos.x, tilePos.y].bIsAlive = false;
            tilemap.SetTile(tilePos, null);
            userTilemap.SetTile(tilePos, null);
            theGrid[tilePos.x, tilePos.y].bIsAlive = false;
        }
    }

    //users grid
    private void updateVisualGrid() {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c);
                if (theGrid[i, c].bIsAlive) {
                    tilemap.SetTile(currentGridPos, drawedTile);
                    userTilemap.SetTile(currentGridPos, drawedTile);
                } else {
                    tilemap.SetTile(currentGridPos, null);
                    userTilemap.SetTile(currentGridPos, null);
                }
            }
        }
    }

    //Funtion to delete theGrid
    public void clearTheGrid() {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c);
                if (theGrid[i, c].bIsAlive) {
                    theGrid[i, c].bIsAlive = false;
                    tilemap.SetTile(currentGridPos, null);
                    userTilemap.SetTile(currentGridPos, null);
                } else {
                    theGrid[i, c].bIsAlive = false;
                    tilemap.SetTile(currentGridPos, null);
                    userTilemap.SetTile(currentGridPos, null);
                }
            }
        }
    }

    //claseeeeeeee
    int checkNeighCells(int x, int y) {
        int aliveNeighs = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }
                int checkX = x + i;
                int checkY = y + j;
                if (checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns) {
                    if (theGrid[checkX, checkY].bIsAlive) {
                        aliveNeighs++;
                    }

                }
            }
        }
        return aliveNeighs;
    }

    IEnumerator generationsInterval() {
        while (simulationIsRunning) {
            yield return new WaitForSeconds(0.3f);
            runGameOfLife();
            //updateVisualGrid();
        }
    }

    //buttons functions
    public void startGameOfLife() {
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
