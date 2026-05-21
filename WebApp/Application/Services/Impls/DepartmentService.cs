using WebApp.Application.Domains;
using WebApp.Application.Repositories;
using WebApp.Infrastructure.Context;

namespace WebApp.Application.Services.Impls;

/// 部署サービスインターフェイスの実装
public class DepartmentService(
    AppDbContext context,
    IDepartmentRepository departmentRepository)
: IDepartmentService
{
    private readonly AppDbContext _context = context;
    /// 部署リポジトリ
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    /// すべての部署を取得する
    public IReadOnlyList<Department> GetDepartments()
    {
        return _departmentRepository.FindAll();
    }

    /// 指定された部署Idの部署を取得する
    public Department GetDepartmentById(int id)
    {
        return _departmentRepository.FindById(id: id)
            ?? throw new Exception(message: $"部署Id{id}に該当する部署は存在しません");
    }
}
