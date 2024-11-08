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

    private void Update() {
        if (simulationIsRunning && !isGenerationInProgress) {
            if (!isGridUpdated) {
                copyVisualToLogicGrid();
            } else {
                proceduralGenerationRules();
                applyLogicToVisualGrid();
            }
        }
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




    private void proceduralGenerationRules() {
        TileTypes[,] tempGrid = new TileTypes[rows, columns];

        for (int i = 0; i < rows; ++i) {
            for (int c = 0; c < columns; ++c) {
                TileTypes tileActual = logicGrid[i, c];
                if (tileActual != null && (!tileActual.isEditable || tileActual is FinishTile)) {
                    tempGrid[i, c] = tileActual;
                    continue;
                }

                int grassNeighs = checkTypeNeighs(i, c, typeof(Grass));
                int mudNeighs = checkTypeNeighs(i, c, typeof(Mud));
                int waterNeighs = checkTypeNeighs(i, c, typeof(Water));
                int stoneNeighs = checkTypeNeighs(i, c, typeof(Stone));
                int spikesNeighs = checkTypeNeighs(i, c, typeof(Spikes));
                int deadNeighs = checkTypeNeighs(i, c, typeof(DeadTile));

                int tileIndex = GetTileTypeIndex(tileActual, grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);
                Debug.Log($"Posición ({i}, {c}) tiene índice de tile {tileIndex}");

                switch (tileIndex) {
                    case 0:
                        tempGrid[i, c] = new Grass();
                        break;
                    case 1:
                        tempGrid[i, c] = new Stone();
                        break;
                    case 2:
                        tempGrid[i, c] = new Water();
                        break;
                    case 3:
                        tempGrid[i, c] = new Mud();
                        break;
                    case 4:
                        tempGrid[i, c] = new Spikes();
                        break;
                    case 5:
                        tempGrid[i, c] = new Water();
                        break;
                    case 6:
                        tempGrid[i, c] = new Spikes();
                        break;
                    case 7:
                        tempGrid[i, c] = new Stone();
                        break;
                    case 8:
                        tempGrid[i, c] = new Grass();
                        break;
                    case 9:
                        tempGrid[i, c] = new Water();
                        break;
                    case 10:
                        tempGrid[i, c] = new Water();
                        break;
                    case 11:
                        tempGrid[i, c] = new Mud();
                        break;
                    case 12:
                        tempGrid[i, c] = new Spikes();
                        break;
                    case 13:
                        tempGrid[i, c] = new Mud();
                        break;
                    case 14:
                        tempGrid[i, c] = new Grass();
                        break;
                    case 15:
                        tempGrid[i, c] = new Spikes();
                        break;
                    case 16:
                        tempGrid[i, c] = new Mud();
                        break;
                    case 17:
                        tempGrid[i, c] = new Water();
                        break;
                    case 18:
                        tempGrid[i, c] = new Grass();
                        break;
                    case 19:
                        tempGrid[i, c] = new Stone();
                        break;
                    case 20:
                        tempGrid[i, c] = new Stone();
                        break;
                    case 21:
                        tempGrid[i, c] = new Grass();
                        break;
                    case 22:
                        tempGrid[i, c] = new Mud();
                        break;
                    case 23:
                        tempGrid[i, c] = new Spikes();
                        break;
                    case 24:
                        tempGrid[i, c] = new Water();
                        break;
                    default:
                        tempGrid[i, c] = new DeadTile();
                        break;
                }

                if (tempGrid[i, c] != null && !(tempGrid[i, c] is DeadTile)) {
                    tempGrid[i, c].isEditable = false;
                }
            }
        }

        logicGrid = tempGrid;
        isGridUpdated = true;
        Debug.Log("Reglas aplicadas y grid lógico actualizado");
    }

    private int GetTileTypeIndex(TileTypes tileActual, int grassNeighs, int mudNeighs, int waterNeighs, int stoneNeighs, int spikesNeighs, int deadNeighs) {
        switch (tileActual) {
            case Grass :
                if (deadNeighs > 0) {

                    Debug.LogWarning("case 0");
                    return 0;  // Grass
                }
                else if (grassNeighs >= 2) 
                    return 1;  // Stone
                else if (mudNeighs >= 1)
                    return 2;  // Water
                else if (spikesNeighs >= 1)
                    return 3;  // Mud
                else
                    return 4;  // Spikes

            case Mud :
                if (deadNeighs > 0)
                    return 5;  // Water
                else if (mudNeighs >= 2)
                    return 6;  // Spikes
                else if (grassNeighs >= 1)
                    return 7;  // Stone
                else if (stoneNeighs >= 1)
                    return 8;  // Grass
                else
                    return 9;  // Water

            case Water :
                if (deadNeighs > 0)
                    return 10;  // Water
                else if (waterNeighs >= 2)
                    return 11;  // Mud
                else if (grassNeighs >= 1)
                    return 12;  // Spikes
                else if (stoneNeighs >= 1)
                    return 13;  // Mud
                else
                    return 14;  // Grass

            case Spikes :
                if (deadNeighs > 0)
                    return 15;  // Spikes
                else if (spikesNeighs >= 2)
                    return 16;  // Mud
                else if (grassNeighs >= 1)
                    return 17;  // Water
                else if (stoneNeighs >= 1)
                    return 18;  // Grass
                else
                    return 19;  // Stone

            case Stone :
                if (deadNeighs > 0)
                    return 20;  // Stone
                else if (stoneNeighs >= 2)
                    return 21;  // Grass
                else if (grassNeighs >= 1)
                    return 22;  // Mud
                else if (waterNeighs >= 1)
                    return 23;  // Spikes
                else
                    return 24;  // Water

            default:
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

    public void ToggleSimulation() {
        if (!simulationIsRunning) {
            initializeLevelGeneration();
        }
        simulationIsRunning = !simulationIsRunning;
        if (simulationIsRunning) {
            copyVisualToLogicGrid();
        } else {
            Debug.Log("Simulación detenida.");
        }
    }

}
