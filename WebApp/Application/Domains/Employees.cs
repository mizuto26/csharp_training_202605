namespace WebApp.Application.Domains;

/// 従業員を表すドメインオブジェクト
public class Employee
{
    public int? Id { get; private set; } // 社員Id
    public string Name { get; private set; } = string.Empty; // 氏名

    public string? Phone { get; private set; } = string.Empty;
    public string? Mail { get; private set; } = string.Empty;

    public Department? Department { get; private set; } // 所属部署（null可）

    private const int MaxLength = 20;

    public Employee(int? id, string name, string? phone, string? mail, Department? department)
    {
        ValidateName(name: name);
        Id = id;
        Name = name;
        Phone = phone;
        Mail = mail;
        Department = department;
    }

    public Employee(string name, string? phone, string? mail, Department? department)
    : this(null, name, phone, mail, department) { }

    /// 氏名を変更する
    public void ChangeName(string name)
    {
        ValidateName(name: name);
        Name = name;
    }

    /// 所属部署を変更する
    public void ChangeDepartment(Department? department)
    {
        Department = department;
    }

    /// 等価性（IDによる比較）
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(objA: this, objB: obj)) return true;
        if (obj is not Employee other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;

    public override string ToString()
        => $"{Id?.ToString() ?? "未登録"}: {Name} / {Department?.Name ?? "未配属"}";

    /// 氏名の検証
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(value: name))
            throw new Exception("氏名は必須です");
        if (name.Length > MaxLength)
            throw new Exception($"氏名は{MaxLength}文字以内で入力してください");
    }
}