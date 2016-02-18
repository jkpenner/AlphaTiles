using UnityEngine;
using System.Collections;

public class TilePickUpManager : Singleton<TilePickUpManager> {
    protected TilePickUpManager() { }

    private GameBoardSlot _lastSlot;
    private Tile _pickedUpTile;
    private bool _invalidDrop = false;

    public enum DropType {
        // Returns the currently picked up
        // tile to it's last slot
        ReturnToLast,
        // Places the currently picked up
        // tile into the slot under the mouse
        // and sets the tile in the drop slot
        // as the currently picked Up tile
        SwapPickUp,
        // Places the currently picked up
        // tile into the slot under the mouse
        // and sends the tile in the drop slot
        // to the last slot
        SwapToLast,
    }
    public DropType dropType = DropType.ReturnToLast;

    public void Update() {
        // Only Check for player input when the game is active
        if (GameManager.Instance.ActiveState == GameState.Active) {
            // On Mouse Down pick up tile if there is one
            // under the mouse's current location
            if (Input.GetMouseButtonDown(0)) {
                if (_pickedUpTile != null) {
                    HandleTileDrop();
                } else {
                    HandleTilePickUp();
                }
            }

            // On Mouse Up drop tile into open slot under
            // the mouse or move it back to the last slot
            if (Input.GetMouseButtonUp(0)) {
                if (_pickedUpTile != null) {
                    HandleTileDrop();
                }
            }

            // Updates the Following Tile towards the mouse's position
            if (_pickedUpTile != null) {
                Vector3 followPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                followPoint.z = _pickedUpTile.transform.position.z;
                _pickedUpTile.transform.position = Vector3.Lerp(followPoint,
                    _pickedUpTile.transform.position, 10 * Time.deltaTime);
            }
        }
    }

    private RaycastHit2D RaycastFromMouse() {
         return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    }

    private void HandleTilePickUp() {
        var hitinfo = RaycastFromMouse();
        if (hitinfo.collider != null) {
            var boardSlot = hitinfo.transform.GetComponent<GameBoardSlot>();
            if (boardSlot != null && boardSlot.SlotTile != null) {
                PickUpTile(boardSlot);
            }
        }
    }

    private void HandleTileDrop() {
        var hitinfo = RaycastFromMouse();
        if (hitinfo.collider != null) {
            var boardSlot = hitinfo.transform.GetComponent<GameBoardSlot>();
            if (boardSlot != null) {
                if (boardSlot.SlotTile == null) {
                    DropTile(boardSlot);
                } else {
                    if (dropType == DropType.ReturnToLast) {
                        ReturnTileToLastSlot();
                    } else if (dropType == DropType.SwapPickUp) {
                        SwapTileWithPickedUp(boardSlot);
                    } else if (dropType == DropType.SwapToLast) {
                        SwapTileWithLastSlot(boardSlot);
                    }
                }
            } else {
                ReturnTileToLastSlot();
            }
        } else {
            _invalidDrop = true;
            ReturnTileToLastSlot();
        }
    }

    private void PickUpTile(GameBoardSlot slot) {
        _pickedUpTile = slot.SlotTile;
        _lastSlot = slot;

        slot.SlotTile.PickUp();
        slot.SlotTile = null;
    }

    private void DropTile(GameBoardSlot slot) {
        slot.SlotTile = _pickedUpTile;
        slot.SlotTile.Drop(slot.transform);

        _lastSlot = null;
        _pickedUpTile = null;
    }

    private void ReturnTileToLastSlot() {
        _lastSlot.SlotTile = _pickedUpTile;
        if (_invalidDrop) {
            _pickedUpTile.Drop(_lastSlot.transform, true);
        } else {
            _pickedUpTile.Drop(_lastSlot.transform);
        }

        _lastSlot = null;
        _pickedUpTile = null;
        _invalidDrop = false;
    }

    private void SwapTileWithPickedUp(GameBoardSlot slot) {
        var oldtile = _pickedUpTile;
        oldtile.Drop(slot.transform);

        _pickedUpTile = slot.SlotTile;
        _pickedUpTile.PickUp();

        slot.SlotTile = oldtile;
        _lastSlot = slot;
    }

    private void SwapTileWithLastSlot(GameBoardSlot slot) {
        var oldtile = _pickedUpTile;
        oldtile.Drop(slot.transform);

        var newTile = slot.SlotTile;
        newTile.Drop(_lastSlot.transform);

        slot.SlotTile = oldtile;
        _lastSlot.SlotTile = newTile;

        _lastSlot = null;
        _pickedUpTile = null;
    }
}
