using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Signing.Tests {

    [TestClass]
    public class HS256TextSigningTests {

        [TestMethod]
        public void Ctor() {
            Action TestCase(int testNumber, byte[] signKey, Type expectedException) => () => {
                var version = (byte)Rand.Int();

                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => new HS256TextSigning(signKey, version))
                    .Verify((actual, desc) => {
                        actual.Version.Is(version, desc);
                    }, expectedException);
            };

            new[] {
                TestCase( 0, null       , typeof(ArgumentNullException)),
                TestCase( 1, new byte[0], null)
            }.Run();
        }

        [TestMethod]
        public void ComputeSignature() {
            var version = (byte)Rand.Int();
            var signKey = Binary.Parse("12BEF7DA731D6A1DB6F0AA98904E54FC9822A5592FBD8DAA1535C02153A34094D7B15A1ED31EF007DDDB0CD0F0728D4985D67775CAA31ECF5BD23B2CC4004614");
            var signing = new HS256TextSigning(signKey, version);

            Action TestCase(int testNumber, string message, UrlSafeSignature expected, Type expectedExceptionType) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => signing.ComputeSignature(message))
                    .Verify(expected, expectedExceptionType);
            };

            new[] {
                TestCase( 0, null    , default                                                                                                        , typeof(ArgumentNullException)),
                TestCase( 1, ""      , new UrlSafeSignature(version, Binary.Parse("1d1ca4d8b9b68c3416d2e31bbcb9a80e4d0ce83a14676e2f73acea7254414a1b")), null),
                TestCase( 2, "A"     , new UrlSafeSignature(version, Binary.Parse("22ebf48a60160e341eac898fd816d0a798ec93651b29c7bb18497e0907843056")), null),
                TestCase( 3, "${FOO}", new UrlSafeSignature(version, Binary.Parse("3fc90fb91d915c3262e214d9fcac2bd18dca6cc97325b6f0aa4c9eb32415ae12")), null),
            }.Run();
        }
    }
}
