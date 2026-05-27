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
    [StringLength(20, ErrorMessage = "{0}は20文字以内で入力してください")]
    public string Name { get; set; } = string.Empty;
    /// 所属部署
    [Display(Name = "所属部署")]
    public int? DeptId { get; set; }

    [Display(Name = "電話番号")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [RegularExpression(@"^0\d{1,4}-\d{1,4}-\d{4}$", ErrorMessage = "{0}の形式が正しくありません。")]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "メールアドレス")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [EmailAddress(ErrorMessage = "{0}の形式が正しくありません。")]
    public string Email { get; set; } = string.Empty;

    /// 選択された部署名
    [Display(Name = "部署名")]
    public string DeptName { get; set; } = string.Empty;

    // 部署のリスト
    public List<SelectListItem> Departments { get; set; } = [];

    /// 部署のリストをSelectListItemのリストに変換してプロパティに設定する
    public void SetDepartments(IReadOnlyList<Department> departments)
    {
        Departments =
        [
            new SelectListItem
            {
                Value = string.Empty,
                Text = "未所属"
            }
        ];

        foreach (var department in departments)
        {
            if (department.Id is not int departmentId) continue;

            Departments.Add(item: new SelectListItem
            {
                Value = departmentId.ToString(),
                Text = string.IsNullOrEmpty(value: department.Name) ? "(名称未設定)" : department.Name
            });
        }
    }

    public override string ToString()
    {
        return $"Name={Name} , DeptId={DeptId} , DeptName={DeptName}  , Departments={Departments}";
    }
}
