# Parallel and distributed computing

Code examples for the course [Parallel and distributed computing](https://mvano.com/courses/parallel/).
Each topic of the course has its own folder named after the topic; the table below lists them in
course order and links each topic page. Topic 13 has no code folder.

## Tools

- .NET 10 SDK and JetBrains Rider (or Visual Studio 2026) for the C# topics. Open `<Topic>\<Topic>.slnx`
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

| #                                                                  | Folder                                           | Topic                                                                                 | Projects                                                                               |
| ------------------------------------------------------------------ | ------------------------------------------------ | ------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------- |
| [1](https://mvano.com/courses/parallel/01-intro/)                  | [`Intro`](Intro)                                 | Fundamentals of parallel computing, Amdahl's and Gustafson's laws; .NET 10 and Rider  | `Speedup`                                                                              |
| [2](https://mvano.com/courses/parallel/02-processes-threads/)      | [`Threads`](Threads)                             | Processes and threads: scheduling, priorities, affinity; threads and the thread pool  | `Threads`                                                                              |
| [3](https://mvano.com/courses/parallel/03-synchronization/)        | [`Synchronization`](Synchronization)             | Race conditions, deadlocks, starvation; thread synchronization in C#                  | `Sync`                                                                                 |
| [4](https://mvano.com/courses/parallel/04-concurrent-collections/) | [`ConcurrentCollections`](ConcurrentCollections) | Thread-safe collections, channels, producer–consumer; false sharing                   | `PipelineDemo`, `FalseSharing`, `FalseSharingBench`                                    |
| [5](https://mvano.com/courses/parallel/05-tpl/)                    | [`Tpl`](Tpl)                                     | Task parallelism (TPL), cancellation, exceptions; async/await                         | `Downloads`, `HashCancel`, `StateMachine`, `Statistics`                                |
| [6](https://mvano.com/courses/parallel/06-data-parallelism/)       | [`DataParallelism`](DataParallelism)             | Data parallelism: parallel loops, PLINQ, partitioning; parallel sorting and reduction | `Mandelbrot`, `MergeSort`                                                              |
| [7](https://mvano.com/courses/parallel/07-simd/)                   | [`Simd`](Simd)                                   | SIMD vectorization in .NET, parallel linear algebra                                   | `Capabilities`, `SumBenchmarks`, `MatMul`                                              |
| [8](https://mvano.com/courses/parallel/08-parallel-algorithms/)    | [`Algorithms`](Algorithms)                       | Models and levels of parallelism; parallel algorithms for vectors, matrices, numerics | `Adaptive`, `MatMul`                                                                   |
| [9](https://mvano.com/courses/parallel/09-cpp-threads/)            | [`CppThreads`](CppThreads)                       | Multithreading in C++ and the parallel algorithms of the standard library             | `parallel_sum`, `pool`, `bank`, `parallel_stl` (CMake)                                 |
| [10](https://mvano.com/courses/parallel/10-openmp/)                | [`OpenMp`](OpenMp)                               | OpenMP: parallel regions and loops, reductions, tasks; affinity and NUMA              | `hello`, `pi`, `mandel`, `quicksort` (CMake)                                           |
| [11](https://mvano.com/courses/parallel/11-gpu/)                   | [`Gpu`](Gpu)                                     | GPU computing: CUDA C++ and ILGPU for C#                                              | `VectorAdd`, `GpuExamples` (CUDA, CMake), `IlgpuDemo` (C#)                             |
| [12](https://mvano.com/courses/parallel/12-mpi/)                   | [`Mpi`](Mpi)                                     | MPI: point-to-point and collective operations, hybrid MPI + OpenMP                    | `MpiExamples` (CMake; `scale.sh`, `jscale.sh`, `hybrid.sh` run the measurement series) |
| [14](https://mvano.com/courses/parallel/14-sockets-rpc/)           | [`Rpc`](Rpc)                                     | Network sockets and remote procedure calls (CoreWCF, gRPC)                            | `Echo`, `Frames` (length-prefixed JSON frames), `Orders` (gRPC)                        |
| [15](https://mvano.com/courses/parallel/15-rabbitmq/)              | [`RabbitMq`](RabbitMq)                           | Messaging with RabbitMQ: work queues, publish–subscribe, delivery guarantees          | `Hello`, `Logs`, `Orders`, `Reliable`; `compose.yaml` starts RabbitMQ                  |
| [16](https://mvano.com/courses/parallel/16-orleans/)               | [`Orleans`](Orleans)                             | The actor model and Microsoft Orleans; CAP and fault tolerance                        | `Bank`, `Sensors`                                                                      |
| [17](https://mvano.com/courses/parallel/17-containers/)            | [`Containers`](Containers)                       | Containers and orchestration: Docker, Kubernetes, .NET Aspire                         | `Primes` (Docker Compose, kind: `kind-setup.ps1`, `submit-job.ps1`), `Primes.Aspire`   |
| [18](https://mvano.com/courses/parallel/18-microservices/)         | [`Microservices`](Microservices)                 | Microservices: API gateway, Saga, Outbox, observability                               | `Shop` (`orders.ps1` sends three test orders through the gateway)                      |

## Notes

- `Rpc\Frames` and the gRPC server `Rpc\Orders` both use port 5001: do not run them at the same time.
- `Rpc\Frames`: `dotnet run -c Release` runs the server and the client in one process; pass
  `server` or `client` to run them in two terminals.
- Build output (`bin`, `obj`, `build`), IDE state and captures are ignored by `.gitignore`.
