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

    public void initializeGrid() {
        initializeLevelGeneration();
        //Debug.Log("Grid aleatorio inicializado");
    }

    public void applyRulesAndVisualize() {
        proceduralGenerationRules();
        applyLogicToVisualGrid();
        Debug.Log("Reglas aplicadas y grid visual actualizado");
    }

    private void Update() {
        //if (simulationIsRunning && !isGenerationInProgress) {
        //    if (!isGridUpdated) {
        //        copyVisualToLogicGrid();
        //    } else {
        //        proceduralGenerationRules();
        //        applyLogicToVisualGrid();
        //    }
        //}
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
    private void printGrid(TileTypes[,] grid) {
        for (int i = 0; i < rows; i++) {
            string row = "";
            for (int c = 0; c < columns; c++) {
                row += grid[i, c]?.GetType().Name[0] ?? 'D';
            }
            Debug.Log(row);
        }
    }

    public void proceduralGenerationRules() {
        TileTypes[,] newLogicGrid = new TileTypes[rows, columns];
        for(int x = 0; x < rows; x++) {
            for(int y = 0; y < columns; y++) {            
                int grassNeighs = 0, mudNeighs = 0, waterNeighs = 0, stoneNeighs = 0, spikesNeighs = 0, deadNeighs = 0;
                for (int dx = -1; dx <= 1; dx++) {
                    for (int dy = -1; dy <= 1; dy++) {
                        if (dx == 0 && dy == 0) continue;
                        int nx = x + dx;
                        int ny = y + dy;
                        if (nx >= 0 && nx < rows && ny >= 0 && ny < columns) {
                            TileTypes neighborTile = logicGrid[nx, ny];
                            if (neighborTile is Grass)
                                grassNeighs++;
                            else if (neighborTile is Mud)
                                mudNeighs++;
                            else if (neighborTile is Water)
                                waterNeighs++;
                            else if (neighborTile is Stone)
                                stoneNeighs++;
                            else if (neighborTile is Spikes)
                                spikesNeighs++;
                            else if (neighborTile is DeadTile)
                                deadNeighs++;
                        }
                    }
                }
                TileTypes currentTile = logicGrid[x, y];
                newLogicGrid[x, y] = TilesRUles.ApplyRules(currentTile, grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);
            }
        }
        logicGrid = newLogicGrid;
        isGridUpdated = true;
        applyLogicToVisualGrid();  
    }



    //private void proceduralGenerationRules() {
    //    Debug.Log("estado del grid antes de las reglas: ");
    //    printGrid(logicGrid);
    //    TileTypes[,] tempGrid = new TileTypes[rows, columns];
    //    for(int i = 0; i < rows; ++i) {
    //        for(int c = 0; c < columns; ++c) {
    //            TileTypes tileActual = logicGrid[i, c];
    //            if (tileActual != null && (!tileActual.isEditable || tileActual is FinishTile)) {
    //                tempGrid[i, c] = tileActual;
    //                continue;
    //            }

    //            int grassNeighs = checkTypeNeighs(i, c, typeof(Grass));
    //            int mudNeighs = checkTypeNeighs(i, c, typeof(Mud));
    //            int waterNeighs = checkTypeNeighs(i, c, typeof(Water));
    //            int stoneNeighs = checkTypeNeighs(i, c, typeof(Stone));
    //            int spikesNeighs = checkTypeNeighs(i, c, typeof(Spikes));
    //            int deadNeighs = checkTypeNeighs(i, c, typeof(DeadTile));

    //            //int tileIndex = GetTileTypeIndex(tileActual, grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);
    //            //Debug.Log($"Posición ({i}, {c}) tiene índice de tile {tileIndex}");

    //            TileTypes nuevoTipoTile = TilesRUles.ApplyRules(tileActual, grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);
    //            Debug.Log($"Posición ({i}, {c}) cambiada de {tileActual.GetType().Name} a {nuevoTipoTile.GetType().Name}");
    //            tempGrid[i, c] = nuevoTipoTile;
    //            if (tempGrid[i, c] != null && !(tempGrid[i, c] is DeadTile)) {
    //                tempGrid[i, c].isEditable = false;
    //            }
    //        }
    //    }

    //    logicGrid = tempGrid;
    //    isGridUpdated = true;
    //    Debug.Log("Reglas aplicadas y grid lógico actualizado");
    //    printGrid(logicGrid);
    //}

    private int GetTileTypeIndex(TileTypes tileActual, int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        Debug.Log($"Evaluando tile en posición con vecinos - Grass: {grassNeighs}, Mud: {mudNeighs}, Water: {waterNeighs}, Stone: {stoneNeighs}, Spikes: {spikesNeighs}, Dead: {deadNeighs}");
        switch (tileActual) {
            case Grass _:
                if (deadNeighs > 0) {
                    Debug.LogWarning("case 0: Grass (por deadNeighs > 0)");
                    return 0;  // Grass
                } else if (grassNeighs >= 2) {
                    Debug.LogWarning("case 1: Stone (por grassNeighs >= 2)");
                    return 1;  // Stone
                } else if (mudNeighs >= 1) {
                    Debug.LogWarning("case 2: Water (por mudNeighs >= 1)");
                    return 2;  // Water
                } else if (spikesNeighs >= 1) {
                    Debug.LogWarning("case 3: Mud (por spikesNeighs >= 1)");
                    return 3;  // Mud
                } else {
                    Debug.LogWarning("case 4: Spikes (por defecto)");
                    return 4;  // Spikes
                }

            case Mud _:
                if (deadNeighs > 0) {
                    Debug.LogWarning("case 5: Water (por deadNeighs > 0)");
                    return 5;  // Water
                } else if (mudNeighs >= 2) {
                    Debug.LogWarning("case 6: Spikes (por mudNeighs >= 2)");
                    return 6;  // Spikes
                } else if (grassNeighs >= 1) {
                    Debug.LogWarning("case 7: Stone (por grassNeighs >= 1)");
                    return 7;  // Stone
                } else if (stoneNeighs >= 1) {
                    Debug.LogWarning("case 8: Grass (por stoneNeighs >= 1)");
                    return 8;  // Grass
                } else {
                    Debug.LogWarning("case 9: Water (por defecto)");
                    return 9;  // Water
                }

            case Water _:
                if (deadNeighs > 0) {
                    Debug.LogWarning("case 10: Water (por deadNeighs > 0)");
                    return 10;  // Water
                } else if (waterNeighs >= 2) {
                    Debug.LogWarning("case 11: Mud (por waterNeighs >= 2)");
                    return 11;  // Mud
                } else if (grassNeighs >= 1) {
                    Debug.LogWarning("case 12: Spikes (por grassNeighs >= 1)");
                    return 12;  // Spikes
                } else if (stoneNeighs >= 1) {
                    Debug.LogWarning("case 13: Mud (por stoneNeighs >= 1)");
                    return 13;  // Mud
                } else {
                    Debug.LogWarning("case 14: Grass (por defecto)");
                    return 14;  // Grass
                }

            case Spikes _:
                if (deadNeighs > 0) {
                    Debug.LogWarning("case 15: Spikes (por deadNeighs > 0)");
                    return 15;  // Spikes
                } else if (spikesNeighs >= 2) {
                    Debug.LogWarning("case 16: Mud (por spikesNeighs >= 2)");
                    return 16;  // Mud
                } else if (grassNeighs >= 1) {
                    Debug.LogWarning("case 17: Water (por grassNeighs >= 1)");
                    return 17;  // Water
                } else if (stoneNeighs >= 1) {
                    Debug.LogWarning("case 18: Grass (por stoneNeighs >= 1)");
                    return 18;  // Grass
                } else {
                    Debug.LogWarning("case 19: Stone (por defecto)");
                    return 19;  // Stone
                }

            case Stone _:
                if (deadNeighs > 0) {
                    Debug.LogWarning("case 20: Stone (por deadNeighs > 0)");
                    return 20;  // Stone
                } else if (stoneNeighs >= 2) {
                    Debug.LogWarning("case 21: Grass (por stoneNeighs >= 2)");
                    return 21;  // Grass
                } else if (grassNeighs >= 1) {
                    Debug.LogWarning("case 22: Mud (por grassNeighs >= 1)");
                    return 22;  // Mud
                } else if (waterNeighs >= 1) {
                    Debug.LogWarning("case 23: Spikes (por waterNeighs >= 1)");
                    return 23;  // Spikes
                } else {
                    Debug.LogWarning("case 24: Water (por defecto)");
                    return 24;  // Water
                }

            default:
                Debug.LogWarning("default case: DeadTile (por defecto)");
                return 25;  // DeadTile
        }
    }



    private int checkTypeNeighs(int x, int y, System.Type tileType) {
        int typeCount = 0;
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }
                int checkX = x + i;
                int checkY = y + j;
                if (checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns) {
                    //aca nada mas se lo paso a cada una de mis variabels locales ,osea el tipo 
                    if (theGrid[checkX, checkY] != null && theGrid[checkX, checkY].GetType() == tileType) {
                        typeCount++;
                    }
                }
            }
        }

        return typeCount;
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
                    instantiateEmptyObj(grassObj, currentGridPos);
                } else if (tile is Mud) {
                    tilemap.SetTile(currentGridPos, mudTile);
                    instantiateEmptyObj(mudObj, currentGridPos);
                } else if (tile is Water) {
                    tilemap.SetTile(currentGridPos, waterTile);
                    instantiateEmptyObj(waterObj, currentGridPos);
                } else if (tile is Spikes) {
                    tilemap.SetTile(currentGridPos, spikesTile);
                    instantiateEmptyObj(spikesObj, currentGridPos);
                } else if (tile is Stone) {
                    tilemap.SetTile(currentGridPos, stoneTile);
                    instantiateEmptyObj(stoneObj, currentGridPos);
                } else if (tile is FinishTile) {
                    tilemap.SetTile(currentGridPos, finishTile);
                    instantiateEmptyObj(finishTileObj, currentGridPos);
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
            Debug.Log($"Instanciado {gameObj.name} en {worldPosition}");
        } else {
            Debug.LogWarning($"El objeto {gameObj.name} ya existe en {worldPosition}. No se instancia duplicados.");
        }
    }

    public void toggleSimulation() {
        simulationIsRunning = !simulationIsRunning;
        if (simulationIsRunning) {
            Debug.Log("Simulación iniciada");
        } else {
            Debug.Log("Simulación detenida");
        }
    }


}
