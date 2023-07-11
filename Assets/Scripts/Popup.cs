using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public float destroyTime = 1f;
    public Vector3 popupOffset;

    void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition += popupOffset;
    }
}
