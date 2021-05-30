using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathfinding : MonoBehaviour
{
    [SerializeField] private CellGrid grid;

    [SerializeField] private Vector2 StartCell;
    [SerializeField] private Vector2 EndCell;

    private LineRenderer lineRenderer;

    private void Start() => lineRenderer = GetComponent<LineRenderer>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            StartCell = new Vector2(0, 1);
            EndCell = new Vector2(grid.Width - 1, grid.Height - 2);

            DateTime before = DateTime.Now;
            Find();
            DateTime after = DateTime.Now;
            TimeSpan duration = after.Subtract(before);
            Debug.Log($"Duration in milliseconds: {duration} difficulty: {lineRenderer.positionCount}");
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            lineRenderer.enabled = !lineRenderer.enabled;
        }
    }

    private void Find()
    {
        List<Cell> openList = new List<Cell>();
        List<Cell> closedList = new List<Cell>();

        Cell startingCell = grid.cellArray[(int)StartCell.x, (int)StartCell.y];
        openList.Add(startingCell);

        while (true)
        {
            Cell currentCell = openList[0];
            currentCell.FCost = currentCell.gCost + currentCell.hCost;

            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].FCost > currentCell.FCost)
                {
                    if (openList[i].FCost < currentCell.FCost || openList[i].FCost == currentCell.FCost)
                    {
                        currentCell = openList[i];
                    }
                }
            }

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            if (currentCell.position == EndCell)
            {
                GetPath();
                break;
            }

            foreach (var neighbour in currentCell.neighbours)
            {
                if (!neighbour.IsWalkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                var newCostToNeighbour = currentCell.gCost + 1;

                if(neighbour != startingCell)
                {
                    neighbour.gCost = currentCell.gCost + 1;
                }

                if (newCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.CalculateHCost(EndCell);
                    neighbour.FCost = neighbour.gCost + neighbour.hCost;
                    neighbour.parentCell = currentCell;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
    }

    private void GetPath()
    {
        Cell startingCell = grid.cellArray[(int)StartCell.x, (int)StartCell.y];
        Cell endCell = grid.cellArray[grid.Width - 1, grid.Height - 2];

        List<Cell> path = new List<Cell>();
        Cell currentCell = endCell;

        while (currentCell != startingCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.parentCell;
        }
        path.Add(currentCell);

        lineRenderer.positionCount = path.Count;
        Cell[] pathArray = path.ToArray();

        for (int i = 0; i < pathArray.Length; i++)
        {
            lineRenderer.SetPosition(i ,pathArray[i].position);
        }
    }
}
