using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockEditor : MonoBehaviour
{
    public List<Tile> blockTiles;
    public List<Tile> wallTiles;
    public Tilemap blockMap;

    public List<string> movableTiles;

    public float innerSpawnRadius;
    public float outerSpawnRadius;
    public int numberOfBlocks;
    public List<float> generationProbabilities;

    private Tile activeTile;
    private Vector3Int previous;
    private Dictionary<Tile, Tile> blockToWall = new Dictionary<Tile, Tile>();

    // Start is called before the first frame update
    void Start()
    {
        previous = blockMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        for (int i = 0; i < wallTiles.Count; i++)
        {
            blockToWall.Add(blockTiles[i], wallTiles[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlockWalls();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateBlocks();
        }

        Vector3Int curCell = blockMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Tile curTile = blockMap.GetTile<Tile>(curCell);

        // Reset activeTile when mouse button is let go
        if (Input.GetMouseButtonUp(0))
        {
            activeTile = null;
            AstarPath.active.Scan();
        }

        // Initialize variables when mouse button is first pressed
        if (Input.GetMouseButtonDown(0) && curTile != null && movableTiles.Contains(curTile.name))
        {
            previous = curCell;
            activeTile = blockMap.GetTile<Tile>(previous);
        }

        // Don't do anything when
        // 1) button is not pressed
        // 2) or activeTile is null because player didn't click on a tile
        // 3) or player is trying to move a tile on top of another tile
        if (!Input.GetMouseButton(0) || activeTile == null || (previous != curCell && IsMovable(curCell)))
        {
            return;
        }

        Vector3Int hoveredTile = curCell;

        // Set the previous tile to nothing
        blockMap.SetTile(previous, null);
        previous = hoveredTile;

        // Set the mouse position cell to selected tile
        blockMap.SetTile(hoveredTile, activeTile);
    }

    private void GenerateBlocks()
    {
        blockMap.ClearAllTiles();

        for (int i = 0; i < numberOfBlocks; i++)
        {
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            float randomDist = Random.Range(innerSpawnRadius, outerSpawnRadius);
            Vector2 dir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

            Vector3Int randCell = blockMap.WorldToCell(dir * randomDist);
            if(blockMap.HasTile(randCell))
            {
                i--;
                continue;
            }
            blockMap.SetTile(randCell, blockTiles[0]);
        }
    }

    // Draw block walls
    private void UpdateBlockWalls()
    {
        for (int x = blockMap.cellBounds.min.x; x < blockMap.cellBounds.max.x; x++)
        {
            for (int y = blockMap.cellBounds.min.y; y < blockMap.cellBounds.max.y; y++)
            {
                Vector3Int cur = new Vector3Int(x, y);
                if (!blockMap.HasTile(cur))
                {
                    if (!BelowIsMovable(cur, out Vector3Int cellBelow))
                    {
                        blockMap.SetTile(cellBelow, null);
                    }
                    continue;
                }
                if (IsMovable(cur) && !BelowIsMovable(cur, out Vector3Int cellBelow1))
                {
                    Tile curTile = blockMap.GetTile<Tile>(cur);
                    blockMap.SetTile(cellBelow1, blockToWall[curTile]);
                }
            }
        }
    }

    // Check if cell below is a movable block
    private bool BelowIsMovable(Vector3Int cur, out Vector3Int cellBelow)
    {
        cellBelow = cur;
        cellBelow.y -= 1;

        return IsMovable(cellBelow);
    }

    // Check if cell is a movable block
    private bool IsMovable(Vector3Int cell)
    {
        if (!blockMap.HasTile(cell))
        {
            return false;
        }
        Tile tileBelow = blockMap.GetTile<Tile>(cell);
        return movableTiles.Contains(tileBelow.name);
    }
}
