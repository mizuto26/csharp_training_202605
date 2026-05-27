using WebApp.Presentation.Middlewares;
using WebApp.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
// ControllerやViewの依存関係を構築する
builder.Services.AddControllersWithViews();

// アプリケーションの依存関係を構築する
builder.Services.SettingDependencyInjection(builder.Configuration);

// WbApplicationをビルダーから構築する
var app = builder.Build();

// HTTPリクエストパイプラインの構成を行う
if (!app.Environment.IsDevelopment())
{
    // 開発環境でない場合は、エラーハンドラを設定する（/Home/Errorにリダイレクト）
    app.UseExceptionHandler("/Home/Error");

    // HTTP Strict Transport Security を有効にする（既定は30日間）
    // 本番環境ではHSTSの期間を調整することも推奨される
    app.UseHsts();

    // HTTPをHTTPSへリダイレクトする
    app.UseHttpsRedirection();
}

// IngternalExceptionをハンドリングするミドルウェアを有効にする
app.UseMiddleware<InternalExceptionLoggingMiddleware>();

// 静的ファイル（CSS, JS, 画像など）を提供できるようにする
app.UseStaticFiles();

// ルーティングを有効にする（URLからコントローラやアクションを判断できるようにする）
app.UseRouting();

// 認可（Authorization）を有効にする（認証後のアクセス制御に使用）
app.UseAuthorization();

// 既定のルートパターンを設定する（例: /Home/Index）
app.MapControllerRoute(
    name: "default",                            // ルートの名前
    pattern: "{controller=Home}/{action=Index}/{id?}"); // URLパターン（省略時はHomeコントローラとIndexアクション）

// アプリケーションを起動し、リクエストの受付を開始する
app.Run();
