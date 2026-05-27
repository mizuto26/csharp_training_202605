using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Domains;
using WebApp.Application.Services;
using WebApp.Presentation.ViewModels;

namespace WebApp.Presentation.Controllers;

/// 従業員一覧コントローラ
[Route("Employee")]
public class EmployeeListController(
    IEmployeeService employeeService,
    EmployeeListViewModelAdapter employeeListViewModelAdapter)
: Controller
{
    /// 従業員サービスインターフェイス
    private readonly IEmployeeService _employeeService = employeeService;
    /// 従業員一覧ViewModelを作成するアダプター
    private readonly EmployeeListViewModelAdapter _employeeListViewModelAdapter = employeeListViewModelAdapter;

    /// 従業員一覧画面表示 アクションメソッド
    [HttpGet("Employees")]
    public IActionResult Employees()
    {
        IReadOnlyList<Employee> employees = _employeeService.GetEmployees();
        EmployeeListViewModel viewModel = _employeeListViewModelAdapter.Restore(employees);

        return View(viewModel);
    }
}
