using WebApp.Application.Adapters;
using WebApp.Application.Domains;
namespace WebApp.Presentation.ViewModels;

/// EmployeeCreateViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Employeeに変換するアダプターインターフェイスの実装
public class EmployeeCreateViewModelAdapter : IRestorer<Employee, EmployeeCreateViewModel>
{
    /// EmployeeCreateViewModelをドメインオブジェクト:Employeeに変換する
    public Employee Restore(EmployeeCreateViewModel target)
    {
        if (string.IsNullOrWhiteSpace(target.Name))
            throw new InvalidOperationException(message: "氏名が設定されていません。");
        if (string.IsNullOrWhiteSpace(target.Phone))
            throw new InvalidOperationException(message: "電話番号が設定されていません。");
        if (string.IsNullOrWhiteSpace(target.Email))
            throw new InvalidOperationException(message: "メールアドレスが設定されていません。");

        Department? department = null;
        if (target.DeptId != null)
        {
            department = new Department(
                id: target.DeptId,
                name: target.DeptName ?? string.Empty
            );
        }

        // 登録するEmployee(従業員)を作成する
        Employee employee = new(
            name: target.Name,
            phone: target.Phone,
            email: target.Email,
            department: department
        );

        return employee;
    }
}
