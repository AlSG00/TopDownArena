using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClculationScript : MonoBehaviour
{
    //public LightProbes probes;
    //public LightProbeGroup probes2;
    //float _time = 1f;
    //float _lastTimeUpdate;
    //// Update is called once per frame
    //private void Start()
    //{
    //    LightProbes.Tetrahedralize();

    //    for (int i = 0; i < probes2.probePositions.Length; i++)
    //    {

    //    }
    //}

    //void Update()
    //{
    //    if (_lastTimeUpdate + _time <= Time.time)
    //    {
    //        _lastTimeUpdate = Time.time;
    //        LightProbes.CalculateInterpolatedLightAndOcclusionProbes(probes2.probePositions, )
    //        Debug.Log("updated");
    //    }
    //    //LightProbes.CalculateInterpolatedLightAndOcclusionProbes(probes.positions, );
    //}

    public Material material;

    private Matrix4x4[] transforms;
    private MaterialPropertyBlock properties;
    private Mesh cubeMesh;

    void Start()
    {
        const int kCount = 100;

        // Generate 100 random positions
        var positions = new Vector3[kCount];
        for (int i = 0; i < kCount; ++i)
            positions[i] = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));

        // Calculate probes at these positions
        var lightprobes = new UnityEngine.Rendering.SphericalHarmonicsL2[kCount];
        var occlusionprobes = new Vector4[kCount];
        LightProbes.CalculateInterpolatedLightAndOcclusionProbes(positions, lightprobes, occlusionprobes);

        // Put them into the MPB
        properties = new MaterialPropertyBlock();
        properties.CopySHCoefficientArraysFrom(lightprobes);
        properties.CopyProbeOcclusionArrayFrom(occlusionprobes);

        // Compute the transforms list
        transforms = new Matrix4x4[kCount];
        for (int i = 0; i < kCount; ++i)
            transforms[i] = Matrix4x4.Translate(positions[i]);

        // Create the cube mesh
        cubeMesh = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>().sharedMesh;

        // Make sure the material property is assigned
        if (material == null || !material.enableInstancing)
            Debug.LogError("material must be assigned with one with instancing enabled.");
    }

    // OnPreCull happens before every culling, which is the perfect timing to inject DrawMesh* function calls.
    void OnPreCull()
    {
        if (material != null && material.enableInstancing)
        {
            var lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.CustomProvided; // enable instancing for probes
            Graphics.DrawMeshInstanced(cubeMesh, 0, material, transforms, transforms.Length, properties, UnityEngine.Rendering.ShadowCastingMode.On, true, 0, null, lightProbeUsage);
        }
    }

    //void Update()
    //{
    //    if (_lastTimeUpdate + _time <= Time.time)
    //    {
    //        _lastTimeUpdate = Time.time;
    //        LightProbes.CalculateInterpolatedLightAndOcclusionProbes(probes2.probePositions, )
    //        Debug.Log("updated");
    //    }
    //    //LightProbes.CalculateInterpolatedLightAndOcclusionProbes(probes.positions, );
    //}
}
