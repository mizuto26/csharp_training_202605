using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApp.Infrastructure.Entities;

/// 従業員テーブル(employee)を扱うEntity Framework Coreのエンティティクラス
[Table("employee")]
public class EmployeeEntity
{
    /// 従業員Id(主キー)
    [Key]
    [Column("id")]
    public int EmpId { get; set; }
    [Column("name")]
    /// 従業員名
    public string EmpName { get; set; } = string.Empty;

    [Column("phone_number")]
    /// 従業員名
    public string? EmpPhone { get; set; } = string.Empty;

    [Column("mail_address")]
    /// 従業員名
    public string? EmpMail { get; set; } = string.Empty;

    /// 所属部署Id(外部キー)
    [Column("dept_id")]
    public int? DeptId { get; set; }
}