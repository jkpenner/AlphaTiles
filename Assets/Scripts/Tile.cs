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
        Drop(parent, false);
    }

    public void Drop(Transform parent, bool invalid) {
        _isInvalidDrop = invalid;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        animator.SetTrigger("Drop");
    }

    public void FlipDown() {
        animator.SetTrigger("FlipDown");
    }

    public void FlipUp() {
        animator.SetTrigger("FlipUp");
    }

    #region Animation Events
    public void OnPickUpStart() {
        if (audioSourc != null) {
            audioSourc.PlayOneShot(pickUpClip);
        }
        RendererUtility.SetSortingLayerRecursive(transform, "PickUp");
    }

    public void OnPickUpComplete() {

    }

    public void OnDropStart() {
        if (audioSourc != null) {
            if (_isInvalidDrop) {
                audioSourc.PlayOneShot(invalidDropClip);
            } else {
                audioSourc.PlayOneShot(dropClip);
            }
        }
    }

    public void OnDropComplete() {
        RendererUtility.SetSortingLayerRecursive(transform, "Default");
    }

    public void OnFlipDownStart() {
        if (audioSourc != null) {
            audioSourc.PlayOneShot(pickUpClip);
        }
    }

    public void OnFlipDownComplete() {
        textmesh.gameObject.SetActive(false);
    }

    public void OnFlipUpStart() {
        if (audioSourc != null) {
            audioSourc.PlayOneShot(pickUpClip);
        }
    }

    public void OnFlipUpComplete() {
        textmesh.gameObject.SetActive(true);
    }
    #endregion
}
