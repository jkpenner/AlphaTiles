using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Tile : MonoBehaviour, IPointerClickHandler, IDragHandler {
    public TextMesh textmesh;
    public AudioSource audioSourc;
    public AudioClip dropClip;
    public AudioClip pickUpClip;
    public AudioClip invalidDropClip;

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

    public void OnPickUp() {
        if (audioSourc != null) {
            audioSourc.PlayOneShot(pickUpClip);
        }
    }

    public void OnDrop() {
        if (audioSourc != null) {
            audioSourc.PlayOneShot(dropClip);
        }
    }

    public void OnInvalidDrop() {
        if (audioSourc != null) {
            audioSourc.PlayOneShot(invalidDropClip);
        }
    }
}
