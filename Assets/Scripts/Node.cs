using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code retrieved from https://web.archive.org/web/20170505034417/http://blog.two-cats.com/2014/06/a-star-example

public class Node {
    public Vector2 location;
    public bool isWalkable;
    public float g;
    public float h;
    public float f;
    public NodeState state;
    public Node parent;

    public Node(Vector2 location, Node parent, Vector2 startLocation, Vector2 endLocation) {
        this.location = location;
        this.parent = parent;
        this.g = Mathf.Abs((location.x - startLocation.x) + (location.y - startLocation.y));
        this.h = Vector2.Distance(location, endLocation);
        f = g + h;
    }

    public float CompareTo(Node obj) {
        return this.f.CompareTo(obj.f);
    }
}

