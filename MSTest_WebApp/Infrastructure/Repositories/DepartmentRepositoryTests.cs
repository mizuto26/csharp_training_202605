using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using MSTest_WebApp.Tests.TestDoubles;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Context;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Repositories;

namespace MSTest_WebApp.Infrastructure.Repositories;

[TestClass]
public class DepartmentRepositoryTests
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=WebApp;Username=postgres;Password=training;";

    [TestMethod("部署一覧を取得できる")]
    public void FindAll_ReturnsDepartment()
    {
        List<DepartmentEntity> departmentEntities =
        [
            new DepartmentEntity { DeptId = 2, DeptName = "総務部" },
            new DepartmentEntity { DeptId = 1, DeptName = "営業部" }
        ];

        using var context = CreateContext([], departmentEntities);
        DepartmentRepository repository = CreateRepository(context);

        IReadOnlyList<Department> departments = repository.FindAll();

        Assert.AreEqual(2, departments.Count);
        Assert.AreEqual(1, departments[0].Id);
        Assert.AreEqual("営業部", departments[0].Name);
        Assert.AreEqual(2, departments[1].Id);
        Assert.AreEqual("総務部", departments[1].Name);
    }

    [TestMethod("存在する部署Idを指定すると部署を取得できる")]
    public void FindById_WhenDepartmentExists_ReturnsDepartment()
    {
        List<DepartmentEntity> departmentEntities =
        [
            new DepartmentEntity { DeptId = 10, DeptName = "営業部" }
        ];

        using var context = CreateContext([], departmentEntities);
        DepartmentRepository repository = CreateRepository(context);

        Department? department = repository.FindById(10);

        Assert.IsNotNull(department);
        Assert.AreEqual(10, department.Id);
        Assert.AreEqual("営業部", department.Name);
    }

    [TestMethod("存在しない部署Idを指定するとnullが返る")]
    public void FindById_WhenDepartmentDoesNotExist_ReturnsNull()
    {
        using var context = CreateContext([], []);
        DepartmentRepository repository = CreateRepository(context);

        Department? department = repository.FindById(999);

        Assert.IsNull(department);
    }

    [TestMethod("部署Entityが追加されTrueが返る")]
    public void Create_AddsDepartmentEntity_ReternTrue()
    {
        List<DepartmentEntity> departmentEntities = [];

        using var context = CreateContext([], departmentEntities);
        DepartmentRepository repository = CreateRepository(context);

        Department department = new(name: "営業部");
        bool created = repository.Create(department);

        IReadOnlyList<DepartmentEntity> savedDepartments = ((QueryableDbSet<DepartmentEntity>)context.Departments).Entities;

        Assert.IsTrue(created);
        Assert.AreEqual(1, savedDepartments.Count);
        Assert.AreEqual("営業部", savedDepartments[0].DeptName);
    }

    [TestMethod("指定した部署名の部署が存在する場合はtrueを返す")]
    public void ExistsByName_WhenDepartmentExists_ReturnsTrue()
    {
        List<DepartmentEntity> departmentEntities =
        [
            new DepartmentEntity { DeptId = 1, DeptName = "営業部" }
        ];

        using var context = CreateContext([], departmentEntities);
        DepartmentRepository repository = CreateRepository(context);

        bool exists = repository.ExistsByName("営業部");

        Assert.IsTrue(exists);
    }

    [TestMethod("指定した部署名の部署が存在しない場合はfalseを返す")]
    public void ExistsByName_WhenDepartmentDoesNotExist_ReturnsFalse()
    {
        using var context = CreateContext([], []);
        DepartmentRepository repository = CreateRepository(context);

        bool exists = repository.ExistsByName("総務部");

        Assert.IsFalse(exists);
    }

    [TestMethod("指定した部署Idを持つ従業員が存在する場合はtrueを返す")]
    public void ExistsEmployeeByDepartmentId_WhenEmployeeExists_ReturnsTrue()
    {
        List<EmployeeEntity> employeeEntities =
        [
            new EmployeeEntity { EmpId = 1, EmpName = "山田", DeptId = 10 }
        ];

        using var context = CreateContext(employeeEntities, []);
        DepartmentRepository repository = CreateRepository(context);

        bool exists = repository.ExistsEmployeeByDepartmentId(10);

        Assert.IsTrue(exists);
    }

    [TestMethod("指定した部署Idを持つ従業員が存在しない場合はfalseを返す")]
    public void ExistsEmployeeByDepartmentId_WhenEmployeeDoesNotExist_ReturnsFalse()
    {
        List<EmployeeEntity> employeeEntities =
        [
            new EmployeeEntity { EmpId = 1, EmpName = "山田", DeptId = 10 }
        ];

        using var context = CreateContext(employeeEntities, []);
        DepartmentRepository repository = CreateRepository(context);

        bool exists = repository.ExistsEmployeeByDepartmentId(20);

        Assert.IsFalse(exists);
    }

    [TestMethod("存在しない部署Idを削除するとfalseを返す")]
    public void DeleteById_WhenDepartmentDoesNotExist_ReturnsFalse()
    {
        using var context = CreateContext([], []);
        DepartmentRepository repository = CreateRepository(context);

        bool deleted = repository.DeleteById(999);

        Assert.IsFalse(deleted);
    }

    [TestMethod("存在する部署を削除しtrueを返す")]
    public void DeleteById_WhenTargetExists_ReturnTrue()
    {
        using AppDbContext context = CreateDatabaseContextWithInitSql();
        DepartmentRepository repository = CreateRepository(context);

        bool deleted = repository.DeleteById(3);
        context.SaveChanges();

        Assert.IsTrue(deleted);
        Assert.IsNull(repository.FindById(3));
    }

    private static DepartmentRepository CreateRepository(AppDbContext context)
    {
        return new DepartmentRepository(context, new DepartmentEntityAdapter());
    }

    private static TestAppDbContext CreateContext(
        IEnumerable<EmployeeEntity> employees,
        IEnumerable<DepartmentEntity> departments)
    {
        return new TestAppDbContext
        {
            Employees = new QueryableDbSet<EmployeeEntity>(employees),
            Departments = new QueryableDbSet<DepartmentEntity>(departments),
        };
    }

    private static AppDbContext CreateDatabaseContextWithInitSql()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        AppDbContext context = new(options);
        string path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        string sql = File.ReadAllText(path);
        context.Database.ExecuteSqlRaw(sql);

        return context;
    }
}
