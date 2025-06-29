```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.26100.4484/24H2/2024Update/HudsonValley)
AMD Ryzen 7 7800X3D 4.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.205
  [Host]     : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method              | CallDepth | Iterations | Mean       | Error     | StdDev    | Ratio | RatioSD | Rank | Gen0      | Gen1     | Allocated | Alloc Ratio |
|-------------------- |---------- |----------- |-----------:|----------:|----------:|------:|--------:|-----:|----------:|---------:|----------:|------------:|
| **ExceptionBasedFlow**  | **20**        | **2000**       | **111.169 ms** | **2.0446 ms** | **1.9125 ms** |  **1.00** |    **0.02** |    **3** |  **200.0000** |        **-** |  **15.69 MB** |        **1.00** |
| MultiErrorBasedFlow | 20        | 2000       |   2.597 ms | 0.0500 ms | 0.0535 ms |  0.02 |    0.00 |    1 |  144.5313 |        - |   6.93 MB |        0.44 |
| ListBasedErrorFlow  | 20        | 2000       |   2.955 ms | 0.0352 ms | 0.0312 ms |  0.03 |    0.00 |    2 |  156.2500 |        - |   7.54 MB |        0.48 |
|                     |           |            |            |           |           |       |         |      |           |          |           |             |
| **ExceptionBasedFlow**  | **200**       | **2000**       |         **NA** |        **NA** |        **NA** |     **?** |       **?** |    **?** |        **NA** |       **NA** |        **NA** |           **?** |
| MultiErrorBasedFlow | 200       | 2000       |  25.010 ms | 0.3939 ms | 0.3684 ms |     ? |       ? |    1 | 1343.7500 | 125.0000 |   65.3 MB |           ? |
| ListBasedErrorFlow  | 200       | 2000       |  38.270 ms | 0.5235 ms | 0.4371 ms |     ? |       ? |    2 | 1428.5714 | 142.8571 |  70.14 MB |           ? |

Benchmarks with issues:
  MultiErrorBenchmarks.ExceptionBasedFlow: DefaultJob [CallDepth=200, Iterations=2000]
