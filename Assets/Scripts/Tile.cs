using UnityEngine;

public class Tile : MonoBehaviour {
    private Animator animator;

    public TextMesh textmesh;

    public AudioSource audioSourc;
    public AudioClip dropClip;
    public AudioClip pickUpClip;
    public AudioClip invalidDropClip;

    private bool _isInvalidDrop = false;

    public void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetText(string value) {
        if (textmesh != null) {
            textmesh.text = value;
        }
    }

    public void PickUp() {
        transform.SetParent(null);
        animator.SetTrigger("PickUp");
    }

    public void Drop(Transform parent) {
        Internal_Drop(parent, false);
    }

    public void DropInvalid(Transform parent) {
        Internal_Drop(parent, true);
    }

    private void Internal_Drop(Transform parent, bool invalid) {
        _isInvalidDrop = invalid;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        animator.SetTrigger("Drop");
    }

    // Called via animation
    public void OnPickUpStart() {
        if (audioSourc != null) {
            audioSourc.PlayOneShot(pickUpClip);
        }
        RendererUtility.SetSortingLayerRecursive(transform, "PickUp");
    }

    // Called via animation
    public void OnPickUpComplete() {

    }

    // Called via animation
    public void OnDropStart() {
        if (audioSourc != null) {
            if (_isInvalidDrop) {
                audioSourc.PlayOneShot(invalidDropClip);
            } else {
                audioSourc.PlayOneShot(dropClip);
            }
        }
    }

    // Called via animation
    public void OnDropComplete() {
        RendererUtility.SetSortingLayerRecursive(transform, "Default");
    }
}
