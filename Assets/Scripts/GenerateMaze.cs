using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class GenerateMaze : MonoBehaviour {

    public Maze maze;
    public GameObject wall;
    List<GameObject> generatedObjects = new List<GameObject>();

    public byte width = 16;
    public byte height = 16;

    public InputField widthField, heightField;
    public GameObject warningMessage;

    public GameObject enemy;
    public GameObject enemy2;
    public GameObject enemy3;
    int enemyCount = 0;

    // Use this for initialization
    void Start() {
        maze = new Maze(width, height);
        widthField.text = width.ToString();
        heightField.text = height.ToString();
        RenderMaze();
    }

    private void Update() {

        //Set actual Width and Height to Slider/TextField values               
        

        if (Input.GetKeyDown(KeyCode.F1)) {
            if (Int32.Parse(widthField.text) <= 255 && Int32.Parse(heightField.text) <= 255 && Int32.Parse(widthField.text) >= 0 && Int32.Parse(heightField.text) >= 0) {
                Debug.Log("A byte");
                warningMessage.SetActive(false);
                width = byte.Parse(widthField.text);
                height = byte.Parse(heightField.text);
                DestroyMaze();
                maze = new Maze(width, height);
                RenderMaze();
                foreach (GameObject enemies in GameObject.FindGameObjectsWithTag("Enemy")) {
                    Destroy(enemies);
                }
                enemyCount = 0;
            }
            if (Int32.Parse(widthField.text) > 255 || Int32.Parse(heightField.text) > 255 || Int32.Parse(widthField.text) < 0 || Int32.Parse(heightField.text) < 0) {
                Debug.Log("Not a Byte");
                warningMessage.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && enemyCount < 3) {
            Instantiate(enemy, new Vector3(UnityEngine.Random.Range(5, 21), UnityEngine.Random.Range(5, 21), 0), Quaternion.identity);
            enemyCount++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && enemyCount < 3) {
            Instantiate(enemy2, new Vector3(UnityEngine.Random.Range(5, 21), UnityEngine.Random.Range(5, 21), 0), Quaternion.identity);
            enemyCount++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && enemyCount < 3) {
            Instantiate(enemy3, new Vector3(UnityEngine.Random.Range(5, 21), UnityEngine.Random.Range(5, 21), 0), Quaternion.identity);
            enemyCount++;
        }
    }

    void RenderMaze() {

        for (byte x = 0; x < width; x++) {
            for (byte y = 0; y < height; y++) {
                if (maze.maze[x, y] == 1) {
                    GameObject obj = Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity);
                    generatedObjects.Add(obj);
                }
            }
        }
    }

    void DestroyMaze() {
        foreach(GameObject obj in generatedObjects) {
            Destroy(obj);
        }
    }
}