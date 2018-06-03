using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Crypt;
using System.IO;
using System.Collections.Generic;

namespace Main
{
    public partial class MainWindow : Window
    {
        #region Var

        public MainWindow()
        {
            InitializeComponent();
        }

        private string _alphabet;

        #endregion

        #region Click

        private void UnTranslate_Click(object sender, RoutedEventArgs e)
        {
            TranslateHandler(false);
        }

        private void Translate_Click(object sender, RoutedEventArgs e)
        {
            TranslateHandler(true);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Text_Click(object sender, RoutedEventArgs e)
        {
            Text_Box1.Clear();
        }

        private void Text_box3_KeyDown(object sender, KeyEventArgs e)
        {
            Regex reg = new Regex("[0-9]|(Subtract)|(OemMinus)");
            if (!reg.IsMatch(e.Key.ToString()))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Select&Change

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox1.SelectedIndex.ToString())
            {
                case "0":
                    Text_box3.Visibility = Visibility.Hidden;
                    Text_block1.Text = "";
                    Text_block2.Text = "Вторичный алфавит:";
                    Text_Box4.Visibility = Visibility;
                    Text_Box4.Text = "";
                    ComboBox2.Visibility = Visibility;
                    break;
                case "1":
                    Text_box3.Visibility = Visibility;
                    Text_block1.Text = "Укажите сдвиг";
                    Text_block2.Text = "";
                    Text_Box4.Visibility = Visibility.Hidden;
                    ComboBox2.Visibility = Visibility;
                    break;
                case "2":
                    Text_box3.Visibility = Visibility;
                    Text_block2.Text = "";
                    Text_block1.Text = "Укажите ключ";
                    Text_Box4.Visibility = Visibility.Hidden;
                    ComboBox2.Visibility = Visibility.Hidden;
                    break;
                case "3":
                    Text_box3.Visibility = Visibility;
                    Text_block1.Text = "Кол-во столбцов";
                    Text_block2.Text = "Последовательность:";
                    Text_Box4.Visibility = Visibility;
                    Text_Box4.Text = "";
                    ComboBox2.Visibility = Visibility.Hidden;
                    break;
                case "4":
                    Text_box3.Visibility = Visibility;
                    Text_block1.Text = "Кол-во столбцов";
                    Text_block2.Text = "Последовательность:";
                    Text_Box4.Visibility = Visibility;
                    Text_Box4.Text = "";
                    ComboBox2.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }

        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = ComboBox2.SelectedIndex.ToString();
            switch (a)
            {
                case "0":
                    _alphabet = "а-б-в-г-д-е-ё-ж-з-и-й-к-л-м-н-о-п-р-с-т-у-ф-х-ц-ч-ш-щ-ъ-ы-ь-э-ю-я";
                    Text_Box4.Text = EncryptionAlphabet(_alphabet);
                    break;
                case "1":
                    _alphabet = "a-b-c-d-e-f-g-h-i-j-k-l-m-n-o-p-q-r-s-t-u-v-w-x-y-z";
                    Text_Box4.Text = EncryptionAlphabet(_alphabet);
                    break;
                default:
                    break;
            }
        }

        private void Text_box3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text_box3.Text != "" && Text_box3.Text != "-")
                if (Convert.ToInt32(Text_box3.Text) > 0 && (ComboBox1.SelectedIndex.ToString() == "3" || ComboBox1.SelectedIndex.ToString() == "4"))
                    Text_Box4.Text = Ref(Convert.ToInt32(Text_box3.Text));
                else Text_Box4.Text = "";
        }

        #endregion

        #region Private Methods

        private string EncryptionAlphabet(string alphabet)
        {
            alphabet = Regex.Replace(alphabet, "-", "");
            Random ran = new Random();
            char[] EncryptionAlphabet = alphabet.ToArray();
            for (var i = alphabet.Length - 1; i > 0; i--)
            {
                var r = ran.Next(i);
                var c = EncryptionAlphabet[r];
                EncryptionAlphabet[r] = EncryptionAlphabet[i];
                EncryptionAlphabet[i] = c;
            }
            return string.Join("-", EncryptionAlphabet);
        }

        private string Ref(int n)
        {
            var x = new int[n];
            var i = 1;
            var rand = new Random();
            x[0] = rand.Next(1, n + 1);
            while (i < n)
            {
                x[i] = rand.Next(1, n + 1);
                if (Prov(x, i))
                    i++;
            }
            return string.Join("-", x);
        }

        private bool Prov(int[] x, int i)
        {
            for (int j = i - 1; j >= 0; j--)
                if (x[i] == x[j])
                    return false;
            return true;
        }

        private void CheckReshuff(int a, List<string> reshuff)
        {
            if (a != reshuff.Count)
                throw new InvalidDataException("Неверная перестановка!");
            var result = false;
            for (int i = 1; i < a + 1; i++)
            {
                result = false;
                foreach (string c in reshuff)
                    if (!string.IsNullOrEmpty(c) ? Convert.ToInt32(c) == i : false)
                        result = true;
                if (!result)
                    throw new InvalidDataException("Неверная перестановка!");
            }
            if (!result)
                throw new InvalidDataException("Неверное количество столбцов!");
        }

        private void CheckXOR(string text, string key)
        {
            Regex reg = new Regex("[^01]");
            if (reg.IsMatch(text))
                throw new InvalidDataException("Введен неверный текст!");
            if (reg.IsMatch(key) || string.IsNullOrEmpty(key))
                throw new InvalidDataException("Указан неверный ключ!");
        }

        private void TranslateHandler(bool cr)
        {

            try
            {
                ICrypt crypt;
                switch (ComboBox1.SelectedIndex.ToString())
                {
                    case "0":
                        crypt = new Substition(Text_Box1.Text, _alphabet, Text_Box4.Text);
                        break;
                    case "1":
                        if (_alphabet == null)
                            throw new NullReferenceException();
                        crypt = new Caesar(Text_Box1.Text, Convert.ToInt32(Text_box3.Text), _alphabet);
                        break;
                    case "2":
                        CheckXOR(Text_Box1.Text, Text_box3.Text);
                        crypt = new XOR(Text_Box1.Text, Text_box3.Text);
                        break;
                    case "3":
                        CheckReshuff(Convert.ToInt32(Text_box3.Text), Text_Box4.Text.Split('-').ToList());
                        crypt = new Permutation(Text_Box1.Text, Convert.ToInt32(Text_box3.Text), Text_Box4.Text);
                        break;
                    case "4":
                        CheckReshuff(Convert.ToInt32(Text_box3.Text), Text_Box4.Text.Split('-').ToList());
                        crypt = new PermutationHard(Text_Box1.Text, Convert.ToInt32(Text_box3.Text), Text_Box4.Text);
                        break;
                    default:
                        throw new InvalidDataException("Не выбран алгоритм шифрования!");
                }
                if (cr)
                    Text_Box2.Text = crypt.Encryption();
                else
                    Text_Box2.Text = crypt.Decryption();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Неверный алфавит шифрования", "Error");
                File.AppendAllText("../../../Error/StackTrace.txt", ex.StackTrace);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Не выбран алфавит шифрования", "Error");
                File.AppendAllText("../../../Error/StackTrace.txt", ex.StackTrace);
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show(ex.Message, "Error");
                File.AppendAllText("../../../Error/StackTrace.txt", ex.StackTrace);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Не указан сдвиг", "Error");
                File.AppendAllText("../../../Error/StackTrace.txt", ex.StackTrace);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show("Исходный алфавит не содержит символов шифруемого текста", "Error");
                File.AppendAllText("../../../Error/StackTrace.txt", ex.StackTrace);
            }
        }

        #endregion
    }
}