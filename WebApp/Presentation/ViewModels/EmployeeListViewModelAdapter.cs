using WebApp.Application.Adapters;
using WebApp.Application.Domains;

namespace WebApp.Presentation.ViewModels;

/// Employeeのリストを従業員一覧ViewModelに変換するアダプターの実装
public class EmployeeListViewModelAdapter
: IRestorer<EmployeeListViewModel, IReadOnlyList<Employee>>
{
    /// EmployeeのリストをEmployeeListViewModelに変換する
    public EmployeeListViewModel Restore(IReadOnlyList<Employee> target)
    {
        EmployeeListViewModel viewModel = new()
        {
            Employees = target
                .Select(employee => new EmployeeListItemViewModel
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.Name,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    DepartmentId = employee.Department?.Id,
                    DepartmentName = employee.Department?.Name ?? "未所属"
                })
                .ToList()
        };

        return viewModel;
    }
}
