using WebApp.Application.Domains;
namespace WebApp.Application.Services;

/// 従業員登録サービスインターフェイス
public interface IEmployeeService
{
    /// 新しい従業員を登録する
    void Create(Employee employee);
    /// すべての従業員を取得する
    IReadOnlyList<Employee> GetEmployees();
    /// 指定されたメールアドレスの従業員が存在するか確認する
    bool ExistsByEmail(string email);
    /// 指定された電話番号の従業員が存在するか確認する
    bool ExistsByPhone(string phone);
    /// 指定された従業員Idの従業員を取得する
    void DeleteById(int id);
}
