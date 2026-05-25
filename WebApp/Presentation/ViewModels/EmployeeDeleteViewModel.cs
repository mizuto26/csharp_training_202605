using System.ComponentModel.DataAnnotations;

namespace WebApp.Presentation.ViewModels;

/// 従業員削除ViewModelクラス
public class EmployeeDeleteViewModel
{
    /// 従業員Id
    [Display(Name = "従業員ID")]
    public int EmployeeId { get; set; }

    /// 氏名
    [Display(Name = "氏名")]
    public string? EmployeeName { get; set; } = string.Empty;

    /// 電話番号
    [Display(Name = "電話番号")]
    public string? EmployeePhone { get; set; } = string.Empty;

    [Display(Name = "メールアドレス")]
    public string? EmployeeEmail { get; set; } = string.Empty;

    /// 部署名
    [Display(Name = "部署名")]
    public string? DepartmentName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"EmployeeId={EmployeeId} , EmployeeName={EmployeeName} , EmployeePhone={EmployeePhone} , EmployeeEmail={EmployeeEmail} , DepartmentName={DepartmentName}";
    }
}
