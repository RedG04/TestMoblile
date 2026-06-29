using UnityEngine;

/// Adatta automaticamente la camera
/// alle dimensioni del livello.
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private float padding = 2f;

    private void Start()
    {
        AdjustCamera();
    }

    public void AdjustCamera()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        targetCamera.orthographic = true;

        float aspect =
            (float)Screen.width /
            Screen.height;

        targetCamera.orthographicSize =
            5f + padding;

        targetCamera.transform.position =
            new Vector3(
                2f,
                10f,
                -2f
            );

        targetCamera.transform.rotation =
            Quaternion.Euler(
                60f,
                0f,
                0f
            );
    }
}