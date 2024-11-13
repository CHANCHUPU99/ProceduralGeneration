using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour 
{
    public float moveSpeed = 5f;
    private bool isMoving;
    private Vector3 targetPosition;
    public Tilemap tilemap;
    [SerializeField] string winScene = "MainMenu";
    [SerializeField] string loseScene = "YouLoseScene";
    private NewLevelGeneration newLevelGeneration1;
    
    
    //private LevelGeneration levelGeneration;
    void Start() {
        //levelGeneration = FindObjectOfType<LevelGeneration>();
        targetPosition = transform.position;
        newLevelGeneration1 = FindObjectOfType<NewLevelGeneration>();
    }

    void Update() {
        if (!isMoving) {
            input();
        } else {
            moveToTarget();
        }
        checkPlayerPosition();
    }

    private void input() {
        Vector3Int moveDirection = Vector3Int.zero;

        if(Input.GetKeyDown(KeyCode.W)) {
            moveDirection = new Vector3Int(0,1,0);
        } else if(Input.GetKeyDown(KeyCode.S)) {
            moveDirection = new Vector3Int(0,-1,0);
        } else if(Input.GetKeyDown(KeyCode.A)) {
            moveDirection = new Vector3Int(-1, 0, 0);
        } else if(Input.GetKeyDown(KeyCode.D)) {
            moveDirection = new Vector3Int(1, 0, 0);
        }
        if(moveDirection != Vector3Int.zero) {
            Vector3Int currentGridPosition = tilemap.WorldToCell(transform.position);
            Vector3Int newGridPosition = currentGridPosition + moveDirection;
            if(isPLayerOnThegrid(newGridPosition)) {
                TileTypes tile = getTileType(newGridPosition);
                if (tile != null && tile.bIsSafeToWalk) {
                    targetPosition = tilemap.CellToWorld(newGridPosition) + new Vector3(0.5f, 0.5f, 0);
                    isMoving = true;
                } else if (tile != null && tile is Spikes) {
                    Debug.Log("detecto spikeeee");
                    SceneManager.LoadScene(loseScene);

                } 
                //else if (tile is FinishTile) {
                //    Debug.Log("Jugador ha ganado el juego!");
                //    SceneManager.LoadScene(MainMenu);
             //   } 
            else {
                    Debug.Log("no se puede caminar en ese tile");
                }
            } else {
                Debug.Log("gfuera de thegrid");
            }
        }
    }

    private void checkPlayerPosition() {
        if (newLevelGeneration1 == null) {
            return;
        } 
        Vector3Int playerGridPosition = tilemap.WorldToCell(transform.position);
        Vector2Int finishTilePosition = newLevelGeneration1.finishTilePosition;
        if (playerGridPosition.x == finishTilePosition.x && playerGridPosition.y == finishTilePosition.y) {
            Debug.Log("Jugador ha ganado el juego!");
            SceneManager.LoadScene("MainMenu"); 
        }
    }

    private void moveToTarget() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
            transform.position = targetPosition;
            isMoving = false; 
        }
    }

    private bool isPLayerOnThegrid(Vector3Int position) {
        NewLevelGeneration newLevelGeneration = FindObjectOfType<NewLevelGeneration>();
        return position.x >= 0 && position.y >= 0
            && position.x < newLevelGeneration.theGrid.GetLength(0)
            && position.y < newLevelGeneration.theGrid.GetLength(1);
    }

    private TileTypes getTileType(Vector3Int position) {
        NewLevelGeneration newLevelGeneration = FindObjectOfType<NewLevelGeneration>();
        return newLevelGeneration.theGrid[position.x, position.y];
    }
}
