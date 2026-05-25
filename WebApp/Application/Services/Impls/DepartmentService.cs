using Microsoft.EntityFrameworkCore.Storage;
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

    /// 指定された部署名の部署が存在するか確認する
    public bool ExistsByName(string name)
    {
        return _departmentRepository.ExistsByName(name: name);
    }

    // 新しい部署を登録する
    public void Create(Department department)
    {
        using IDbContextTransaction transaction = _context.Database.BeginTransaction();

        try
        {
            bool created = _departmentRepository.Create(department: department);
            if (!created) throw new Exception(message: "部署を登録できませんでした。");

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            throw new Exception(message: "部署を登録できませんでした。",
                                    innerException: exception);
        }
    }
}
