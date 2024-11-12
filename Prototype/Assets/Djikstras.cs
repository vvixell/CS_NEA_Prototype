using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Djikstras
{
    public static int[] FindPath(int[,] AdjacencyMatrix, int StartNode, int TargetNode)
    {
        //Create New list of empty nodes
        Node[] Nodes = new Node[AdjacencyMatrix.GetLength(0)];
        for(int i = 0; i < Nodes.Length; i++) Nodes[i] = new Node(i);
        
        //Set all nodes distances to infinity
        int[] NodeDistances = new int[AdjacencyMatrix.GetLength(0)];
        for (int i = 0; i < NodeDistances.Length; i++)
            NodeDistances[i] = int.MaxValue;

        //Set the distance of the start node to 0
        NodeDistances[StartNode] = 0;

        //Add all nodes to unvisited node list
        List<int> UnvisitedNodes = new List<int>();
        for(int i = 0; i < Nodes.Length; i++) UnvisitedNodes.Add(i);
       
        while(UnvisitedNodes.Count > 0)
        {
            int CurrentNode = FindClosestUnvisitedNode(NodeDistances, UnvisitedNodes);

            int[] UnvisitedNeighbours = GetAllUnvisitedNeighbours(AdjacencyMatrix, UnvisitedNodes, CurrentNode);

            for (int i = 0; i < UnvisitedNeighbours.Length; i++)
            {
                int Neighbour = UnvisitedNeighbours[i];
                int NewPathDistance = NodeDistances[CurrentNode] + AdjacencyMatrix[CurrentNode, Neighbour];
                if (NewPathDistance < NodeDistances[Neighbour]) 
                {
                    NodeDistances[Neighbour] = NewPathDistance;
                    Nodes[Neighbour].ParentNode = Nodes[CurrentNode];
                }
            }

            UnvisitedNodes.Remove(CurrentNode);
        }

        List<int> ShortestPath = new List<int>();

        Node CurrentPathNode = Nodes[TargetNode];
        while(!CurrentPathNode.IsStartNode)
        {
            ShortestPath.Add(CurrentPathNode.Index);
            CurrentPathNode = CurrentPathNode.ParentNode;
        }
        ShortestPath.Add(StartNode);
        ShortestPath.Reverse();
        
        return ShortestPath.ToArray();
    }   

    static int FindClosestUnvisitedNode(int[] NodeDistances, List<int> UnvisitedNodes)
    {
        int ClosestNode = UnvisitedNodes[0];
        for(int i = 0; i < NodeDistances.Length; i++)
        {
            if (UnvisitedNodes.Contains(i)) 
            { 
                if (NodeDistances[i] < NodeDistances[ClosestNode]) ClosestNode = i; 
            }
        }
        return ClosestNode;
    }

    static int[] GetAllUnvisitedNeighbours(int[,] AdjacencyMatrix, List<int> UnvisitedNodes, int CurrentNode)
    {
        List<int> Neighbours = new List<int>();
        int TotalNodes = AdjacencyMatrix.GetLength(0);
        
        for(int i = 0; i < TotalNodes; i++)
        {
            if (i != CurrentNode && UnvisitedNodes.Contains(i))
            {
                if (AdjacencyMatrix[CurrentNode, i] != 0) Neighbours.Add(i);
            }
        }
        return Neighbours.ToArray();
    }

    public static int[,] GetCavePathAdjacencyMatrix(Vector2[] Points, int[] Caverns, int[,] AdjacencyMatrix, int MainCavern)
    {
        int[,] CaveAdjacencyMatrix = new int[Points.Length, Points.Length];
        for(int i = 0; i < Caverns.Length; i++)
        {
            if(MainCavern == i) continue;
            int[] Path = Djikstras.FindPath(AdjacencyMatrix, MainCavern, Caverns[i]);
            for(int j = 1; j < Path.Length; j++)
            {
                CaveAdjacencyMatrix[Path[j], Path[j-1]] = 1;
                CaveAdjacencyMatrix[Path[j-1], Path[j]] = 1;
            }
        }

        return CaveAdjacencyMatrix;
    }
}

public class Node
{
    public int Index;
    public Node ParentNode;

    public Node(int Index)
    {
        this.Index = Index;
    }

    public bool IsStartNode { get => ParentNode == null; }
}