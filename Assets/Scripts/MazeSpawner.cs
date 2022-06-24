using UnityEngine;
using UnityEngine.AI;

public class MazeSpawner : MonoBehaviour
{
    public Vector2Int MazeSize = new Vector2Int(10, 10);

    [SerializeField] private Cell CellPrefab;
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private bool RandomFinishPosition;
    [SerializeField] private Transform finish;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private bool needDrawPath = true;

    public HintRenderer path;
    public Maze maze;
    private Vector2Int CellSize = new Vector2Int(5, 5);

    private void Start()
    {
        Instantiate(playerPrefab);

    }

    private void Awake()
    {
        MazeGenerator generator = new MazeGenerator();

        maze = generator.GenerateMaze(MazeSize.x, MazeSize.y, RandomFinishPosition);

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, 0f, y * CellSize.y), Quaternion.identity, transform);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
                c.Floor.SetActive(maze.cells[x, y].Floor);
                c.DeathZone.SetActive(maze.cells[x, y].DeathZone);
            }
        }

        finish.position = new Vector3(maze.finishPosition.x * CellSize.x, 0.25f, maze.finishPosition.y * CellSize.y);

        if (needDrawPath) path.DrawPath();
        BakeNavMesh();
    }

    private void BakeNavMesh() {
        surface.BuildNavMesh();
    }
}
