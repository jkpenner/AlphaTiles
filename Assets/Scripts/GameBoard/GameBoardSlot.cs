using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GameBoardSlot : MonoBehaviour {
    private Tile _slotTile;
    public Tile SlotTile {
        get { return _slotTile; }
        set {
            if (_slotTile != value) {
                _slotTile = value;
                if (OnSlotTileChange != null) {
                    OnSlotTileChange(this);
                }
            }
        }
    }

    public delegate void SlotTileChangeEvent(GameBoardSlot slot);
    private event SlotTileChangeEvent OnSlotTileChange;

    public void AddListener(SlotTileChangeEvent del) {
        OnSlotTileChange += del;
    }

    public void RemoveListener(SlotTileChangeEvent del) {
        OnSlotTileChange -= del;
    }
}