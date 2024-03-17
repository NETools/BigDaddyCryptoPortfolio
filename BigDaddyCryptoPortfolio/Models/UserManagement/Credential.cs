using BigDaddyCryptoPortfolio.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.UserManagement
{
    public class Credential
    {
        private string _value;

        public Credential(string value, CredentialType credentialType)
        {
            _value = Toolkit.HashingAlgorithms[credentialType].Hash(value);
        }
    }
}
