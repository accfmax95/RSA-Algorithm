using System;
using System.Numerics;

class Program {

    public static BigInteger generatePrivateKey(BigInteger e, BigInteger p, BigInteger q) {
        
        BigInteger phiN = (p - 1) * (q - 1);

        // Use the Extended Euclidean Algorithm to find d: (e * d) % phi(n) = 1
        BigInteger d = ExtendedGCD(e, phiN).Item1;

        while (d < 0) {

            d += phiN;
        }

        return d;
    }

    public static string decrypt(string ciphertext, BigInteger key, BigInteger N) {

        BigInteger c = BigInteger.Parse(ciphertext);
        BigInteger decryptedValue = BigInteger.ModPow(c, key, N);
        return decryptedValue.ToString();
    }

    public static string encrypt(string plaintext, BigInteger key, BigInteger N) {

        BigInteger m = BigInteger.Parse(plaintext);
        BigInteger encryptedValue = BigInteger.ModPow(m, key, N);
        return encryptedValue.ToString();
    }

    private static Tuple<BigInteger, BigInteger> ExtendedGCD(BigInteger a, BigInteger b) {
        
        if (a == 0) {

            return Tuple.Create(BigInteger.Zero, BigInteger.One);
        } else {

            Tuple<BigInteger, BigInteger> result = ExtendedGCD(b % a, a);
            BigInteger x = result.Item1;
            BigInteger y = result.Item2;
            BigInteger gcd = result.Item1;

            return Tuple.Create(y - (b / a) * x, x);
        }
    }

    static void Main(string[] args) {

        if (args.Length != 8) {

            Console.WriteLine("Err / Input should be 'dotnet run pe pc qe qc ee ec Ciphertext Plaintext'");
            return;
        }

        BigInteger pe = BigInteger.Parse(args[0]);
        BigInteger pc = BigInteger.Parse(args[1]);
        BigInteger qe = BigInteger.Parse(args[2]);
        BigInteger qc = BigInteger.Parse(args[3]);
        BigInteger ee = BigInteger.Parse(args[4]);
        BigInteger ec = BigInteger.Parse(args[5]);
        String Ciphertext = args[6];
        String Plaintext = args[7];

        // Finding p, q, e, and N
        BigInteger p = BigInteger.Pow(2, (int) pe) - pc;
        BigInteger q = BigInteger.Pow(2, (int) qe) - qc;
        BigInteger e = BigInteger.Pow(2, (int) ee) - ec;
        BigInteger N = p * q;

        BigInteger d = generatePrivateKey(e, p, q);
        string decryptedText = decrypt(Ciphertext, d, N);
        string encryptedCiphertext = encrypt(Plaintext, e, N);

        Console.WriteLine($"{decryptedText},{encryptedCiphertext}");
    }

    // dotnet run 254 1223 251 1339 17 65535 66536047120374145538916787981868004206438539248910734713495276883724693574434582104900978079701174539167102706725422582788481727619546235440508214694579 1756026041 

    // Expected output: 1756026041,66536047120374145538916787981868004206438539248910734713495276883724693574434582104900978079701174539167102706725422582788481727619546235440508214694579 
}
