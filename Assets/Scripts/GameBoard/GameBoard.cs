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

    public delegate void GameBoardEvent();
    public event GameBoardEvent OnGameBoardFilled;

	void Awake () {
        // Create instance of the selected tile object
        selectedTileObject = Instantiate(prefabTileSelected);
        selectedTileObject.transform.SetParent(this.transform);
        selectedTileObject.SetActive(false);

        if (generateBoardOnStart) {
            GenerateBoard(boardWidth, boardHeight);
        }
	}

    public void Update() {
        // Only Check for player input when the game is active
        if (GameManager.Instance.ActiveState == GameState.Active) {
            // Gets the GameBoardSlot under the mouse and move the selected tile object to its position
            var hitinfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitinfo.collider != null && hitinfo.transform.GetComponent<GameBoardSlot>() != null) {
                selectedTileObject.SetActive(true);
                selectedTileObject.transform.position = hitinfo.transform.position;
            } else {
                selectedTileObject.SetActive(false);
            }
        } else {
            selectedTileObject.SetActive(false);
        }
    }

    public void GenerateBoard(int width, int height) {
        // ToDo: Reuse old GameBoardSlot's to reduce
        // amount of instantiating during generating board
        DestroyBoard();

        // Create a sub group for all the tiles to be parented to
        tileGroup = new GameObject("Tiles").transform;
        tileGroup.SetParent(this.transform);
        tileGroup.localPosition = Vector2.zero;

        boardWidth = width;
        boardHeight = height;

        // Calculate the min values for positions of tiles
        float xMin = -(boardWidth + (boardSpacing.x * (boardWidth - 1f))) / 2f;
        xMin += (xMin % 2 == 0 ? 0 : 0.5f + boardSpacing.x);

        float yMin = -(boardHeight + (boardSpacing.y * (boardHeight - 1f))) / 2f;
        yMin += (yMin % 2 == 0 ? 0 : 0.5f + boardSpacing.y);

        // Create the background slots for each tile
        slots = new GameBoardSlot[boardWidth, boardHeight];
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                var tileGO = Instantiate(prefabGameBoardSlot);
                slots[x, y] = tileGO.GetComponent<GameBoardSlot>();
                slots[x, y].AddListener(OnSlotTileChange);

                tileGO.name = prefabGameBoardSlot.name + (y * boardWidth + x);
                tileGO.transform.SetParent(tileGroup);
                tileGO.transform.localPosition = new Vector3(
                    xMin + x + (boardSpacing.x * (x - 1)),
                    yMin + y + (boardSpacing.y * (y - 1)), 0f);
            }
        }
    }

    public void DestroyBoard() {
        if (slots != null) {
            Destroy(tileGroup.gameObject);
            slots = null;
        }
    }

    public void ClearBoard() {
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                if (slots[x, y].SlotTile != null) {
                    Destroy(slots[x, y].SlotTile.gameObject);
                }
            }
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

    public void OnSlotTileChange(GameBoardSlot slot) {
        bool isFilled = true;
        foreach (var s in slots) {
            if (s.SlotTile == null) {
                isFilled = false;
                break;
            }
        }

        if (isFilled && OnGameBoardFilled != null) {
            OnGameBoardFilled();
        }
    }
}
