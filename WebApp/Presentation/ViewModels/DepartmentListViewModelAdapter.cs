using WebApp.Application.Adapters;
using WebApp.Application.Domains;

namespace WebApp.Presentation.ViewModels;

/// ドメインオブジェクト:Departmentのリストを
/// 部署一覧ViewModelに変換するアダプターインターフェイスの実装
public class DepartmentListViewModelAdapter
: IRestorer<DepartmentListViewModel, IReadOnlyList<Department>>
{
    /// DepartmentのリストをDepartmentListViewModelに変換する
    public DepartmentListViewModel Restore(IReadOnlyList<Department> target)
    {
        DepartmentListViewModel viewModel = new()
        {
            Departments = target
                .Select(department => new DepartmentListItemViewModel
                {
                    DepartmentId = department.Id ?? throw new InvalidOperationException(message: "部署IDが未設定です。"),
                    DepartmentName = department.Name
                })
                .ToList()
        };

        return viewModel;
    }
}
