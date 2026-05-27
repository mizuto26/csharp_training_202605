using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApp.Infrastructure.Entities;

/// 部署テーブル(department)を扱うEntity Framework Coreのエンティティクラス
[Table("department")]
public class DepartmentEntity
{
    /// 部署Id(主キー)
    [Key]
    [Column("id")]
    public int DeptId { get; set; } = 0;
    /// 部署名
    [Column("name")]
    public string DeptName { get; set; } = string.Empty;
}