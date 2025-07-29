using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;


[MemoryDiagnoser]
public class DapperBenchmark
{
    private readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=p1f_apps;Trusted_Connection=True;";
    private DbContextOptions<ApplicationDbContext> _efCoreOptions = null!;

    [GlobalSetup] // <-- TAMBAHKAN INI
    public void Setup()
    {
        // Pindahkan pendaftaran handler ke sini
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        _efCoreOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options;
    }

    [Benchmark(Description = "Dengan 'EXEC'")]
    public async Task<List<EmployeeVM>> AdHocQuery()
    {
        using var connection = new SqlConnection(connectionString);
        var employees = await connection.QueryAsync<EmployeeVM>("EXEC getEmployee");
        return employees.AsList();
    }

    [Benchmark(Description = "Dengan CommandType")]
    public async Task<List<EmployeeVM>> StoredProcedureQuery()
    {
        using var connection = new SqlConnection(connectionString);
        var employees = await connection.QueryAsync<EmployeeVM>(
            "getEmployee",
            commandType: CommandType.StoredProcedure);
        return employees.AsList();
    }

    [Benchmark(Description = "EF Core (Tanpa SP)")]
    public async Task<List<EmployeeDept>> EfCoreLinqQuery()
    {
        using var context = new ApplicationDbContext(_efCoreOptions);

        var employees = await context.EmployeeDept
                .AsNoTracking()
               .Include(item => item.Employee!)
                   .ThenInclude(item => item.Genders)
               .Include(item => item.Department)
               .OrderBy(item => item.Employee!.SesaId)
               .ToListAsync(); // <-- Tidak ada .Select() di sini

        return employees;
    }

}