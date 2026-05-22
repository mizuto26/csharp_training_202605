using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Entities;

namespace MSTest_WebApp.Infrastructure.Adapters;

[TestClass]
public class EmployeeEntityAdapterTests
{
    [TestMethod("部署ありの従業員はすべてのプロパティをEntityへ変換する")]
    public void Convert_WithDepartment_MapsAllProperties()
    {
        EmployeeEntityAdapter adapter = new();
        Employee employee = new(
            id: 1,
            name: "山田太郎",
            email: "yamada@example.com",
            phone: "03-1234-5678",
            department: new Department(id: 10, name: "営業部")
        );

        EmployeeEntity entity = adapter.Convert(employee);

        Assert.AreEqual(1, entity.EmpId);
        Assert.AreEqual("山田太郎", entity.EmpName);
        Assert.AreEqual("yamada@example.com", entity.EmpEmail);
        Assert.AreEqual("03-1234-5678", entity.EmpPhone);
        Assert.AreEqual(10, entity.DeptId);
    }

    [TestMethod("部署なしの従業員はDeptIdがnullのまま変換される")]
    public void Convert_WithoutDepartment_LeavesDeptIdNull()
    {
        EmployeeEntityAdapter adapter = new();
        Employee employee = new(
            id: null,
            name: "山田太郎",
            email: "yamada@example.com",
            phone: "03-1234-5678",
            department: null
        );

        EmployeeEntity entity = adapter.Convert(employee);

        Assert.AreEqual(0, entity.EmpId);
        Assert.AreEqual("山田太郎", entity.EmpName);
        Assert.AreEqual("yamada@example.com", entity.EmpEmail);
        Assert.AreEqual("03-1234-5678", entity.EmpPhone);
        Assert.IsNull(entity.DeptId);
    }
}
