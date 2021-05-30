using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 position;
    public List<Cell> mazeNeighbours = new List<Cell>();
    public List<Cell> neighbours = new List<Cell>();

    public bool IsWalkable = false;
    public bool IsVisitedMaze { get; set; }

    public int hCost = 0;
    public int gCost = 0;
    public int FCost = 0;

    public Cell parentCell;

    private SpriteRenderer sprite;

    [SerializeField] private Color32 walkableColor;
    [SerializeField] private Color32 notWalkableColor;
    [SerializeField] private Color32 topOfStack;

    private void Awake() => sprite = GetComponent<SpriteRenderer>();

    public void SetCell(bool isWalkable)
    {
        IsWalkable = isWalkable;

        SetColor();
    }

    public void SetColor()
    {
        if (IsWalkable)
        {
            sprite.color = walkableColor;
        }
        else
        {
            sprite.color = notWalkableColor;
        }
    }

    public void SetColor(Color32 color) => sprite.color = color;

    public void TopOfStack() => sprite.color = topOfStack;

    public void CalculateHCost(Vector2 goal)
    {
        int x = Mathf.Abs((int)position.x - (int)goal.x);
        int y = Mathf.Abs((int)position.y - (int)goal.y);

        hCost = x + y;
    }

    public void CalculateGCost(int currentSteps, Cell currentCell)
    {
        gCost = currentSteps + 1;
    }

    private void OnMouseDown()
    {
        if (IsWalkable)
        {
            SetCell(false);
        }
        else
        {
            SetCell(true);
        }
    }
}
