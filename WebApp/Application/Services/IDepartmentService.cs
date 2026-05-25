using WebApp.Application.Domains;

namespace WebApp.Application.Services;

/// 部署サービスインターフェイス
public interface IDepartmentService
{
    /// すべての部署を取得する
    IReadOnlyList<Department> GetDepartments();
    /// 指定された部署Idの部署を取得する
    Department GetDepartmentById(int id);
    /// 新しい部署を登録する
    void Create(Department department);
    /// 指定された部署名の部署が存在するか確認する
    bool ExistsByName(string name);
}
