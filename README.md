# ZeroResult: High-Performance Result Monads for .NET 🌟

[![NuGet](https://img.shields.io/nuget/v/ZeroResult.svg)](https://www.nuget.org/packages/ZeroResult/)
[![CI](https://github.com/baryon-asymm/ZeroResult/actions/workflows/ci.yml/badge.svg)](https://github.com/baryon-asymm/ZeroResult/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

ZeroResult provides allocation-free result monads for .NET 8+ with full async support and fluent APIs. Perfect for high-performance applications where traditional exception handling is too costly.

## Why ZeroResult? 🚀

✅ **Zero Allocations in Happy Path**  
`StackResult` uses `ref struct` to eliminate heap allocations completely when operations succeed

✅ **Modern C# 12+ Integration**  
Leverages the latest language features for optimal performance and expressiveness

✅ **Seamless Async Support**  
Full async/await compatibility with ValueTask-based operations and fluent chaining

## Features ✨

- Dual result types: `StackResult` (allocation-free `ref struct`) and `Result` (flexible `readonly struct`)
- Full async/await support with ValueTask
- Fluent API: `Map`, `Bind`, `Match`, `Ensure`, `Tap`
- Comprehensive error handling with `IError`, `SingleError`, and `MultiError`
- Optimized for .NET 8+ and C# 12
- Implicit conversions for clean syntax
- Stack-safe operations with inlining
- Comprehensive benchmark suite

## Installation 📦

```bash
dotnet add package ZeroResult
```

## Quick Examples 💻

### 1. Basic Result Handling with Match
```csharp
Result<int, SingleError> Calculate(int input)
{
    return input != 0 
        ? 100 / input 
        : new SingleError("Division by zero");
}

var result = Calculate(10);
result.Match(
    onSuccess: value => Console.WriteLine($"Result: {value}"),
    onFailure: error => Console.WriteLine($"Error: {error.Message}")
);
```

### 2. Fluent API Chaining with Map and Bind
```csharp
Result<int, SingleError> result = Result.Success<int, SingleError>(5)
    .Map(x => x * 2)  // Transforms value if successful
    .Bind(x => x < 100 
        ? x * 3 
        : Result.Failure<int, SingleError>(new SingleError("Too large")))
    .Ensure(x => x % 2 == 0, () => new SingleError("Must be even"));
```

### 3. Async Operations with ValueTask
```csharp
async ValueTask<Result<string, SingleError>> ProcessDataAsync(int id)
{
    return await FetchDataAsync(id)
        .MapAsync(async data => await TransformAsync(data))
        .BindAsync(async transformed => await ValidateAsync(transformed))
        .OnSuccessAsync(async result => await LogSuccessAsync(result))
        .OnFailureAsync(async error => await LogErrorAsync(error));
}
```

### 4. Conditional Validation with Ensure
```csharp
Result<User, ValidationError> ValidateUser(User user)
{
    return Result.Success<User, ValidationError>(user)
        .Ensure(u => u.Age >= 18, () => new ValidationError("Underage"))
        .Ensure(u => !string.IsNullOrEmpty(u.Email), () => new ValidationError("Invalid email"));
}
```

### 5. Combining Sync and Async Operations
```csharp
async ValueTask<Result<Report, BusinessError>> GenerateReportAsync(int userId)
{
    return await GetUser(userId)
        .Map(user => new ReportRequest(user))
        .BindAsync(async request => await ValidateRequestAsync(request))
        .MapAsync(async validRequest => await GenerateReportAsync(validRequest));
}
```

### 6. Error Handling Comparison
```csharp
// Traditional approach (expensive exceptions)
try {
    var value = RiskyOperation();
    Process(value);
}
catch (Exception ex) {
    HandleError(ex);
}

// ZeroResult approach (explicit error handling)
var result = SafeOperation();
result.Match(
    onSuccess: Process,
    onFailure: HandleError
);
```

### 7. Advanced Matching with Return Values
```csharp
Result<Order, OrderError> orderResult = ProcessOrder(orderId);
string message = orderResult.Match(
    onSuccess: order => $"Order {order.Id} processed",
    onFailure: error => $"Failed: {error.Message}"
);

// Async version
string asyncMessage = await orderResult.MatchAsync(
    onSuccess: async order => await FormatOrderAsync(order),
    onFailure: async error => await FormatErrorAsync(error)
);
```

### 8. Side Effects with Tap
```csharp
await GetUserAsync(userId)
    .TapAsync(async user => await AuditAccessAsync(user))
    .MapAsync(user => user.Profile)
    .Tap(profile => CacheProfile(profile));
```

## Advanced Error Handling with MultiError 🚨

ZeroResult's `MultiError` provides sophisticated error aggregation for complex validation scenarios, batch processing, and cases where multiple failures need to be reported simultaneously.

### Key Features:
- **Efficient error collection** with minimal allocations
- **Builder pattern** for incremental construction
- **Automatic message formatting** with error codes
- **Merge capability** for combining error sets
- **Lazy evaluation** of error messages

### Usage Examples:

#### 1. Basic MultiError Creation
```csharp
var errors = new IError[] {
    new SingleError("Invalid email format", "VAL-001"),
    new SingleError("Password must be 8+ characters", "SEC-002"),
    new SingleError("Username already exists", "USER-003")
};

Result<Unit, MultiError> validationResult = new MultiError(errors);
```

#### 2. Builder Pattern for Validation
```csharp
Result<User, MultiError> ValidateUser(UserInput input)
{
    var builder = MultiError.CreateBuilder();
    
    if (string.IsNullOrEmpty(input.Email))
        builder.Add(new SingleError("Email required", "REQ-001"));
    
    if (input.Password.Length < 8)
        builder.Add(new SingleError("Password too short", "SEC-001"));
    
    if (input.Age < 18)
        builder.Add(new SingleError("Must be 18+", "AGE-001"));
    
    return builder.Count > 0
        ? Result.Failure<User, MultiError>(builder.Build())
        : MapToUser(input);
}
```

#### 3. Batch Processing with Error Aggregation
```csharp
async ValueTask<Result<BatchReport, MultiError>> ProcessBatchAsync(int[] ids)
{
    var builder = MultiError.CreateBuilder();
    var successes = new List<ItemResult>();
    
    foreach (var id in ids)
    {
        var result = await ProcessItemAsync(id);
        result.Match(
            onSuccess: successes.Add,
            onFailure: builder.Add
        );
    }
    
    return builder.Count > 0
        ? Result.Failure<BatchReport, MultiError>(builder.Build())
        : new BatchReport(successes);
}
```

#### 4. Merging Multiple Error Sets
```csharp
var addressResult = ValidateAddress(order.Address);
var paymentResult = ValidatePayment(order.PaymentMethod);

if (addressResult.IsFailure || paymentResult.IsFailure)
{
    var mergedErrors = MultiError.Merge(
        addressResult.IsFailure ? addressResult.Error : MultiError.Empty,
        paymentResult.IsFailure ? paymentResult.Error : MultiError.Empty
    );
    
    return Result.Failure<OrderConfirmation, MultiError>(mergedErrors);
}
```

#### 5. Complex Domain Validation
```csharp
Result<LoanApplication, MultiError> ValidateApplication(LoanApplication app)
{
    var builder = MultiError.CreateBuilder();
    
    // Financial validation
    if (app.Income < app.MonthlyPayment * 3)
        builder.Add(new SingleError("Income insufficient", "FIN-001"));
    
    // Document validation
    if (app.RequiredDocuments.Count < 3)
        builder.Add(new SingleError("Missing documents", "DOC-002"));
    
    // Business rules
    if (app.Age < 21)
        builder.Add(new SingleError("Minimum age not met", "AGE-003"));
    
    // Custom validation method
    ValidateCreditHistory(app.CreditScore, builder);
    
    return builder.Count > 0
        ? Result.Failure<LoanApplication, MultiError>(builder.Build())
        : app;
}

void ValidateCreditHistory(int score, MultiErrorBuilder builder)
{
    if (score < 650)
        builder.Add(new SingleError("Poor credit history", "CRD-004"));
}
```

#### 6. Formatted Error Output
MultiError automatically generates structured error messages:
```csharp
var error = new MultiError(new IError[] {
    new SingleError("Invalid email format", "VAL-001"),
    new SingleError("Password too short", "SEC-002"),
    new SingleError("Terms not accepted", "REQ-003")
});

Console.WriteLine(error.Message);
/* Output:
Multiple errors occurred (3):
- Invalid email format (Code: VAL-001)
- Password too short (Code: SEC-002)
- Terms not accepted (Code: REQ-003)
*/
```

### Performance Optimizations
- **Pre-allocated collections**: Builder uses optimal initial capacity
- **Struct-based enumerators**: Avoids boxing allocations
- **Lazy message formatting**: Message concatenation deferred until first access
- **Merge without copying**: Reuses existing error collections

### When to Use MultiError
✔ Complex form validations  
✔ Batch processing pipelines  
✔ Distributed system integrations  
✔ Business rule engines  
✔ Data migration tools  

---

**Pro Tip**: Combine `MultiError` with `StackResult` for allocation-free validation in hot paths:
```csharp
StackResult<Transaction, MultiError> ValidateTransaction(Transaction tx)
{
    var builder = MultiError.CreateBuilder();
    // ... validation logic
    return builder.Count > 0
        ? StackResult.Failure<Transaction, MultiError>(builder.Build())
        : tx;
}
```

## Performance ⚡

ZeroResult dramatically outperforms traditional exception handling, especially in deep call stacks and error scenarios. Benchmarks were run on **.NET 9.0.6** using **BenchmarkDotNet v0.15.1**, with results collected from two major platforms:

- **Windows x64**: AMD Ryzen 7 7800X3D
- **macOS Arm64**: Apple M2

### Key Findings:
- **100-180x faster** than exceptions in method chaining scenarios
- **77-89% less memory allocated** in failure cases
- **Zero allocations** in success paths with imperative style
- **Handles 200+ call depths** where exceptions cause stack overflows
- **Fluent APIs add minimal overhead** (~2μs) compared to imperative style

### Benchmark Highlights

#### 1. Method Chaining (2000 iterations)
| Scenario                | Approach               | Mean Time  | Allocated | vs Try/Catch |
|-------------------------|------------------------|------------|-----------|--------------|
| **All Failures**        | Try/Catch              | 2,521 μs   | 427 KB    | 1.00x        |
|                         | StackResult (Imperative)| 14.8 μs    | 47 KB     | **170x faster** |
|                         | Result (Fluent)        | 24.7 μs    | 175 KB    | **100x faster** |
| **75% Success Rate**    | Try/Catch              | 631 μs     | 103 KB    | 1.00x        |
|                         | StackResult (Imperative)| 18.4 μs    | 11 KB     | **34x faster** |
|                         | Result (Fluent)        | 27.8 μs    | 139 KB    | **23x faster** |

#### 2. Deep Call Stack Performance
| Approach                | Call Depth | Mean Time  | Memory  | Outcome            |
|-------------------------|------------|------------|---------|--------------------|
| **Exceptions**          | 20         | 111 ms     | 15.7 MB | Works              |
|                         | 200        | -          | -       | **Stack Overflow** |
| **ZeroResult (MultiError)** | 20      | 2.6 ms     | 6.9 MB  | **43x faster**     |
|                         | 200        | 25 ms      | 65 MB   | Still works        |

#### 3. Memory Efficiency
| Scenario                | Approach       | Allocations | Reduction |
|-------------------------|----------------|-------------|-----------|
| Async Operations (100% errors) | Try/Catch | 2.77 MB     | -         |
|                         | ZeroResult     | 625 KB      | **77% less** |
| Method Chaining (100% errors) | Try/Catch | 427 KB      | -         |
|                         | ZeroResult     | 47 KB       | **89% less** |

### Performance Takeaways

1. **Error Handling:** ZeroResult is **100-170x faster** with **77-89% less memory** than exceptions
2. **Success Paths:** Near-zero overhead with **sub-microsecond operations**
3. **Scalability:** Handles call depths impossible with exceptions
4. **Fluent APIs:** Add just **10μs overhead** vs imperative style while being more expressive
5. **Memory Efficiency:** Dramatically reduces GC pressure in error scenarios

If you're interested in the full benchmark results or want to explore detailed metrics for specific scenarios, check out the [**benchmark results folder**](./benchmarks/results/).

---

**ZeroResult** - Where performance meets reliability in .NET error handling. Contribute on [GitHub](https://github.com/baryon-asymm/ZeroResult)!
