using WebApp.Infrastructure.Context;
using WebApp.Application.Domains;
using WebApp.Application.Repositories;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Entities;

namespace WebApp.Infrastructure.Repositories;

/// ドメインオブジェクト:部署のCRUD操作インターフェイス実装
public class DepartmentRepository(
    AppDbContext context,
    DepartmentEntityAdapter adapter)
: IDepartmentRepository
{
    /// アプリケーション用DbContext
    private readonly AppDbContext _context = context;
    /// ドメインモデル:部署と部署エンティティの相互変換インターフェイスの実装
    private readonly DepartmentEntityAdapter _adapter = adapter;

    /// すべての部署を取得する
    public IReadOnlyList<Department> FindAll()
    {
        try
        {
            var departments = _context.Departments
                .OrderBy(departmentEntity => departmentEntity.DeptId)
                .Select(_adapter.Restore)
                .ToList();

            return departments;
        }
        catch (Exception exception)
        {
            throw new Exception(message: "すべての部署を取得できませんでした。",
                                        innerException: exception);
        }
    }

    /// 指定された部署Idの部署を取得する
    public Department? FindById(int id)
    {
        try
        {
            var result = _context.Departments
                .FirstOrDefault(departmentEntity => departmentEntity.DeptId == id);

            if (result == null) return null;

            return _adapter.Restore(target: result);
        }
        catch (Exception exception)
        {
            throw new Exception(message: "指定された部署Idの部署を取得できませんでした。",
                                        innerException: exception);
        }
    }

    public bool Create(Department department)
    {
        try
        {
            DepartmentEntity entity = _adapter.Convert(domain: department);

            _context.Departments.Add(entity: entity);
            return true;
        }
        catch (Exception exception)
        {
            throw new Exception(message: "部署の永続化ができませんでした。",
                                        innerException: exception);
        }
    }

    /// 指定された部署名の部署が存在するか確認する
    public bool ExistsByName(string name)
    {
        try
        {
            return _context.Departments
                .Any(departmentEntity => departmentEntity.DeptName == name);
        }
        catch (Exception exception)
        {
            throw new Exception(message: "指定された部署名の部署を確認できませんでした。",
                                        innerException: exception);
        }
    }
}
