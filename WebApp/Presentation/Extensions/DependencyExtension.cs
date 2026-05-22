using Microsoft.EntityFrameworkCore;
using WebApp.Application.Repositories;
using WebApp.Application.Services;
using WebApp.Application.Services.Impls;
using WebApp.Infrastructure.Adapters;
using WebApp.Infrastructure.Context;
using WebApp.Infrastructure.Repositories;
using WebApp.Presentation.TempData;
using WebApp.Presentation.ViewModels;

namespace WebApp.Presentation.Extensions;

/// 依存定義および依存性注入クラス
public static class DependencyExtension
{
    /// アプリケーション全体の依存定義を設定する拡張メソッド
    public static void SettingDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EntityFramework Coreのインスタンス生成と依存定義
        SettingEntityFrameworkCore(configuration, services);
        // インフラストラクチャ層のインスタンス生成と依存定義
        SettingInfrastructures(services);
        // アプリケーション層のインスタンス生成と依存定義
        SettingApplications(services);
        // プレゼンテーション層のインスタンス生成と依存定義
        SettingPresentations(services);
    }

    /// EntityFramework Coreのインスタンス生成と依存定義
    private static void SettingEntityFrameworkCore(IConfiguration configuration, IServiceCollection services)
    {
        // 接続文字列(appsettings.json)から取得
        string connectionString = configuration.GetConnectionString(name: "PostgreSqlConnection")
            ?? throw new InvalidOperationException("PostgreSQLの接続文字列が設定されていません。");
        // DbContext登録(PostgreSQL用)
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
    }

    /// インフラストラクチャ層のインスタンス生成と依存定義
    private static void SettingInfrastructures(IServiceCollection services)
    {
        // ドメインモデル:部署と部署エンティティの相互変換インターフェイスの実装
        services.AddScoped<DepartmentEntityAdapter>();
        // ドメインモデル:従業員と従業員エンティティの相互変換インターフェイスの実装
        services.AddScoped<EmployeeEntityAdapter>();
        // ドメインオブジェクト:部署のCRUD操作インターフェイス実装
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        // ドメインオブジェクト:従業員のCRUD操作インターフェイスの実装
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    }

    /// アプリケーション層のインスタンス生成と依存定義
    private static void SettingApplications(IServiceCollection services)
    {
        // 従業員登録サービスインターフェイスの実装
        services.AddScoped<IEmployeeService, EmployeeService>();
        // 部署サービスインターフェイスの実装
        services.AddScoped<IDepartmentService, DepartmentService>();
    }

    /// プレゼンテーション層のインスタンス生成と依存定義
    private static void SettingPresentations(IServiceCollection services)
    {
        // 従業員登録ViewModelをドメインオブジェクト:従業員に変換するアダプターインターフェイスの実装
        services.AddScoped<EmployeeCreateViewModelAdapter>();
        // 従業員リストを従業員一覧ViewModelに変換するアダプターインターフェイスの実装
        services.AddScoped<EmployeeListViewModelAdapter>();
        // TempDataへのEmployeeViewの保存・復元するためのクラス
        // コンストラクタを利用して明示的にDIコンテナにインスタンスを登録する
        services.AddScoped(
            provider => new TempDataStore<EmployeeCreateViewModel>(key: "EmployeeCreateViewModel")
        );
        // TempDataへのDepartmentViewの保存・復元するためのクラス
        // コンストラクタを利用して明示的にDIコンテナにインスタンスを登録する
        services.AddScoped(
            provider => new TempDataStore<DepartmentCreateViewModel>(key: "DepartmentCreateViewModel")
        );
    }
}
