using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.Sec;

namespace Mastersign.Gate
{
    static class CertificateBuilder
    {
        public static void CreateSelfSignedCertificate(Certificate certInfo,
            string certificateFile, string keyFile)
        {
            var rndGen = new CryptoApiRandomGenerator();
            var rnd = new SecureRandom(rndGen);

            var keyGenParams = new KeyGenerationParameters(rnd, 2048);
            var keyPairGen = new RsaKeyPairGenerator();
            // var keyGenParams = new ECKeyGenerationParameters(SecObjectIdentifiers.SecP256r1, rnd);
            // var keyPairGen = new ECKeyPairGenerator();
            keyPairGen.Init(keyGenParams);
            var keyPair = keyPairGen.GenerateKeyPair();

            var certGen = new X509V3CertificateGenerator();
            var serialNo = BigIntegers.CreateRandomInRange(
                BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), rnd);
            certGen.SetSerialNumber(serialNo);
            certGen.SetSubjectDN(certInfo.X509Name());
            certGen.SetIssuerDN(certInfo.X509Name());
            var now = DateTime.UtcNow.Date;
            certGen.SetNotBefore(now);
            certGen.SetNotAfter(now.AddDays(certInfo.ValidDays));
            certGen.SetPublicKey(keyPair.Public);

            var sigFac = new Asn1SignatureFactory("SHA256WithRSA", keyPair.Private, rnd);
            var cert = certGen.Generate(sigFac);

            using (var w = new StreamWriter(certificateFile, false, Encoding.ASCII)) {
                var pemWriter = new PemWriter(w);
                pemWriter.WriteObject(cert);
            }
            using (var w = new StreamWriter(keyFile, false, Encoding.ASCII))
            {
                var pemWriter = new PemWriter(w);
                pemWriter.WriteObject(keyPair);
            }
        }

        public static X509Name X509Name(this Certificate cert)
        {
            return new X509Name("CN=" + cert.CommonName);
        }
    }
}
