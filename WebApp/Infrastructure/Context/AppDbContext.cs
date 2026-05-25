using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Context;

/// DbContext継承クラス
public class AppDbContext : DbContext
{
    private DbSet<EmployeeEntity>? _employees;
    private DbSet<DepartmentEntity>? _departments;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// employeeテーブルにアクセスするプロパティ
    public virtual DbSet<EmployeeEntity> Employees
    {
        get => _employees ?? Set<EmployeeEntity>();
        set => _employees = value;
    }

    /// departmentテーブルにアクセスするプロパティ
    public virtual DbSet<DepartmentEntity> Departments
    {
        get => _departments ?? Set<DepartmentEntity>();
        set => _departments = value;
    }
}
