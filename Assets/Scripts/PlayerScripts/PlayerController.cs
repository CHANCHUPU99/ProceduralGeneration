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

    void Start() {
        targetPosition = transform.position;
    }

    void Update() {
        if (!isMoving) {
            input();
        }
    }

    private void input() {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W)) {
            moveDirection = Vector3.up;
        }else if (Input.GetKeyDown(KeyCode.S)) {
            moveDirection = Vector3.down;
        }else if (Input.GetKeyDown(KeyCode.A)) {
            moveDirection = Vector3.left;
        } else if (Input.GetKeyDown(KeyCode.D)) {
            moveDirection = Vector3.right;
        }
        if(moveDirection != Vector3.zero) {
            Vector3 newPosition = targetPosition + moveDirection;
            //Collider2D tileCollider = Physics2D.OverlapPoint(newPosition);
            Vector3Int pos = tilemap.WorldToCell(newPosition);
            LevelGeneration levelGeneration = FindObjectOfType<LevelGeneration>();
            if(pos.x >= 0 && pos.y >= 0 && pos.x < levelGeneration.theGrid.GetLength(0) && pos.y < levelGeneration.theGrid.GetLength(1)) {
                TileTypes tile = levelGeneration.theGrid[pos.x, pos.y];
                if(tile != null && tile.bIsSafeToWalk) {
                    targetPosition = tilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);
                    isMoving = true;
                } else {
                    Debug.Log("no es seguro caminar");
                }
            } else {
                Debug.Log("fuera del grid");
            }
        }
            //targetPosition = newPosition;
            //isMoving = true;
    }
 
}
