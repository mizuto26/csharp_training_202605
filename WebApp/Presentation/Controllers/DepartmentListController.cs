using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Domains;
using WebApp.Application.Services;
using WebApp.Presentation.ViewModels;

namespace WebApp.Presentation.Controllers;

[Route("Departments")]
public class DepartmentListController(
    IDepartmentService departmentService,
    DepartmentListViewModelAdapter departmentListViewModelAdapter)
: Controller
{
    /// 部署サービスインターフェイス
    private readonly IDepartmentService _departmentService = departmentService;
    /// 部署一覧ViewModelを作成するアダプター
    private readonly DepartmentListViewModelAdapter _departmentListViewModelAdapter = departmentListViewModelAdapter;

    /// 部署一覧画面表示 アクションメソッド
    [HttpGet("Departments")]
    public IActionResult Departments()
    {
        var viewModel = CreateViewModel();

        return View(model: viewModel);
    }

    private DepartmentListViewModel CreateViewModel()
    {
        var departments = _departmentService.GetDepartments();

        return _departmentListViewModelAdapter.Restore(target: departments);
    }
}
