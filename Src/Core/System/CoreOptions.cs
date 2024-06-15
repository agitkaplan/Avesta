using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avesta.Core.System
{
    public class CoreOptions
    {
        public const string Key = "Avesta:Core";

        public CryptographyOptions Cryptography { get; set; }
    }

    public class CryptographyOptions
    {
        public int KeySize { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
    }

}
