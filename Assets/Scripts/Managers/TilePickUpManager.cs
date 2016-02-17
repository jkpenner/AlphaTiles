using UnityEngine;
using System.Collections;

public class TilePickUpManager : Singleton<TilePickUpManager> {
    protected TilePickUpManager() { }

    private GameBoardSlot _lastSlot;
    private Tile _pickedUpTile;

    public void Update() {
        // On Mouse Down pick up tile if there is one
        // under the mouse's current location
        if (Input.GetMouseButtonDown(0)) {
            var hitinfo = RaycastFromMouse();
            if (hitinfo.collider != null) {
                var boardSlot = hitinfo.transform.GetComponent<GameBoardSlot>();
                if (boardSlot != null && boardSlot.SlotTile != null) {
                    PickUpTile(boardSlot);
                }
            }
        }

        // On Mouse Up drop tile into open slot under
        // the mouse or move it back to the last slot
        if (Input.GetMouseButtonUp(0) && _pickedUpTile != null) {
            var hitinfo = RaycastFromMouse();
            if (hitinfo.collider != null) {
                var boardSlot = hitinfo.transform.GetComponent<GameBoardSlot>();
                if (boardSlot != null && boardSlot.SlotTile == null) {
                    DropTile(boardSlot);
                } else {
                    ReturnTileToLastSlot();
                }
            } else {
                ReturnTileToLastSlot();
            }
        }

        // Updates the Following Tile towards the mouse's position
        if (_pickedUpTile != null) {
            var followPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            followPoint.z = _pickedUpTile.transform.position.z;

            _pickedUpTile.transform.position = Vector3.Lerp(followPoint,
                _pickedUpTile.transform.position, 10 * Time.deltaTime);
        }
    }

    private RaycastHit2D RaycastFromMouse() { 
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }

    private void ReturnTileToLastSlot() {
        _lastSlot.SlotTile = _pickedUpTile;

        _pickedUpTile.transform.position = _lastSlot.transform.position;
        _pickedUpTile.GetComponent<Animator>().SetTrigger("Drop");

        SetSortingLayerRecur(_pickedUpTile.transform, "Default");

        _lastSlot = null;
        _pickedUpTile = null;
    }

    private void PickUpTile(GameBoardSlot slot) {
        _lastSlot = slot;

        _pickedUpTile = slot.SlotTile;
        _pickedUpTile.GetComponent<Animator>().SetTrigger("PickUp");

        SetSortingLayerRecur(_pickedUpTile.transform, "PickUp");

        slot.SlotTile = null;
    }

    private void DropTile(GameBoardSlot slot) {
        slot.SlotTile = _pickedUpTile;

        _pickedUpTile.transform.position = slot.transform.position;
        _pickedUpTile.GetComponent<Animator>().SetTrigger("Drop");

        SetSortingLayerRecur(_pickedUpTile.transform, "Default");

        _lastSlot = null;
        _pickedUpTile = null;
    }

    private void SetSortingLayerRecur(Transform obj, string layer) {
        if (obj != null) {
            var sRender = obj.GetComponent<Renderer>();
            if (sRender != null) {
                sRender.sortingLayerName = layer;
            }

            for (int i = 0; i < obj.childCount; i++) {
                SetSortingLayerRecur(obj.GetChild(i), layer);
            }
        }
    }
}
