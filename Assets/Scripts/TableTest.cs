using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTest : MonoBehaviour
{
    [SerializeField] Transform spatialParent;
    [SerializeField] Material wireframeMat;
    [SerializeField] BoxCollider colider;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject table;
    Vector3 clickPos;
    float numOfCubes = 26;
    float negX, posX, negZ, posZ;
    bool negXset, posXset, negZset, posZset;
    bool planeCreated;
    bool tableFound;

    // Start is called before the first frame update
    void Start()
    {
        CreateSpatialSurface();


    }

    // Update is called once per frame
    void Update()
    {
        if (tableFound == false && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                tableFound = true;
                clickPos = hit.point;

                StartCoroutine(RaycastUntilFloor(new Vector3(0.05f, 0.0f, 0.0f)));
                StartCoroutine(RaycastUntilFloor(new Vector3(-0.05f, 0.0f, 0.0f)));
                StartCoroutine(RaycastUntilFloor(new Vector3(0.0f, 0.0f, 0.05f)));
                StartCoroutine(RaycastUntilFloor(new Vector3(0.0f, 0.0f, -0.05f)));
            }
        }
        
        if (!planeCreated && negZset && posZset && negXset && posXset)
        {
            Debug.Log("creating plane");
            planeCreated = true;

            Vector3 origin = clickPos;
            if (posX > negX)
            {
                origin.x += (posX - negX) / 2.0f;
                //xScale = posX - ((posX - negX) / 2.0f);
            }
            else if (negX > posX)
            {
                origin.x -= (negX - posX) / 2.0f;
               // xScale = negX - ((negX - posX) / 2.0f);
            }
            //else
            //    xScale = posX;

            if (posZ > negZ)
            {
                origin.z += (posZ - negZ) / 2.0f;
               // zScale = posZ - ((posZ - negZ) / 2.0f);
            }
            else if (negZ > posZ)
            {
                origin.z -= (negZ - posZ) / 2.0f;
               // zScale = negZ - ((negZ - posZ) / 2.0f);
            }
            //else
                //zScale = posZ;

            Debug.Log("NegX = " + negX + "PosX = " + posX);
            Debug.Log("NegZ = " + negZ + "PosZ = " + posZ);

            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = Vector3.zero;
            plane.transform.localScale = new Vector3((posX + negX) / plane.GetComponent<MeshRenderer>().bounds.size.x, plane.transform.localScale.y, (posZ + negZ) / plane.GetComponent<MeshRenderer>().bounds.size.z);
            plane.transform.position = origin;
            plane.AddComponent<GetCourse>();
        }
    }

    IEnumerator RaycastUntilFloor(Vector3 moveDir)
    {
        // Make sure surface has been created on Start
        yield return new WaitForEndOfFrame();

        Vector3 origin = clickPos + new Vector3(0.0f, 0.1f, 0.0f);
        //Debug.DrawRay(origin, Vector3.down, Color.red, 25.0f);
        RaycastHit hit;
        while (Physics.Raycast(origin, Vector3.down, out hit, 1.0f, mask))
        {
            Debug.DrawRay(origin, Vector3.down, Color.red, 25.0f);
            origin += moveDir;
            yield return null;
        }

        if (moveDir.x < 0.0f)
        {
            negX = Mathf.Abs(clickPos.x - origin.x);
            negXset = true;
        }
        if (moveDir.x > 0.0f)
        {
            posX = Mathf.Abs(clickPos.x - origin.x);
            posXset = true;
        }
        if (moveDir.z > 0.0f)
        {
            posZ = Mathf.Abs(clickPos.z - origin.z);
            posZset = true;
        }
        if (moveDir.z < 0.0f)
        {
            negZ = Mathf.Abs(clickPos.z - origin.z);
            negZset = true;
        }
    }

    void CreateSpatialSurface()
    {
        float z = -colider.bounds.extents.z;
        while (z < colider.bounds.extents.z)
        {
            float x = -colider.bounds.extents.x;
            while (x < colider.bounds.extents.x)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(0.1f, 0.05f, 0.1f);
                cube.transform.position = new Vector3(transform.position.x + x, transform.position.y + 0.1f, transform.position.z + z);
                cube.layer = LayerMask.NameToLayer("Spatial Awareness");
                cube.GetComponent<MeshRenderer>().material = wireframeMat;
                cube.transform.SetParent(spatialParent);
                x += (colider.bounds.extents.x * 2) / numOfCubes;
            }
            z += (colider.bounds.extents.z * 2) / numOfCubes;
        }
    }
}
