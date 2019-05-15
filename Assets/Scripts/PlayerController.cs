using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum Direction { Null, Up, Right, Down, Left};
    Direction intendedDirection;

    //using maze as the grid system.
    GenerateMaze maze;

    public byte currentX;
    public byte currentY;

    bool readyToMove;
    float moveTimer;

    // Use this for initialization
    void Start () {
        maze = GameObject.FindGameObjectWithTag("GameController").GetComponent<GenerateMaze>();
        currentX = 1;
        currentY = 1;
	}
	
	// Update is called once per frame
	void Update () {
        GetDirection();
        if (readyToMove) {
            Move();
        } else {
            Wait();
        }
        DisplayNewPosition();
    }

    void GetDirection() {
        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            intendedDirection = Direction.Null;
        }

        if (Input.GetAxis("Horizontal") > 0) {
            intendedDirection = Direction.Right;
        } else if (Input.GetAxis("Horizontal") < 0) {
            intendedDirection = Direction.Left;
        }
        if (Input.GetAxis("Vertical") > 0) {
            intendedDirection = Direction.Up;
        } else if (Input.GetAxis("Vertical") < 0) {
            intendedDirection = Direction.Down;
        }
    }

    void Move() {
        //We want to check if there's a space open before we move
        switch (intendedDirection) {
            case Direction.Right:
                if(maze.maze.maze[currentX + 1, currentY] == 0) {
                    currentX++;
                    readyToMove = false;
                }
                break;
            case Direction.Left:
                if (maze.maze.maze[currentX - 1, currentY] == 0) {
                    currentX--;
                    readyToMove = false;
                }
                break;
            case Direction.Up:
                if (maze.maze.maze[currentX, currentY + 1] == 0) {
                    currentY++;
                    readyToMove = false;
                }
                break;
            case Direction.Down:
                if (maze.maze.maze[currentX, currentY - 1] == 0) {
                    currentY--;
                    readyToMove = false;
                }
                break;
        }
    }

    void Wait() {
        moveTimer -= Time.deltaTime;

        if(moveTimer <= 0) {
            readyToMove = true;
            moveTimer = 0.2f;
        }
    }

    void DisplayNewPosition() {
        transform.position = new Vector3(currentX, currentY, 0);
    }
}
