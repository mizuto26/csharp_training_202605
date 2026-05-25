using WebApp.Application.Domains;
namespace WebApp.Application.Repositories;

/// ドメインオブジェクト:従業員のCRUD操作インターフェイス
public interface IEmployeeRepository
{
    /// 従業員を永続化する
    bool Create(Employee employee);
    /// すべての従業員を取得する
    IReadOnlyList<Employee> FindAll();
    /// 指定されたメールアドレスの従業員が存在するか確認する
    bool ExistsByEmail(string email);
    /// 指定された電話番号の従業員が存在するか確認する
    bool ExistsByPhone(string phone);
}
