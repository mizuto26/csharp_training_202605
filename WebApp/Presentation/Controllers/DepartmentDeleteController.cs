using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Domains;
using WebApp.Application.Services;
using WebApp.Presentation.TempData;
using WebApp.Presentation.ViewModels;

namespace WebApp.Presentation.Controllers;

/// 部署削除コントローラ
[Route("Departments")]
public class DepartmentDeleteController(
    ILogger<DepartmentDeleteController> logger,
    IDepartmentService departmentService,
    TempDataStore<DepartmentDeleteViewModel> departmentDeleteDataStore)
: Controller
{
    /// ロガー
    private readonly ILogger<DepartmentDeleteController> _logger = logger;
    /// 部署サービスインターフェイス
    private readonly IDepartmentService _departmentService = departmentService;
    /// TempDataを通じて一時的に削除ViewModelを保存・復元するためのクラス
    private readonly TempDataStore<DepartmentDeleteViewModel> _departmentDeleteDataStore = departmentDeleteDataStore;

    /// 部署削除画面の[確認]ボタンクリックアクションメソッド
    [HttpPost("DeleteConfirm")]
    public IActionResult DeleteConfirm(DepartmentDeleteViewModel viewModel)
    {
        int departmentId = viewModel.DepartmentId;

        ValidateDeletableDepartment(departmentId: departmentId);

        _logger.LogInformation(message: "{ViewModel}", args: viewModel.ToString());

        return View(viewName: "DeleteConfirm", model: viewModel);
    }

    /// 部署削除確認画面の[削除]ボタンクリックアクションメソッド
    [HttpPost("DeleteExecute")]
    public IActionResult DeleteExecute(DepartmentDeleteViewModel viewModel)
    {
        ValidateDeletableDepartment(departmentId: viewModel.DepartmentId);
        if (ModelState.IsValid is false)
        {
            return View(viewName: "DeleteConfirm", model: viewModel);
        }

        _departmentDeleteDataStore.Save(controller: this, model: viewModel);

        return RedirectToAction(actionName: "DeleteComplete");
    }

    /// 部署削除確認画面の[戻る]ボタンクリックアクションメソッド
    [HttpPost("DeleteBack")]
    public IActionResult DeleteBack()
    {
        return RedirectToAction(actionName: "Departments", controllerName: "DepartmentList");
    }

    /// 部署削除処理GETアクションメソッド
    [HttpGet("DeleteComplete")]
    public IActionResult DeleteComplete()
    {
        DepartmentDeleteViewModel? viewModel = _departmentDeleteDataStore.Load(controller: this);
        if (viewModel is null)
        {
            return RedirectToAction(actionName: "Departments", controllerName: "DepartmentList");
        }

        _departmentService.DeleteById(id: viewModel.DepartmentId);

        return View(viewName: "DeleteComplete", model: viewModel);
    }

    private void ValidateDeletableDepartment(int departmentId)
    {
        if (_departmentService.ExistsEmployeeByDepartmentId(departmentId: departmentId) is false) return;

        ModelState.AddModelError(
            key: string.Empty,
            errorMessage: string.Empty
        );
    }
}
