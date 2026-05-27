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
        bool foundValue = controller.TempData.TryGetValue(_key, out object? value);
        if (!foundValue) return null;

        // 値を文字列として取得
        if (value is null) return null;
        if (value is not string json) return null;
        if (string.IsNullOrWhiteSpace(json)) return null;

        try
        {
            // JSONをオブジェクトに変換して返す
            return JsonSerializer.Deserialize<T>(json);
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
        string json = JsonSerializer.Serialize(model);
        controller.TempData[_key] = json;
    }
}
