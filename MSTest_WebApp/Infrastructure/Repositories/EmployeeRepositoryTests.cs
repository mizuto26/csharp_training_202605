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
public class EmployeeRepositoryTests
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=WebAppTest;Username=postgres;Password=training;";

    [TestMethod("従業員Entityを追加しtrueが返る")]
    public void Create_AddsEmployeeEntity_ReturnTrue()
    {
        using var context = CreateContext([], []);
        EmployeeRepository repository = CreateRepository(context);

        Department department = new(id: 10, name: "営業部");

        Employee employee = new(name: "山田", email: "yamada@example.com", phone: "03-1234-5678", department: department);

        bool created = repository.Create(employee);
        Assert.IsTrue(created);

        List<EmployeeEntity> createdEntities = context.Employees.ToList();
        Assert.AreEqual(1, createdEntities.Count);
        EmployeeEntity createdEntity = createdEntities[0];
        Assert.AreEqual(10, createdEntity.DeptId);
    }

    [TestMethod("従業員一覧を取得できる")]
    public void FindAll_ReturnsEmployees()
    {
        List<EmployeeEntity> employeeEntities =
        [
            new EmployeeEntity { EmpId = 1, EmpName = "山田", EmpEmail = "yamada@example.com", EmpPhone = "03-1234-5678", DeptId = 1 },
            new EmployeeEntity { EmpId = 2, EmpName = "鈴木", EmpEmail = "suzuki@example.com", EmpPhone = "090-1111-2222", DeptId = null }
        ];

        using var context = CreateContext(employeeEntities, []);
        EmployeeRepository repository = CreateRepository(context);

        IReadOnlyList<Employee> employees = repository.FindAll();

        Assert.AreEqual(2, employees.Count);

        Assert.AreEqual(1, employees[0].Id);
        Assert.AreEqual("山田", employees[0].Name);
        Assert.AreEqual("yamada@example.com", employees[0].Email);
        Assert.AreEqual("03-1234-5678", employees[0].Phone);
        Assert.AreEqual(1, employees[0].Department?.Id);

        Assert.AreEqual(2, employees[1].Id);
        Assert.AreEqual("鈴木", employees[1].Name);
        Assert.AreEqual("suzuki@example.com", employees[1].Email);
        Assert.AreEqual("090-1111-2222", employees[1].Phone);
        Assert.IsNull(employees[1].Department);
    }

    [TestMethod("存在する従業員Idを指定すると従業員を取得できる")]
    public void FindById_WhenEmployeeExists_ReturnsEmployee()
    {
        List<EmployeeEntity> employeeEntities =
        [
            new EmployeeEntity { EmpId = 1, EmpName = "山田", EmpEmail = "yamada@example.com", EmpPhone = "03-1234-5678", DeptId = 1 }
        ];

        using var context = CreateContext(employeeEntities, []);
        EmployeeRepository repository = CreateRepository(context);

        Employee? employee = repository.FindById(1);

        Assert.IsNotNull(employee);
        Assert.AreEqual(1, employee.Id);
        Assert.AreEqual("山田", employee.Name);
        Assert.AreEqual("yamada@example.com", employee.Email);
        Assert.AreEqual("03-1234-5678", employee.Phone);
        Assert.AreEqual(1, employee.Department?.Id);
    }

    [TestMethod("存在しない従業員Idを指定するとnullが返る")]
    public void FindById_WhenEmployeeDoesNotExist_ReturnsNull()
    {
        using var context = CreateContext([], []);
        EmployeeRepository repository = CreateRepository(context);

        Employee? employee = repository.FindById(999);
        Assert.IsNull(employee);
    }

    [TestMethod("指定したメールアドレスの従業員が存在する場合はtrueを返す")]
    public void ExistsByEmail_WhenEmployeeExists_ReturnsTrue()
    {
        List<EmployeeEntity> employeeEntities =
        [
            new EmployeeEntity { EmpId = 1, EmpName = "山田", EmpEmail = "yamada@example.com", EmpPhone = "03-1234-5678" }
        ];

        using var context = CreateContext(employeeEntities, []);
        EmployeeRepository repository = CreateRepository(context);

        bool exists = repository.ExistsByEmail("YAMADA@example.com");
        Assert.IsTrue(exists);
    }

    [TestMethod("指定したメールアドレスの従業員が存在しない場合はfalseを返す")]
    public void ExistsByEmail_WhenEmployeeDoesNotExist_ReturnsFalse()
    {
        using var context = CreateContext([], []);
        EmployeeRepository repository = CreateRepository(context);

        bool exists = repository.ExistsByEmail("none@example.com");
        Assert.IsFalse(exists);
    }

    [TestMethod("指定した電話番号の従業員が存在する場合はtrueを返す")]
    public void ExistsByPhone_WhenEmployeeExists_ReturnsTrue()
    {
        List<EmployeeEntity> employeeEntities =
        [
            new EmployeeEntity { EmpId = 1, EmpName = "山田", EmpEmail = "yamada@example.com", EmpPhone = "03-1234-5678" }
        ];

        using var context = CreateContext(employeeEntities, []);
        EmployeeRepository repository = CreateRepository(context);

        bool exists = repository.ExistsByPhone("03-1234-5678");
        Assert.IsTrue(exists);
    }

    [TestMethod("指定した電話番号の従業員が存在しない場合はfalseを返す")]
    public void ExistsByPhone_WhenEmployeeDoesNotExist_ReturnsFalse()
    {
        using var context = CreateContext([], []);
        EmployeeRepository repository = CreateRepository(context);

        bool exists = repository.ExistsByPhone("090-0000-0000");
        Assert.IsFalse(exists);
    }

    [TestMethod("存在しない従業員は削除できずfalseが返る")]
    public void DeleteById_WhenTargetDoesNotExist_ReturnsFalse()
    {
        using var context = CreateContext([], []);
        EmployeeRepository repository = CreateRepository(context);

        bool deleted = repository.DeleteById(999);
        Assert.IsFalse(deleted);
    }

    [TestMethod("存在する従業員を削除しtrueが返る")]
    public void DeleteById_WhenTargetExists_ReternTrue()
    {
        using AppDbContext context = CreateDatabaseContextWithInitSql();
        EmployeeRepository repository = CreateRepository(context);

        bool deleted = repository.DeleteById(1);
        Assert.IsTrue(deleted);
    }

    [TestMethod("CreateでDBアクセスに失敗した場合は例外を投げる")]
    public void Create_WhenDbAccessFails_ThrowsException()
    {
        using var context = new TestAppDbContext
        {
            Employees = new ThrowingDbSet<EmployeeEntity>(),
            Departments = new QueryableDbSet<DepartmentEntity>([])
        };

        EmployeeRepository repository = CreateRepository(context);

        Employee employee = new(
            name: "山田",
            phone: "03-1234-5678",
            email: "yamada@example.com",
            department: null
        );

        Exception exception = Assert.ThrowsException<Exception>(() => repository.Create(employee));

        Assert.AreEqual("従業員の永続化ができませんでした。", exception.Message);
    }

    [TestMethod("FindAllでDBアクセスに失敗した場合は例外を投げる")]
    public void FindAll_WhenDbAccessFails_ThrowsException()
    {
        using var context = new TestAppDbContext
        {
            Employees = new ThrowingDbSet<EmployeeEntity>(),
            Departments = new QueryableDbSet<DepartmentEntity>([])
        };

        EmployeeRepository repository = CreateRepository(context);

        Exception exception = Assert.ThrowsException<Exception>(repository.FindAll);

        Assert.AreEqual("すべての従業員を取得できませんでした。", exception.Message);
    }

    [TestMethod("ExistsByEmailでDBアクセスに失敗した場合は例外を投げる")]
    public void ExistsByEmail_WhenDbAccessFails_ThrowsException()
    {
        using var context = new TestAppDbContext
        {
            Employees = new ThrowingDbSet<EmployeeEntity>(),
            Departments = new QueryableDbSet<DepartmentEntity>([])
        };
        EmployeeRepository repository = CreateRepository(context);

        Exception exception = Assert.ThrowsException<Exception>(
            () => repository.ExistsByEmail("yamada@example.com")
        );

        Assert.AreEqual("指定されたメールアドレスの従業員を確認できませんでした。", exception.Message);
    }

    [TestMethod("ExistsByPhoneでDBアクセスに失敗した場合は例外を投げる")]
    public void ExistsByPhone_WhenDbAccessFails_ThrowsException()
    {
        using var context = new TestAppDbContext
        {
            Employees = new ThrowingDbSet<EmployeeEntity>(),
            Departments = new QueryableDbSet<DepartmentEntity>([])
        };

        EmployeeRepository repository = CreateRepository(context);

        Exception exception = Assert.ThrowsException<Exception>(
            () => repository.ExistsByPhone("03-1234-5678")
        );

        Assert.AreEqual("指定された電話番号の従業員を確認できませんでした。", exception.Message);
    }

    [TestMethod("FindByIdでDBアクセスに失敗した場合は例外を投げる")]
    public void FindById_WhenDbAccessFails_ThrowsException()
    {
        using var context = new TestAppDbContext
        {
            Employees = new ThrowingDbSet<EmployeeEntity>(),
            Departments = new QueryableDbSet<DepartmentEntity>([])
        };
        EmployeeRepository repository = CreateRepository(context);

        Exception exception = Assert.ThrowsException<Exception>(() => repository.FindById(1));

        Assert.AreEqual("指定された従業員Idの従業員を取得できませんでした。", exception.Message);
    }

    [TestMethod("DeleteByIdでDBアクセスに失敗した場合は例外を投げる")]
    public void DeleteById_WhenDbAccessFails_ThrowsException()
    {
        using var context = new TestAppDbContext
        {
            Employees = new ThrowingDbSet<EmployeeEntity>(),
            Departments = new QueryableDbSet<DepartmentEntity>([])
        };
        EmployeeRepository repository = CreateRepository(context);

        Exception exception = Assert.ThrowsException<Exception>(() => repository.DeleteById(1));

        Assert.AreEqual("指定された従業員Idの従業員を削除できませんでした。", exception.Message);
    }

    private static EmployeeRepository CreateRepository(AppDbContext context)
    {
        return new EmployeeRepository(context, new EmployeeEntityAdapter());
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
        context.Database.EnsureCreated();

        string path = Path.Combine(AppContext.BaseDirectory, "sql", "init.sql");
        string sql = File.ReadAllText(path);
        context.Database.ExecuteSqlRaw(sql);

        return context;
    }
}
