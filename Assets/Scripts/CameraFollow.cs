﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    Camera cam;

    Vector3 camOffset = new Vector3(0,0,-5);

	// Use this for initialization
	void Start () {
        
        cam = GetComponent<Camera>();
		
	}
	
	// Update is called once per frame
	void Update () {
        // cam.orthographicSize = 1 -> in screen height are 2 Unity units
        // cam.orthographicSize = 2 -> in screen height are 4 Unity units
        // size = (screen.height / (2*pixelsPerUnit)) / scaleFactor
        // 1 = (64 / (2*32)) / 1
        // 2 = (128 / 64) / 1
        cam.orthographicSize = (Screen.height / 64f) / 2f;
        if(target) {
            transform.position = Vector3.Lerp(transform.position, target.position+camOffset, 0.1f);
        }
	}
}
