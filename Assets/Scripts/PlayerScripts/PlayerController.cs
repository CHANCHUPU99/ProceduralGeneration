using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour 
{
    public float moveSpeed = 5f;
    private bool isMoving;
    private Vector3 targetPosition;
    public Tilemap tilemap; 
    private LevelGeneration levelGeneration;
    void Start() {
        levelGeneration = FindObjectOfType<LevelGeneration>();
        targetPosition = transform.position;
    }

    void Update() {
        if (!isMoving) {
            input();
        } else {
            moveToTarget();
        }
    }

    private void input() {
        Vector3Int moveDirection = Vector3Int.zero;

        if(Input.GetKeyDown(KeyCode.W)) {
            moveDirection = Vector3Int.up;
        } else if(Input.GetKeyDown(KeyCode.S)) {
            moveDirection = Vector3Int.down;
        } else if(Input.GetKeyDown(KeyCode.A)) {
            moveDirection = Vector3Int.left;
        } else if(Input.GetKeyDown(KeyCode.D)) {
            moveDirection = Vector3Int.right;
        }

        if(moveDirection != Vector3Int.zero) {
            Vector3Int currentGridPosition = tilemap.WorldToCell(transform.position);
            Vector3Int newGridPosition = currentGridPosition + moveDirection;
            if(isPosInThegrid(newGridPosition)) {
                TileTypes tile = getTileType(newGridPosition);
                if (tile != null && tile.bIsSafeToWalk) {
                    targetPosition = tilemap.CellToWorld(newGridPosition) + new Vector3(0.5f, 0.5f, 0);
                    isMoving = true;
                } else if (tile != null && tile is Spikes) {
                    Debug.Log("detecto spikeeee");

                } else if (tile is FinishTile) {
                    Debug.Log("Jugador ha ganado el juego!");
                } else {
                    Debug.Log("no se puede caminar en ese tile");
                }
            } else {
                Debug.Log("gfuera de thegrid");
            }
        }
    }

    private void moveToTarget() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
            transform.position = targetPosition;
            isMoving = false; 
        }
    }

    private bool isPosInThegrid(Vector3Int position) {
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
