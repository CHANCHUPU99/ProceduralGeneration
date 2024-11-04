using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    public Tilemap tilemap;

    private Vector3 targetPosition;
    private Rigidbody2D rb;

    void Start() {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; 
    }

    void Update() {
        MovePlayer();
    }

    void MovePlayer() {
        if(Vector3.Distance(transform.position, targetPosition) <= 0.1f) {
            if(Input.GetKeyDown(KeyCode.W)) {
                TryMove(Vector3.up);
            } else if(Input.GetKeyDown(KeyCode.S)) {
                TryMove(Vector3.down);
            } else if(Input.GetKeyDown(KeyCode.A)) {
                TryMove(Vector3.left);
            } else if(Input.GetKeyDown(KeyCode.D)) {
                TryMove(Vector3.right);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void TryMove(Vector3 direction) {
        Vector3Int gridPosition = tilemap.WorldToCell(transform.position + direction);
        TileBase tileBase = tilemap.GetTile(gridPosition);

        if(tileBase != null) {
            TileTypes tile = GetTileType(gridPosition);
            if(tile != null && tile.bIsSafeToWalk) {
                targetPosition = tilemap.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f, 0);
            }
        }
    }

    TileTypes GetTileType(Vector3Int gridPosition) {
        LevelGeneration levelGeneration = FindObjectOfType<LevelGeneration>();
        if(levelGeneration != null) {
            return levelGeneration.theGrid[gridPosition.x, gridPosition.y];
        }
        return null;
    }
}
