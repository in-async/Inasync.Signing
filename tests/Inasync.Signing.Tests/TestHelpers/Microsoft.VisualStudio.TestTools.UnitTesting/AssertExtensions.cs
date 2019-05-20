using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.VisualStudio.TestTools.UnitTesting {

    /// <summary>
    /// テスト検証のヘルパークラス。
    /// </summary>
    public static class AssertExtensions {

        public static void Is<T>(this T actual, T expected, string message = null) {
            Is(typeof(T), actual, expected, message);
        }

        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string message = null) {
            Is(typeof(IEnumerable<T>), actual, expected, message);
        }

        private static void Is(Type type, object actual, object expected, string message) {
            Debug.Assert(type != null);

            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string)) {
                CollectionAssert.AreEqual(((IEnumerable)expected).ToCollection(), ((IEnumerable)actual).ToCollection(), message);
                return;
            }

            if (type.FullName.StartsWith("System.ValueTuple`")) {
                foreach (var field in type.GetFields()) {
                    Is(field.FieldType, field.GetValue(actual), field.GetValue(expected), message + ":" + field.Name);
                }
                return;
            }

            Assert.AreEqual(expected, actual, message);
        }

        private static ICollection ToCollection(this IEnumerable source) {
            if (source == null) { return null; }
            if (source is ICollection casted) { return casted; }

            var list = new List<object>();
            foreach (var item in source) {
                list.Add(item);
            }
            return list;
        }
    }
}
