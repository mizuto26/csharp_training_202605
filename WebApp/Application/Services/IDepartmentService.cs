using WebApp.Application.Domains;

namespace WebApp.Application.Services;

/// 部署サービスインターフェイス
public interface IDepartmentService
{
    /// すべての部署を取得する
    IReadOnlyList<Department> GetDepartments();

    /// 指定された部署Idの部署を取得する
    Department GetDepartmentById(int id);
}