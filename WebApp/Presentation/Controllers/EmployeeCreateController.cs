using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Services;
using WebApp.Presentation.TempData;
using WebApp.Presentation.ViewModels;

namespace WebApp.Presentation.Controllers;

/// 従業員作成コントローラ
[Route("Employee")]
public class EmployeeCreateController(
    ILogger<EmployeeCreateController> logger,
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    EmployeeCreateViewModelAdapter employeeCreateViewModelAdapter,
    TempDataStore<EmployeeCreateViewModel> empDataStore)
: Controller
{
    /// ロガー
    private readonly ILogger<EmployeeCreateController> _logger = logger;
    /// 従業員サービスインターフェイス
    private readonly IEmployeeService _employeeService = employeeService;

    private readonly IDepartmentService _departmentService = departmentService;
    /// 従業員作成ViewModelをEmployeeに変換するアダプター
    private readonly EmployeeCreateViewModelAdapter _adapter = employeeCreateViewModelAdapter;
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    private readonly TempDataStore<EmployeeCreateViewModel> _empDataStore = empDataStore;

    /// 従業員作成画面表示 アクションメソッド
    [HttpGet("Create")]
    public IActionResult Create()
    {
        var viewModel = _empDataStore.Load(controller: this);
        viewModel ??= new EmployeeCreateViewModel();
        PopulateDepartments(viewModel: viewModel);

        return View(viewName: "Create", model: viewModel);
    }

    /// 従業員作成画面の[完了]ボタンクリックアクションメソッド
    [HttpPost("CreateConfirm")]
    public IActionResult CreateConfirm(EmployeeCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel: viewModel);
            return View(viewName: "Create", model: viewModel);
        }

        ValidateUniqueEmployee(viewModel: viewModel);

        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel: viewModel);
            return View(viewName: "Create", model: viewModel);
        }

        if (viewModel.DeptId is null)
        {
            viewModel.DeptName = "未所属";
        }
        else if (viewModel.DeptId is int departmentId)
        {
            var department = _departmentService.GetDepartmentById(departmentId);
            viewModel.DeptName = department.Name;
        }

        _logger.LogInformation(message: "{ViewModel}", args: viewModel.ToString());

        return View(viewName: "CreateConfirm", model: viewModel);
    }

    /// 従業員作成確認画面の[登録]ボタンクリックアクションメソッド
    [HttpPost("CreateExecute")]
    public IActionResult CreateExecute(EmployeeCreateViewModel viewModel)
    {
        ValidateUniqueEmployee(viewModel: viewModel);
        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel: viewModel);
            return View(viewName: "Create", model: viewModel);
        }

        _empDataStore.Save(controller: this, model: viewModel);

        return RedirectToAction(actionName: "CreateComplete");
    }

    /// 従業員作成処理GETアクションメソッド
    [HttpGet("CreateComplete")]
    public IActionResult CreateComplete()
    {
        var viewModel = _empDataStore.Load(controller: this);
        if (viewModel == null) return RedirectToAction("Create");

        var employee = _adapter.Restore(target: viewModel);
        _employeeService.Create(employee: employee);

        return View(viewName: "CreateComplete", model: viewModel);
    }

    /// 従業員作成確認画面の[戻る]ボタンクリックアクションメソッド
    [HttpPost("CreateBack")]
    public IActionResult CreateBack(EmployeeCreateViewModel viewModel)
    {
        _empDataStore.Save(controller: this, model: viewModel);

        return RedirectToAction(actionName: "Create");
    }

    /// 部署一覧を取得してViewModelに設定する(SelectListItem形式)
    private void PopulateDepartments(EmployeeCreateViewModel viewModel)
    {
        var departments = _departmentService.GetDepartments();
        viewModel.SetDepartments(departments: departments);
        _logger.LogInformation(message: "{ViewModel}", args: viewModel.ToString());
    }

    private void ValidateUniqueEmployee(EmployeeCreateViewModel viewModel)
    {
        if (_employeeService.ExistsByEmail(email: viewModel.Email ?? string.Empty))
        {
            ModelState.AddModelError(
                key: nameof(EmployeeCreateViewModel.Email),
                errorMessage: "同じメールアドレスの従業員が既に存在します。"
            );
        }

        if (_employeeService.ExistsByPhone(phone: viewModel.Phone ?? string.Empty))
        {
            ModelState.AddModelError(
                key: nameof(EmployeeCreateViewModel.Phone),
                errorMessage: "同じ電話番号の従業員が既に存在します。"
            );
        }
    }
}
