using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapPan : MonoBehaviour {
    public float mouseSensitivity;
    private Vector3 lastPosition;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }
    }
}
