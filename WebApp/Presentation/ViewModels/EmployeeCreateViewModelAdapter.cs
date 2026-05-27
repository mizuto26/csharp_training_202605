using WebApp.Application.Adapters;
using WebApp.Application.Domains;
namespace WebApp.Presentation.ViewModels;

/// EmployeeCreateViewModel(従業員登録ViewModel)を
/// ドメインオブジェクトEmployeeに変換するアダプターインターフェイスの実装
public class EmployeeCreateViewModelAdapter
: IRestorer<Employee, EmployeeCreateViewModel>
{
    /// EmployeeCreateViewModelをドメインオブジェクトEmployeeに変換する
    public Employee Restore(EmployeeCreateViewModel target)
    {
        if (string.IsNullOrWhiteSpace(target.Name)) throw new InvalidOperationException("氏名が設定されていません。");
        if (string.IsNullOrWhiteSpace(target.Phone)) throw new InvalidOperationException("電話番号が設定されていません。");
        if (string.IsNullOrWhiteSpace(target.Email)) throw new InvalidOperationException("メールアドレスが設定されていません。");

        Department? department = null;

        if (target.DeptId is int departmentId)
        {
            department = new Department(
                id: departmentId,
                name: target.DeptName
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
