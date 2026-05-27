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
        Department department = new(target.DeptId, target.DeptName);
        return department;
    }

    public DepartmentEntity Convert(Department domain)
    {
        // DeptId はDB保存時に自動採番されるため設定しない
        DepartmentEntity entity = new()
        {
            DeptName = domain.Name,
        };

        return entity;
    }
}
