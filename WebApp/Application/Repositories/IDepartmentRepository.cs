using WebApp.Application.Domains;
namespace WebApp.Application.Repositories;

/// ドメインオブジェクト:部署のCRUD操作インターフェイス
public interface IDepartmentRepository
{
    /// すべての部署を取得する
    IReadOnlyList<Department> FindAll();
    /// 指定された部署Idの部署を取得する
    Department? FindById(int id);
}
