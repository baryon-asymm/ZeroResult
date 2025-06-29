```

BenchmarkDotNet v0.15.1, macOS Sequoia 15.5 (24F74) [Darwin 24.5.0]
Apple M2, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]     : .NET 9.0.4 (9.0.425.16305), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 9.0.4 (9.0.425.16305), Arm64 RyuJIT AdvSIMD


```
| Method              | CallDepth | Iterations | Mean         | Error      | StdDev     | Ratio | RatioSD | Rank | Gen0       | Gen1      | Allocated | Alloc Ratio |
|-------------------- |---------- |----------- |-------------:|-----------:|-----------:|------:|--------:|-----:|-----------:|----------:|----------:|------------:|
| **ExceptionBasedFlow**  | **20**        | **2000**       |   **634.080 ms** |  **9.8683 ms** |  **9.2308 ms** | **1.000** |    **0.02** |    **3** |  **1000.0000** |         **-** |  **15.69 MB** |        **1.00** |
| MultiErrorBasedFlow | 20        | 2000       |     2.608 ms |  0.0019 ms |  0.0017 ms | 0.004 |    0.00 |    1 |   867.1875 |    7.8125 |   6.93 MB |        0.44 |
| ListBasedErrorFlow  | 20        | 2000       |     3.257 ms |  0.0041 ms |  0.0038 ms | 0.005 |    0.00 |    2 |   941.4063 |   11.7188 |   7.54 MB |        0.48 |
|                     |           |            |              |            |            |       |         |      |            |           |           |             |
| **ExceptionBasedFlow**  | **200**       | **2000**       | **6,515.816 ms** | **15.7069 ms** | **13.9238 ms** | **1.000** |    **0.00** |    **3** | **18000.0000** | **3000.0000** |  **151.1 MB** |        **1.00** |
| MultiErrorBasedFlow | 200       | 2000       |    26.659 ms |  0.0220 ms |  0.0205 ms | 0.004 |    0.00 |    1 |  8156.2500 |  875.0000 |   65.3 MB |        0.43 |
| ListBasedErrorFlow  | 200       | 2000       |    38.871 ms |  0.7432 ms |  0.6952 ms | 0.006 |    0.00 |    2 |  8769.2308 |  846.1538 |  70.13 MB |        0.46 |
