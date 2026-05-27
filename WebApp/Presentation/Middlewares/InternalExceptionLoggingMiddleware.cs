namespace WebApp.Presentation.Middlewares;

/// IngternalExceptionをハンドリングするミドルウェア
public class InternalExceptionLoggingMiddleware(
    RequestDelegate next,
    ILogger<InternalExceptionLoggingMiddleware> logger)
{
    /// 次に処理を渡すデリゲート(Controllerなど)
    private readonly RequestDelegate _next = next;
    /// ロガー
    private readonly ILogger<InternalExceptionLoggingMiddleware> _logger = logger;

    /// ASP.NET Coreのミドルウェア処理
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // 次のミドルウェアまたはControllerへ処理を渡す別スレッドで監視するためaysnc await
            await _next(context);
        }
        catch (Exception exception)
        {
            // エラーログを出力する
            _logger.LogError(exception, "InternalException が発生しました");

            // レスポンスが未送信の場合のみ処理
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                // システム停止中画面へ遷移
                context.Response.Redirect(location: "/System/Maintenance");
            }
        }
    }
}
