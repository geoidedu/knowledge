using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace Bank.Auth.Features.Auth.Services.crypto
{
    public class Argon2Crypto
    {
        private IConfiguration _config;
        private readonly CryptoSalt _salt;

        public Argon2Crypto(IConfiguration config)
        { 
            _config = config;
            _salt = new CryptoSalt { Salt = _config.GetValue<string>("CryptoOptions:Salt") };
        }

        /// <summary>
        /// Проверка хэша
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashpassword"></param>
        /// <returns></returns>
        public Boolean VerifyHash(string password, string hashpassword)
        {
            var newHash = HashPassword(password);
            return Convert.FromBase64String(hashpassword).SequenceEqual(newHash);
        }

        /// <summary>
        /// Хеширование пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public byte[] HashPassword(string password)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = Convert.FromBase64String(_salt.Salt);
            argon2.DegreeOfParallelism = 8; // four cores
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; // 1 GB

            return argon2.GetBytes(16);
        }
    }
}
