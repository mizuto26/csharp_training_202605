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
        List<EmployeeListItemViewModel> employees = [];

        foreach (var employee in target)
        {
            int employeeId = employee.Id ?? throw new InvalidOperationException("従業員IDが未設定です。");

            int? departmentId = null;
            string departmentName = "未所属";

            Department? department = employee.Department;

            if (department is not null)
            {
                departmentId = department.Id;
                departmentName = department.Name;
            }

            employees.Add(new EmployeeListItemViewModel
            {
                EmployeeId = employeeId,
                EmployeeName = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                DepartmentId = departmentId,
                DepartmentName = departmentName
            });
        }

        EmployeeListViewModel viewModel = new()
        {
            Employees = employees
        };

        return viewModel;
    }
}
