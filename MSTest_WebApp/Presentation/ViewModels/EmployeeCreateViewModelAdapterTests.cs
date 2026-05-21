using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Presentation.ViewModels;

namespace MSTest_WebApp.Presentation.ViewModels;

[TestClass]
public class EmployeeCreateViewModelAdapterTests
{
    [TestMethod]
    public void Restore_WithoutDepartmentId_ThrowsInvalidOperationException()
    {
        EmployeeCreateViewModel viewModel = new()
        {
            Name = "山田太郎",
            DeptId = null
        };
        EmployeeCreateViewModelAdapter adapter = new();

        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(() =>
            adapter.Restore(viewModel));

        Assert.AreEqual("部署Idが設定されていません。", exception.Message);
    }

    [TestMethod]
    public void Restore_WithoutName_ThrowsInvalidOperationException()
    {
        EmployeeCreateViewModel viewModel = new()
        {
            Name = " ",
            DeptId = 3
        };
        EmployeeCreateViewModelAdapter adapter = new();

        InvalidOperationException exception = Assert.ThrowsException<InvalidOperationException>(() =>
            adapter.Restore(viewModel));

        Assert.AreEqual("氏名が設定されていません。", exception.Message);
    }
}
