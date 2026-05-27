using WebApp.Application.Adapters;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Adapters;

/// ドメインオブジェクトDepartmentとDepartmentEntityの相互変換インターフェイスの実装
public class DepartmentEntityAdapter
: IRestorer<Department, DepartmentEntity>, IConverter<Department, DepartmentEntity>
{
    /// DepartmentEntityからドメインオブジェクトDepartmentを復元する
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
        // DeptId はDB保存時に自動採番されるため設定しない
        DepartmentEntity entity = new()
        {
            DeptName = domain.Name,
        };

        return entity;
    }
}
