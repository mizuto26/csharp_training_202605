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
        if (target.DeptId is null)
            throw new InvalidOperationException("部署Idが設定されていません。");
        if (string.IsNullOrWhiteSpace(target.Name))
            throw new InvalidOperationException("氏名が設定されていません。");

        // Department(部署)を作成する
        Department? department = new(
            id: target.DeptId.Value,
            name: target.DeptName
        );

        // 登録するEmployee(従業員)を作成する
        Employee? employee = new(
            name: target.Name,
            phone: target.Phone,
            mail: target.Mail,
            department: department
        );

        return employee;
    }
}
