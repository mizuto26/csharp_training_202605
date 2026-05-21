using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Context;

/// DbContext継承クラス
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// employeeテーブルにアクセスするプロパティ
    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();
    /// departmentテーブルにアクセスするプロパティ
    public DbSet<DepartmentEntity> Departments => Set<DepartmentEntity>();
}