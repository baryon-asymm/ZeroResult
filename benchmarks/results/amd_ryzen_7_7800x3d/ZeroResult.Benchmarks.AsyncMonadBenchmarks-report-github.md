```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.26100.4484/24H2/2024Update/HudsonValley)
AMD Ryzen 7 7800X3D 4.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.205
  [Host]     : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method          | SuccessThreshold | Iterations | Mean    | Error   | StdDev  | Ratio | Rank | Allocated  | Alloc Ratio |
|---------------- |----------------- |----------- |--------:|--------:|--------:|------:|-----:|-----------:|------------:|
| **TryCatchAsync**   | **0**                | **2000**       | **30.93 s** | **0.037 s** | **0.035 s** |  **1.00** |    **1** | **2770.53 KB** |        **1.00** |
| ZeroResultAsync | 0                | 2000       | 30.90 s | 0.022 s | 0.020 s |  1.00 |    1 |  625.27 KB |        0.23 |
|                 |                  |            |         |         |         |       |      |            |             |
| **TryCatchAsync**   | **75**               | **2000**       | **30.87 s** | **0.038 s** | **0.035 s** |  **1.00** |    **1** | **1058.05 KB** |        **1.00** |
| ZeroResultAsync | 75               | 2000       | 30.90 s | 0.022 s | 0.019 s |  1.00 |    1 |  589.83 KB |        0.56 |
|                 |                  |            |         |         |         |       |      |            |             |
| **TryCatchAsync**   | **100**              | **2000**       | **30.91 s** | **0.036 s** | **0.034 s** |  **1.00** |    **1** |  **532.09 KB** |        **1.00** |
| ZeroResultAsync | 100              | 2000       | 30.94 s | 0.045 s | 0.042 s |  1.00 |    1 |  578.95 KB |        1.09 |
