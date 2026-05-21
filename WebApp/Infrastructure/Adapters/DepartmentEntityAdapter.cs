using WebApp.Application.Adapters;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Adapters;

/// ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換インターフェイスの実装
public class DepartmentEntityAdapter
: IConverter<Department, DepartmentEntity>, IRestorer<Department, DepartmentEntity>
{
    /// ドメインオブジェクト:DepartmentをDepartmentEntityに変換する
    public DepartmentEntity Convert(Department domain)
    {
        DepartmentEntity entity = new()
        {
            DeptName = domain.Name,
        };

        if (domain.Id != null) entity.DeptId = domain.Id.Value;

        return entity;
    }

    /// DepartmentEntityからドメインオブジェクト:Departmentを復元する
    public Department Restore(DepartmentEntity target)
    {
        Department department = new(
            id: target.DeptId,
            name: target.DeptName
        );

        return department;
    }
}
