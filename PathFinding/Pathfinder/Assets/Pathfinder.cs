using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinder : AbstractKinematic
{
    public Node start;
    public Node goal;
    Graph myGraph;

    PathFollow myMoveType;
    LookWhereGoing myRotateType;

    //public GameObject[] myPath = new GameObject[4];
    GameObject[] myPath;

    // Start is called before the first frame update
    void Start()
    {
        myRotateType = new LookWhereGoing();
        myRotateType.ai = this;
        myRotateType.target = target;

        Graph myGraph = new Graph();
        myGraph.Build();
        List<Connection> path = Dijkstra.pathfind(myGraph, start, goal);
        // path is a list of connections - convert this to gameobjects for the FollowPath steering behavior
        myPath = new GameObject[path.Count + 1];
        int i = 0;
        foreach (Connection c in path)
        {
            Debug.Log("from " + c.getFromNode() + " to " + c.getToNode() + " @" + c.getCost());
            myPath[i] = c.getFromNode().gameObject;
            i++;
        }
        myPath[i] = goal.gameObject;

        myMoveType = new PathFollow();
        myMoveType.ai = this;
        myMoveType.path = myPath;
        myMoveType.maxAcceleration = maxAcceleration;
        myMoveType.maxSpeed = maxSpeed;
    }

    // Update is called once per frame
    public override void Update()
    {
        mySteering = new SteeringOutput();
        if (myRotateType.GetSteering() != null)
        {
            mySteering.angular = myRotateType.GetSteering().angular;
        }
        mySteering.linear = myMoveType.GetSteering().linear;
        base.Update();
    }

}


// Easy fix to if closed node has shorter path found - if end path found is shorter than cost so far remove from open node
// if you have path and didnt come up with shortest save