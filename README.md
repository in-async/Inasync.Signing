# Inasync.Signing
[![Build status](https://ci.appveyor.com/api/projects/status/tlpx2yc9ret43fd2/branch/master?svg=true)](https://ci.appveyor.com/project/inasync/inasync-signing/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Inasync.Signing.svg)](https://www.nuget.org/packages/Inasync.Signing/)

***Inasync.Signing*** はバージョニングされた署名の為のシンプルな .NET ライブラリです。


## Target Frameworks
- .NET Standard 2.0+
- .NET Standard 1.3+
- .NET Framework 4.5+


## Usage
### Sign
```cs
byte[] signKey = Base16.Decode("12BEF7DA731D6A1DB6F0AA98904E54FC9822A5592FBD8DAA1535C02153A34094D7B15A1ED31EF007DDDB0CD0F0728D4985D67775CAA31ECF5BD23B2CC4004614");
ITextSigning signing = new HS256TextSigning(signKey, version: 1);

var message = "FOO";
UrlSafeSignature signature = signing.ComputeSignature(message);

Console.WriteLine(signature.ToString());  // Afh9IuBiL8HaPxiCrPLMC4vKgdWIqaeM-n0ke8H8KUjM
```

### Verify
```cs
byte[] signKey = Base16.Decode("12BEF7DA731D6A1DB6F0AA98904E54FC9822A5592FBD8DAA1535C02153A34094D7B15A1ED31EF007DDDB0CD0F0728D4985D67775CAA31ECF5BD23B2CC4004614");
ITextSigning signing = new HS256TextSigning(signKey, version: 1);

var success = Verify(
      message: "FOO"
    , signatureStr: "Afh9IuBiL8HaPxiCrPLMC4vKgdWIqaeM-n0ke8H8KUjM"
);

Console.WriteLine(success);  // True

bool Verify(string message, string signatureStr) {
    if (!UrlSafeSignature.TryParse(signatureStr, out var signature)) { return false; }

    UrlSafeSignature expected = signing.ComputeSignature(message);
    return signature == expected;
}
```


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
