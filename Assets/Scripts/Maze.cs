using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze {

    public byte[,] maze;
    enum Direction { Up, Right, Down, Left};
    Direction nextDirection;


    public Maze(byte width, byte height) {
        maze = new byte[width, height];

        InitiateMaze(width, height);
        GenerateMaze(width, height);
        Debug.Log("Actual Maze: ");
        PrintMaze(width, height, maze);
    }

    void InitiateMaze(byte width, byte height) {

        for (byte x = 0; x < width; x++) {
            for (byte y = 0; y < height; y++) {
                maze[x, y] = 1;
            }
        }
    }

    void GenerateMaze(byte width, byte height) {
        byte[,] availableCells = new byte[width, height]; //Keep's track of what has been used and what hasn't

        //Make sure edges are closed FIRST. If this isn't done first, there might be holes on the edges.
        for (byte x = 0; x < width; x++) {
            for (byte y = 0; y < height; y++) {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) {
                    availableCells[x, y] = 1;
                }
            }
        }

        //First attempt, random generates everypoint
        //NodeBasedPathRandomaizer(width, height, availableCells);

        //New version, should "carve" out paths.
        NewGenerator(width, height, availableCells);

        //Let's try the path method, start from one area and 
        //Start top left and end bottom right, leave trail, walls can over ride




        Debug.Log("Cheked Cells After: ");
        PrintMaze(width, height, availableCells);
    }

    void PrintMaze(byte width, byte height, byte[,] maze) {

        for (byte x = 0; x < width; x++) {
            string row = "";
            for (byte y = 0; y < height; y++) {
                row += maze[x, y];
            }
            Debug.Log(row);
        }
    }

    //WIP algorithm
    void NodeBasedPathRandomaizer(byte cellX, byte cellY, byte[,] cellCheck) {


        //Start Location
        for (byte x = 1; x < cellX - 1; x++) {
            for (byte y = 1; y < cellY - 1; y++) {

                if (cellCheck[x, y] == 0) {
                    //Make current Cell open
                    maze[x, y] = (byte)Random.Range(0, 2);
                    cellCheck[x, y] = 1;
                    //check surrounding walls
                    byte[] wallX = new byte[] { (byte)(x - 1), x, (byte)(x + 1), x };
                    byte[] wallY = new byte[] { y, (byte)(y - 1), y, (byte)(y + 1) };
                    //make one random cell open
                    bool doorOpened = false;

                    Debug.Log("x: " + x + "y; " + y);
                    //for (int i = 0; i < 10; i++) {
                    //    //while (doorOpened == false) {
                    //    byte openDoor = (byte)Random.Range(0, 4);
                    //    Debug.Log(openDoor);
                    //    if (cellCheck[wallY[openDoor], wallX[openDoor]] == 0) {
                    //        maze[wallY[openDoor], wallX[openDoor]] = 0;
                    //        cellCheck[wallY[openDoor], wallX[openDoor]] = 1;
                    //        doorOpened = true;
                    //    }
                    //    Debug.Log(doorOpened);
                    //    doorOpened = false;

                    //    //}
                    //}


                    for (byte i = 0; i < 4; i++) {
                        if (cellCheck[wallX[i], wallY[i]] == 0) {
                            maze[wallX[i], wallY[i]] = (byte)Random.Range(0, 2);
                            cellCheck[wallX[i], wallY[i]] = 1;
                        }
                    }
                }
            }
        }
    }

    void NewGenerator(byte maxX, byte cellY, byte[,] cellCheck) {

        for (byte x = 1; x < maxX - 1; x++) {
            for (byte y = 1; y < cellY - 1; y++) {
                if (cellCheck[x, y] == 0) {
                    //loop check debug
                    Debug.Log("begun a new path loop");

                    //initiate first cell
                    cellCheck[x, y] = 1;
                    maze[x, y] = 0;


                    bool creatingPath = true;
                    //after finding a suitable point, start carving out a path
                    //Keep track of where we are in this path
                    byte currentX = x; 
                    byte currentY = y;

                    //LOOP: Choose random direction, check if path is clear.
                    //If clear, open path and put walls up around last node
                    //If not clear, check one extra cell, if is open node, bridge it. if not, end cycle.

                    //NOTE: Make sure it can't move backwards!! **Store the "last move" var to not allow a move backwards**

                    bool[] lastDirection = new bool[4];

                    //while (creatingPath) {
                        //initial random
                        bool loop = true;



                        //Pick Direction that doesnt go backwards.
                        while (loop) {
                            nextDirection = (Direction)Random.Range(0, 4);
                            //check if it was 
                            if ((byte)nextDirection == 0) {
                                if (lastDirection[2] == false) {
                                    lastDirection[0] = true;
                                    loop = false;
                                    Debug.Log("We did it! The next direction is: " + nextDirection);
                                }
                            }
                            if ((byte)nextDirection == 1) {
                                if (lastDirection[3] == false) {
                                    lastDirection[1] = true;
                                    loop = false;
                                    Debug.Log("We did it! The next direction is: " + nextDirection);
                                }
                            }
                            if ((byte)nextDirection == 2) {
                                if (lastDirection[0] == false) {
                                    lastDirection[2] = true;
                                    loop = false;
                                    Debug.Log("We did it! The next direction is: " + nextDirection);
                                }
                            }
                            if ((byte)nextDirection == 3) {
                                if (lastDirection[1] == false) {
                                    lastDirection[3] = true;
                                    loop = false;
                                    Debug.Log("We did it! The next direction is: " + nextDirection);
                                }
                            }
                        }

                        Debug.Log("new step in the path");
                        Debug.Log("My path is: " + currentX + ", " + currentY);

                        switch (nextDirection) {
                            case Direction.Up:
                                //Check if cell above has been checked
                                if (cellCheck[currentX, currentY - 1] == 0) {
                                    //if cell up is avaliable, mark it and open it.
                                    cellCheck[currentX, currentY - 1] = 1;
                                    maze[currentX, currentY - 1] = 0;
                                    //wall up the previous possible paths
                                    if (cellCheck[currentX, currentY + 1] == 0) {    //Down
                                        cellCheck[currentX, currentY + 1] = 1;
                                        maze[currentX, currentY + 1] = 1;
                                    }
                                    if (cellCheck[currentX - 1, currentY] == 0) {   //Left
                                        cellCheck[currentX - 1, currentY] = 1;
                                        maze[currentX - 1, currentY] = 1;
                                    }
                                    if (cellCheck[currentX + 1, currentY] == 0) {   //Right
                                        cellCheck[currentX + 1, currentY] = 1;
                                        maze[currentX + 1, currentY] = 1;
                                    }
                                    //Make "up" node the current Node
                                    currentY -= 1;
                                } else if (currentY - 2 <= 0) {
                                    //if 2 nodes down is out of range, end loop
                                    loop = true;        //creatingPath = false;   
                                                        //Made an edit: Trying to rechoose dir if it leads out of bounds, instead of ending path
                                } else if (maze[currentX, currentY - 2] == 0) {
                                    //if theres an open space, instead, bridge them
                                    maze[currentX, currentY - 1] = 0;
                                    creatingPath = false;
                                }
                                Debug.Log("Went Up");
                                break;
                            case Direction.Right:
                                //Check if cell above has been checked
                                if (cellCheck[currentX + 1, currentY] == 0) {
                                    //if cell up is avaliable, mark it and open it.
                                    cellCheck[currentX + 1, currentY] = 1;
                                    maze[currentX + 1, currentY] = 0;
                                    //wall up the previous possible paths
                                    if (cellCheck[currentX, currentY + 1] == 0) {    //Down
                                        cellCheck[currentX, currentY + 1] = 1;
                                        maze[currentX, currentY + 1] = 1;
                                    }
                                    if (cellCheck[currentX - 1, currentY] == 0) {   //Left
                                        cellCheck[currentX - 1, currentY] = 1;
                                        maze[currentX - 1, currentY] = 1;
                                    }
                                    if (cellCheck[currentX, currentY - 1] == 0) {    //Up
                                        cellCheck[currentX, currentY - 1] = 1;
                                        maze[currentX, currentY + 1] = 1;
                                    }
                                    //Make "right" node the current Node
                                    currentX += 1;
                                } else if (currentX + 2 >= maxX) {
                                    //if 2 nodes down is out of range, end loop
                                    loop = true;        //creatingPath = false;
                                } else if (maze[currentX + 2, currentY] == 0) {
                                    //if theres an open space, instead, bridge them
                                    maze[currentX + 1, currentY] = 0;
                                    creatingPath = false;
                                }
                                Debug.Log("Took a Right");
                                break;
                            case Direction.Down:
                                //Check if cell above has been checked
                                if (cellCheck[currentX, currentY + 1] == 0) {
                                    //if cell up is avaliable, mark it and open it.
                                    cellCheck[currentX, currentY + 1] = 1;
                                    maze[currentX, currentY + 1] = 0;
                                    //wall up the previous possible paths
                                    if (cellCheck[currentX - 1, currentY] == 0) {   //Left
                                        cellCheck[currentX - 1, currentY] = 1;
                                        maze[currentX - 1, currentY] = 1;
                                    }
                                    if (cellCheck[currentX + 1, currentY] == 0) {   //Right
                                        cellCheck[currentX + 1, currentY] = 1;
                                        maze[currentX + 1, currentY] = 1;
                                    }
                                    if (cellCheck[currentX, currentY - 1] == 0) {    //Up
                                        cellCheck[currentX, currentY - 1] = 1;
                                        maze[currentX, currentY + 1] = 1;
                                    }
                                    //Make "down" node the current Node
                                    currentY += 1;
                                } else if (currentY + 2 >= cellY) {
                                    //if 2 nodes down is out of range, end loop
                                    loop = true;        //creatingPath = false;
                                } else if (maze[currentX, currentY + 2] == 0) {
                                    //if theres an open space, instead, bridge them
                                    maze[currentX, currentY + 1] = 0;
                                    creatingPath = false;
                                }
                                Debug.Log("Went Down");
                                break;
                            case Direction.Left:
                                //Check if cell above has been checked
                                if (cellCheck[currentX - 1, currentY] == 0) {
                                    //if cell up is avaliable, mark it and open it.
                                    cellCheck[currentX - 1, currentY] = 1;
                                    maze[currentX - 1, currentY] = 0;
                                    //wall up the previous possible paths
                                    if (cellCheck[currentX, currentY + 1] == 0) {    //Down
                                        cellCheck[currentX, currentY + 1] = 1;
                                        maze[currentX, currentY + 1] = 1;
                                    }
                                    if (cellCheck[currentX + 1, currentY] == 0) {   //Right
                                        cellCheck[currentX + 1, currentY] = 1;
                                        maze[currentX + 1, currentY] = 1;
                                    }
                                    if (cellCheck[currentX, currentY - 1] == 0) {    //Up
                                        cellCheck[currentX, currentY - 1] = 1;
                                        maze[currentX, currentY + 1] = 1;
                                    }
                                    //Make "left" node the current Node
                                    currentX -= 1;
                                } else if (currentX - 2 <= 0) {
                                    //if 2 nodes down is out of range, end loop
                                    loop = true;        //creatingPath = false;
                                } else if (maze[currentX - 2, currentY] == 0) {
                                    //if theres an open space, instead, bridge them
                                    maze[currentX - 1, currentY] = 0;
                                    creatingPath = false;
                                }
                                Debug.Log("Took a Left");
                                break;
                        }
                    //}
                    //How?
                    //Let's pick a random cell around them and see if it's open.
                    //if it is, move towards it, while making wals around it that are unchecked.
                    
                }
            }
        }
    }


}