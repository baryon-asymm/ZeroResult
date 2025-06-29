```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.26100.4484/24H2/2024Update/HudsonValley)
AMD Ryzen 7 7800X3D 4.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.205
  [Host]     : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                       | SuccessThreshold | Iterations | Mean        | Error     | StdDev    | Ratio | Rank | Gen0   | Allocated | Alloc Ratio |
|----------------------------- |----------------- |----------- |------------:|----------:|----------:|------:|-----:|-------:|----------:|------------:|
| **TryCatchHandling**             | **0**                | **2000**       | **2,482.04 μs** | **26.563 μs** | **24.847 μs** | **1.000** |    **2** | **7.8125** |  **427710 B** |        **1.00** |
| StackResultMonad             | 0                | 2000       |    16.60 μs |  0.070 μs |  0.058 μs | 0.007 |    1 | 0.9460 |   47519 B |        0.11 |
| ResultMonad                  | 0                | 2000       |    16.46 μs |  0.040 μs |  0.033 μs | 0.007 |    1 | 0.9460 |   47519 B |        0.11 |
| StackResult_ExceptionToMonad | 0                | 2000       | 2,508.62 μs | 10.338 μs |  8.633 μs | 1.011 |    2 | 7.8125 |  475233 B |        1.11 |
| Result_ExceptionToMonad      | 0                | 2000       | 2,509.58 μs | 23.374 μs | 21.864 μs | 1.011 |    2 | 7.8125 |  475233 B |        1.11 |
|                              |                  |            |             |           |           |       |      |        |           |             |
| **TryCatchHandling**             | **75**               | **2000**       |   **626.61 μs** |  **3.442 μs** |  **3.052 μs** |  **1.00** |    **2** | **1.9531** |  **103486 B** |        **1.00** |
| StackResultMonad             | 75               | 2000       |    19.65 μs |  0.048 μs |  0.040 μs |  0.03 |    1 | 0.2136 |   11518 B |        0.11 |
| ResultMonad                  | 75               | 2000       |    20.05 μs |  0.055 μs |  0.046 μs |  0.03 |    1 | 0.2136 |   11518 B |        0.11 |
| StackResult_ExceptionToMonad | 75               | 2000       |   635.12 μs |  3.320 μs |  3.105 μs |  1.01 |    2 | 1.9531 |  114984 B |        1.11 |
| Result_ExceptionToMonad      | 75               | 2000       |   629.65 μs |  2.313 μs |  1.806 μs |  1.00 |    2 | 1.9531 |  114984 B |        1.11 |
|                              |                  |            |             |           |           |       |      |        |           |             |
| **TryCatchHandling**             | **100**              | **2000**       |    **14.01 μs** |  **0.041 μs** |  **0.038 μs** |  **1.00** |    **2** |      **-** |         **-** |          **NA** |
| StackResultMonad             | 100              | 2000       |    13.58 μs |  0.031 μs |  0.029 μs |  0.97 |    1 |      - |         - |          NA |
| ResultMonad                  | 100              | 2000       |    13.92 μs |  0.027 μs |  0.025 μs |  0.99 |    2 |      - |         - |          NA |
| StackResult_ExceptionToMonad | 100              | 2000       |    18.14 μs |  0.061 μs |  0.054 μs |  1.30 |    3 |      - |         - |          NA |
| Result_ExceptionToMonad      | 100              | 2000       |    18.58 μs |  0.044 μs |  0.039 μs |  1.33 |    3 |      - |         - |          NA |
