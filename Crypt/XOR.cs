using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypt
{
    public class XOR: ICrypt
    {
        private string text;
        private string key;

        public XOR(string _text, string _key)
        {
            text = _text;
            key = _key;
        }

        #region Public Method
        
        public string Decryption()
        {
            List<int> Out = new List<int>();
            for (int i = text.Length; i > 0; i--)
                Out.Add(text.Length-i > key.Length ? text[i] : text[i] ^ key[i]); 
            return string.Join("",Out);
        }

        public string Encryption()
        {
            int Tlength = text.Length - 1;
            int Klength = key.Length - 1;
            List<int> Out = new List<int>();
            for (int i = Tlength; i >= 0; i--)
                Out.Add(Tlength - i > Klength ? text[i] : text[i] ^ key[Klength - (Tlength - i)]);
            return string.Join("", Out);
        }

        #endregion
    }
}
