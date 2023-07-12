using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float startSize;
    public float maxSize;
    public float sizeIncrement;

    private new Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        camera.orthographicSize = startSize;
    }

    void Update()
    {
        if (camera.orthographicSize + sizeIncrement < maxSize) {
            camera.orthographicSize = startSize + sizeIncrement * (LevelManager.Instance.GetWave() - 1);
        }
        else {
            camera.orthographicSize = maxSize;
        }
    }
}
