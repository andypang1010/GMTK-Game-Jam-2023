using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockEditor : MonoBehaviour
{
    public Tile boxTile;
    public Tilemap blockMap;

    private Tile activeTile;
    private Vector3Int previous;

    // Start is called before the first frame update
    void Start()
    {
        previous = blockMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int curCell = blockMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        // Reset activeTile when mouse button is let go
        if (Input.GetMouseButtonUp(0))
        {
            activeTile = null;
        }

        // Initialize variables when mouse button is first pressed
        if (Input.GetMouseButtonDown(0))
        {
            previous = curCell;
            activeTile = blockMap.GetTile<Tile>(previous);
        }

        // Don't do anything when
        // 1) button is not pressed
        // 2) or activeTile is null because player didn't click on a tile
        // 3) or player is trying to move a tile on top of another tile
        if (!Input.GetMouseButton(0) || activeTile == null || (previous != curCell && blockMap.HasTile(curCell)))
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
}
