using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float vDecalage = 5f;
    void LateUpdate()
    {
        if (target!=null && target.position.y + vDecalage> transform.position.y)
        {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y + vDecalage, transform.position.z);
            transform.position = newPos;
        }
    }
}
