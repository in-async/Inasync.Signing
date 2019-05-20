using System;
using System.Diagnostics;

namespace Inasync.Signing {

    /// <summary>
    /// 署名を表す構造体。
    /// </summary>
    public readonly struct UrlSafeSignature : IEquatable<UrlSafeSignature> {
        private static readonly byte[] _emptyBytes = new byte[0];
        private readonly byte[] _signatureBytes;
        private readonly string _signature;

        /// <summary>
        /// <see cref="UrlSafeSignature"/> 構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="version"><see cref="Version"/> に渡される値。</param>
        /// <param name="hash">署名対象のハッシュ値。</param>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> is <c>null</c>.</exception>
        public UrlSafeSignature(byte version, byte[] hash) {
            if (hash == null) { throw new ArgumentNullException(nameof(hash)); }

            Version = version;
            _signatureBytes = new byte[1 + hash.Length];
            _signatureBytes[0] = Version;
            Buffer.BlockCopy(hash, 0, _signatureBytes, 1, hash.Length);
            _signature = Base64Url.Encode(_signatureBytes);
        }

        /// <summary>
        /// 署名文字列を <see cref="UrlSafeSignature"/> に変換します。
        /// </summary>
        /// <param name="signature"><see cref="UrlSafeSignature"/> 形式の署名文字列。</param>
        /// <param name="result">変換された <see cref="UrlSafeSignature"/>。変換に失敗した場合は <c>default</c>。</param>
        /// <returns>正常に変換された場合は <c>true</c>、それ以外なら <c>false</c>。</returns>
        public static bool TryParse(string signature, out UrlSafeSignature result) {
            if (signature == null) { goto Failure; }

            if (!Base64Url.TryDecode(signature, out var signatureBytes)) { goto Failure; }
            if (signatureBytes.Length == 0) { goto Failure; }

            result = new UrlSafeSignature(signatureBytes, signature);
            return true;

Failure:
            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="_signatureBytes"/> や <see cref="_signature"/> を直接設定するコンストラクター。
        /// 専ら <see cref="TryParse(string, out UrlSafeSignature)"/> から呼ばれる。
        /// </summary>
        private UrlSafeSignature(byte[] signatureBytes, string signature) {
            Debug.Assert(signatureBytes != null && signatureBytes.Length > 0);
            Debug.Assert(signature != null);

            Version = signatureBytes[0];
            _signatureBytes = signatureBytes;
            _signature = signature;
        }

        /// <summary>
        /// 署名バージョン。
        /// </summary>
        public byte Version { get; }

        /// <summary>
        /// 署名のバイナリ表現を返します。
        /// フォーマットは、署名バージョンと署名ハッシュを連結した <see cref="byte"/> 配列です。
        /// </summary>
        /// <returns>署名を表す <see cref="byte"/> 配列。常に非 <c>null</c>。</returns>
        public byte[] ToByteArray() {
            if (_signatureBytes == null) { return _emptyBytes; }

            var bytes = new byte[_signatureBytes.Length];
            Buffer.BlockCopy(_signatureBytes, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 署名のバイナリ表現を base64url でエンコードした文字列を返します。
        /// </summary>
        /// <returns>署名文字列。URL Safe。常に非 <c>null</c>。</returns>
        public override string ToString() => _signature ?? "";

        /// <summary>
        /// <see cref="object.Equals(object)"/> の再実装。
        /// </summary>
        public override bool Equals(object obj) {
            // ※ 自動実装。
            return obj is UrlSafeSignature && Equals((UrlSafeSignature)obj);
        }

        /// <summary>
        /// <see cref="IEquatable{T}.Equals(T)"/> の実装。
        /// </summary>
        public bool Equals(UrlSafeSignature other) {
            // HACK: バイナリ比較の方が速い？ 又は Span<byte> を使う？
            return _signature == other._signature;
        }

        /// <summary>
        /// <see cref="object.GetHashCode"/> の再実装。
        /// </summary>
        public override int GetHashCode() {
            // HACK: FNV-1a ハッシュを実装する？
            return ToString().GetHashCode();
        }

        /// <summary>
        /// 等値演算子のオーバーロード。
        /// </summary>
        public static bool operator ==(UrlSafeSignature signature1, UrlSafeSignature signature2) {
            // ※ 自動実装。
            return signature1.Equals(signature2);
        }

        /// <summary>
        /// 非等値演算子のオーバーロード。
        /// </summary>
        public static bool operator !=(UrlSafeSignature signature1, UrlSafeSignature signature2) {
            // ※ 自動実装。
            return !(signature1 == signature2);
        }
    }
}
