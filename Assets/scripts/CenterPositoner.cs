using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPositoner : MonoBehaviour
{
    public float LeftPosition;
    public Camera Camera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var width = Camera.aspect * Camera.orthographicSize;
        var x = (width + LeftPosition)/2;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        
    }
}
