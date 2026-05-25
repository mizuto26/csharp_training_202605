namespace WebApp.Presentation.ViewModels;

/// 部署一覧ViewModelクラス
public class DepartmentListViewModel
{
    /// 部署一覧
    public IReadOnlyList<DepartmentListItemViewModel> Departments { get; set; } = [];
}
