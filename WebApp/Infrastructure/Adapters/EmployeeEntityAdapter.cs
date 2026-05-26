using WebApp.Application.Adapters;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Entities;
namespace WebApp.Infrastructure.Adapters;

/// ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換インターフェイスの実装
public class EmployeeEntityAdapter
: IConverter<Employee, EmployeeEntity>
{
    /// ドメインオブジェクト:EmployeeをEmployeeEntityに変換する
    public EmployeeEntity Convert(Employee domain)
    {
        EmployeeEntity entity = new()
        {
            EmpName = domain.Name,
            EmpEmail = domain.Email,
            EmpPhone = domain.Phone
        };

        return entity;
    }

    /// EmployeeEntityとDepartmentEntityからドメインオブジェクト:Employeeを復元する
    public Employee Restore(EmployeeEntity employeeEntity, DepartmentEntity? departmentEntity)
    {
        Department? department = null;

        if (departmentEntity != null)
        {
            department = new Department(
                id: departmentEntity.DeptId,
                name: departmentEntity.DeptName
            );
        }
        else if (employeeEntity.DeptId != null)
        {
            department = new Department(id: employeeEntity.DeptId);
        }

        Employee employee = new(
                id: employeeEntity.EmpId,
                name: employeeEntity.EmpName,
                email: employeeEntity.EmpEmail,
                phone: employeeEntity.EmpPhone,
                department: department
        );

        return employee;
    }
}
