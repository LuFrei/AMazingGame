using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState { Untested, Open, Closed }

public class AStarEnemy : MonoBehaviour {



    //public Transform target;

    //Maze map;

    //Vector2 start;
    //Vector2 end;

    //List<Vector2> open;
    //List<Vector2> closed;

    //private void Start() {
    //    //get map info
    //    map = GameObject.FindGameObjectWithTag("GameController").GetComponent<GenerateMaze>().maze;
    //}

    ////first thing is first, we need to find out where we are on the map, and where we need to go
    //void FindStartAndEnd() {
    //    //we are using trans pos since we are using the 2d array coords with the in-game coords
    //    start = gameObject.transform.position;
    //    end = target.position;
    //}

    ////Now we must find the surounding cells to later calculate F
    //void FindAndNeighboringCells() {
    //    //I'm brain dead from Thanksgiving so forgive me for this extremely simple and drawn out and TERRIBLE code

    //}

    Vector2 startLocation;
    Vector2 endLocation;
    float[,] heureticsMap;
    public byte[,] map;                                                              

    GenerateMaze maze;
    public Transform target;
    List<Vector2> path;

    float pathRefreshTime;

    float moveTimer;

    int currentStep = 0;

    // Use this for initialization
    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        maze = GameObject.FindGameObjectWithTag("GameController").GetComponent<GenerateMaze>();
        map = maze.maze.maze;

        pathRefreshTime = 0;
        
    }

    private void Update() {

        pathRefreshTime -= Time.deltaTime;
        moveTimer -= Time.deltaTime;

        //Check for path every 3 seconds
        if (pathRefreshTime <= 0) {
            startLocation = gameObject.transform.position;
            endLocation = target.position;
            path = CreatePath(startLocation, endLocation);
            DebugPath();
            pathRefreshTime = 3;
            currentStep = 0;
        }

        if (moveTimer <= 0) {
            StartCoroutine(Move(path));
            moveTimer = 1;
        }
    }

    void DebugPath() {
        for(int i = 0; i < path.Count - 1; i++) {
            Debug.DrawLine(new Vector3(path[i].x, path[i].y, 0), new Vector3(path[i++].x, path[i++].y, 0), Color.cyan, 3.0f, false);
        }
    }

    IEnumerator Move(List<Vector2> path) {
        transform.position = path[currentStep];

        if (currentStep < path.Count) {
            currentStep++;
        }
        yield return null;
        
    }

    List<Vector2> CreatePath(Vector2 start, Vector2 end) {

        List<Vector2> path = new List<Vector2>();
        //set up start node, and make it open
        Node startNode = new Node(start, null, start, end);
        startNode.state = NodeState.Open;



        //Loop should return EndNode
        Node node = Loop(startNode);

        //this part should play AFTER we find the path
        //We should loop 'add parent' until parent is null

        while (node.parent != null) {
            //add this node to the list
            path.Add(node.location);
            //go to the next node
            node = node.parent;
        }

        
        path.Reverse();

        //we should then feed this path to the AI during runtime.
        return path;
            
    }

    Node Loop(Node currentNode) {
        //store Nodes that we have found
        List<Node> foundNodes = new List<Node>();

        //store first node in found nodes

        foundNodes.Add(currentNode);

        int nodeIteration = 0;
        int loopIteration = 0;

        bool crashCheck = false;
        float crashTimer = 0;

        bool finalFound = false;
        //While the current node isn't the goal, we will keep searching for a new node
        while (!finalFound) {
            //for (int i = 0; i < 3; i++) {
            //close current node
            currentNode.state = NodeState.Closed;

            //Get all surounding viable nodes and add them to the found list
            foundNodes.AddRange(GetAdjacentWalkableNodes(currentNode));

            //Now we must sort Nodes in open list by F value
            foundNodes.Sort((node1, node2) => node1.f.CompareTo(node2.f));

            //sort by H value
            foundNodes.Sort((node1, node2) => node1.h.CompareTo(node2.h));

            //We make the node with the lowest F cost the new current node
            for (int j = 0; j < foundNodes.Count; j++) {
                //go through list and pick out next open, lowest F cost node
                if (foundNodes[j].state == NodeState.Open) {
                    currentNode = foundNodes[j];
                    break;
                }
            }

            if(currentNode.location == endLocation) {
                finalFound = true;
            }

            //~~~~~~
            //For testing purpouses I wanna print out all found nodes and their states for 2 loops.
            loopIteration++;
            Debug.Log("Loop #" + loopIteration + ": ");

            nodeIteration = 0;
            foreach (Node node in foundNodes) {
                nodeIteration++;
                Debug.Log("Node " + nodeIteration + ": " + node.location + " G: " + node.g + " H: " + node.h + " F: " + node.f + " State: " + node.state);
            }

            //~~~~~~ 

            crashTimer += Time.deltaTime;
            if (crashTimer >= 3) {
                Debug.Log("Programmed looped for " + 3 + " seconds, and broke out of it.");
                crashCheck = true;
            }

        }

            Debug.Log("end Node found: " + currentNode.location + " G: " + currentNode.g + " H: " + currentNode.h + " F: " + currentNode.f + " State: " + currentNode.state);

            //once we find the target node, we return it
            return currentNode;
    }

    #region CarlosNotes01
    //Works on arrays AND lists (and any other things that implement IEnumerable interface!)
    //public void Test(IEnumerable<Vector3> data) {

    //}
    #endregion
    //You can keep returning new values to add to the "IEnumerable" thing you can iterate over
    public IEnumerable<Vector2> GetAdjacentLocations(Vector2 location) {
        yield return location + new Vector2(-1, 0);
        yield return location + new Vector2(1, 0);
        yield return location + new Vector2(0, -1);
        yield return location + new Vector2(0, 1);
    }

    //THIS IS CHECKED AND MAKES SENSE AS FAR AS I KNOW
    private List<Node> GetAdjacentWalkableNodes(Node fromNode) {

        #region CarlosNotes02
        //Vector3[] someTestArray = new Vector3[] { /*...*/ };
        //Test(someTestArray);
        //List<Vector3> someTestList = new List<Vector3>(new Vector3[] { /*...*/ });
        //Test(someTestList);

        //ArrayList<String> listOfStrings = new ArrayList<String>();
        //listOfStrings.get(0); //array[0]

        //someTestList[0] = new Vector3(0, 0, 0);
        #endregion

        //make an empty local list of walkable nodes
        List<Node> walkableNodes = new List<Node>();

        //fills list with sorrounding nodes                                                                                                         
        IEnumerable<Vector2> nextLocations = GetAdjacentLocations(fromNode.location);

        #region CarlosNotes03
        //<Player>
        //  <Health>76/100</Health>
        //  <Money>5000</Money>
        //  <Car>Tesla</Car>
        //</Player>
        //I personally like using System.Xml.Linq, with XDocuments and XElements -- these use IEnumerables/IEnumerators too :)
        //IEnumerator<Vector2> enumerator = nextLocations.GetEnumerator();
        //while (enumerator.MoveNext()) {
        //    //...
        //    enumerator.Current
        //}

        //oh, okay
        #endregion

        foreach (Vector2 location in nextLocations) {
            float x = location.x;
            float y = location.y;

            // Stay within the grid's boundaries                                                                             
            if (x < 1 || x >= maze.width || y < 0 || y >= maze.height)
                continue;

            Node node = new Node(location, fromNode, startLocation, endLocation );

            //check if its open
            if (map[(int)node.location.x, (int)node.location.y] == 1)
                continue;

            // Ignore already- nodes
            if (node.state == NodeState.Closed || node.state == NodeState.Open)
                continue;

            // Already-open nodes are only added to the list if their G-value is lower going via this route.
            //if (node.state == NodeState.Open) {
            //    float traversalCost = Node.GetTraversalCost(node.location, node.parent.location);
            //    float gTemp = fromNode.g + traversalCost;
            //    if (gTemp < node.g) {
            //        node.parent = fromNode;
            //        walkableNodes.Add(node);
            //    }
            //} else {
            // If it's untested, set the parent and flag it as 'Open' for consideration

            //Only ading parent if parent is null, This might be a cause for an infinite loop
            
            node.parent = fromNode;
            node.state = NodeState.Open;
            walkableNodes.Add(node);
            //}
        }

        return walkableNodes;
    }

    ////Code retrieved from https://web.archive.org/web/20170505034417/http://blog.two-cats.com/2014/06/a-star-example
    ////with some editing by: Lucas Freitas
    ////and Carlos

    //public List<Vector2> FindPath() {
    //    List<Vector2> path = new List<Vector2>();
    //    Node startNode = new Node(startLocation, null, startLocation, endLocation);
    //    bool success = Search(startNode);
    //    if (success) {
    //        Node node =  ;
    //        while (node.parent != null) {
    //            path.Add(node.location);
    //            node = node.parent;
    //        }
    //        path.Reverse();
    //    }
    //    return path;
    //}

    ////look for surounding nodes

    //private Node Search(Node currentNode) {
    //    //Make this node closed
    //    currentNode.state = NodeState.Closed;
    //    //Get a list of surounding nodes

    //    //sort Nodes based on F


    //    //If next lowest  isn't endNode, look for next lowest F
    //    foreach(Node nextNode in nextNodes) {
    //        if(nextNode.location == endLocation) {
    //            return nextNode;
    //        } else {

    //        }
    //    }


    //    foreach (var nextNode in nextNodes) {
    //        if (Search(nextNode) || ) // Note: Recurses back into Search(Node)
    //            return true;
    //    }
    //    return false;
    //}

    //private class PrefabCollection {
    //    private GameObject[] prefabs;

    //    public GameObject this[int index] {
    //        get { return prefabs[index]; }
    //    }
    //}
}