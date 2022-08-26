using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class nav_point : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDrawGizmos()
    {
        //draw nav point gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
