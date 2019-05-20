using System;
using System.Security.Cryptography;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Signing.Tests {

    [TestClass]
    public class HMACTextSigningTests {

        [TestMethod]
        public void Ctor() {
            Action TestCase(int testNumber, Func<HMAC> factory, Type expectedException) => () => {
                var version = (byte)Rand.Int();

                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => new HMACTextSigning(factory, version))
                    .Verify((actual, desc) => {
                        actual.Version.Is(version, desc);
                    }, expectedException);
            };

            new[] {
                TestCase( 0, null                , typeof(ArgumentNullException)),
                TestCase( 1, () => new HMACSHA1(), null)
            }.Run();
        }

        [TestMethod]
        public void ComputeSignature() {
            var version = (byte)Rand.Int();
            var signKey = Binary.Parse("12BEF7DA731D6A1DB6F0AA98904E54FC9822A5592FBD8DAA1535C02153A34094D7B15A1ED31EF007DDDB0CD0F0728D4985D67775CAA31ECF5BD23B2CC4004614");
            var signing = new HMACTextSigning(() => new HMACSHA1(signKey), version);

            Action TestCase(int testNumber, string message, UrlSafeSignature expected, Type expectedExceptionType) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => signing.ComputeSignature(message))
                    .Verify(expected, expectedExceptionType);
            };

            new[] {
                TestCase( 0, null    , default                                                                                , typeof(ArgumentNullException)),
                TestCase( 1, ""      , new UrlSafeSignature(version, Binary.Parse("a8c6cc220cd4b0a134bdb7b6096a762f87e0e369")), null),
                TestCase( 2, "A"     , new UrlSafeSignature(version, Binary.Parse("2f629b48aec06aea512bd1eb883a10b5da6c6c01")), null),
                TestCase( 3, "${FOO}", new UrlSafeSignature(version, Binary.Parse("df04d6e795952e2ffaed31e2bb13c74c12574d42")), null),
            }.Run();
        }
    }
}
