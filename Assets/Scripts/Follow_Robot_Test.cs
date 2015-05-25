using UnityEngine;
using System.Collections;

public class Follow_Robot_Test : MonoBehaviour {
    public Transform target;

    public float xOffset;
    public float yOffset;
    public float zOffset;

    public float height;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(target == null){
            return;
        }

        //transform.LookAt(target);
        transform.position = new Vector3(target.position.x + xOffset, target.position.y + yOffset, target.position.z - zOffset);
        transform.rotation = target.rotation;
	}
}
