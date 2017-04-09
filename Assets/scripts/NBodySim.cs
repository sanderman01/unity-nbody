using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySim : MonoBehaviour {

    [SerializeField]
    private int nBodies = 100;

    [SerializeField]
    private int targetFramerate = 60;
    [SerializeField]
    private float timeScale = 1;

    [SerializeField]
    private float bodyMassMultiplier = 1;
    [SerializeField]
    private float gConstant = 6.7f * 0.00001f;
    [SerializeField]
    private float softening = 2;

    [SerializeField]
    private float particleSize = 0.01f;

    [SerializeField]
    private ComputeShader computeShader;
    [SerializeField]
    private Material renderMaterial;

    [SerializeField]
    private bool simulate = true;
    [SerializeField]
    private bool render = true;

    private int kernel;
    private ComputeBuffer buffer;
    private ParticleBody[] data;

    private long frame;


    struct ParticleBody {
        public Vector3 pos;
        public float mass;
        public Vector3 vel;
    };

    void Start() {
        //Application.targetFrameRate = targetFramerate;
        Setup(nBodies);
        Init(nBodies);
    }

    void Update() {
        frame++;

        if(simulate) {
            RunComputeShader();
        }
    }

    void OnDestroy() {
        Cleanup();
    }

    void Setup(int maxCount) {
        try {
            kernel = computeShader.FindKernel("NBody");
        } catch(UnityException ex) {
            Debug.Break();
        }

        // buffer count is number of elements in the buffer
        // buffer stride is size in bytes of the datatype inside the buffer.
        int bufferStride = sizeof(float) * 7;
        buffer = new ComputeBuffer(maxCount, bufferStride);
        computeShader.SetBuffer(kernel, "buffer", buffer);
    }

    void Cleanup() {
        buffer.Dispose();
    }

    void Init(int count) {
        // init simulation data
        computeShader.SetInt("bodyCount", nBodies);
        computeShader.SetFloat("bodyMassM", bodyMassMultiplier);
        computeShader.SetFloat("gravConstant", gConstant);
        computeShader.SetFloat("deltaTime", (1f/(float)targetFramerate) * timeScale);
        computeShader.SetFloat("softening", softening);

        data = new ParticleBody[count];
        for(int i = 0; i < data.Length; i++) {
            ParticleBody p = new ParticleBody();
            p.pos = UnityEngine.Random.onUnitSphere;
            p.mass = UnityEngine.Random.value;
            //p.vel = UnityEngine.Random.insideUnitSphere * 0.01f;
            data[i] = p;
        }

        buffer.SetData(data);

        
    }

    void RunComputeShader() {
        computeShader.Dispatch(kernel, data.Length, 1, 1);
    }

    void OnRenderObject() {
        // particle rendering using gpu
        if(render) {
            renderMaterial.SetFloat("_Size", particleSize);
            renderMaterial.SetPass(0);
            renderMaterial.SetBuffer("bodies", buffer);
            Graphics.DrawProcedural(MeshTopology.Points, nBodies, 0);
            UnityEngine.Rendering.CommandBuffer b;
        }
    }
}
