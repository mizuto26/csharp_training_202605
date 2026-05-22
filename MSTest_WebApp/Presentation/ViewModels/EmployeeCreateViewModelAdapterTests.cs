using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Presentation.ViewModels;

namespace MSTest_WebApp.Presentation.ViewModels;


[TestClass]
public class EmployeeCreateViewModelAdapterTests
{
    [TestMethod("正しい入力の作成ViewModelは従業員へ変換できる")]
    public void Restore_WithValidViewModel_ReturnsEmployee()
    {
        EmployeeCreateViewModel viewModel = new()
        {
            Name = "山田太郎",
            Email = "yamada@example.com",
            Phone = "03-1234-5678",
            DeptId = 3,
            DeptName = "営業部"
        };
        EmployeeCreateViewModelAdapter adapter = new();

        var employee = adapter.Restore(viewModel);

        Assert.IsNull(employee.Id);
        Assert.AreEqual("山田太郎", employee.Name);
        Assert.AreEqual("yamada@example.com", employee.Email);
        Assert.AreEqual("03-1234-5678", employee.Phone);
        Assert.IsNotNull(employee.Department);
        Assert.AreEqual(3, employee.Department!.Id);
        Assert.AreEqual("営業部", employee.Department.Name);
    }
}
