namespace WebApp.Application.Adapters;

/// 指定のクラス(TTarget)からドメインオブジェクト(TDomain)を復元するインターフェイス
public interface IRestorer<TDomain, TTarget>
{
    ///  他のクラスからドメインオブジェクトへの復元する
    TDomain Restore(TTarget target);
}
