using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Domains;
using WebApp.Application.Services;
using WebApp.Presentation.TempData;
using WebApp.Presentation.ViewModels;

namespace WebApp.Presentation.Controllers;

/// 部署コントローラ
[Route("Department")]
public class DepartmentCreateController(
    IDepartmentService departmentService,
    TempDataStore<DepartmentCreateViewModel> deptDataStore)
: Controller
{
    /// 部署サービスインターフェイス
    private readonly IDepartmentService _departmentService = departmentService;
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    private readonly TempDataStore<DepartmentCreateViewModel> _deptDataStore = deptDataStore;

    /// 部署登録画面表示 アクションメソッド
    [HttpGet("Create")]
    public IActionResult Create()
    {
        var viewModel = _deptDataStore.Load(controller: this);
        viewModel ??= new DepartmentCreateViewModel();

        return View(viewName: "Create", model: viewModel);
    }

    /// 部署登録画面の[確認]ボタンクリックアクションメソッド
    [HttpPost("CreateConfirm")]
    public IActionResult CreateConfirm(DepartmentCreateViewModel viewModel)
    {
        if (ModelState.IsValid is false) return View(viewName: "Create", model: viewModel);

        ValidateDepartmentName(name: viewModel.DepartmentName);
        if (ModelState.IsValid is false) return View(viewName: "Create", model: viewModel);

        return View(viewName: "CreateConfirm", model: viewModel);
    }

    /// 部署登録確認画面の[登録]ボタンクリックアクションメソッド
    [HttpPost("CreateExecute")]
    public IActionResult CreateExecute(DepartmentCreateViewModel viewModel)
    {
        ValidateDepartmentName(name: viewModel.DepartmentName);
        if (ModelState.IsValid is false) return View(viewName: "Create", model: viewModel);

        _deptDataStore.Save(controller: this, model: viewModel);

        return RedirectToAction(actionName: "CreateComplete");
    }

    /// 部署登録完了画面表示 アクションメソッド
    [HttpGet("CreateComplete")]
    public IActionResult CreateComplete()
    {
        var viewModel = _deptDataStore.Load(controller: this);
        if (viewModel is null) return RedirectToAction(actionName: "Create");

        Department department = new(name: viewModel.DepartmentName);
        _departmentService.Create(department);

        return View(viewName: "CreateComplete", model: viewModel);
    }

    /// 部署登録確認画面の[戻る]ボタンクリックアクションメソッド
    [HttpPost("CreateBack")]
    public IActionResult CreateBack(DepartmentCreateViewModel viewModel)
    {
        _deptDataStore.Save(controller: this, model: viewModel);

        return RedirectToAction(actionName: "Create");
    }

    private void ValidateDepartmentName(string name)
    {
        if (_departmentService.ExistsByName(name) is false) return;

        ModelState.AddModelError(
            key: nameof(DepartmentCreateViewModel.DepartmentName),
            errorMessage: "同じ部署名の部署が既に存在します。"
        );
    }
}
