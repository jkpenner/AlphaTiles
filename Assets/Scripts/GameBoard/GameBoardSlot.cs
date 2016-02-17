using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GameBoardSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
    private Tile _slotTile;
    public Tile SlotTile {
        get { return _slotTile; }
        set { _slotTile = value; }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if(eventData.selectedObject != null)
        Debug.Log("OnPointerUp: " + eventData.selectedObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.selectedObject != null)
            Debug.Log("OnPointerEnter: " + eventData.selectedObject.name);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.selectedObject != null)
            Debug.Log("OnPointerDown: " + eventData);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.selectedObject != null)
            Debug.Log("OnPointerExit: " + eventData.selectedObject.name);
    }
}