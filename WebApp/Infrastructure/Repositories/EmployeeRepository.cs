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

    /// すべての従業員を取得する
    public IReadOnlyList<Employee> FindAll()
    {
        try
        {
            // 社員一覧取得
            List<EmployeeEntity> employeeEntities = _context.Employees
                .AsNoTracking()
                .OrderBy(employeeEntity => employeeEntity.EmpId)
                .ToList()
            ;

            // 部署一覧取得
            List<DepartmentEntity> departmentEntities = _context.Departments
                .AsNoTracking()
                .ToList()
            ;

            Dictionary<int, DepartmentEntity> departmentById = [];

            // 部署を1件ずつDictionaryへ追加
            foreach (DepartmentEntity departmentEntity in departmentEntities)
            {
                departmentById.Add(key: departmentEntity.DeptId, value: departmentEntity);
            }

            List<Employee> employees = [];

            foreach (EmployeeEntity employeeEntity in employeeEntities)
            {
                DepartmentEntity? departmentEntity = null;
                // DeptIdがある場合
                if (employeeEntity.DeptId != null)
                {
                    // Dictionaryから部署取得
                    // TryGetValuehメソッドが「bool + outで２つの（departmentEntity））」を返す
                    departmentById.TryGetValue(key: employeeEntity.DeptId.Value, value: out departmentEntity);
                }

                // Entity → Domain変換
                Employee employee = _adapter.Restore(employeeEntity: employeeEntity, departmentEntity: departmentEntity);

                // Listへ追加
                employees.Add(employee);
            }

            return employees;
        }
        catch (Exception exception)
        {
            throw new Exception(message: "すべての従業員を取得できませんでした。",
                                        innerException: exception);
        }
    }
}
