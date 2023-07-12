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
    }
}
