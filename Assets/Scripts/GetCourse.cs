using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCourse : MonoBehaviour
{
    public List<GameObject> courseObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Collider[] collisions = Physics.OverlapBox(transform.position, transform.localScale);

        foreach (Collider collision in collisions)
        {
            courseObjects.Add(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
