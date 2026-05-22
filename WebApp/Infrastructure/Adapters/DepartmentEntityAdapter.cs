using WebApp.Application.Adapters;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Adapters;

/// ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換インターフェイスの実装
public class DepartmentEntityAdapter
: IRestorer<Department, DepartmentEntity>
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
}
