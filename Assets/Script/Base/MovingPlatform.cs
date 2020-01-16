using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Platform {

    private Vector3 originPosition;

    private void Awake()
    {
        originPosition = transform.position;
    }

    void LateUpdate () {
        transform.position += originPosition;
    }
}
