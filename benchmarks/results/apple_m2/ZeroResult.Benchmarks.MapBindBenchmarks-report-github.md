```

BenchmarkDotNet v0.15.1, macOS Sequoia 15.5 (24F74) [Darwin 24.5.0]
Apple M2, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]     : .NET 9.0.4 (9.0.425.16305), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 9.0.4 (9.0.425.16305), Arm64 RyuJIT AdvSIMD


```
| Method                                       | SuccessThreshold | Iterations | Mean         | Error     | StdDev    | Ratio | Rank | Gen0    | Allocated | Alloc Ratio |
|--------------------------------------------- |----------------- |----------- |-------------:|----------:|----------:|------:|-----:|--------:|----------:|------------:|
| **TryCatchChaining**                             | **0**                | **2000**       | **17,574.36 μs** | **92.223 μs** | **81.753 μs** | **1.000** |    **3** | **31.2500** |  **428040 B** |        **1.00** |
| StackResult_ImperativeStyle                  | 0                | 2000       |     17.47 μs |  0.017 μs |  0.014 μs | 0.001 |    1 |  5.6763 |   47519 B |        0.11 |
| StackResult_ImperativeStyle_ExceptionToMonad | 0                | 2000       | 17,938.17 μs | 44.186 μs | 39.170 μs | 1.021 |    3 | 31.2500 |  475598 B |        1.11 |
| Result_ImperativeStyle                       | 0                | 2000       |     17.57 μs |  0.015 μs |  0.013 μs | 0.001 |    1 |  5.6763 |   47519 B |        0.11 |
| Result_ImperativeStyle_ExceptionToMonad      | 0                | 2000       | 17,959.72 μs | 45.863 μs | 42.900 μs | 1.022 |    3 | 31.2500 |  475577 B |        1.11 |
| StackResult_Bind                             | 0                | 2000       |     28.41 μs |  0.028 μs |  0.025 μs | 0.002 |    2 | 20.9656 |  175519 B |        0.41 |
| StackResult_Bind_ExceptionToMonad            | 0                | 2000       | 18,067.08 μs | 57.778 μs | 54.046 μs | 1.028 |    3 | 62.5000 |  603577 B |        1.41 |
| Result_Bind                                  | 0                | 2000       |     28.38 μs |  0.013 μs |  0.011 μs | 0.002 |    2 | 20.9656 |  175519 B |        0.41 |
| Result_Bind_ExceptionToMonad                 | 0                | 2000       | 18,085.80 μs | 25.577 μs | 22.674 μs | 1.029 |    3 | 62.5000 |  603578 B |        1.41 |
| StackResult_Map                              | 0                | 2000       |     28.30 μs |  0.011 μs |  0.010 μs | 0.002 |    2 | 20.9656 |  175519 B |        0.41 |
| StackResult_Map_ExceptionToMonad             | 0                | 2000       | 18,102.21 μs | 38.530 μs | 36.041 μs | 1.030 |    3 | 62.5000 |  603577 B |        1.41 |
| Result_Map                                   | 0                | 2000       |     28.30 μs |  0.024 μs |  0.022 μs | 0.002 |    2 | 20.9656 |  175519 B |        0.41 |
| Result_Map_ExceptionToMonad                  | 0                | 2000       | 18,483.37 μs | 41.715 μs | 39.020 μs | 1.052 |    3 | 62.5000 |  603598 B |        1.41 |
|                                              |                  |            |              |           |           |       |      |         |           |             |
| **TryCatchChaining**                             | **75**               | **2000**       |  **4,343.79 μs** | **24.744 μs** | **23.145 μs** | **1.000** |    **3** |  **7.8125** |  **103600 B** |        **1.00** |
| StackResult_ImperativeStyle                  | 75               | 2000       |     21.45 μs |  0.027 μs |  0.025 μs | 0.005 |    1 |  1.3733 |   11518 B |        0.11 |
| StackResult_ImperativeStyle_ExceptionToMonad | 75               | 2000       |  4,490.82 μs | 22.770 μs | 21.299 μs | 1.034 |    3 |  7.8125 |  115105 B |        1.11 |
| Result_ImperativeStyle                       | 75               | 2000       |     21.60 μs |  0.041 μs |  0.036 μs | 0.005 |    1 |  1.3733 |   11518 B |        0.11 |
| Result_ImperativeStyle_ExceptionToMonad      | 75               | 2000       |  4,381.48 μs | 26.684 μs | 24.960 μs | 1.009 |    3 |  7.8125 |  115110 B |        1.11 |
| StackResult_Bind                             | 75               | 2000       |     33.49 μs |  0.040 μs |  0.034 μs | 0.008 |    2 | 16.6626 |  139517 B |        1.35 |
| StackResult_Bind_ExceptionToMonad            | 75               | 2000       |  4,540.32 μs | 39.018 μs | 36.497 μs | 1.045 |    3 | 23.4375 |  243108 B |        2.35 |
| Result_Bind                                  | 75               | 2000       |     32.91 μs |  0.019 μs |  0.016 μs | 0.008 |    2 | 16.6626 |  139517 B |        1.35 |
| Result_Bind_ExceptionToMonad                 | 75               | 2000       |  4,456.38 μs | 39.940 μs | 37.360 μs | 1.026 |    3 | 23.4375 |  243108 B |        2.35 |
| StackResult_Map                              | 75               | 2000       |     32.57 μs |  0.026 μs |  0.023 μs | 0.007 |    2 | 16.6626 |  139517 B |        1.35 |
| StackResult_Map_ExceptionToMonad             | 75               | 2000       |  4,399.85 μs | 29.275 μs | 25.952 μs | 1.013 |    3 | 23.4375 |  243108 B |        2.35 |
| Result_Map                                   | 75               | 2000       |     32.50 μs |  0.028 μs |  0.024 μs | 0.007 |    2 | 16.6626 |  139517 B |        1.35 |
| Result_Map_ExceptionToMonad                  | 75               | 2000       |  4,443.96 μs | 32.638 μs | 30.529 μs | 1.023 |    3 | 23.4375 |  243105 B |        2.35 |
|                                              |                  |            |              |           |           |       |      |         |           |             |
| **TryCatchChaining**                             | **100**              | **2000**       |     **14.04 μs** |  **0.011 μs** |  **0.010 μs** |  **1.00** |    **2** |       **-** |         **-** |          **NA** |
| StackResult_ImperativeStyle                  | 100              | 2000       |     13.03 μs |  0.026 μs |  0.023 μs |  0.93 |    1 |       - |         - |          NA |
| StackResult_ImperativeStyle_ExceptionToMonad | 100              | 2000       |     15.61 μs |  0.073 μs |  0.068 μs |  1.11 |    3 |       - |         - |          NA |
| Result_ImperativeStyle                       | 100              | 2000       |     13.05 μs |  0.026 μs |  0.025 μs |  0.93 |    1 |       - |         - |          NA |
| Result_ImperativeStyle_ExceptionToMonad      | 100              | 2000       |     15.59 μs |  0.073 μs |  0.065 μs |  1.11 |    3 |       - |         - |          NA |
| StackResult_Bind                             | 100              | 2000       |     25.29 μs |  0.025 μs |  0.022 μs |  1.80 |    5 | 15.2893 |  128000 B |          NA |
| StackResult_Bind_ExceptionToMonad            | 100              | 2000       |     26.38 μs |  0.014 μs |  0.012 μs |  1.88 |    6 | 15.2893 |  128000 B |          NA |
| Result_Bind                                  | 100              | 2000       |     24.67 μs |  0.009 μs |  0.008 μs |  1.76 |    4 | 15.2893 |  128000 B |          NA |
| Result_Bind_ExceptionToMonad                 | 100              | 2000       |     26.53 μs |  0.019 μs |  0.017 μs |  1.89 |    6 | 15.2893 |  128000 B |          NA |
| StackResult_Map                              | 100              | 2000       |     24.19 μs |  0.018 μs |  0.015 μs |  1.72 |    4 | 15.2893 |  128000 B |          NA |
| StackResult_Map_ExceptionToMonad             | 100              | 2000       |     25.89 μs |  0.016 μs |  0.014 μs |  1.84 |    6 | 15.2893 |  128000 B |          NA |
| Result_Map                                   | 100              | 2000       |     24.21 μs |  0.018 μs |  0.016 μs |  1.73 |    4 | 15.2893 |  128000 B |          NA |
| Result_Map_ExceptionToMonad                  | 100              | 2000       |     26.10 μs |  0.015 μs |  0.013 μs |  1.86 |    6 | 15.2893 |  128000 B |          NA |
