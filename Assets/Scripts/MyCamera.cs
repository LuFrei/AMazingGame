using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour {

    private Camera cam;
    public GenerateMaze maze;


    private float currentSize;
    private float xMax;
    private float yMax;
    private float maxSize;
    public float panSpeed;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        xMax = maze.width;
        yMax = maze.height;
        transform.position = new Vector3((xMax / 2) - 0.5f, (yMax / 2) - 0.5f, -10);
        maxSize = (maze.height / 2);
        currentSize = maxSize;
    }
	
	// Update is called once per frame
	void Update () {
        var pos = transform.position;
        maxSize = (maze.height / 2);
        xMax = maze.width;
        yMax = maze.height;

        //cam.orthographicSize = currentSize;

        //Camera Controls
        //Zoom in
        if (Input.GetKey(KeyCode.KeypadMinus)) {
            currentSize -= 4 * Time.deltaTime;
        }
        //Zoom out
        if (Input.GetKey(KeyCode.KeypadPlus)) {
            currentSize += 4 * Time.deltaTime;
        }
        //Reset camera position
        if (Input.GetKey(KeyCode.KeypadMultiply)) {
            currentSize = maxSize;
            transform.position = new Vector3((xMax / 2) - 0.5f, (yMax / 2) - 0.5f, -10);
        }

        //Move camera with Arrows
        if (Input.GetKey(KeyCode.Keypad6)) {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad4)) {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad8)) {
            transform.Translate(Vector3.up * panSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad2)) {
            transform.Translate(Vector3.down * panSpeed * Time.deltaTime);
        }

        //Set parameters
        //Speed limit
        if (panSpeed < 5)
            panSpeed = 5;
        if (panSpeed > 20)
            panSpeed = 20;
        //Camera size limit
        if (currentSize > maxSize)
            currentSize = maxSize;
        if (currentSize < 3)
            currentSize = 3;
        //Camera position limit
        if (transform.position.x < 0) {
            pos.x = 0;
            transform.position = pos;
        }
        if (transform.position.x > xMax) {
            pos.x = xMax;
            transform.position = pos;
        }
        if (transform.position.y < 0) {
            pos.y = 0;
            transform.position = pos;
        }
        if (transform.position.y > yMax) {
            pos.y = yMax;
            transform.position = pos;
        }
        

        cam.orthographicSize = currentSize;
    }
}

