using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float startSize;
    public float maxSize;
    public float sizeIncrement;
    public int incrementInterval;

    private new Camera camera;
    private int wave;

    void Start()
    {
        camera = GetComponent<Camera>();
        camera.orthographicSize = startSize;
        wave = 1;
    }

    void Update()
    {
        if (LevelManager.Instance.GetWave() - wave >= incrementInterval && camera.orthographicSize < maxSize) {
            if (camera.orthographicSize + sizeIncrement > maxSize)
            {
                camera.orthographicSize = maxSize;
            }
            else {
                camera.orthographicSize += sizeIncrement;
            }
            wave = LevelManager.Instance.GetWave();
        }
    }
}
