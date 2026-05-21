using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
namespace WebApp.Presentation.TempData;

/// TempDataを通じて一時的にデータ(フォームなど)を保存・復元するためのクラス
public class TempDataStore<T>(string key)
where T : class
{
    /// 初期化後は変更不可
    /// オブジェクトにアクセスするキー
    private readonly string _key = key;

    /// TempDataに保存されたJSONを復元して、指定した型のオブジェクトを返す
    public T? Load(Controller controller)
    {
        // TempDataにキーが存在するか確認
        if (!controller.TempData.TryGetValue(key: _key, value: out object? value)) return null;

        // 値を文字列として取得
        string? json = value as string;
        if (string.IsNullOrWhiteSpace(value: json)) return null;

        try
        {
            // JSONをオブジェクトに変換して返す
            return JsonSerializer.Deserialize<T>(json: json);
        }
        catch (JsonException)
        {
            // JSONの形式が不正な場合はnullを返す
            return null;
        }
    }

    /// 指定されたコントローラのTempDataに、オブジェクトをJSONとして保存する
    public void Save(Controller controller, T model)
    {
        string json = JsonSerializer.Serialize(value: model);
        controller.TempData[_key] = json;
    }
}