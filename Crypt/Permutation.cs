using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Crypt
{
    public class Permutation : ICrypt
    {
        private static string text;
        private static int number;
        private static int[] reshuff;

        public Permutation(string _text, int _number, string _reshuff)
        {
            text = _text;
            number = _number;
            reshuff = _reshuff.Split('-').Select(m => Convert.ToInt32(m)).ToArray();
        }

        #region Public Methods
        public string Encryption()
        {
            //text = Regex.Replace(text, "[-.?!)(,:]", "");
            text = Regex.Replace(text, " ", "\a");
            //text = text.ToLower();
            List<char> ou = new List<char>();
            for (int i = 0; i < text.Length; i += number)
            {
                for (int j = 0; j < number; j++)
                {
                    if (reshuff[j] - 1 + i < text.Length)
                        ou.Add(text[reshuff[j] - 1 + i]);
                    else ou.Add('\a');
                }
            }
            return string.Join("", ou);
        }

        public string Decryption()
        {
            text = Regex.Replace(text, "\a", " ");
            List<char> ou = new List<char>();
            for (int i = 0; i < text.Length; i += number)
            {
                for (int j = 0; j < number; j++)
                    ou.Add(Init(i, j));
            }
            return string.Join("", ou);
        }

        private char Init(int a, int b)
        {
            var j = 0;
            while (reshuff[j] != 0)
            {
                if (a + j >= text.Length)
                    return '\a';
                else
                {
                    if (reshuff[j] == b + 1)
                        return text[a + j];
                    j++;
                }
            }
            return '\0';
        }
        #endregion
    }
}