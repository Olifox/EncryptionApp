using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Crypt
{
    public class Caesar : ICrypt
    {
        private static string text;
        private static string initialAlphabet;
        private static int shift;

        public Caesar(string _text, int _shift, string _initialAlphabet)
        {
            text = _text;
            shift = _shift;
            initialAlphabet = Regex.Replace(_initialAlphabet, "[-]", "");
        }

        #region Public Methods
        public string Encryption()
        {
            var encryptAlphabet = Mix(initialAlphabet, shift);
            var ou = new String[text.Length];
            int j = 0;
            foreach (char q in text)
            {
                if (char.IsLetter(q))
                {
                    for (int i = 0; i < 33; i++)
                    {
                        if ((q.CompareTo(initialAlphabet[i]) == 0) || (q.CompareTo(char.ToUpper(initialAlphabet[i])) == 0))
                        {
                            if (char.IsUpper(q))
                                ou[j] = char.ToUpper(encryptAlphabet[i]).ToString();
                            else
                                ou[j] = encryptAlphabet[i].ToString();
                            break;
                        }
                    }
                }
                else ou[j] = q.ToString();
                j++;
            }
            return string.Join("", ou);
        }

        public string Decryption()
        {
            var encryptAlphabet = Mix(initialAlphabet, shift);
            var ou = new String[text.Length];
            int j = 0;
            foreach (char q in text)
            {
                if (char.IsLetter(q))
                {
                    for (int i = 0; i < 33; i++)
                    {
                        if ((q.CompareTo(encryptAlphabet[i]) == 0) || (q.CompareTo(char.ToUpper(encryptAlphabet[i])) == 0))
                        {
                            if (char.IsUpper(q))
                                ou[j] = char.ToUpper(initialAlphabet[i]).ToString();
                            else
                                ou[j] = initialAlphabet[i].ToString();
                            break;
                        }
                    }
                }
                else ou[j] = q.ToString();
                j++;
            }
            return string.Join("", ou);
        }
        #endregion

        #region Private Methods
        private static string Mix(string a, int n)
        {
            var b = a.ToCharArray();
            for (int i = 0; i < a.Count(); i++)
                if (i + n >= a.Count())
                    b[i] = a[(i + n) % a.Count()];
                else if (i + n < 0)
                {
                    var m = (i + n) % a.Count();
                    b[i] = a[a.Count() - 1 + ((i + n) % a.Count())];
                }
                else
                    b[i] = a[i + n];
            return string.Join("", b);
        }
        #endregion
    }
}
