using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class NewLevelGeneration : MonoBehaviour {
    public string[] gridStateDebug;
    public TileTypes[,] debugGridState;
    public TileTypes[,] theGrid;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile;
    public bool theGridIsFull = true;
    public Tile grassTile;
    public Tile mudTile;
    public Tile spikesTile;
    public Tile stoneTile;
    public Tile waterTile;
    public Tile finishTile;
    public bool[,] tileEmpty;
    public int rows = 146;
    public int columns = 146;
    public int initialRowPos = 0;
    public int initialColumnPos = 0;
    private bool simulationIsRunning;
    public Tile deadTile;
    private bool isGenerationInProgress = false;
    private int generatedTileCount = 0;
    private bool finishTilePlaced = false;
    public GameObject grassObj;
    public GameObject spikesObj;
    public GameObject stoneObj;
    public GameObject waterObj;
    public GameObject mudObj;
    public GameObject deadObj;
    public GameObject Playeeeeer;
    public GameObject finishTileObj;
    public TileTypes[,] logicGrid;
    bool isGridUpdated;
    public TileBase finalTile;
    public Vector2Int playerStartPosition { get; private set; }
    public Vector2Int finishTilePosition { get; private set; }
    public void initializeGrid() {
        initializeLevelGeneration();
    }

    public void applyRulesAndVisualize() {
        proceduralGenerationRules();
        applyLogicToVisualGrid();
        Debug.Log("reglas aplicadas y grid visual actualizado");
    }

    private void initializeLevelGeneration() {
        theGrid = new TileTypes[rows, columns];
        logicGrid = new TileTypes[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = randomTile();
                logicGrid[i, c] = theGrid[i, c];
            }
        }
        updateVisualGrid();
    }

    private TileTypes randomTile() {
        float randomTileF = Random.value;
        if (randomTileF < 0.25f) {
            return new Grass();
        } else if (randomTileF < 0.50f) {
            return new Mud();
        } else if (randomTileF < 0.75f) {
            return new Water();
        } else if (randomTileF < 0.85f) {
            return new Spikes();
        } else {
            return new Stone();
        }
    }

    private void copyVisualToLogicGrid() {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                logicGrid[i, c] = theGrid[i, c];
            }
        }
        isGridUpdated = false;
    }
    public void proceduralGenerationRules() {
        TileTypes[,] newLogicGrid = new TileTypes[rows, columns];
        for(int xrows = 0; xrows < rows; xrows++) {
            for(int ycolumns = 0; ycolumns < columns; ycolumns++) {            
                int grassNeighs = 0, mudNeighs = 0, waterNeighs = 0, stoneNeighs = 0, spikesNeighs = 0, deadNeighs = 0;
                for (int neighsX = -1; neighsX <= 1; neighsX++) {
                    for (int neighsY = -1; neighsY <= 1; neighsY++) {
                        if (neighsX == 0 && neighsY == 0) {
                            continue;
                        } 
                        int x = xrows + neighsX;
                        int y = ycolumns + neighsY;
                        if (x >= 0 && x < rows && y >= 0 && y < columns) {
                            TileTypes neighborTile = logicGrid[x, y];
                            if (neighborTile is Grass) {
                                grassNeighs++;
                            }                             
                            else if (neighborTile is Mud) {
                                mudNeighs++;
                            }                              
                            else if (neighborTile is Water) {
                                waterNeighs++;
                            }                               
                            else if (neighborTile is Stone) {
                                stoneNeighs++;
                            }                               
                            else if (neighborTile is Spikes) {
                                spikesNeighs++;
                            }                               
                            else if (neighborTile is DeadTile) {
                                deadNeighs++;
                            }
                        }
                    }
                }
                TileTypes currentTile = logicGrid[xrows, ycolumns];
                newLogicGrid[xrows, ycolumns] = TilesRUles.ApplyRules(currentTile, grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);
            }
        }
        logicGrid = newLogicGrid;
        isGridUpdated = true;
        applyLogicToVisualGrid();  
    }

    private void applyLogicToVisualGrid() {
        if (!isGridUpdated) return;
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = logicGrid[i, c];
            }
        }
        updateVisualGrid();
        isGridUpdated = false;
    }

    private void updateVisualGrid() {
        HashSet<Vector3Int> instantiatedPositions = new HashSet<Vector3Int>();

        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c, 0);
                TileTypes tile = logicGrid[i, c]; 
                if (instantiatedPositions.Contains(currentGridPos)) {
                    continue;
                }
                if (tile is Grass) {
                    tilemap.SetTile(currentGridPos, grassTile);
                    //instantiateEmptyObj(grassObj, currentGridPos);
                } else if (tile is Mud) {
                    tilemap.SetTile(currentGridPos, mudTile);
                    //instantiateEmptyObj(mudObj, currentGridPos);
                } else if (tile is Water) {
                    tilemap.SetTile(currentGridPos, waterTile);
                    //instantiateEmptyObj(waterObj, currentGridPos);
                } else if (tile is Spikes) {
                    tilemap.SetTile(currentGridPos, spikesTile);
                    //instantiateEmptyObj(spikesObj, currentGridPos);
                } else if (tile is Stone) {
                    tilemap.SetTile(currentGridPos, stoneTile);
                    //instantiateEmptyObj(stoneObj, currentGridPos);
                } else if (tile is FinishTile) {
                    tilemap.SetTile(currentGridPos, finishTile);
                    //instantiateEmptyObj(finishTileObj, currentGridPos);
                } else {
                    tilemap.SetTile(currentGridPos, deadTile);
                }

                instantiatedPositions.Add(currentGridPos);
            }
        }
    }

    void instantiateEmptyObj(GameObject gameObj, Vector3Int thegridPos) {
        Vector3 worldPosition = tilemap.CellToWorld(thegridPos);
        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition);
        if (colliders.Length == 0) {
            GameObject instance = Instantiate(gameObj, worldPosition, Quaternion.identity);
           // Debug.Log($"Instanciado {gameObj.name} en {worldPosition}");
        } else {
            //Debug.LogWarning($"El objeto {gameObj.name} ya existe en {worldPosition}. No se instancia duplicados.");
        }
    }

    public void isRunniiiiing() {
        simulationIsRunning = !simulationIsRunning;
        if(simulationIsRunning) {
            Debug.Log("Simulación iniciada");
        } else {
            Debug.Log("Simulación detenida");
        }
    }
    public void placeFinalTileAndSetPlayerStart() {
        Vector2Int finalTilePos = getRandomSafePosition();
        Vector2Int playerStartPos = getRandomSafePosition();
        theGrid[finalTilePos.x, finalTilePos.y] = new FinishTile();
        finishTilePosition = finalTilePos;
        playerStartPosition = playerStartPos;
        Vector3Int tilemapPosition = new Vector3Int(finalTilePos.x, finalTilePos.y, 0);
        Tilemap tilemap = FindObjectOfType<Tilemap>();
        tilemap.SetTile(tilemapPosition, finalTile);
        GameObject player = Instantiate(Playeeeeer);
        player.transform.position = tilemap.CellToWorld(new Vector3Int(playerStartPos.x, playerStartPos.y, 0)) + new Vector3(0.5f, 0.5f, 0);
    }
    public Vector2Int getRandomSafePosition() {
        Vector2Int randomPos;
        do {
            randomPos = new Vector2Int(Random.Range(0, theGrid.GetLength(0)), Random.Range(0, theGrid.GetLength(1)));
        } while (theGrid[randomPos.x, randomPos.y] is Spikes || theGrid[randomPos.x, randomPos.y] is Stone);
        return randomPos;
    }
}
