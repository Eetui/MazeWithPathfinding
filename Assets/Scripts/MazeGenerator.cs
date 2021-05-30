using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private CellGrid grid;
    private List<Cell> stack = new List<Cell>();

    private int visitedCellCount;
    private int cells;

    int currentCellNeighbourCount = 0;

    private void Start() => cells = grid.mazeCells;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(CreateMazeCo());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateMaze();
        }
    }

    IEnumerator CreateMazeCo()
    {
        var currentCell = grid.cellArray[1, 1];
        currentCell.IsVisitedMaze = true;
        stack.Add(currentCell);
        visitedCellCount++;

        while(visitedCellCount < cells)
        {
            currentCell.TopOfStack();
            currentCellNeighbourCount = currentCell.mazeNeighbours.Count;
            var nextCell = currentCell.mazeNeighbours[Random.Range(0, currentCellNeighbourCount)];

            if(nextCell.IsVisitedMaze)
            {
                for (int i = 0; i < currentCellNeighbourCount; i++)
                {
                    nextCell = currentCell.mazeNeighbours[i];
                    if (!nextCell.IsVisitedMaze)
                    {
                        break;
                    }
                }

                if(nextCell.IsVisitedMaze)
                {
                    currentCell.SetColor();
                    stack.RemoveAt(stack.Count - 1);
                    currentCell = stack[stack.Count - 1];
                    currentCell.TopOfStack();
                }
            }

            if (!nextCell.IsVisitedMaze)
            {
                RemoveWall(currentCell, nextCell);

                currentCell.SetColor();
                currentCell = nextCell;
                currentCell.TopOfStack();
                currentCell.IsVisitedMaze = true;
                stack.Add(currentCell);
                visitedCellCount++;
            }

            yield return new WaitForSeconds(.01f);
        }
        currentCell.SetColor();
        grid.cellArray[0, 1].SetCell(true);
        grid.cellArray[grid.Width - 1, grid.Height - 2].SetCell(true);
    }

    private void CreateMaze()
    {
        var currentCell = grid.cellArray[1, 1];
        currentCell.IsVisitedMaze = true;
        stack.Add(currentCell);
        visitedCellCount++;

        while (visitedCellCount < cells)
        {
            currentCellNeighbourCount = currentCell.mazeNeighbours.Count;
            var nextCell = currentCell.mazeNeighbours[Random.Range(0, currentCellNeighbourCount)];

            if (nextCell.IsVisitedMaze)
            {
                for (int i = 0; i < currentCellNeighbourCount; i++)
                {
                    nextCell = currentCell.mazeNeighbours[i];
                    if (!nextCell.IsVisitedMaze)
                    {
                        break;
                    }
                }

                if (nextCell.IsVisitedMaze)
                {
                    stack.RemoveAt(stack.Count - 1);
                    currentCell = stack[stack.Count - 1];
                }
            }

            if (!nextCell.IsVisitedMaze)
            {
                RemoveWall(currentCell, nextCell);

                currentCell = nextCell;
                currentCell.IsVisitedMaze = true;
                stack.Add(currentCell);
                visitedCellCount++;
            }
        }

        grid.cellArray[0, 1].SetCell(true);
        grid.cellArray[grid.Width - 1, grid.Height - 2].SetCell(true);
    }

    private void RemoveWall(Cell currentCell, Cell nextCell)
    {
        var cellArray = grid.cellArray;
        var currentCellPos = currentCell.position;
        var nextCellPos = nextCell.position;

        if (currentCellPos.x < nextCellPos.x)
        {
            cellArray[(int)currentCellPos.x + 1, (int)currentCellPos.y].SetCell(true);
        }

        if (currentCellPos.x > nextCellPos.x)
        {
            cellArray[(int)currentCellPos.x - 1, (int)currentCellPos.y].SetCell(true);
        }

        if (currentCellPos.y < nextCellPos.y)
        {
            cellArray[(int)currentCellPos.x, (int)currentCellPos.y + 1].SetCell(true);
        }

        if (currentCellPos.y > nextCellPos.y)
        {
            cellArray[(int)currentCellPos.x, (int)currentCellPos.y - 1].SetCell(true);
        }
    }
}
