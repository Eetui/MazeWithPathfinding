using UnityEngine;

public class CameraSize : MonoBehaviour
{

    [SerializeField] private CellGrid grid;
    private void Start()
    {
        transform.position = new Vector3(grid.Width * 0.5f, grid.Height * 0.5f, transform.position.z);
        var cam = GetComponent<Camera>();

        if (cam.orthographicSize < grid.Width)
        {
            cam.orthographicSize = grid.Width /3 + 10;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
