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
    public Vector4 boardPadding = Vector4.zero;

    public GameBoardSlot[,] slots;

	void Start () {
        // Create instance of the selected tile object
        selectedTileObject = Instantiate(selectedTilePrefab);
        selectedTileObject.transform.SetParent(this.transform);

        // Set the size of the GameBoard object
        //RectTransform rTransform = GetComponent<RectTransform>();
        //rTransform.sizeDelta = new Vector2(
        //    boardWidth + boardPadding.x + boardPadding.z, 
        //    boardHeight + boardPadding.y + boardPadding.w);

        // Create a sub group for all the tiles to be parented to
        var tileGroupGO = new GameObject("Tiles");
        tileGroupGO.transform.SetParent(this.transform);
        tileGroupGO.transform.Translate(new Vector3(boardPadding.x, boardPadding.y));

        // Create the background slots for each tile
        slots = new GameBoardSlot[boardWidth, boardHeight];
        for (int x = 0; x < boardWidth; x++) {
            for (int y = 0; y < boardHeight; y++) {
                var tileGO = GameObject.Instantiate(gameBoardSlotPrefab);
                tileGO.name = tileGO.name + (y * boardWidth + x);

                slots[x, y] = tileGO.GetComponent<GameBoardSlot>();

                tileGO.transform.SetParent(tileGroupGO.transform);
                tileGO.transform.localPosition = new Vector3(x - (boardWidth / 2f), y - (boardHeight / 2f), 0f);
            }
            slots[x, 0].SlotTile = Instantiate(tilePrefab).GetComponent<Tile>();
            slots[x, 0].SlotTile.SetText("A");
            slots[x, 0].SlotTile.transform.position = slots[x, 0].transform.position;
        }
	}

    public GameBoardSlot lastSlot;
    public Tile pickedUpTile;

    public void Update() {
        // On Mouse Down pick up tile if there is one
        // under the mouse's current location
        if (Input.GetMouseButtonDown(0)) {
            var hitinfo = RaycastFromMouse();
            if (hitinfo.collider != null) {
                var boardSlot = hitinfo.transform.GetComponent<GameBoardSlot>();
                if (boardSlot != null && boardSlot.SlotTile != null) {
                    lastSlot = boardSlot;

                    pickedUpTile = boardSlot.SlotTile;
                    pickedUpTile.GetComponent<Animator>().SetTrigger("PickUp");
                    
                    boardSlot.SlotTile = null;
                }
            }
        }

        // On Mouse Up drop tile into open slot under
        // the mouse or move it back to the last slot
        if (Input.GetMouseButtonUp(0) && pickedUpTile != null) {
            var hitinfo = RaycastFromMouse();
            if (hitinfo.collider != null) {
                var boardSlot = hitinfo.transform.GetComponent<GameBoardSlot>();
                if (boardSlot != null && boardSlot.SlotTile == null) {
                    boardSlot.SlotTile = pickedUpTile;

                    pickedUpTile.transform.position = boardSlot.transform.position;
                    pickedUpTile.GetComponent<Animator>().SetTrigger("Drop");

                    lastSlot = null;
                    pickedUpTile = null;
                } else {
                    ReturnTileToLastSlot();
                }
            } else {
                ReturnTileToLastSlot();
            }
        }

        if (pickedUpTile != null) {
            var followPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            followPoint.z = pickedUpTile.transform.position.z;

            pickedUpTile.transform.position = Vector3.Lerp(followPoint, 
                pickedUpTile.transform.position, 10 * Time.deltaTime);
        }
    }

    private RaycastHit2D RaycastFromMouse() {
        var mousePosition = Input.mousePosition;
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }

    private void ReturnTileToLastSlot() {
        lastSlot.SlotTile = pickedUpTile;

        pickedUpTile.transform.position = lastSlot.transform.position;
        pickedUpTile.GetComponent<Animator>().SetTrigger("Drop");

        lastSlot = null;
        pickedUpTile = null;
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
