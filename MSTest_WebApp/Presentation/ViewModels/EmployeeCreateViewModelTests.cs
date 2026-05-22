using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Application.Domains;
using WebApp.Presentation.ViewModels;

namespace MSTest_WebApp.Presentation.ViewModels;

[TestClass]
public class EmployeeCreateViewModelTests
{
    [TestMethod("未所属を先頭に追加し,Idなし部署を除外し,名称未設定を補完して部署一覧を設定する")]
    public void SetDepartments_FiltersOutDepartmentsWithoutId_AndUsesFallbackName()
    {
        EmployeeCreateViewModel viewModel = new();

        IReadOnlyList<Department> departments =
        [
            new Department(id: 1, name: "営業部"),
            new Department(id: 2, name: null),
            new Department(id: null, name: "未登録部署")
        ];

        viewModel.SetDepartments(departments);

        Assert.AreEqual(3, viewModel.Departments.Count);
        Assert.AreEqual(string.Empty, viewModel.Departments[0].Value);
        Assert.AreEqual("未所属", viewModel.Departments[0].Text);
        Assert.AreEqual("1", viewModel.Departments[1].Value);
        Assert.AreEqual("営業部", viewModel.Departments[1].Text);
        Assert.AreEqual("2", viewModel.Departments[2].Value);
        Assert.AreEqual("(名称未設定)", viewModel.Departments[2].Text);
    }
}
