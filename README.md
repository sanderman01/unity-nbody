
# N-Body Particle Simulation on the GPU in Unity3D

**Note: This is purely a fun and educational project for myself and as such is not intended for production uses as-is.**

This is an implementation of a n-body simulation. An n-body system is a simulation of a dynamical system of bodies (also called particles) under the influence of physical forces. Predicting the behaviour of an n-body system is called an n-body problem. Such problems are usually solved using a simulation approach. Examples of n-body simulations in astronomy are simulations of planetary interactions in our solar system, and galaxy formation out of star clouds. Other uses of n-body simulation occur in fields like chemistry and biology.

Due to the nature of n-body systems where each particle typically exerts forces on every other particle in the simulation, the computational complexity of the simulation will tend to rise quadratically with the number of particles to be simulated. This poses a performance problem at high particle counts.

There are several ways to deal with this, allowing better performance.

1. Throw more hardware at it: You could get the best, most expensive CPU on the market to run your sim, but that will not help you much. Industry advanced in CPU speed have been slowing down for a while now.
2. Use different hardware: This nets a huge performance gain. GPU devices are designed for massive parallelization to efficiently crunch a lot of numbers, as long as those calculations are all very similar and can easily be parallelized.
3. Use a trick to reduce algorithmic complexity. A hierarchical divide and conquer strategy to break up the problem into smaller chunks. Group particles in area tiles, and then resolve the forces between particles in different areas in a more global fashion, reducing the amount of work required because it is no longer neccesary for each and every particle to be paired with each and every other particle.

As of this moment, this project is using the second approach and simply using the GPU to calculate the interactions for every particle with every other particle. The forces in this simulation are based on newtonian physics and gravity. The simulation parameters were chosen simply to be interesting and may or may not actually be realistic.


## Notes

Geometry shaders are rumored to be somewhat slow. In spite of that, the fact that we no longer need to pull particle data to the CPU only to send it back to the GPU is already a big performance boost compared to the previously used rendering method. Right now the actual simulation calculations are the largest bottleneck by far. 

Testing shows my laptop GPU (Nvidia GTX970M) can easily handle six million (6*10^6) static particles at 80 fps using the current method. Running the simulation itself (without rendering) with only six thousand particles barely gets me 20 fps.

## Todo

1. Use tiles/areas of particles for even greater performance. This will require significant changes in the compute shader and perhaps elsewhere.

2. Replace rendering using geometry shader with something even faster? Creating new triangles from scratch for every single particle does not strike me as efficient. Perhaps just index into the vertices of an existing mesh? Would it even matter much for performance?