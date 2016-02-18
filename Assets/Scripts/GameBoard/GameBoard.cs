using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class GameBoard : MonoBehaviour{
    public bool generateBoardOnStart = false;

    // All prefabs used
    public GameObject prefabGameBoardSlot;
    public GameObject prefabTileSelected;

    // The active object to show the selected tile
    private GameObject selectedTileObject;
    private Transform tileGroup;

    // The size of the board
    public int boardWidth = 5;
    public int boardHeight = 5;

    // Spacing between each tile
    public Vector2 boardSpacing = Vector2.zero;

    // References all tiles on the board
    public GameBoardSlot[,] slots;

	void Awake () {
        // Create instance of the selected tile object
        selectedTileObject = Instantiate(prefabTileSelected);
        selectedTileObject.transform.SetParent(this.transform);
        selectedTileObject.SetActive(false);

        // Create a sub group for all the tiles to be parented to
        tileGroup = new GameObject("Tiles").transform;
        tileGroup.SetParent(this.transform);
        tileGroup.localPosition = Vector2.zero;

        if (generateBoardOnStart) {
            GenerateBoard(boardWidth, boardHeight);
        }
	}

    public void Update() {
        // Gets the GameBoardSlot under the mouse and move the selected tile object to its position
        var hitinfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hitinfo.collider != null && hitinfo.transform.GetComponent<GameBoardSlot>() != null) {
            selectedTileObject.SetActive(true);
            selectedTileObject.transform.position = hitinfo.transform.position;
        } else {
            selectedTileObject.SetActive(false);
        }
    }

    public void GenerateBoard(int width, int height) {
        // ToDo: Reuse old GameBoardSlot's to reduce
        // amount of instantiating during generating board
        DestroyBoard();

        boardWidth = width;
        boardHeight = height;

        // Calculate the min values for positions of tiles
        float xMin = (boardWidth + (boardSpacing.x * (boardWidth - 1f))) / 2f;
        float yMin = (boardHeight + (boardSpacing.y * (boardHeight - 1f))) / 2f;

        // Create the background slots for each tile
        slots = new GameBoardSlot[boardWidth, boardHeight];
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                var tileGO = Instantiate(prefabGameBoardSlot);
                slots[x, y] = tileGO.GetComponent<GameBoardSlot>();

                tileGO.name = prefabGameBoardSlot.name + (y * boardWidth + x);
                tileGO.transform.SetParent(tileGroup);
                tileGO.transform.localPosition = new Vector3(
                    x + (boardSpacing.x * (x - 1)) - xMin,
                    y + (boardSpacing.y * (y - 1)) - yMin, 0f);
            }
        }
    }

    public void DestroyBoard() {
        if (slots != null) {
            for (int x = 0; x < slots.GetLength(0); x++) {
                for (int y = 0; y < slots.GetLength(1); y++) {
                    Destroy(slots[x, y].SlotTile.gameObject);
                    Destroy(slots[x, y].gameObject);
                }
            }
            slots = null;
        }
    }

    public void SetTile(int x, int y, Tile tile) {
        SetTile(slots[x, y], tile);
    }

    public void SetTile(GameBoardSlot slot, Tile tile) {
        if (slot != null && tile != null) {
            slot.SlotTile = tile;
            slot.SlotTile.transform.SetParent(slot.transform);
            slot.SlotTile.transform.position = slot.transform.position;
        }
    }
}
