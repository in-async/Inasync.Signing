using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Signing.Tests {

    [TestClass]
    public class UrlSafeSignatureTests {

        [TestMethod]
        public void Ctor() {
            Action TestCase(int testNumber, byte[] hash, Type expectedException) => () => {
                var version = (byte)Rand.Int();

                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => new UrlSafeSignature(version, hash))
                    .Verify((actual, desc) => {
                        actual.Version.Is(version, desc);
                    }, expectedException);
            };

            new[] {
                TestCase( 0, null        , typeof(ArgumentNullException)),
                TestCase( 1, new byte[0] , null),
                TestCase( 2, Rand.Bytes(), null),
            }.Run();
        }

        [TestMethod]
        public void TryParse() {
            Action TestCase(int testNumber, string signature, (bool success, UrlSafeSignature result) expected) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => (success: UrlSafeSignature.TryParse(signature, out var result), result))
                    .Verify((actual, desc) => {
                        actual.success.Is(expected.success, desc);
                        actual.result.Is(expected.result, desc);
                    }, (Type)null);
            };

            new[] {
                TestCase( 0, null        , (false, default)),
                TestCase( 1, ""          , (false, default)),
                TestCase( 2, "A"         , (false, default)),
                TestCase( 3, "AA"        , (true , new UrlSafeSignature(0 , new byte[0]))),
                TestCase( 3, "DCI4"      , (true , new UrlSafeSignature(12, new byte[]{ 34, 56 }))),
            }.Run();
        }

        [TestMethod]
        public void ToByteArray() {
            Action TestCase(int testNumber, UrlSafeSignature signature, byte[] expected) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => signature.ToByteArray())
                    .Verify(expected, (Type)null);
            };

            new[] {
                TestCase( 0, new UrlSafeSignature()                              , new byte[0]),
                TestCase( 1, new UrlSafeSignature(0  , new byte[0])              , new byte[]{0}),
                TestCase( 2, new UrlSafeSignature(123, new byte[]{ 234, 255, 0 }), new byte[]{123, 234, 255, 0}),
            }.Run();
        }

        [TestMethod]
        public new void ToString() {
            Action TestCase(int testNumber, UrlSafeSignature signature, string expected) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => signature.ToString())
                    .Verify(expected, (Type)null);
            };

            new[] {
                TestCase( 0, new UrlSafeSignature()                              , ""),
                TestCase( 1, new UrlSafeSignature(0  , new byte[0])              , "AA"),
                TestCase( 2, new UrlSafeSignature(123, new byte[]{ 234, 255, 0 }), "e-r_AA"),
            }.Run();
        }

        [TestMethod]
        public void Equals() {
            Action TestCase(int testNumber, UrlSafeSignature signature, UrlSafeSignature other, bool expected) => () => {
                new TestCaseRunner($"No.{testNumber}: Equals")
                    .Run(() => signature.Equals(other))
                    .Verify(expected, null);

                new TestCaseRunner($"No.{testNumber}: Equals(object)")
                    .Run(() => signature.Equals((object)other))
                    .Verify(expected, null);

                // 全く関係のない object との比較なので、常に false。
                new TestCaseRunner($"No.{testNumber}: Equals(new object)")
                    .Run(() => signature.Equals(new object()))
                    .Verify(false, null);

                new TestCaseRunner($"No.{testNumber}: ==")
                    .Run(() => signature == other)
                    .Verify(expected, null);

                new TestCaseRunner($"No.{testNumber}: !=")
                    .Run(() => signature != other)
                    .Verify(!expected, null);

                new TestCaseRunner($"No.{testNumber}: object.Equals(object)")
                    .Run(() => ((object)signature).Equals((object)other))
                    .Verify(expected, null);

                // == 演算子オーバーロードが機能しないので、常に false。
                new TestCaseRunner($"No.{testNumber}: object == object")
                    .Run(() => (object)signature == (object)other)
                    .Verify(false, null);
            };

            new[] {
                TestCase( 0, new UrlSafeSignature()                   , new UrlSafeSignature()              , true ),
                TestCase( 1, new UrlSafeSignature(1, new byte[]{2, 3}), new UrlSafeSignature(1, new byte[]{2, 3}), true ),
                TestCase( 2, new UrlSafeSignature(1, new byte[]{2, 3}), new UrlSafeSignature(1, new byte[]{2})   , false),
                TestCase( 3, new UrlSafeSignature(1, new byte[]{2, 3}), new UrlSafeSignature(0, new byte[]{2, 3}), false),
            }.Run();
        }

        [TestMethod]
        public new void GetHashCode() {
            // 一意性が同一のインスタンス同士の GetHashCode() が同値の検証。
            new TestCaseRunner()
                .Run(() => new UrlSafeSignature(1, new byte[] { 2, 3 }).GetHashCode() == new UrlSafeSignature(1, new byte[] { 2, 3 }).GetHashCode())
                .Verify(true, null);

            // 一意性の異なるインスタンス同士の GetHashCode() が異なる事の検証。
            // ２つのインスタンスだと偶然同じ値になる可能性があるので、３つのインスタンスが同じ値にならない事を確認する。
            var sig0 = new UrlSafeSignature();
            var sig1 = new UrlSafeSignature(1, new byte[] { 1 });
            var sig2 = new UrlSafeSignature(2, new byte[] { 2, 2 });
            new TestCaseRunner()
                .Run(() => sig0.GetHashCode() == sig1.GetHashCode() && sig1.GetHashCode() == sig2.GetHashCode())
                .Verify(false, null);
        }
    }
}
