using System;
using System.Security.Cryptography;

namespace Inasync.Signing {

    /// <summary>
    /// 文字列を <see cref="HMACSHA256"/> で署名するサービス。
    /// </summary>
    public sealed class HS256Signing : HMACSigning {

        /// <summary>
        /// <see cref="HS256Signing"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="signKey">署名鍵。</param>
        /// <param name="version"><see cref="HMACSigning.Version"/> に渡される値。</param>
        /// <exception cref="ArgumentNullException"><paramref name="signKey"/> is <c>null</c>.</exception>
        public HS256Signing(byte[] signKey, byte version) : base(() => new HMACSHA256(signKey), version) {
            if (signKey == null) { throw new ArgumentNullException(nameof(signKey)); }
        }
    }
}