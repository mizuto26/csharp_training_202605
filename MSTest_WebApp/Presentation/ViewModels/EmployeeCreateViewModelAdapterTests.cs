using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Presentation.ViewModels;

namespace MSTest_WebApp.Presentation.ViewModels;

[TestClass]
public class EmployeeCreateViewModelAdapterTests
{
    [TestMethod("部署idを設定していない場合")]
    public void Restore_WithoutDepartmentId_ThrowsException()
    {
        Exception exception;
        EmployeeCreateViewModel viewModel = new()
        {
            Name = "山田太郎",
            DeptId = null
        };
        EmployeeCreateViewModelAdapter adapter = new();

        try
        {
            adapter.Restore(viewModel);
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("部署Idが設定されていません。", exception.Message);
    }

    [TestMethod("氏名を設定していない場合")]
    public void Restore_WithoutName_ThrowsException()
    {
        Exception exception;
        EmployeeCreateViewModel viewModel = new()
        {
            Name = " ",
            DeptId = 3
        };

        EmployeeCreateViewModelAdapter adapter = new();

        try
        {
            adapter.Restore(viewModel);
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("氏名が設定されていません。", exception.Message);
    }
}
