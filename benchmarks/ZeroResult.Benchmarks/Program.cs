using BenchmarkDotNet.Running;
using ZeroResult.Benchmarks;

BenchmarkRunner.Run<ResultBenchmarks>();
BenchmarkRunner.Run<MapBindBenchmarks>();
