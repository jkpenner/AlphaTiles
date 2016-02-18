using UnityEngine;
using System.Collections;

/// <summary>
/// Allows Sorting Layer variables to be shown on GameObject 
/// </summary>
public class SortingLayer : MonoBehaviour {
    public string SortingLayerName = "Default";
    public int SortingOrder = 0;

    public void Awake() {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }
}
