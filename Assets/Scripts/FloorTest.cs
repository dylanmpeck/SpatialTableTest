using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTest : MonoBehaviour
{
    [SerializeField] BoxCollider collider;
    [SerializeField] Material wireframeMat;
    float numOfCubes = 256;
    // Start is called before the first frame update
    void Start()
    {
        CreateSpatialSurface();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateSpatialSurface()
    {
        float z = -collider.bounds.extents.z;
        while (z < collider.bounds.extents.z)
        {
            float x = -collider.bounds.extents.x;
            while (x < collider.bounds.extents.x)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(0.1f, 0.05f, 0.1f);
                cube.transform.position = new Vector3(transform.position.x + x, transform.position.y + 0.1f, transform.position.z + z);
                cube.layer = LayerMask.NameToLayer("Spatial Awareness");
                cube.GetComponent<MeshRenderer>().material = wireframeMat;
                cube.transform.SetParent(this.transform);
                x += (collider.bounds.extents.x * 2) / numOfCubes;
            }
            z += (collider.bounds.extents.z * 2) / numOfCubes;
        }
    }
}
