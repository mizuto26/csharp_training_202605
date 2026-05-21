namespace WebApp.Application.Adapters;

/// TDomainに指定されたドメインオブジェクトをTTargetに指定されたクラスに変換するインターフェイス
public interface IConverter<TDomain, TTarget>
{
    /// TDomainに指定されたドメインオブジェクトをTTargetに指定されたクラスに変換する
    TTarget Convert(TDomain domain);
}