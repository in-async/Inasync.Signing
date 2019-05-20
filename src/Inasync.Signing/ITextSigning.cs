using System;

namespace Inasync.Signing {

    /// <summary>
    /// テキストの署名を生成するインターフェース。
    /// </summary>
    public interface ITextSigning {

        /// <summary>
        /// 署名バージョン。
        /// </summary>
        byte Version { get; }

        /// <summary>
        /// 指定した文字列の署名を生成します。
        /// </summary>
        /// <param name="value">署名の対象となる文字列。</param>
        /// <returns><paramref name="value"/> の署名を表す <see cref="UrlSafeSignature"/>。常に非 <c>default</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        UrlSafeSignature ComputeSignature(string value);
    }
}
