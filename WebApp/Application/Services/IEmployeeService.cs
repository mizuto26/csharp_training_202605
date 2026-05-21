using WebApp.Application.Domains;
namespace WebApp.Application.Services;

/// 従業員登録サービスインターフェイス
public interface IEmployeeService
{
    /// 新しい従業員を登録する
    void Create(Employee employee);
}
