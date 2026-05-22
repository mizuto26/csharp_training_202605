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
    [TestMethod("Createで従業員Entityが追加される")]
    public void Create_AddsEmployeeEntity()
    {
        using var context = CreateContext([], []);
        Department department = new(
            id: 10,
            name: "営業部"
        );

        Employee employee = new(
            name: "山田",
            email: "yamada@example.com",
            phone: "03-1234-5678",
            department: department
        );

        EmployeeRepository repository = CreateRepository(context);

        bool created = repository.Create(employee);

        IReadOnlyList<EmployeeEntity> employeeEntities =
            ((QueryableDbSet<EmployeeEntity>)context.Employees).Entities;

        Assert.IsTrue(created);
        Assert.AreEqual(1, employeeEntities.Count);
        Assert.AreEqual("山田", employeeEntities[0].EmpName);
        Assert.AreEqual("yamada@example.com", employeeEntities[0].EmpEmail);
        Assert.AreEqual("03-1234-5678", employeeEntities[0].EmpPhone);
        Assert.AreEqual(10, employeeEntities[0].DeptId);
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
