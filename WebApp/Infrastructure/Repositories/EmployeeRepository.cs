using WebApp.Infrastructure.Context;
using WebApp.Application.Domains;
using WebApp.Application.Repositories;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
namespace WebApp.Infrastructure.Repositories;

/// ドメインオブジェクト従業員のCRUD操作インターフェイスの実装
public class EmployeeRepository(AppDbContext context, EmployeeEntityAdapter adapter) : IEmployeeRepository
{
    /// アプリケーション用DbContext
    private readonly AppDbContext _context = context;
    /// ドメインモデル従業員と従業員エンティティの相互変換インターフェイスの実装
    private readonly EmployeeEntityAdapter _adapter = adapter;

    /// 従業員を永続化する
    public bool Create(Employee employee)
    {
        try
        {
            var entity = _adapter.Convert(employee);

            _context.Employees.Add(entity);
            return true;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException("従業員の永続化ができませんでした。", exception);
        }
    }

    /// すべての従業員を取得する
    public IReadOnlyList<Employee> FindAll()
    {
        try
        {
            // 社員一覧取得
            var employeeEntities = _context.Employees
                .OrderBy(employeeEntity => employeeEntity.EmpId)
                .ToList();

            List<Employee> employees = [];

            foreach (var employeeEntity in employeeEntities)
            {
                var employee = _adapter.Restore(employeeEntity);
                employees.Add(employee);
            }

            return employees;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException("すべての従業員を取得できませんでした。", exception);
        }
    }

    /// 指定された従業員Idの従業員を削除する
    public bool DeleteById(int id)
    {
        try
        {
            var entity = _context.Employees
                .FirstOrDefault(employeeEntity => employeeEntity.EmpId == id);

            if (entity is null) return false;

            _context.Employees.Remove(entity);
            return true;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException("指定された従業員Idの従業員を削除できませんでした。", exception);
        }
    }

    /// 指定されたメールアドレスの従業員が存在するか確認する
    public bool ExistsByEmail(string email)
    {
        try
        {
            string normalizedEmail = email.ToUpper();

            return _context.Employees
                .Any(employeeEntity => employeeEntity.EmpEmail.ToUpper() == normalizedEmail);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException("指定されたメールアドレスの従業員を確認できませんでした。", exception);
        }
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
            throw new InvalidOperationException("指定された電話番号の従業員を確認できませんでした。", exception);
        }
    }
}
