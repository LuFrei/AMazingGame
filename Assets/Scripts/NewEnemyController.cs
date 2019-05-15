using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyController : MonoBehaviour {


    enum Direction { Null, Up, Right, Down, Left };
    Direction intendedDirection;

    //using maze as the grid system.
    GenerateMaze maze;

    public byte currentX;
    public byte currentY;

    public bool readyToMove = true;
    public float moveTimer;

    // Use this for initialization
    void Start() {
        maze = GameObject.FindGameObjectWithTag("GameController").GetComponent<GenerateMaze>();
        currentX = (byte)transform.position.x;
        currentY = (byte)transform.position.y;
        GetDirection();
    }

    // Update is called once per frame
    void Update() {
        if (readyToMove) {
            Move();
        } else {
            Wait();
        }
        DisplayNewPosition();
    }

    void GetDirection() {
        intendedDirection = (Direction)Random.Range(1, 5);
        Debug.Log("IMMA GO " + intendedDirection + " NOW");
    }

    void Move() {
        //We want to check if there's a space open before we move
        switch (intendedDirection) {
            case Direction.Right:
                if (maze.maze.maze[currentX + 1, currentY] == 0) {
                    currentX++;
                    readyToMove = false;
                    
                } else if (maze.maze.maze[currentX + 1, currentY] == 1) {
                    GetDirection();
                }
                break;
            case Direction.Left:
                if (maze.maze.maze[currentX - 1, currentY] == 0) {
                    currentX--;
                    readyToMove = false;
                } else if (maze.maze.maze[currentX - 1, currentY] == 1) {
                    GetDirection();
                }
                break;
            case Direction.Up:
                if (maze.maze.maze[currentX, currentY + 1] == 0) {
                    currentY++;
                    readyToMove = false;
                } else if (maze.maze.maze[currentX, currentY + 1] == 1) {
                    GetDirection();
                }
                break;
            case Direction.Down:
                if (maze.maze.maze[currentX, currentY - 1] == 0) {
                    currentY--;
                    readyToMove = false;
                } else if (maze.maze.maze[currentX, currentY - 1] == 1) {
                    GetDirection();
                }
                break;
        }
    }

    void Wait() {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0) {
            readyToMove = true;
            moveTimer = 0.07f;
        }
    }

    void DisplayNewPosition() {
        transform.position = new Vector3(currentX, currentY, 0);
    }
}
