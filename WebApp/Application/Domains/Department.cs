namespace WebApp.Application.Domains;

/// 所属部署を表すドメインオブジェクト
public class Department
{
    public int? Id { get; private set; } = null;      // 部署Id
    public string Name { get; private set; } = string.Empty;    // 部署名
    private const int MaxLength = 20; // 部署名の長さ

    public Department(int? id, string name)
    {
        // 部署名のルール検証
        ValidateDepartmentName(name);
        Id = id;
        Name = name;
    }

    public Department(string name) : this(id: null, name) { }

    public Department(int? id)
    {
        Id = id;
    }

    /// 部署名の変更
    public void ChangeName(string name)
    {
        // 部署名のルール検証
        ValidateDepartmentName(name);
        Name = name;
    }

    /// 等価性の検証
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(objA: this, objB: obj)) return true;
        Department? other = obj as Department;
        if (other is null) return false;

        return Id == other.Id;
    }

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;

    public override string ToString() => $"{Id?.ToString() ?? "未登録"}: {Name}";

    /// 部署名のルール検証
    private static void ValidateDepartmentName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("部署名は必須です");
        if (name.Length > MaxLength)
            throw new InvalidOperationException($"部署名は{MaxLength}文字以内で入力してください");
    }
}
