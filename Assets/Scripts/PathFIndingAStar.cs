using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFIndingAStar : MonoBehaviour
{
    public TileTypes[,] theGrid; 
    private Vector2Int start;
    private Vector2Int goal;
    private List<Vector2Int> path = new List<Vector2Int>();

    public void FindPath(Vector2Int start, Vector2Int goal) {
        this.start = start;
        this.goal = goal;
        path = AStarAlgorithm();
        highlightPath();
    }

    private List<Vector2Int> AStarAlgorithm() {
        List<Vector2Int> openSet = new List<Vector2Int> { start };
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> gScore = new Dictionary<Vector2Int, int> { [start] = 0 };
        Dictionary<Vector2Int, int> fScore = new Dictionary<Vector2Int, int> { [start] = heuristic(start, goal) };

        while (openSet.Count > 0) {
            Vector2Int current = getNodeWithLowestFScore(openSet, fScore);
            if (current == goal) return reconstructPath(cameFrom, current);
            openSet.Remove(current);
            closedSet.Add(current);
            foreach (Vector2Int neighbor in getNeighbors(current)) {
                if (closedSet.Contains(neighbor) || isObstacle(neighbor)) continue;

                int tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return new List<Vector2Int>(); 
    }

    private int heuristic(Vector2Int a, Vector2Int b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private Vector2Int getNodeWithLowestFScore(List<Vector2Int> openSet, Dictionary<Vector2Int, int> fScore) {
        Vector2Int lowestNode = openSet[0];
        foreach (Vector2Int node in openSet) {
            if (fScore[node] < fScore[lowestNode]) lowestNode = node;
        }
        return lowestNode;
    }

    private List<Vector2Int> getNeighbors(Vector2Int pos) {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(pos.x + 1, pos.y),
            new Vector2Int(pos.x - 1, pos.y),
            new Vector2Int(pos.x, pos.y + 1),
            new Vector2Int(pos.x, pos.y - 1)
        };
        neighbors.RemoveAll(n => n.x < 0 || n.y < 0 || n.x >= theGrid.GetLength(0) || n.y >= theGrid.GetLength(1));
        return neighbors;
    }

    private bool isObstacle(Vector2Int pos) {
        TileTypes tile = theGrid[pos.x, pos.y];
        return tile is Spikes || tile is Stone;
    }

    private List<Vector2Int> reconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current) {
        List<Vector2Int> path = new List<Vector2Int> { current };
        while(cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private void highlightPath() {
        foreach(Vector2Int pos in path) {
            Debug.Log("Camino a: " + pos);
        }
    }
}
