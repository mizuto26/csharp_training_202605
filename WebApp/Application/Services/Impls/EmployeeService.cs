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
    /// ドメインオブジェクト従業員のCRUD操作インターフェイス
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    /// ドメインオブジェクト部署のCRUD操作インターフェイス
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    /// 新しい従業員を登録する
    public void Create(Employee employee)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            bool create = _employeeRepository.Create(employee);
            if (!create) throw new InvalidOperationException($"従業員Id{employee.Id}に該当する従業員は存在しません");

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            throw new InvalidOperationException("従業員を登録できませんでした。", exception);
        }
    }

    /// すべての従業員を取得する
    public IReadOnlyList<Employee> GetEmployees()
    {
        IReadOnlyList<Employee> employees = _employeeRepository.FindAll();
        IReadOnlyList<Department> departments = _departmentRepository.FindAll();

        Dictionary<int, Department> departmentById = CreateDepartmentDictionary(departments);

        foreach (var employee in employees)
        {
            Department? department = FindDepartment(employee.Department?.Id, departmentById);
            employee.ChangeDepartment(department);
        }

        return employees;
    }

    private static Dictionary<int, Department> CreateDepartmentDictionary(IReadOnlyList<Department> departments)
    {
        Dictionary<int, Department> departmentById = [];

        foreach (var department in departments)
        {
            int departmentId = department.Id ?? throw new InvalidOperationException("部署IDが未設定です。");
            departmentById[departmentId] = department;
        }

        return departmentById;
    }

    private static Department? FindDepartment(int? departmentId, Dictionary<int, Department> departmentById)
    {
        if (departmentId is not int departmentIdValue) return null;

        //Dictionary にそのキーが存在するか探して、あれば値も取り出す
        bool foundDepartment = departmentById.TryGetValue(departmentIdValue, out Department? department);

        if (!foundDepartment) return null;

        return department;
    }

    /// 指定された従業員Idの従業員を削除する
    public void DeleteById(int id)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            bool deleted = _employeeRepository.DeleteById(id);
            if (!deleted) throw new InvalidOperationException($"従業員Id{id}に該当する従業員は存在しません");

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            throw new InvalidOperationException("従業員を削除できませんでした。", exception);
        }
    }

    /// 指定されたメールアドレスの従業員が存在するか確認する
    public bool ExistsByEmail(string email)
    {
        return _employeeRepository.ExistsByEmail(email);
    }

    /// 指定された電話番号の従業員が存在するか確認する
    public bool ExistsByPhone(string phone)
    {
        return _employeeRepository.ExistsByPhone(phone);
    }
}
