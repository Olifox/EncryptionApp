using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace Crypt
{
    public class Substition : ICrypt
    {
        private static string text;
        private static List<string> initialAlphabet;
        private static List<string> encryptAlphabet;

        public Substition(string _text, string _initialAlphabet, string _encryptAlphabet)
        {
            text = _text;

            initialAlphabet = _initialAlphabet.ToLower().Split('-').ToList();

            encryptAlphabet = _encryptAlphabet.ToLower().Split('-').ToList();
        }

        #region Public Methods
        public string Encryption()
        {
            if (initialAlphabet.Contains("") || encryptAlphabet.Contains(""))
                throw new InvalidDataException("Алфавит шифрования содержит пустые символы!");
            var AlphabetDictionare = MakeAlphabetDictionary( initialAlphabet,encryptAlphabet);
            var OutString = new List<string>();
            foreach (char b in text)
            {
                if (char.IsLetter(b))
                {
                    if (char.IsUpper(b))
                        OutString.Add(AlphabetDictionare[b.ToString().ToLower()].ToUpper());
                    else OutString.Add(AlphabetDictionare[b.ToString()]);
                }
                else
                    OutString.Add(b.ToString());
            }
            return string.Join("\a", OutString);
        }

        public string Decryption()
        {
            var reg = new Regex("[А-Я]|[A-Z]");
            var reg2 = new Regex("[-.?!)(, :]");
            var AlphabetDictionare = MakeAlphabetDictionary(encryptAlphabet, initialAlphabet);
            var OutString = new List<string>();
            var s = "";
            foreach (char b in text)
            {
                if (!b.Equals('\a'))
                {
                    if (reg2.IsMatch(b.ToString()))
                        OutString.Add(b.ToString());
                    else
                        s += String.Concat(b);
                } else
                if (!s.Equals("") && (AlphabetDictionare.ContainsKey(s) || AlphabetDictionare.ContainsKey(s.ToLower())))
                {
                    if (reg.IsMatch(s))
                        OutString.Add(AlphabetDictionare[s.ToLower()].ToUpper());
                    else OutString.Add(AlphabetDictionare[s]);
                    s = "";
                }
            }
            return string.Join("", OutString);
        }
        #endregion

        #region Private Methods

        private Dictionary<string, string> MakeAlphabetDictionary(List<string> alphabet, List<string> encryprionAlphabet)
        {
            var AlphabetDictionary = new Dictionary<string, string>();
            for (int i = 0; i < alphabet.Count; i++)
            {
                if (AlphabetDictionary.ContainsValue(encryprionAlphabet[i]))
                    throw new ArgumentException();
                AlphabetDictionary.Add(alphabet[i], encryprionAlphabet[i]);
            }
            return AlphabetDictionary;
        }
        #endregion
    }
}
