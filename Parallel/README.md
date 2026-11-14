# Parallel and distributed computing

Code examples for the course [Parallel and distributed computing](https://mvano.com/courses/parallel/).
Each `LecNN` folder belongs to topic NN of the course; topic 13 has no code folder.

## Tools

- .NET 10 SDK and JetBrains Rider (or Visual Studio 2026) for the C# topics. Open `LecNN\LecNN.slnx`
  or a project folder, or use `dotnet run -c Release` (benchmarks and speedup measurements are only
  meaningful in Release).
- Topics 9–10: a C++23 compiler with OpenMP, CMake 3.28+ and Ninja; each project has `CMakePresets.json`
  (`cmake --preset release`, `cmake --build --preset release`).
- Topic 11: an NVIDIA GPU with the CUDA Toolkit (`GpuExamples`, `VectorAdd`); `IlgpuDemo` also runs
  on the CPU accelerator.
- Topic 12: an MPI implementation (Open MPI on Linux or WSL) and OpenMP.
- Topics 15–18: Docker (RabbitMQ, Redis, PostgreSQL containers); topic 17 also uses kind and kubectl,
  and .NET Aspire.

## Topics

| Folder            | Topic                                                                                   | Projects                                                     |
| ----------------- | --------------------------------------------------------------------------------------- | ------------------------------------------------------------ |
| [`Lec01`](Lec01)  | Fundamentals of parallel computing, Amdahl's and Gustafson's laws; .NET 10 and Rider    | `Speedup`                                                    |
| [`Lec02`](Lec02)  | Processes and threads: scheduling, priorities, affinity; threads and the thread pool    | `Threads`                                                    |
| [`Lec03`](Lec03)  | Race conditions, deadlocks, starvation; thread synchronization in C#                    | `Sync`                                                       |
| [`Lec04`](Lec04)  | Thread-safe collections, channels, producer–consumer; false sharing                     | `PipelineDemo`, `FalseSharing`, `FalseSharingBench`          |
| [`Lec05`](Lec05)  | Task parallelism (TPL), cancellation, exceptions; async/await                           | `Downloads`, `HashCancel`, `StateMachine`, `Statistics`      |
| [`Lec06`](Lec06)  | Data parallelism: parallel loops, PLINQ, partitioning; parallel sorting and reduction   | `Mandelbrot`, `MergeSort`                                    |
| [`Lec07`](Lec07)  | SIMD vectorization in .NET, parallel linear algebra                                     | `Capabilities`, `SumBenchmarks`, `MatMul`                    |
| [`Lec08`](Lec08)  | Models and levels of parallelism; parallel algorithms for vectors, matrices, numerics   | `Adaptive`, `MatMul`                                         |
| [`Lec09`](Lec09)  | Multithreading in C++ and the parallel algorithms of the standard library               | `parallel_sum`, `pool`, `bank`, `parallel_stl` (CMake)       |
| [`Lec10`](Lec10)  | OpenMP: parallel regions and loops, reductions, tasks; affinity and NUMA                | `hello`, `pi`, `mandel`, `quicksort` (CMake)                 |
| [`Lec11`](Lec11)  | GPU computing: CUDA C++ and ILGPU for C#                                                | `VectorAdd`, `GpuExamples` (CUDA, CMake), `IlgpuDemo` (C#)   |
| [`Lec12`](Lec12)  | MPI: point-to-point and collective operations, hybrid MPI + OpenMP                      | `MpiExamples` (CMake; `scale.sh`, `jscale.sh`, `hybrid.sh` run the measurement series) |
| [`Lec14`](Lec14)  | Network sockets and remote procedure calls (CoreWCF, gRPC)                              | `Echo`, `Frames` (length-prefixed JSON frames), `Orders` (gRPC) |
| [`Lec15`](Lec15)  | Messaging with RabbitMQ: work queues, publish–subscribe, delivery guarantees            | `Hello`, `Logs`, `Orders`, `Reliable`; `compose.yaml` starts RabbitMQ |
| [`Lec16`](Lec16)  | The actor model and Microsoft Orleans; CAP and fault tolerance                          | `Bank`, `Sensors`                                            |
| [`Lec17`](Lec17)  | Containers and orchestration: Docker, Kubernetes, .NET Aspire                           | `Primes` (Docker Compose, kind: `kind-setup.ps1`, `submit-job.ps1`), `Primes.Aspire` |
| [`Lec18`](Lec18)  | Microservices: API gateway, Saga, Outbox, observability                                 | `Shop` (`orders.ps1` sends three test orders through the gateway) |

## Notes

- `Lec14\Frames` and the gRPC server `Lec14\Orders` both use port 5001: do not run them at the same time.
- `Lec14\Frames`: `dotnet run -c Release` runs the server and the client in one process; pass
  `server` or `client` to run them in two terminals.
- Build output (`bin`, `obj`, `build`), IDE state and captures are ignored by `.gitignore`.
