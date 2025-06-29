```

BenchmarkDotNet v0.15.1, macOS Sequoia 15.5 (24F74) [Darwin 24.5.0]
Apple M2, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]     : .NET 9.0.4 (9.0.425.16305), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 9.0.4 (9.0.425.16305), Arm64 RyuJIT AdvSIMD


```
| Method          | SuccessThreshold | Iterations | Mean    | Error   | StdDev  | Ratio | Rank | Allocated  | Alloc Ratio |
|---------------- |----------------- |----------- |--------:|--------:|--------:|------:|-----:|-----------:|------------:|
| **TryCatchAsync**   | **0**                | **2000**       | **24.20 s** | **0.020 s** | **0.019 s** |  **1.00** |    **1** | **2770.86 KB** |        **1.00** |
| ZeroResultAsync | 0                | 2000       | 23.92 s | 0.033 s | 0.030 s |  0.99 |    1 |  625.59 KB |        0.23 |
|                 |                  |            |         |         |         |       |      |            |             |
| **TryCatchAsync**   | **75**               | **2000**       | **24.00 s** | **0.028 s** | **0.026 s** |  **1.00** |    **1** | **1058.05 KB** |        **1.00** |
| ZeroResultAsync | 75               | 2000       | 23.93 s | 0.020 s | 0.016 s |  1.00 |    1 |  590.16 KB |        0.56 |
|                 |                  |            |         |         |         |       |      |            |             |
| **TryCatchAsync**   | **100**              | **2000**       | **23.90 s** | **0.025 s** | **0.022 s** |  **1.00** |    **1** |  **532.42 KB** |        **1.00** |
| ZeroResultAsync | 100              | 2000       | 23.92 s | 0.017 s | 0.015 s |  1.00 |    1 |  579.28 KB |        1.09 |
