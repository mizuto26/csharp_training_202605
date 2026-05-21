using WebApp.Application.Adapters;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Adapters;

/// ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換インターフェイスの実装
public class EmployeeEntityAdapter
: IConverter<Employee, EmployeeEntity>, IRestorer<Employee, EmployeeEntity>
{
    /// ドメインオブジェクト:EmployeeをEmployeeEntityに変換する
    public EmployeeEntity Convert(Employee domain)
    {
        EmployeeEntity entity = new()
        {
            EmpName = domain.Name
        };

        if (domain.Id != null) entity.EmpId = domain.Id.Value;
        if (domain.Department != null) entity.DeptId = domain.Department.Id;

        return entity;
    }

    public Employee Restore(EmployeeEntity target)
    {
        Employee employee = new(
            id: target.EmpId,
            name: target.EmpName,
            department: null
        );

        return employee;
    }
}
