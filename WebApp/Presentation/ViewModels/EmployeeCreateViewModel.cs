using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Domains;
namespace WebApp.Presentation.ViewModels;

/// 部署登録ViewModelクラス
public class EmployeeCreateViewModel
{
    /// 氏名
    [Display(Name = "氏名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    public string? Name { get; set; } = string.Empty;
    /// 所属部署
    [Display(Name = "所属部署")]
    [Required(ErrorMessage = "{0}は選択必須です。")]
    public int? DeptId { get; set; } = 0;

    [Display(Name = "電話番号")]
    [Phone]
    public string? Phone { get; set; } = string.Empty;

    [Display(Name = "メールアドレス")]
    [EmailAddress]
    public string? Email { get; set; } = string.Empty;

    /// 選択された部署名
    [Display(Name = "部署名")]
    public string? DeptName { get; set; } = string.Empty;

    // 部署のリスト
    public List<SelectListItem> Departments { get; set; } = [];

    /// 部署のリストをSelectListItemのリストに変換してプロパティに設定する
    public void SetDepartments(IReadOnlyList<Department> departments)
    {
        Departments = departments
            .Where(department => department.Id is not null)
            .Select(department => new SelectListItem
            {
                Value = department.Id.ToString(),
                Text = string.IsNullOrEmpty(department.Name) ? "(名称未設定)" : department.Name
            })
            .ToList();
    }

    public override string ToString()
    {
        return $"Name={Name} , DeptId={DeptId} , DeptName={DeptName}  , Departments={Departments}";
    }
}