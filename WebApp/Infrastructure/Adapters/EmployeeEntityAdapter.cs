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
            EmpName = domain.Name,
        };

        if (domain.Id != null) entity.EmpId = domain.Id.Value;
        if (domain.Department != null) entity.DeptId = domain.Department.Id;
        if (domain.Mail != null) entity.EmpMail = domain.Mail;
        if (domain.Phone != null) entity.EmpPhone = domain.Phone;

        return entity;
    }

    public Employee Restore(EmployeeEntity target)
    {
        Employee employee = new(
            id: target.EmpId,
            name: target.EmpName,
            phone: target.EmpPhone,
            mail: target.EmpMail,
            department: null
        );

        return employee;
    }
}
