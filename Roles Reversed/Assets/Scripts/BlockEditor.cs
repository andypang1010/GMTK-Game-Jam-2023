using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateBlocks();
        }

        Vector3Int curCell = blockMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Tile curTile = blockMap.GetTile<Tile>(curCell);

        // Reset activeTile when mouse button is let go
        if (Input.GetMouseButtonUp(0))
        {
            //activeTile.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            activeTile = null;
            AstarPath.active.Scan();
        }

        // Initialize variables when mouse button is first pressed
        if (Input.GetMouseButtonDown(0) && curTile != null && movableTiles.Contains(curTile.name))
        {
            previous = curCell;
            activeTile = blockMap.GetTile<Tile>(previous);
            //activeTile.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        // Don't do anything when
        // 1) button is not pressed
        // 2) or activeTile is null because player didn't click on a tile
        // 3) or player is trying to move a tile on top of another tile
        if (!Input.GetMouseButton(0) || activeTile == null || (previous != curCell && IsMovable(curCell)))
        {
            return;
        }

        // Set the previous tile to nothing
        blockMap.SetTile(previous, null);

        // Set the mouse position cell to selected tile
        blockMap.SetTile(curCell, activeTile);

        if (previous != curCell)
        {
            UpdateTileAtPos(previous);
            UpdateTileAtPos(curCell);
        }

        previous = curCell;
    }

    // randomly generate blocks within the ring defined by the inner and outer radius
    private void GenerateBlocks()
    {
        blockMap.ClearAllTiles();

        for (int i = 0; i < numberOfBlocks; i++)
        {
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            float randomDist = Random.Range(innerSpawnRadius, outerSpawnRadius);
            Vector2 dir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

            Vector3Int randCell = blockMap.WorldToCell(dir * randomDist);

            // if the randomly generated cell already has a tile, regenerate
            if (blockMap.HasTile(randCell))
            {
                i--;
                continue;
            }

            // Generate based on custom probability
            float probabilitySum = 0;
            float randomNum = Random.value;
            for(int j = 0; j < generationProbabilities.Count; j++)
            {
                if (randomNum >= probabilitySum && randomNum < probabilitySum + generationProbabilities[j])
                {
                    blockMap.SetTile(randCell, blockTiles[j]);
                    break;
                }
                probabilitySum += generationProbabilities[j];
            }

            // set wall below
            randCell.y--;
            UpdateTileAtPos(randCell);
        }
        // TODO: optimize performance
        AstarPath.active.Scan();
    }

    // Draw block walls
    private void UpdateBlockWalls()
    {
        for (int x = Mathf.FloorToInt(-outerSpawnRadius); x < Mathf.CeilToInt(outerSpawnRadius); x++)
        {
            for (int y = Mathf.FloorToInt(-outerSpawnRadius); y < Mathf.CeilToInt(outerSpawnRadius); y++)
            {
                Vector3Int cur = new Vector3Int(x, y);
                if (!IsMovable(cur))
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
        AstarPath.active.Scan();
    }

    // update surrounding tiles at a given position
    private void UpdateTileAtPos(Vector3Int pos)
    {
        Vector3Int above = pos;
        Vector3Int below = pos;

        above.y += 1;
        below.y -= 1;

        if (!blockMap.HasTile(pos))
        {
            if (IsMovable(above))
            {
                Tile tileAbove = blockMap.GetTile<Tile>(above);
                if (blockToWall.ContainsKey(tileAbove))
                {
                    blockMap.SetTile(pos, blockToWall[tileAbove]);
                }
            }
            if (!IsMovable(below))
            {
                blockMap.SetTile(below, null);
            }
        }
        if (blockMap.HasTile(pos) && !IsMovable(below))
        {
            Tile curTile = blockMap.GetTile<Tile>(pos);
            if (blockToWall.ContainsKey(curTile))
            {
                blockMap.SetTile(below, blockToWall[curTile]);
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
