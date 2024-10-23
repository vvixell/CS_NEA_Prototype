using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Djikstras
{
    public int[] FindPath(int[,] AdjacencyMatrix, int StartNode, int TargetNode)
    {
        Node[] Nodes = new Node[AdjacencyMatrix.GetLength(0)];
        for(int i = 0; i < Nodes.Length; i++) Nodes[i] = new Node(i);
        
        int[] NodeDistances = new int[AdjacencyMatrix.GetLength(0)];
        foreach(int distance in NodeDistances) distance = int.MaxValue;
        NodeDistances[StartNode] = 0;

        List<int> UnvisitedNodes = new List<int>();
        for(int i = 0; i < AdjacencyMatrix.GetLength(0); i++) UnvisitedNodes.Add(i);
       
        while(UnvisitedNodes.Count > 0)
        {
            int CurrentNode = FindClosestUnvisitedNode(NodeDistances, UnvisitedNodes);

            int[] UnvisitedNeighbours = GetAllUnvisitedNeighbours(AdjacencyMatrix, UnvisitedNodes, CurrentNode);

            for(int i = 0; i < UnvisitedNeighbours.Length; i++)
            {
                int Neighbour = UnvisitedNeighbours[i];
                int NewPathDistance = NodeDistances[CurrentNode] + AdjacencyMatrix[CurrentNode, Neighbour];
                if(NewPathDistance < NodeDistances[Neighbour]) 
                {
                    NodeDistances[Neighbour] = NewPathDistance;
                    Nodes[Neighbour].Parent = Nodes[CurrentNode];
                }
            }

            UnvistedNodes.Remove(CurrentNode);
        }

        List<int> ShortestPath = new List<int>();

        Node CurrentPathNode = Nodes[TargetNode];
        while(!CurrentPathNode.IsStartNode)
        {
            ShortestPath.Add(CurrentPathNode.Index);
            CurrentPathNode = CurrentPathNode.Parent;
        }
        ShortestPath.Add(StartNode);
        ShortestPath.Reverse();
        
        return ShortestPath.ToArray();
    }   

    int FindClosestUnvisitedNode(int[] NodeDistances, List<int> UnvisitedNodes)
    {
        int ClosestNode = UnvisitedNodes[0];
        for(int i = 0; i < NodeDistances.Length; i++)
        {
            if(!UnvisitedNodes.Contains(i)) continue;
               
            if(NodeDistances[i] < NodeDistances[ClosestNode]) ClosestNode = i;
        }
        return ClosestNode;
    }
    
    int[] GetAllUnvisitedNeighbours(int[,] AdjacencyMatrix, List<int> UnvisitedNodes, int CurrentNode)
    {
        List<int> Neighbours = new List<int>();
        int TotalNodes = AdjacencyMatrix.GetLength(0);
        
        for(int i = 0; i < TotalNodes; i++)
        {
            if(i == CurrentNode || !UnvisitedNodes.Contains(i)) continue;
            
            if(AdjacencyMatrix[CurrentNode, i] != -1) Neighbours.Add(i);
        }
        return Neighbours.ToArray();
    }

    public int[,] GetCavePathAdjacencyMatrix(Vector2 Caverns, int[,] AdjacencyMatrix, int MainCavern)
    {
        int[,] CaveAdjacencyMatrix = new int[Caverns.Length, Caverns.Length];

        for(int i = 0; i < Caverns.Length; i++)
        {
            if(MainCavern == i) continue;
            int[] Path = Djikstras.FindPath(AdjacencyMatrix, MainCavern, i);
            for(int j = 1; j < Path.Length; j++)
            {
                CaveAdjacencyMatrix[Path[i], Path[i-1]] = 1;
                CaveAdjacencyMatrix[Path[i-1], Path[i]] = 1;
            }
        }
    }
    
    struct Node
    {
        public int Index;
        public Node ParentNode;
        public bool IsStartNode {  get => ParentNode == null; }
        
        public Node(int Index)
        {
            this.Index = Index;
        }

        public bool IsStartNode()
        {
            Return
        }
    }
}
