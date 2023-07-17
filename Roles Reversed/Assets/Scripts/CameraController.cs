using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public float startSize;
    public float maxSize;
    public float sizeIncrement;

    private new Camera camera;
    private Vector3 initMousePos;

    void Start()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -1);
        camera = GetComponent<Camera>();
        camera.orthographicSize = startSize;
    }

    void Update()
    {
        if (LevelManager.Instance.GetWave() == 0) {
            camera.orthographicSize = startSize;
        }
        else if (camera.orthographicSize + sizeIncrement < maxSize) {
            camera.orthographicSize = startSize + sizeIncrement * (LevelManager.Instance.GetWave() - 1);
        }
        else {
            camera.orthographicSize = maxSize;
        }

        // initialize drag
        if (Input.GetMouseButtonDown(1))
        {
            initMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Vector3 pos = initMousePos - curMousePos;

            transform.position = pos;
        }
    }
}
