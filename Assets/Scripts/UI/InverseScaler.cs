using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseScaler : MonoBehaviour
{
    public Transform target;
    private float startingScale;
    // Start is called before the first frame update
    void Start()
    {
        startingScale = target.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (startingScale / target.localScale.x);
    }
}
