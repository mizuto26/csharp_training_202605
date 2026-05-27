using WebApp.Infrastructure.Context;
using WebApp.Application.Domains;
using WebApp.Application.Repositories;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
namespace WebApp.Infrastructure.Repositories;

/// ドメインオブジェクト:従業員のCRUD操作インターフェイスの実装
public class EmployeeRepository(AppDbContext context, EmployeeEntityAdapter adapter) : IEmployeeRepository
{
    /// アプリケーション用DbContext
    private readonly AppDbContext _context = context;
    /// ドメインモデル:従業員と従業員エンティティの相互変換インターフェイスの実装
    private readonly EmployeeEntityAdapter _adapter = adapter;

    /// 従業員を永続化する
    public bool Create(Employee employee)
    {
        try
        {
            EmployeeEntity entity = _adapter.Convert(domain: employee);

            _context.Employees.Add(entity);
            return true;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(message: "従業員の永続化ができませんでした。",
                                                innerException: exception);
        }
    }

    /// すべての従業員を取得する
    public IReadOnlyList<Employee> FindAll()
    {
        try
        {
            // 社員一覧取得
            List<EmployeeEntity> employeeEntities = _context.Employees
                .OrderBy(employeeEntity => employeeEntity.EmpId)
                .ToList()
            ;

            List<Employee> employees = [];

            foreach (EmployeeEntity employeeEntity in employeeEntities)
            {
                Employee employee = _adapter.Restore(employeeEntity);

                employees.Add(employee);
            }

            return employees;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(message: "すべての従業員を取得できませんでした。",
                                                innerException: exception);
        }
    }

    /// 指定された従業員Idの従業員を削除する
    public bool DeleteById(int id)
    {
        try
        {
            EmployeeEntity? entity = _context.Employees
                .FirstOrDefault(employeeEntity => employeeEntity.EmpId == id);

            if (entity is null) return false;

            _context.Employees.Remove(entity: entity);
            return true;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(message: "指定された従業員Idの従業員を削除できませんでした。",
                                                innerException: exception);
        }
    }

    /// 指定されたメールアドレスの従業員が存在するか確認する
    public bool ExistsByEmail(string email)
    {
        try
        {
            string normalizedEmail = email.ToLower();

            return _context.Employees
                .Any(employeeEntity => HasSameEmail(employeeEntity, normalizedEmail));
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(message: "指定されたメールアドレスの従業員を確認できませんでした。",
                                                innerException: exception);
        }
    }

    private static bool HasSameEmail(EmployeeEntity employeeEntity, string normalizedEmail)
    {
        string? employeeEmail = employeeEntity.EmpEmail;
        if (employeeEmail is null) return false;

        return employeeEmail.ToLower() == normalizedEmail;
    }

    /// 指定された電話番号の従業員が存在するか確認する
    public bool ExistsByPhone(string phone)
    {
        try
        {
            return _context.Employees
                .Any(employeeEntity => employeeEntity.EmpPhone == phone);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(message: "指定された電話番号の従業員を確認できませんでした。",
                                                innerException: exception);
        }
    }
}
