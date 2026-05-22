using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestMethod("部署一覧はId順で取得できる")]
    public void FindAll_ReturnsDepartmentsOrderedById()
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

    [Description("Createで部署Entityが追加される")]
    [TestMethod]
    public void Create_AddsDepartmentEntity()
    {
        List<DepartmentEntity> departmentEntities = [];
        using var context = CreateContext([], departmentEntities);

        Department department = new(name: "営業部");
        DepartmentRepository repository = CreateRepository(context);

        bool created = repository.Create(department);

        IReadOnlyList<DepartmentEntity> savedDepartments =
            ((QueryableDbSet<DepartmentEntity>)context.Departments).Entities;

        Assert.IsTrue(created);
        Assert.AreEqual(1, savedDepartments.Count);
        Assert.AreEqual("営業部", savedDepartments[0].DeptName);
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
}
