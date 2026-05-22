using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Context;

/// DbContext継承クラス
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// employeeテーブルにアクセスするプロパティ
    public virtual DbSet<EmployeeEntity> Employees { get; set; } = null!;
    /// departmentテーブルにアクセスするプロパティ
    public virtual DbSet<DepartmentEntity> Departments { get; set; } = null!;
}
