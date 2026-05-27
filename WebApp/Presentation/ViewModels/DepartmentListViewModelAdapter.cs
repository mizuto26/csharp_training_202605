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
        List<DepartmentListItemViewModel> departments = [];

        foreach (var department in target)
        {
            int departmentId = department.Id ?? throw new InvalidOperationException(message: "部署IDが未設定です。");

            departments.Add(item: new DepartmentListItemViewModel
            {
                DepartmentId = departmentId,
                DepartmentName = department.Name
            });
        }

        DepartmentListViewModel viewModel = new()
        {
            Departments = departments
        };

        return viewModel;
    }
}
