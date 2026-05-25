using WebApp.Application.Domains;
namespace WebApp.Application.Repositories;

/// ドメインオブジェクト:部署のCRUD操作インターフェイス
public interface IDepartmentRepository
{
    /// すべての部署を取得する
    IReadOnlyList<Department> FindAll();
    /// 指定された部署Idの部署を取得する
    Department? FindById(int id);
    /// 部署を永続化する
    bool Create(Department department);
    /// 指定された部署名の部署が存在するか確認する
    bool ExistsByName(string name);
    /// 指定された部署Idの部署を削除する
    bool DeleteById(int id);
    /// 指定された部署Idを持つ従業員が存在するか確認する
    bool ExistsEmployeeByDepartmentId(int departmentId);
}
