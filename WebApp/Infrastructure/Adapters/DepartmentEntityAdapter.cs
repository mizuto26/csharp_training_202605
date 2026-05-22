using WebApp.Application.Adapters;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Adapters;

/// ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換インターフェイスの実装
public class DepartmentEntityAdapter
: IRestorer<Department, DepartmentEntity>, IConverter<Department, DepartmentEntity>
{
    /// DepartmentEntityからドメインオブジェクト:Departmentを復元する
    public Department Restore(DepartmentEntity target)
    {
        Department department = new(
            id: target.DeptId,
            name: target.DeptName
        );

        return department;
    }

    public DepartmentEntity Convert(Department domain)
    {
        DepartmentEntity entity = new()
        {
            DeptName = domain.Name,
        };

        return entity;
    }
}
