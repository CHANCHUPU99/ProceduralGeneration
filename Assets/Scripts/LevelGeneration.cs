using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelGeneration : MonoBehaviour
{
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
    public int initialColumnPos=0;
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
    

    //ya que tengo mi thegrid generado aleatoriamente, copio ese grid aleatorio a uno temp y que ahi haga los cambios de logica 
    //y despues ese thegrid de logica o temp lo paso a mi funcion de update visual 
    private void initializeLevelGeneration() {
        theGrid = new TileTypes[rows, columns];
        debugGridState = new TileTypes[rows, columns];
        gridStateDebug = new string[rows];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                //theGrid[i, c] = new DeadTile();
                //debugGridState[i, c] = new DeadTile();
                theGrid[i, c] = randomTile();
                //llamar a mi funcion para copiar mi thegrid temp 
                //assignGameObjToTile(theGrid[i, c], i,c);
            }
        }
        theGrid[Random.Range(0, rows), Random.Range(0, columns)] = randomTile();
        //theGrid[Random.Range(0, rows ), Random.Range(0, columns)] = randomTile();
        //proceduralGenerationRules();
        //updateVisualGrid();
        //Vector2Int finishTilePos = createFinishTile(); 
        updateVisualGrid();
        //Debug.Log(PrintGridState());
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

    public Vector2Int createFinishTile() {
        int finishRow = Random.Range(0, rows);
        int finishColumn = Random.Range(0, columns);
        while(!(theGrid[finishRow, finishColumn] is DeadTile)) {
            finishRow = Random.Range(0, rows);
            finishColumn = Random.Range(0, columns);
        }
        theGrid[finishRow, finishColumn] = new FinishTile();
        return new Vector2Int(finishRow, finishColumn);
    }

     Vector2Int returnFinishTilePos() {
        for(int i = 0; i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                if (theGrid[i, c] is DeadTile) {
                    theGrid[i, c] = new FinishTile();
                    return new Vector2Int(i, c);
                }
            }
        }
        return new Vector2Int(-1000,-1000);
    }


    private void proceduralGenerationRules() {
        generatedTileCount = 0;
        TileTypes[,] tempGrid = new TileTypes[rows, columns];
        for (int i = 0; i < rows; ++i) {
            for (int c = 0; c < columns; ++c) {
                //con esto funciona el finishTile
                //if (theGrid[i, c] is FinishTile) {
                //    tempGrid[i, c] = theGrid[i, c];
                //    continue;
                //}
                //if (theGrid[i, c] == null || !theGrid[i, c].isEditable) {
                //    tempGrid[i, c] = theGrid[i, c];
                //    continue;
                //}
                ////////////////////////////////////////////////////
                //con esto funciona lo de no modificar los anteriores
                TileTypes tileActual = theGrid[i, c];
                if (tileActual != null && (!tileActual.isEditable || tileActual is FinishTile)) {
                    tempGrid[i, c] = tileActual;
                    continue;
                }

                //if (theGrid[i, c] is FinishTile || theGrid[i, c] == null || !theGrid[i, c].isEditable) {
                //    tempGrid[i, c] = theGrid[i, c];
                //    continue; 
                //}

                int grassNeighs = checkTypeNeighs(i, c, typeof(Grass));
                int mudNeighs = checkTypeNeighs(i, c, typeof(Mud));
                int waterNeighs = checkTypeNeighs(i, c, typeof(Water));
                int stoneNeighs = checkTypeNeighs(i, c, typeof(Stone));
                int spikesNeighs = checkTypeNeighs(i, c, typeof(Spikes));
                int deadNeighs = checkTypeNeighs(i, c, typeof(DeadTile));
                ///////////////////////////////////////con esto funciona lo de finishTile
                //tempGrid[i, c] = theGrid[i, c].neighsTypeCount(grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);
                //if (tempGrid[i, c] != null && !(tempGrid[i, c] is DeadTile)) {
                //    generatedTileCount++;
                //}
                ///////////////////////////////////////////
                TileTypes nuevoTipoTile = tileActual.neighsTypeCount(grassNeighs, mudNeighs, waterNeighs, stoneNeighs, spikesNeighs, deadNeighs);
                tempGrid[i, c] = nuevoTipoTile;
                if (nuevoTipoTile != null && !(nuevoTipoTile is DeadTile)) {
                    tempGrid[i, c].isEditable = false;
                }
            }
        }
        //con esto funciona lo de finishTile
        //theGrid = tempGrid;
        //updateVisualGrid();
        //if (isGridFull()) {
        //    if (!finishTilePlaced) {
        //        placeFinishTile();
        //        finishTilePlaced = true;
        //        Debug.LogWarning("FinishTile colocado en una posición aleatoria.");
        //    }
        //    updateVisualGrid();
        //    Debug.Log("Grid completo con FinishTile.");
        //    simulationIsRunning = false;
        //    Debug.Log(PrintGridState());
        //}
        ///////////////////////

        theGrid = tempGrid;
        updateVisualGrid();
        if (isGridFull()) {
            placeFinishTile();
            simulationIsRunning = false;
            Debug.LogWarning("Grid completo con el FinishTile colocado.");
            Debug.Log(PrintGridState());
        }
    }

    private void placeFinishTile() {
        bool placed = false;
        while (!placed) {
            int x = Random.Range(0, rows);
            int y = Random.Range(0, columns);
            if (theGrid[x, y] == null || theGrid[x, y] is DeadTile) {
                theGrid[x, y] = new FinishTile();
                theGrid[x, y].isEditable = false;
                placed = true;
                Debug.Log("FinishTile colocado en posición: " + x + ", " + y);
            }
        }
        updateVisualGrid();
        //////////////////////
        //con esto funciona lo de finifhTile
        //bool placed = false;
        //while (!placed) {
        //    int randomRow = Random.Range(0, rows);
        //    int randomColumn = Random.Range(0, columns);

        //    if (!(theGrid[randomRow, randomColumn] is DeadTile)) {
        //        theGrid[randomRow, randomColumn] = new FinishTile();  
        //        placed = true;
        //    }
        //}
        //updateVisualGrid();
        ///////////////////////////////////////////////////

        //bool placed = false;
        //while (!placed) {
        //    int randomRow = Random.Range(0, rows);
        //    int randomColumn = Random.Range(0, columns);
        //    if (theGrid[randomRow, randomColumn] == null || theGrid[randomRow, randomColumn] is DeadTile || theGrid[randomRow, randomColumn] is FinishTile) {
        //        theGrid[randomRow, randomColumn] = new FinishTile();
        //        theGrid[randomRow, randomColumn].isEditable = false;  
        //        placed = true;
        //        Debug.Log("FinishTile colocado en posición: " + randomRow + ", " + randomColumn);
        //    }
        //}
        //updateVisualGrid();
    }
    private bool isGridFull() {
       
        return generatedTileCount >= rows * columns; 
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
                    //aca nada mas se lo paso a cada una de mis variabels locales ,osea el tipo 
                    if (theGrid[checkX, checkY] != null && theGrid[checkX,checkY].GetType() == tileType) {
                        typeCount++;
                    }
                }
            }
        }

        return typeCount;
    }

    private void Update() {
        if (simulationIsRunning && !isGenerationInProgress) {
            Debug.Log("Iniciando la corrutina...");
            StartCoroutine(generationsInterval());
        }

    }

    private void updateVisualGrid() {
        HashSet<Vector3Int> instantiatedPositions = new HashSet<Vector3Int>();
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(i, c, 0);
                TileTypes tile = theGrid[i, c];
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
                } else if(tile is FinishTile) {
                    tilemap.SetTile(currentGridPos, finishTile);
                    instantiateEmptyObj(finishTileObj, currentGridPos);
                } else {
                    tilemap.SetTile(currentGridPos, deadTile);
                }
                //tile.isChanging = false;
                instantiatedPositions.Add(currentGridPos);
                //TileTypes tileToAssign = theGrid[i, c];
                //assignGameObjToTile(tileToAssign, i,c);
            }
        }
    }

    void instantiateEmptyObj(GameObject gameObj, Vector3Int thegridPos) {
        Vector3 worldPosition = tilemap.CellToWorld(thegridPos);
        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition);
        if(colliders.Length == 0) {
            GameObject instance = Instantiate(gameObj, worldPosition, Quaternion.identity);
            Debug.Log($"Instanciado {gameObj.name} en {worldPosition}");
        } else {
            Debug.LogWarning($"El objeto {gameObj.name} ya existe en {worldPosition}. No se instancia duplicados.");
        }
    }

    IEnumerator generationsInterval() {
        isGenerationInProgress = true;  
        while (simulationIsRunning) {
            Debug.Log("Generando nueva iteración...");
            proceduralGenerationRules();  
            updateVisualGrid();  
            yield return new WaitForSeconds(1.5f);  
        }
        isGenerationInProgress = false;  
        Debug.Log("Simulación finalizada.");
    }

    public void startGameOfLife() {
        initializeLevelGeneration();
        simulationIsRunning = true;
        //proceduralGenerationRules();
        //updateVisualGrid();
        StartCoroutine(generationsInterval());
    }

    public void stopGameOfLife() {
        simulationIsRunning = false;
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

        private string PrintGridState() {
            for(int i = 0;i < rows; i++) {
                gridStateDebug[i] = "";
                for(int c = 0; c < columns; c++) {
                    if (theGrid[i, c] is Grass) {
                        gridStateDebug[i] += "G ";
                    } else if (theGrid[i, c] is Mud) {
                        gridStateDebug[i] += "M ";
                    } else if (theGrid[i, c] is Water) {
                        gridStateDebug[i] += "W ";
                    } else if (theGrid[i, c] is Spikes) {
                        gridStateDebug[i] += "S ";
                    } else if (theGrid[i, c] is Stone) {
                        gridStateDebug[i] += "R ";
                    } else if (theGrid[i, c] is FinishTile) {
                        gridStateDebug[i] += "F ";
                    } else {
                        gridStateDebug[i] += "D ";
                    }
                }
            }
            string gridState = "";
            foreach (string row in gridStateDebug) {
                gridState += row + "\n";
            }
            return gridState;
        }

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

    //void assignGameObjToTile(TileTypes tileType, int x, int y) {
    //    GameObject objToTile = null;
    //    if(tileType is Grass) {
    //        objToTile = grassObj;
    //    } else if(tileType is Mud) {
    //        objToTile = mudObj;
    //    } else if (tileType is Water) {
    //        objToTile = waterObj;
    //    }else if (tileType is Stone) {
    //        objToTile = stoneObj;
    //    }else if(tileType is Spikes) {
    //        objToTile = spikesObj;
    //    }else {
    //        objToTile = deadObj;
    //    }

    //    if(objToTile != null) {
    //        Vector3Int pos = new Vector3Int(x, y, 0);
    //        GameObject tileObj = Instantiate(objToTile, tilemap.CellToWorld(pos) +new Vector3(0.5f,0.5f,0), Quaternion.identity);
    //        tileObj.transform.SetParent(tilemap.transform);
    //    }
    //}


    //public void runGameOfLife() {
    //    TileTypes[,] tempGrid = new TileTypes[rows, columns];
    //    for (int i = 0; i < rows; i++) {
    //        for (int c = 0; c < columns; c++) {
    //            tempGrid[i, c] = new Grass();
    //        }
    //    }
    //    for (int i = 0; i < rows; i++) {
    //        for (int c = 0; c < columns; c++) {
    //            if (theGrid[i, c] != null) {
    //                int currentNeighs = checkNeighCells(i, c);
    //                if (theGrid[i, c].bIsAlive) {
    //                    tempGrid[i, c].bIsAlive = currentNeighs >= 2 && currentNeighs <= 3;
    //                } else {
    //                    tempGrid[i, c].bIsAlive = currentNeighs == 3;
    //                }
    //            }
    //        }
    //    }

    //    theGrid = tempGrid;
    //    updateVisualGrid();
    //}


    //int checkNeighCells(int x, int y) {
    //    int aliveNeighs = 0;
    //    for (int i = -1; i <= 1; i++) {
    //        for (int j = -1; j <= 1; j++) {
    //            if (i == 0 && j == 0) continue;
    //            int checkX = x + i;
    //            int checkY = y + j;
    //            if (checkX >= 0 && checkX < rows && checkY >= 0 && checkY < columns) {
    //                if (theGrid[checkX, checkY] != null && theGrid[checkX, checkY].bIsAlive) {
    //                    aliveNeighs++;
    //                }
    //            }
    //        }
    //    }
    //    return aliveNeighs;
    //}
