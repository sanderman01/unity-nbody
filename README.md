
# N-Body Simulation on the GPU in Unity3D

**Note: This is purely a fun and educational project for myself and as such is not intended for production uses as-is.**

This is an implementation of a n-body simulation. An n-body system is a simulation of a dynamical system of particles (also called bodies) under the influence of physical forces. Predicting the behaviour of an n-body system is called an n-body problem. Such problems are usually solved using a simulation approach. Examples of n-body simulations in astronomy are simulations of planetary interactions in our solar system, and galaxy formation out of star clouds. Other uses of n-body simulation occur in fields like chemistry and biology.

Due to the nature of n-body systems where each particle typically exerts forces on every other particle in the simulation, the computational complexity of the simulation will tend to rise quadratically with the number of particles to be simulated. This poses a performance problem at high particle counts.

There are several ways to deal with this, allowing better performance.

1. Throw more hardware at it: You could get the best, most expensive CPU on the market to run your sim, but that will not help you much. Industry advanced in CPU speed have been slowing down for a while now.
2. Use different hardware: This nets a huge performance gain. GPU devices are designed for massive parallelization to efficiently crunch a lot of numbers, as long as those calculations are all very similar and can easily be parallelized.
3. Use a trick to reduce algorithmic complexity. A hierarchical divide and conquer strategy to break up the problem into smaller chunks. Group particles in area tiles, and then resolve the forces between particles in different areas in a more global fashion, reducing the amount of work required because it is no longer neccesary for each and every particle to be paired with each and every other particle.

As of this moment, this project is using the second approach and simply using the GPU to calculate the interactions for every particle with every other particle. The forces in this simulation are based on newtonian physics and gravity. The simulation parameters were simply chosen to be interesting and may or may not actually be realistic.


## Todo

1. Currently the particle interactions are done on the GPU, which is fast, but we still pull the data to the CPU each frame, which is very slow. Replace the current rendering system. Do particle rendering on GPU using geometry shaders or something.
2. Use tiles/areas of particles for even greater performance.
