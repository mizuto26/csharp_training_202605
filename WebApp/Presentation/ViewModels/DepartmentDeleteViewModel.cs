using System.ComponentModel.DataAnnotations;

namespace WebApp.Presentation.ViewModels;

/// 部署削除ViewModelクラス
public class DepartmentDeleteViewModel
{
    /// 部署Id
    [Display(Name = "部署ID")]
    public int DepartmentId { get; set; } = 0;

    /// 部署名
    [Display(Name = "部署名")]
    public string DepartmentName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"DepartmentId={DepartmentId} , DepartmentName={DepartmentName}";
    }
}
