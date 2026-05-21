using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Context;
using WebApp.Application.Domains;
using WebApp.Application.Repositories;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Entities;
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
            var entity = _adapter.Convert(employee);

            if (entity is null) return false;

            _context.Employees.Add(entity);
            return true;
        }
        catch (Exception exception)
        {
            throw new Exception(message: "従業員の永続化ができませんでした。",
                                            innerException: exception);
        }
    }
}
