using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySim : MonoBehaviour {

    [SerializeField]
    private int nBodies = 1000;

    [SerializeField]
    private ParticleSystem fx;
    private ParticleSystem.Particle[] particles;

    [SerializeField]
    private ComputeShader shader;
    private int kernel;

    private ComputeBuffer buffer;
    private ParticleBody[] data;

    struct ParticleBody {
        public Vector3 pos;
        public Vector3 vel;
    };

    void Start() {
        fx = GetComponentInChildren<ParticleSystem>();
        Setup(nBodies);
        Init(nBodies);
    }

    void Update() {
        RunShader();
        UpdateParticles();
    }

    void OnDestroy() {
        Cleanup();
    }

    void Setup(int maxCount) {
        kernel = shader.FindKernel("NBody");

        // buffer count is number of elements in the buffer
        // buffer stride is size in bytes of the datatype inside the buffer.
        int bufferStride = sizeof(float) * 6;
        buffer = new ComputeBuffer(maxCount, bufferStride);
        shader.SetBuffer(kernel, "buffer", buffer);
    }

    void Cleanup() {
        buffer.Dispose();
    }

    void Init(int count) {
        // init simulation data
        data = new ParticleBody[count];

        for(int i = 0; i < data.Length; i++) {
            ParticleBody p = new ParticleBody();
            p.vel = Vector3.one * 0.1f;
            p.pos = UnityEngine.Random.insideUnitSphere;
            data[i] = p;
        }

        buffer.SetData(data);

        // init particlesystem component
        particles = new ParticleSystem.Particle[count];
        for(int i = 0; i < particles.Length; i++) {
            ParticleSystem.Particle a = particles[i];
            particles[i].position = Vector3.zero;
            particles[i].startColor = Color.white;
            particles[i].startSize = 0.1f;
        }
        fx.SetParticles(particles, particles.Length);
    }

    void RunShader() {
        shader.Dispatch(kernel, data.Length, 1, 1);
        buffer.GetData(data);
    }

    void UpdateParticles() {
        // copy new positions to particles renderer
        for (int i = 0; i < particles.Length; i++) {
            particles[i].position = data[i].pos;
        }
        fx.SetParticles(particles, particles.Length);
    }
}
