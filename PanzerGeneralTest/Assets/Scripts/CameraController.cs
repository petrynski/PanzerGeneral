using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    [SerializeField]
    float zoomFactor = 1.0f;

    [SerializeField]
    float zoomSpeed = 5.0f;

    private float originalSize = 0f;

    private Camera thisCamera;

    private float maxZoom = 1.0f;
    private float minZoom = 0.3f;


    void Start()
    {
        thisCamera = GetComponent<Camera>();
        originalSize = thisCamera.orthographicSize;
    }

    void Update()
    {
        float targetSize = originalSize * zoomFactor;

        if (targetSize != thisCamera.orthographicSize)
            thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);

        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            if (pos.y < 13)
                pos.y += panSpeed * Time.deltaTime * zoomFactor;

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            if (pos.y > 2)
                pos.y -= panSpeed * Time.deltaTime * zoomFactor;

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            if (pos.x > 10)
                pos.x -= panSpeed * Time.deltaTime * zoomFactor;

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            if (pos.x < 40)
                pos.x += panSpeed * Time.deltaTime * zoomFactor;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            if (zoomFactor > minZoom)
                zoomFactor -= 0.1f;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            if (zoomFactor < maxZoom)
                zoomFactor += 0.1f;

        transform.position = pos;
    }
}
