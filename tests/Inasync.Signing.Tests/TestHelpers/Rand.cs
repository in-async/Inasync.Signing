using System.Linq;

namespace System {

    /// <summary>
    /// 乱数ヘルパー クラス。
    /// </summary>
    public static class Rand {
        private static readonly Random _rnd = new Random();

        /// <summary>
        /// 0.0 から 1.0 の間の浮動小数点数を返します。
        /// </summary>
        /// <returns>0.0 以上 1.0 未満の <see cref="double"/> 値。</returns>
        public static double Next() => _rnd.NextDouble();

        /// <summary>
        /// <see cref="int"/> 値の乱数を返します。
        /// </summary>
        /// <param name="min">乱数の下限。既定値は 0。</param>
        /// <param name="max">乱数の上限。既定値は <see cref="int.MaxValue"/>。</param>
        /// <returns><paramref name="min"/> 以上 <paramref name="max"/> 未満の <see cref="int"/> 値。</returns>
        public static int Int(int min = 0, int max = int.MaxValue) => _rnd.Next(min, max);

        /// <summary>
        /// <see cref="decimal"/> 値の乱数を返します。
        /// </summary>
        /// <param name="min">乱数の下限。既定値は 0。</param>
        /// <param name="max">乱数の上限。既定値は <see cref="decimal.MaxValue"/>。</param>
        /// <returns><paramref name="min"/> 以上 <paramref name="max"/> 未満の <see cref="decimal"/> 値。</returns>
        public static decimal Decimal(decimal min = 0, decimal max = decimal.MaxValue) => (decimal)_rnd.NextDouble() * max;

        /// <summary>
        /// <see cref="bool"/> 値をランダムに返します。
        /// </summary>
        /// <returns><see cref="bool"/> 値。</returns>
        public static bool Bool() => _rnd.NextDouble() < .5;

        /// <summary>
        /// <typeparamref name="TEnum"/> に定義されている値をランダムに返します。
        /// </summary>
        /// <typeparam name="TEnum">ランダムに選ばれる列挙値の型。</typeparam>
        /// <returns><typeparamref name="TEnum"/> に定義されている値。何も定義されていない場合は <c>default</c>。</returns>
        public static TEnum Enum<TEnum>() where TEnum : Enum {
            var enums = System.Enum.GetValues(typeof(TEnum));
            if (enums.Length == 0) { return default; }

            return (TEnum)enums.GetValue(_rnd.Next(enums.Length));
        }

        /// <summary>
        /// 0x20 から 0x7E の文字で構成されたランダムな文字列を返します。
        /// </summary>
        /// <param name="minLength">最小文字数。既定値は 0。</param>
        /// <param name="maxLength">最大文字数。既定値は 10。</param>
        /// <returns>文字数が <paramref name="minLength"/> 以上 <paramref name="maxLength"/> 未満のランダムな文字列。常に非 <c>null</c>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minLength"/> または <paramref name="maxLength"/> が負の値です。
        /// または、<paramref name="minLength"/> の値が <paramref name="maxLength"/> を超えています。
        /// </exception>
        public static string String(int minLength = 0, int maxLength = 10) {
            if (minLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(minLength)); }
            if (maxLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(maxLength)); }

            var chars = new char[_rnd.Next(minLength, maxLength)];
            for (var i = 0; i < chars.Length; i++) {
                chars[i] = (char)_rnd.Next(0x20, 0x7e);
            }
            return new string(chars);
        }

        /// <summary>
        /// 英数字のみで構成されたランダムな文字列を返します。
        /// </summary>
        /// <param name="minLength">最小文字数。既定値は 0。</param>
        /// <param name="maxLength">最大文字数。既定値は 10。</param>
        /// <returns>文字数が <paramref name="minLength"/> 以上 <paramref name="maxLength"/> 未満のランダムな文字列。常に非 <c>null</c>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minLength"/> または <paramref name="maxLength"/> が負の値です。
        /// または、<paramref name="minLength"/> の値が <paramref name="maxLength"/> を超えています。
        /// </exception>
        public static string AlphaNums(int minLength = 0, int maxLength = 10) {
            const string reserved = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (minLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(minLength)); }
            if (maxLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(maxLength)); }

            var chars = new char[_rnd.Next(minLength, maxLength)];
            for (var i = 0; i < chars.Length; i++) {
                chars[i] = reserved[_rnd.Next(reserved.Length)];
            }
            return new string(chars);
        }

        /// <summary>
        /// ランダムな <see cref="byte"/> の配列を返します。
        /// </summary>
        /// <param name="minLength">最小配列長。既定値は 0。</param>
        /// <param name="maxLength">最大配列長。既定値は 10。</param>
        /// <returns>配列長が <paramref name="minLength"/> 以上 <paramref name="maxLength"/> 未満のランダムな <see cref="byte"/> 配列。常に非 <c>null</c>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minLength"/> または <paramref name="maxLength"/> が負の値です。
        /// または、<paramref name="minLength"/> の値が <paramref name="maxLength"/> を超えています。
        /// </exception>
        public static byte[] Bytes(int minLength = 0, int maxLength = 10) {
            if (minLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(minLength)); }
            if (maxLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(maxLength)); }

            var bytes = new byte[_rnd.Next(minLength, maxLength)];
            _rnd.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// ランダムな <see cref="byte"/> の配列を返します。
        /// </summary>
        /// <param name="count">配列長。</param>
        /// <returns>配列長が <paramref name="count"/> のランダムな <see cref="byte"/> 配列。常に非 <c>null</c>。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> が負の値です。</exception>
        public static byte[] Bytes(int count) {
            if (count < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(count)); }

            var bytes = new byte[count];
            _rnd.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// ランダムな長さの配列を返します。
        /// </summary>
        /// <typeparam name="T">要素の型。</typeparam>
        /// <param name="factory">要素のファクトリー オブジェクト。</param>
        /// <param name="minLength">最小配列長。既定値は 0。</param>
        /// <param name="maxLength">最大配列長。既定値は 10。</param>
        /// <returns>配列長が <paramref name="minLength"/> 以上 <paramref name="maxLength"/> 未満の配列。常に非 <c>null</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minLength"/> または <paramref name="maxLength"/> が負の値です。
        /// または、<paramref name="minLength"/> の値が <paramref name="maxLength"/> を超えています。
        /// </exception>
        public static T[] Array<T>(Func<int, T> factory, int minLength = 0, int maxLength = 10) {
            if (factory == null) { throw new ArgumentNullException(nameof(factory)); }
            if (minLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(minLength)); }
            if (maxLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(maxLength)); }

            return Enumerable.Range(0, _rnd.Next(minLength, maxLength)).Select(factory).ToArray();
        }

        /// <summary>
        /// 配列からランダムに一つを選択します。
        /// </summary>
        /// <typeparam name="T">要素の型。</typeparam>
        /// <param name="array">対象の配列。</param>
        /// <returns>選択された要素。<paramref name="array"/> の配列長が 0 の場合は <c>default</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        public static T One<T>(params T[] array) {
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            if (array.Length == 0) { return default; }

            return array[_rnd.Next(array.Length)];
        }
    }
}
