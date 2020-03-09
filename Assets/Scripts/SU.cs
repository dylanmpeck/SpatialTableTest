using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using UnityEngine.XR.WSA;
using Microsoft.MixedReality.SceneUnderstanding;

public class SU : MonoBehaviour
{
    public Byte test;
    public byte[] scene;
    // Start is called before the first frame update
    async void Start()
    {
        if (SceneObserver.IsSupported())
        {
            Debug.Log("not gonna work");
        }
        else
            Debug.Log("uh oh");

        // This call should grant the access we need.
        await SceneObserver.RequestAccessAsync();

        // Create Query settings for the scene update
        SceneQuerySettings querySettings;

        querySettings.EnableSceneObjectQuads = true;                                       // Requests that the scene updates quads.
        querySettings.EnableSceneObjectMeshes = true;                                      // Requests that the scene updates watertight mesh data.
        querySettings.EnableOnlyObservedSceneObjects = false;                              // Do not explicitly turn off quad inference.
        querySettings.EnableWorldMesh = true;                                              // Requests a static version of the spatial mapping mesh.
        querySettings.RequestedMeshLevelOfDetail = SceneMeshLevelOfDetail.Fine;            // Requests the finest LOD of the static spatial mapping mesh.

        // Initialize a new Scene
        Scene myScene = SceneObserver.ComputeAsync(querySettings, 10.0f).GetAwaiter().GetResult();


        SceneObject firstFloor = null;

        // Find the first floor object
        foreach (var sceneObject in myScene.SceneObjects)
        {
            if (sceneObject.Kind == SceneObjectKind.Floor)
            {
                Debug.Log("Found Floor");
                firstFloor = sceneObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
