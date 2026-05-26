using WebApp.Application.Repositories;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;
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
        using IDbContextTransaction? transaction = _context.Database.BeginTransaction();

        try
        {
            bool create = _employeeRepository.Create(employee: employee);
            if (create is false) throw new InvalidOperationException(message: $"従業員Id{employee.Id}に該当する従業員は存在しません");

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            throw new InvalidOperationException(message: "従業員を登録できませんでした。",
                                                innerException: exception);
        }
    }

    /// すべての従業員を取得する
    public IReadOnlyList<Employee> GetEmployees()
    {
        IReadOnlyList<Employee> employees = _employeeRepository.FindAll();
        IReadOnlyDictionary<int, Department> departmentById = CreateDepartmentDictionary(
            departments: _departmentRepository.FindAll()
        );

        foreach (Employee employee in employees)
        {
            // 従業員一覧取得時は部署IDだけを持つため、部署一覧から正式な部署情報を引き直す。
            Department? department = FindDepartment(
                departmentId: employee.Department?.Id,
                departmentById: departmentById
            );

            if (department is null) continue;

            employee.ChangeDepartment(department: department);
        }

        return employees;
    }

    private static Dictionary<int, Department> CreateDepartmentDictionary(IReadOnlyList<Department> departments)
    {
        Dictionary<int, Department> departmentById = [];

        foreach (Department department in departments)
        {
            if (department.Id is int departmentId) departmentById[departmentId] = department;
        }

        return departmentById;
    }

    private static Department? FindDepartment(int? departmentId, IReadOnlyDictionary<int, Department> departmentById)
    {
        if (departmentId is null) return null;

        bool foundDepartment = departmentById.TryGetValue(
            key: departmentId.GetValueOrDefault(),
            value: out Department? department
        );
        if (foundDepartment is false) return null;

        return department;
    }

    /// 指定された従業員Idの従業員を削除する
    public void DeleteById(int id)
    {
        using IDbContextTransaction transaction = _context.Database.BeginTransaction();

        try
        {
            bool deleted = _employeeRepository.DeleteById(id: id);
            if (deleted is false) throw new InvalidOperationException(message: $"従業員Id{id}に該当する従業員は存在しません");

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            throw new InvalidOperationException(message: "従業員を削除できませんでした。",
                                                innerException: exception);
        }
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
