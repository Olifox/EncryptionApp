using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Crypt
{
    public class PermutationHard : ICrypt
    {
        private static string text;
        private static int number;
        private static int[] reshuff;

        public PermutationHard(string _text, int _number, string _reshuff)
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
            var col = text.Length / number + 1;
            List<char> ou = new List<char>();
            char[,] Input = new char[number, col];
            int j = 0, i = 0;

            foreach (char b in text)
            {
                if (i >= number)
                {
                    i = 0;
                    j++;
                }
                Input[i, j] = b;
                i++;
            }

            for (i = 0; i < number; i++)
            {
                j = 0;
                while (j < number)
                {
                    if (reshuff[j] == i + 1)
                        for (int z = 0; z < col; z++)
                        {
                            if (Input[j, z] == '\0')
                                ou.Add('\a');
                            else ou.Add(Input[j, z]);
                        }
                    j++;
                }
            }

            return string.Join("", ou);
        }

        public string Decryption()
        {
            text = Regex.Replace(text, "\a", " ");
            var col = 0;
            if (text.Length % number == 0)
                col = text.Length / number;
            else col = text.Length / number + 1;
            List<char> ou = new List<char>();
            char[,] Input = new char[number, col];

            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if ((reshuff[i] - 1) * col + j < text.Length)
                        Input[i, j] = (text[(reshuff[i] - 1) * col + j]);
                    else Input[i, j] = ('\a');
                }
            }

            for (int i = 0; i < col; i++)
                for (int j = 0; j < number; j++)
                    ou.Add(Input[j, i]);

            return string.Join("", ou);
        }
        #endregion
    }
}
