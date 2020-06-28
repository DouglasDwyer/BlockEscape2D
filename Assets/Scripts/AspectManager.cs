using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectManager : MonoBehaviour {

    public Camera currentCamera;
    public float aspectRatio;

    private float initialSize;

    public void Awake()
    {
        initialSize = currentCamera.orthographicSize;
    }

    public void Update()
    {
        currentCamera.orthographicSize = initialSize * aspectRatio / currentCamera.aspect;
    }
}
