using UnityEngine;

public class CellGrid : MonoBehaviour
{
    public Cell[,] cellArray;

    [SerializeField] private int width = 6;
    [SerializeField] private int height = 6;

    public int Width  { get { return width; } }
    public int Height  { get { return height; } }

    [SerializeField] private GameObject cellPrefab;

    [HideInInspector]
    public int mazeCells = 0;

    private void Awake()
    {
        if (height % 2 == 0) height++;
        if (width % 2 == 0) width++;

        cellArray = new Cell[width, height];
        PopulateGrid();
        AddMazeNeighbours();
        AddRealNeighbours();
    }

    private void PopulateGrid()
    {
        var cellHolder = new GameObject("Cells");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cellObject = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity, cellHolder.transform) as GameObject;
                var cell = cellObject.GetComponent<Cell>();
                cell.name = $"Cell ({x}, {y})";
                cell.position = new Vector2(x, y);
                cellArray[x, y] = cell;
            }
        }
    }

    private void AddMazeNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    cellArray[x, y].SetCell(false);
                }
                else if((x + 2) % 2 == 0 || (y + 2) % 2 == 0)
                {
                    cellArray[x, y].SetCell(false);
                }
                else
                {
                    if (x - 2 > 0)
                    {
                        cellArray[x, y].mazeNeighbours.Add(cellArray[x - 2, y]); // left
                        cellArray[x, y].SetCell(true);
                    }

                    if (x + 2 < width - 1)
                    {
                        cellArray[x, y].mazeNeighbours.Add(cellArray[x + 2, y]); // right
                        cellArray[x, y].SetCell(true);
                    }

                    if (y - 2 > 0)
                    {
                        cellArray[x, y].mazeNeighbours.Add(cellArray[x, y - 2]); // bottom
                        cellArray[x, y].SetCell(true);
                    }

                    if (y + 2 < height - 1)
                    {
                        cellArray[x, y].mazeNeighbours.Add(cellArray[x, y + 2]); // top
                        cellArray[x, y].SetCell(true);
                    }

                    mazeCells++;
                }
            }
        }
    }

    private void AddRealNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x - 1 >= 0)
                {
                    cellArray[x, y].neighbours.Add(cellArray[x - 1, y]); // left
                }

                if (x + 1 <= width - 1)
                {
                    cellArray[x, y].neighbours.Add(cellArray[x + 1, y]); //right

                }

                if (y - 1 >= 0)
                {
                    cellArray[x, y].neighbours.Add(cellArray[x, y - 1]); // bottom
                }

                if (y + 1 <= height - 1)
                {
                    cellArray[x, y].neighbours.Add(cellArray[x, y + 1]); // top
                }
            }
        }
    }
}
