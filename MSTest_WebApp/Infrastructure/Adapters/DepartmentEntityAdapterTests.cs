using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Entities;

namespace MSTest_WebApp.Infrastructure.Adapters;

[TestClass]
public class DepartmentEntityAdapterTests
{
    [TestMethod("部署EntityからDomainへ復元できる")]
    public void Restore_MapsEntityToDomain()
    {
        DepartmentEntityAdapter adapter = new();

        DepartmentEntity entity = new()
        {
            DeptId = 5,
            DeptName = "総務部"
        };

        Department department = adapter.Restore(entity);

        Assert.AreEqual(5, department.Id);
        Assert.AreEqual("総務部", department.Name);
    }
}
