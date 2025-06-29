using BenchmarkDotNet.Running;
using ZeroResult.Benchmarks;

BenchmarkRunner.Run<MonadBenchmarks>();
BenchmarkRunner.Run<MapBindBenchmarks>();
BenchmarkRunner.Run<AsyncMonadBenchmarks>();

BenchmarkRunner.Run<MultiErrorBenchmarks>();
