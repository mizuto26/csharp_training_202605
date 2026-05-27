using WebApp.Application.Adapters;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Adapters;

/// ドメインオブジェクトEmployeeとEmployeeEntityの相互変換インターフェイスの実装
public class EmployeeEntityAdapter
: IConverter<Employee, EmployeeEntity>
{
    /// EmployeeEntityからドメインオブジェクトEmployeeを復元する
    public Employee Restore(EmployeeEntity target)
    {
        Department? department = null;

        if (target.DeptId is int departmentId)
        {
            //departmentNameは後から入れるため設定しない
            department = new(
                departmentId
            );
        }

        Employee employee = new(
                target.EmpId,
                target.EmpName,
                target.EmpEmail,
                target.EmpPhone,
                department
        );

        return employee;
    }

    /// ドメインオブジェクトEmployeeをEmployeeEntityに変換する
    public EmployeeEntity Convert(Employee domain)
    {
        // EmpId はDB保存時に自動採番されるため設定しない
        EmployeeEntity entity = new()
        {
            EmpName = domain.Name,
            EmpEmail = domain.Email,
            EmpPhone = domain.Phone,
            DeptId = domain.Department?.Id
        };

        return entity;
    }

}
