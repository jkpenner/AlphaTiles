using UnityEngine;
using System.Collections;

public class FollowRotation : MonoBehaviour {
    public GameObject target;
	
	void Update () {
        transform.rotation = target.transform.rotation;
	}
}
