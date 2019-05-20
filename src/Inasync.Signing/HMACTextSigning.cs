using System;
using System.Security.Cryptography;
using System.Text;

namespace Inasync.Signing {

    /// <summary>
    /// テキストを <see cref="HMAC"/> で署名するサービス。
    /// </summary>
    public class HMACTextSigning : ITextSigning {
        private readonly Func<HMAC> _hmacFactory;

        /// <summary>
        /// <see cref="HMACTextSigning"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="hmacFactory">署名に使用する <see cref="HMAC"/> のファクトリー デリゲート。</param>
        /// <param name="version"><see cref="HMACTextSigning.Version"/> に渡される値。</param>
        /// <exception cref="ArgumentNullException"><paramref name="hmacFactory"/> is <c>null</c>.</exception>
        public HMACTextSigning(Func<HMAC> hmacFactory, byte version) {
            _hmacFactory = hmacFactory ?? throw new ArgumentNullException(nameof(hmacFactory));
            Version = version;
        }

        /// <summary>
        /// <see cref="ITextSigning.Version"/> の実装。
        /// </summary>
        public byte Version { get; }

        /// <summary>
        /// <see cref="ITextSigning.ComputeSignature(string)"/> の実装。
        /// </summary>
        /// <remarks>
        /// 署名スキームは以下の通り:
        /// 1. <paramref name="value"/> を UTF-8 の <see cref="byte"/> 配列に変換。
        /// 2. コンストラクターで与えられたファクトリーから <see cref="HMAC"/> を生成しハッシュ化。
        /// 3. 署名バージョンとハッシュで <see cref="UrlSafeSignature"/> を作成。
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        public UrlSafeSignature ComputeSignature(string value) {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            var input = Encoding.UTF8.GetBytes(value);

            byte[] hash;
            using (var hmac = _hmacFactory()) {
                hash = hmac.ComputeHash(input);
            }

            return new UrlSafeSignature(Version, hash);
        }
    }
}
