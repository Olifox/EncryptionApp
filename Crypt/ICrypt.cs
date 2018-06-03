using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypt
{
    public interface ICrypt
    {
        string Encryption();
        string Decryption();
    }
}
