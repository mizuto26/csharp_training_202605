using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest_WebApp.Tests.TestDoubles;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Context;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Repositories;

namespace WebApp_Sample.Tests.Infrastructures.Repositories;

[TestClass]
public class EmployeeRepositoryTests
{
    [TestMethod("CreateでDBに従業員Entityが追加される")]
    public void Create_AddsEmployeeEntity()
    {
        using var context = CreateContext([], []);
        EmployeeRepository repository = CreateRepository(context);

        Department department = new(id: 10, name: "営業部");

        Employee employee = new(name: "山田", email: "yamada@example.com", phone: "03-1234-5678", department: department);

        bool created = repository.Create(employee);

        IReadOnlyList<EmployeeEntity> employeeEntities = ((QueryableDbSet<EmployeeEntity>)context.Employees).Entities;

        Assert.IsTrue(created);
        Assert.AreEqual(1, employeeEntities.Count);
        Assert.AreEqual("山田", employeeEntities[0].EmpName);
        Assert.AreEqual("yamada@example.com", employeeEntities[0].EmpEmail);
        Assert.AreEqual("03-1234-5678", employeeEntities[0].EmpPhone);
        Assert.AreEqual(10, employeeEntities[0].DeptId);
    }

    [TestMethod("従業員一覧を取得できる")]
    public void FindAll_ReturnsEmployeesOrderedByIdWithDepartments()
    {
        List<EmployeeEntity> employeeEntities =
        [
            new EmployeeEntity { EmpId = 1, EmpName = "山田", EmpEmail = "yamada@example.com", EmpPhone = "03-1234-5678", DeptId = 1 },
            new EmployeeEntity { EmpId = 2, EmpName = "鈴木", EmpEmail = "suzuki@example.com", EmpPhone = "090-1111-2222", DeptId = null }
        ];

        List<DepartmentEntity> departmentEntities =
        [
            new DepartmentEntity { DeptId = 1, DeptName = "営業部" },
        ];

        using var context = CreateContext(employeeEntities, departmentEntities);
        EmployeeRepository repository = CreateRepository(context);

        IReadOnlyList<Employee> employees = repository.FindAll();

        Assert.AreEqual(2, employees.Count);
        Assert.AreEqual(1, employees[0].Id);
        Assert.AreEqual("山田", employees[0].Name);
        Assert.AreEqual("yamada@example.com", employees[0].Email);
        Assert.AreEqual("03-1234-5678", employees[0].Phone);
        Assert.AreEqual(1, employees[0].Department?.Id);
        Assert.AreEqual("営業部", employees[0].Department?.Name);

        Assert.AreEqual(2, employees[1].Id);
        Assert.AreEqual("suzuki@example.com", employees[1].Email);
        Assert.AreEqual("090-1111-2222", employees[1].Phone);
        Assert.IsNull(employees[1].Department);
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
}
