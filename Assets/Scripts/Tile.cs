using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Tile : MonoBehaviour, IPointerClickHandler, IDragHandler {
    public TextMesh textmesh;

    public void OnDrag(PointerEventData eventData) {
        throw new NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData) {
        throw new NotImplementedException();
    }

    public void SetText(string value) {
        if (textmesh != null) {
            textmesh.text = value;
        }
    }
}
