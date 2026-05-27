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
        DepartmentCreateViewModel? viewModel = _deptDataStore.Load(this);
        viewModel ??= new DepartmentCreateViewModel();

        return View("Create", viewModel);
    }

    /// 部署登録画面の[確認]ボタンクリックアクションメソッド
    [HttpPost("CreateConfirm")]
    public IActionResult CreateConfirm(DepartmentCreateViewModel viewModel)
    {
        if (!ModelState.IsValid) return View("Create", viewModel);

        ValidateDepartmentName(viewModel.DepartmentName);
        if (!ModelState.IsValid) return View("Create", viewModel);

        return View("CreateConfirm", viewModel);
    }

    /// 部署登録確認画面の[登録]ボタンクリックアクションメソッド
    [HttpPost("CreateExecute")]
    public IActionResult CreateExecute(DepartmentCreateViewModel viewModel)
    {
        ValidateDepartmentName(viewModel.DepartmentName);
        if (!ModelState.IsValid) return View("Create", viewModel);

        _deptDataStore.Save(this, viewModel);

        return RedirectToAction("CreateComplete");
    }

    /// 部署登録完了画面表示 アクションメソッド
    [HttpGet("CreateComplete")]
    public IActionResult CreateComplete()
    {
        DepartmentCreateViewModel? viewModel = _deptDataStore.Load(this);
        if (viewModel is null) return RedirectToAction("Create");

        Department department = new(viewModel.DepartmentName);

        _departmentService.Create(department);

        return View("CreateComplete", viewModel);
    }

    /// 部署登録確認画面の[戻る]ボタンクリックアクションメソッド
    [HttpPost("CreateBack")]
    public IActionResult CreateBack(DepartmentCreateViewModel viewModel)
    {
        _deptDataStore.Save(this, viewModel);

        return RedirectToAction("Create");
    }

    private void ValidateDepartmentName(string name)
    {
        bool existsByEmail = _departmentService.ExistsByName(name);

        if (!existsByEmail) return;

        ModelState.AddModelError(key: nameof(DepartmentCreateViewModel.DepartmentName), errorMessage: "同じ部署名の部署が既に存在します。");
    }
}
