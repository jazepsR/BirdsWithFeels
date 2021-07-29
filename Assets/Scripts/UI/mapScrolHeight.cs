using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapScrolHeight : MonoBehaviour
{
    public float maxY = 22f;
    public float minY = 18f;
[HideInInspector] 
    public float scale = 0.00973f;
[HideInInspector] 
    public float startingY = 0.00973f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x,  (minY + startingY) * scale , transform.position.z), 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(transform.position.x,  (maxY + startingY )* scale, transform.position.z), 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), 1f);
    }
}
