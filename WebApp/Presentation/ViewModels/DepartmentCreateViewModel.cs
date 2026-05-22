using System.ComponentModel.DataAnnotations;

namespace WebApp.Presentation.ViewModels;

/// 部署登録ViewModelクラス
public class DepartmentCreateViewModel
{
    /// 部署名
    [Display(Name = "部署名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    public string? DepartmentName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"DepartmentName={DepartmentName}";
    }
}
