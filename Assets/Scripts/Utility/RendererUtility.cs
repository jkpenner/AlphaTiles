using UnityEngine;
using System.Collections;

static public class RendererUtility {
    static public void SetSortingLayerRecursive(Transform obj, string layer) {
        if (obj != null) {
            var sRender = obj.GetComponent<Renderer>();
            if (sRender != null) {
                sRender.sortingLayerName = layer;
            }

            for (int i = 0; i < obj.childCount; i++) {
                SetSortingLayerRecursive(obj.GetChild(i), layer);
            }
        }
    }
}
