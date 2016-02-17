using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class GameBoard : Singleton<GameBoard>{
    protected GameBoard() { }

    public GameObject gameBoardSlotPrefab;

    public GameObject selectedTilePrefab;
    private GameObject selectedTileObject;

    public GameObject tilePrefab;

    public int boardWidth = 5;
    public int boardHeight = 5;
    public Vector2 boardSpacing = Vector2.zero;

    public GameBoardSlot[,] slots;

	void Start () {
        // Create instance of the selected tile object
        selectedTileObject = Instantiate(selectedTilePrefab);
        selectedTileObject.transform.SetParent(this.transform);
        selectedTileObject.SetActive(false);

        // Set the size of the GameBoard object
        //RectTransform rTransform = GetComponent<RectTransform>();
        //rTransform.sizeDelta = new Vector2(
        //    boardWidth + boardPadding.x + boardPadding.z, 
        //    boardHeight + boardPadding.y + boardPadding.w);

        // Create a sub group for all the tiles to be parented to
        var tileGroupGO = new GameObject("Tiles");
        tileGroupGO.transform.SetParent(this.transform);


        float xMin = (boardWidth + (boardSpacing.x * (boardWidth - 1f))) / 2f;
        float yMin = (boardHeight + (boardSpacing.y * (boardHeight - 1f))) / 2f;

        // Create the background slots for each tile
        slots = new GameBoardSlot[boardWidth, boardHeight];
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                var tileGO = GameObject.Instantiate(gameBoardSlotPrefab);
                tileGO.name = tileGO.name + (y * boardWidth + x);

                slots[x, y] = tileGO.GetComponent<GameBoardSlot>();

                tileGO.transform.SetParent(tileGroupGO.transform);

                tileGO.transform.localPosition = new Vector3(
                    x + (boardSpacing.x * (x - 1)) - xMin, 
                    y + (boardSpacing.y * (y - 1)) - yMin, 0f);
            }
            slots[x, 0].SlotTile = Instantiate(tilePrefab).GetComponent<Tile>();
            slots[x, 0].SlotTile.SetText("A");
            slots[x, 0].SlotTile.transform.position = slots[x, 0].transform.position;
        }
	}

    public void SetSelectedTile(Transform target) {
        if (selectedTileObject != null) {
            if (target != null) {
                selectedTileObject.SetActive(true);
                selectedTileObject.transform.position = target.position;
            } else {
                selectedTileObject.SetActive(false);
            }
        }
    }
}
