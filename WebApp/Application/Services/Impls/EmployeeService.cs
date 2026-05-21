using WebApp.Application.Repositories;
using WebApp.Application.Domains;
using WebApp.Infrastructure.Context;
namespace WebApp.Application.Services.Impls;

/// 従業員登録サービスインターフェイスの実装
public class EmployeeService(
    AppDbContext context,
    IEmployeeRepository employeeRepository)
: IEmployeeService
{
    private readonly AppDbContext _context = context;
    /// ドメインオブジェクト:従業員のCRUD操作インターフェイス
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

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
}
