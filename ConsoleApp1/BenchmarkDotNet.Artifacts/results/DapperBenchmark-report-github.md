```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.22631.5624/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics 3.30GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.18 (8.0.1825.31117), X64 RyuJIT AVX2


```
| Method               | Mean          | Error     | StdDev    | Gen0    | Gen1    | Allocated  |
|--------------------- |--------------:|----------:|----------:|--------:|--------:|-----------:|
| &#39;Dengan &#39;EXEC&#39;&#39;      |      3.982 ms | 0.0589 ms | 0.0523 ms | 39.0625 | 15.6250 |  320.92 KB |
| &#39;Dengan CommandType&#39; |      4.024 ms | 0.0535 ms | 0.0501 ms | 39.0625 | 15.6250 |  321.07 KB |
| &#39;EF Core (Tanpa SP)&#39; | 25,022.463 ms | 6.6893 ms | 5.9299 ms |       - |       - | 2118.66 KB |
