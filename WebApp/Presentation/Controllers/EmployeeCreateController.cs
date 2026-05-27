using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Domains;
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
        EmployeeCreateViewModel? viewModel = _empDataStore.Load(this);
        viewModel ??= new EmployeeCreateViewModel();
        PopulateDepartments(viewModel);

        return View("Create", viewModel);
    }

    /// 従業員作成画面の[完了]ボタンクリックアクションメソッド
    [HttpPost("CreateConfirm")]
    public IActionResult CreateConfirm(EmployeeCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel);
            return View("Create", viewModel);
        }

        ValidateEmployee(viewModel);

        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel);
            return View("Create", viewModel);
        }

        if (viewModel.DeptId is not int departmentId)
        {
            viewModel.DeptName = "未所属";
        }
        else
        {
            Department? department = _departmentService.FindDepartmentById(departmentId);

            if (department is null)
            {
                ModelState.AddModelError(key: nameof(EmployeeCreateViewModel.DeptId), errorMessage: "選択された部署は存在しません。");
                PopulateDepartments(viewModel);
                return View("Create", viewModel);
            }

            viewModel.DeptName = department.Name;
        }

        _logger.LogInformation("{ViewModel}", viewModel.ToString());

        return View("CreateConfirm", viewModel);
    }

    /// 従業員作成確認画面の[登録]ボタンクリックアクションメソッド
    [HttpPost("CreateExecute")]
    public IActionResult CreateExecute(EmployeeCreateViewModel viewModel)
    {
        ValidateEmployee(viewModel);

        if (!ModelState.IsValid)
        {
            PopulateDepartments(viewModel);
            return View("Create", viewModel);
        }

        _empDataStore.Save(this, viewModel);

        return RedirectToAction("CreateComplete");
    }

    /// 従業員作成処理GETアクションメソッド
    [HttpGet("CreateComplete")]
    public IActionResult CreateComplete()
    {
        EmployeeCreateViewModel? viewModel = _empDataStore.Load(this);
        if (viewModel is null) return RedirectToAction("Create");

        Employee employee = _adapter.Restore(viewModel);
        _employeeService.Create(employee);

        return View("CreateComplete", viewModel);
    }

    /// 従業員作成確認画面の[戻る]ボタンクリックアクションメソッド
    [HttpPost("CreateBack")]
    public IActionResult CreateBack(EmployeeCreateViewModel viewModel)
    {
        _empDataStore.Save(this, viewModel);

        return RedirectToAction("Create");
    }

    /// 部署一覧を取得してViewModelに設定する(SelectListItem形式)
    private void PopulateDepartments(EmployeeCreateViewModel viewModel)
    {
        IReadOnlyList<Department> departments = _departmentService.GetDepartments();
        viewModel.SetDepartments(departments);
        _logger.LogInformation("{ViewModel}", viewModel.ToString());
    }

    private void ValidateEmployee(EmployeeCreateViewModel viewModel)
    {
        bool existsByEmail = _employeeService.ExistsByEmail(viewModel.Email);

        if (existsByEmail)
        {
            ModelState.AddModelError(key: nameof(EmployeeCreateViewModel.Email), errorMessage: "同じメールアドレスの従業員が既に存在します。");
        }

        bool existsByPhone = _employeeService.ExistsByPhone(viewModel.Phone);

        if (existsByPhone)
        {
            ModelState.AddModelError(key: nameof(EmployeeCreateViewModel.Phone), errorMessage: "同じ電話番号の従業員が既に存在します。");
        }
    }
}
