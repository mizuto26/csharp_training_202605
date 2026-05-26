using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Domains;
using WebApp.Application.Services;
using WebApp.Presentation.TempData;
using WebApp.Presentation.ViewModels;

namespace WebApp.Presentation.Controllers;

/// 従業員削除コントローラ
[Route("Employee")]
public class EmployeeDeleteController(
    ILogger<EmployeeDeleteController> logger,
    IEmployeeService employeeService,
    TempDataStore<EmployeeDeleteViewModel> employeeDeleteDataStore)
: Controller
{
    /// ロガー
    private readonly ILogger<EmployeeDeleteController> _logger = logger;
    /// 従業員サービスインターフェイス
    private readonly IEmployeeService _employeeService = employeeService;
    /// TempDataを通じて一時的に削除ViewModelを保存・復元するためのクラス
    private readonly TempDataStore<EmployeeDeleteViewModel> _employeeDeleteDataStore = employeeDeleteDataStore;

    /// 従業員削除画面の[確認]ボタンクリックアクションメソッド
    [HttpPost("DeleteConfirm")]
    public IActionResult DeleteConfirm(EmployeeDeleteViewModel viewModel)
    {

        _logger.LogInformation(message: "{ViewModel}", args: viewModel.ToString());

        return View(viewName: "DeleteConfirm", model: viewModel);
    }

    /// 従業員削除確認画面の[削除]ボタンクリックアクションメソッド
    [HttpPost("DeleteExecute")]
    public IActionResult DeleteExecute(EmployeeDeleteViewModel viewModel)
    {
        _employeeDeleteDataStore.Save(controller: this, model: viewModel);

        return RedirectToAction(actionName: "DeleteComplete");
    }

    /// 従業員削除確認画面の[戻る]ボタンクリックアクションメソッド
    [HttpPost("DeleteBack")]
    public IActionResult DeleteBack()
    {
        return RedirectToAction(actionName: "Employees", controllerName: "EmployeeList");
    }

    /// 従業員削除処理GETアクションメソッド
    [HttpGet("DeleteComplete")]
    public IActionResult DeleteComplete()
    {
        EmployeeDeleteViewModel? viewModel = _employeeDeleteDataStore.Load(controller: this);

        if (viewModel is null)
        {
            return RedirectToAction(actionName: "Employees", controllerName: "EmployeeList");
        }

        _employeeService.DeleteById(id: viewModel.EmployeeId);

        return View(viewName: "DeleteComplete", model: viewModel);
    }
}
