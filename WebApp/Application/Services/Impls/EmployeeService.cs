using WebApp.Application.Repositories;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Context;
namespace WebApp.Application.Services.Impls;

/// 従業員登録サービスインターフェイスの実装
public class EmployeeService(
    AppDbContext context,
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository)
: IEmployeeService
{
    private readonly AppDbContext _context = context;
    /// ドメインオブジェクト:従業員のCRUD操作インターフェイス
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    /// ドメインオブジェクト:部署のCRUD操作インターフェイス
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    /// 新しい従業員を登録する
    public void Create(Employee employee)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            bool create = _employeeRepository.Create(employee: employee);
            if (!create) throw new Exception(message: $"従業員Id{employee.Id}に該当する従業員は存在しません");

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            throw new Exception(message: "従業員を登録できませんでした。",
                                innerException: exception);
        }
    }

    /// すべての従業員を取得する
    public IReadOnlyList<Employee> GetEmployees()
    {
        IReadOnlyList<Employee> employees = _employeeRepository.FindAll();
        Dictionary<int, Department> departmentById = _departmentRepository.FindAll()
            .Where(department => department.Id is not null)
            .ToDictionary(
                keySelector: department => department.Id!.Value,
                elementSelector: department => department
            );

        foreach (Employee employee in employees)
        {
            int? departmentId = employee.Department?.Id;

            if (departmentId is not null
                && departmentById.TryGetValue(key: departmentId.Value, value: out Department? department))
            {
                employee.ChangeDepartment(department: department);
            }
        }

        return employees;
    }

    /// 指定されたメールアドレスの従業員が存在するか確認する
    public bool ExistsByEmail(string email)
    {
        return _employeeRepository.ExistsByEmail(email: email);
    }

    /// 指定された電話番号の従業員が存在するか確認する
    public bool ExistsByPhone(string phone)
    {
        return _employeeRepository.ExistsByPhone(phone: phone);
    }
}
