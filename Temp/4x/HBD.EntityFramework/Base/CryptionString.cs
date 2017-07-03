using HBD.Framework;
using HBD.Framework.Security;
using HBD.Framework.Security.Services;
using System;

namespace HBD.EntityFramework.Base
{
    /// <summary>
    /// The CryptionString for entity property to encrypt and decrypt string value for an entity when store value into Db and retrieve value from Db.
    /// </summary>
    [Serializable]
    public class CryptionString : DbWrapValue<string, string>
    {
        private static ICryptionService _cryptionAdapter = null;

        public static ICryptionService CryptionAdapter
        {
            get { return _cryptionAdapter ?? CryptionManager.Default; }
            set { _cryptionAdapter = value; }
        }

        public override string DbValue
        {
            get { return CryptionAdapter.Encrypt(this.Value); }
            set { this.Value = CryptionAdapter.Decrypt(value); }
        }

        public override string ToString() => this.Value;

        public static implicit operator string(CryptionString value) => value?.Value;

        public static implicit operator CryptionString(string value)
        {
            if (value.IsEncrypted())
                return new CryptionString { DbValue = value };
            return new CryptionString { Value = value };
        }
    }
}