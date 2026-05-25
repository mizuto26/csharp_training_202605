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
        entity.EmpEmail = domain.Email;
        entity.EmpPhone = domain.Phone;

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

        Employee employee = new(
                id: employeeEntity.EmpId,
                name: employeeEntity.EmpName,
                email: employeeEntity.EmpEmail ?? string.Empty,
                phone: employeeEntity.EmpPhone ?? string.Empty,
                department: department
        );

        return employee;
    }

    /// EmployeeEntityからドメインオブジェクト:Employeeを復元する
    public Employee Restore(EmployeeEntity target)
    {
        Department? department = target.DeptId is null
            ? null
            : new Department(id: target.DeptId.Value);

        return new Employee(
            id: target.EmpId,
            name: target.EmpName,
            email: target.EmpEmail ?? string.Empty,
            phone: target.EmpPhone ?? string.Empty,
            department: department
        );
    }
}
