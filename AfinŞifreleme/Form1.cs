using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AfinŞifreleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ArrayList sozluk = new ArrayList();
        ArrayList alfabe = new ArrayList()
{"A","B", "C", "Ç", "D","E", "F", "G","Ğ", "H","I", "İ", "J","K","L","M", "N", "O", "Ö", "P", "R", "S", "Ş", "T", "U", "Ü","V", "Y","Z",
            "a","b", "c", "ç", "d","e","2", "f", "g","1","ğ", "h","ı","3", "i","0", "j","k","l","m", "n", "o", "ö", "p", "r", "s", "ş", "t", "u", "ü","v", "y","z"
            ,"4","5","6","7","8","9"};
        ArrayList sayilar = new ArrayList()
        {"0","1","2","3","4","5","6","7","8","9"};
        String text = "";
        int a = 0, b = 0;
        List<int> Alar = new List<int>();
        List<int[]> ab = new List<int[]>();
        ArrayList temp = new ArrayList();
        List<int[]> SozlukIndex = new List<int[]>();

        List<int> Puanlar = new List<int>();
        List<int> Maxlar = new List<int>();
        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            FileStream akis;

            string Yol =file.FileName;
            akis = new FileStream(Yol, FileMode.Open, FileAccess.Read);
            StreamReader Okuma = new StreamReader(akis, Encoding.GetEncoding("iso-8859-9"), false);


            var satirlar = Okuma.ReadToEnd().Split('\n');
            foreach (var satır in satirlar)
            {



                if (!satır.Contains(" ") && alfabe.Contains(satır.Substring(0, 1).ToUpper()) && !satır.Contains("â") && !satır.Contains("'") && !satır.Contains("î"))
                {
                    sozluk.Add(satır.Replace("\r", ""));
                }

            }

            for (int i = 0; i < 29; i++)
            {

                SozlukIndex.Add(new int[2]);


            }
            SozlukIndex[0][0] = 0;
            SozlukIndex[28][1] = sozluk.Count - 1;
            string karakter = "a";
            for (int i = 0; i < sozluk.Count; i++)
            {
                if (karakter.ToLower() != sozluk[i].ToString().ToLower().Substring(0, 1))
                {
                    SozlukIndex[alfabe.IndexOf(karakter.ToUpper())][1] = i - 1;
                    if (alfabe.IndexOf(karakter.ToUpper()) + 1 < 29)
                    {
                        SozlukIndex[alfabe.IndexOf(karakter.ToUpper()) + 1][0] = i;

                    }
                    karakter = sozluk[i].ToString().Substring(0, 1);
                }


            }
            AdeğerleriniUret();
        }
        string Sifrele(string kelime, int a, int b)
        {
            string yeniText = "";

            char[] dizi = kelime.ToCharArray();

            for (int i = 0; i < dizi.Length; i++)
            {
                if (alfabe.Contains(dizi[i].ToString()))
                {
                    int index = alfabe.IndexOf(dizi[i].ToString());
                    int ip = ((a * index) + b) % alfabe.Count;

                    yeniText += alfabe[ip].ToString();
                }
                else
                    yeniText += dizi[i].ToString();



            }
            return yeniText;
        }
        String Desifrele(string kelime, int a, int b)
        {
            int y = 0;
            string ex = "";
            char[] dizi = kelime.ToCharArray();
            int tersno = 0;
            y = alfabe.Count;
            while (true)
            {
                if ((y + 1) % a == 0)
                {
                    tersno = (y + 1) / a;
                    y = 0;

                    break;
                }
                else
                {
                    y += alfabe.Count;
                }

            }



            for (int i = 0; i < dizi.Length; i++)
            {
                if (alfabe.Contains(dizi[i].ToString()))
                {
                    int ic = alfabe.IndexOf(dizi[i].ToString());
                    y = (tersno * (ic - b));
                    while (y < 0)
                    {
                        y += alfabe.Count;
                    }
                    y %= alfabe.Count;

                    ex += alfabe[y];

                }
                else
                    ex += dizi[i].ToString();

            }
            return ex;
        }
        private void btnBasla_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            richTextBox3.Clear();
            try
            {
                a = Convert.ToInt32(textBox1.Text);
                if (!Alar.Contains(a))
                {
                    MessageBox.Show("a değeri Alfabedeki harf sayisi ile aralarında asal olmalıdır ve  küçük olmalıdır", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                b = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("A ve B değerlerini kontrol ediniz", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            Puanlar.Clear();
            temp.Clear();
            ab.Clear();
            text = richTextBox1.Text;
            Maxlar.Clear();
            temp.AddRange(text.Split(' '));
            temp.Remove("");
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].ToString() != "")
                {
                    richTextBox2.Text += Sifrele(temp[i].ToString(), a, b) + " ";
                }
            }

            temp.Clear();
            temp.AddRange(richTextBox2.Text.Split(' '));
            temp.Remove("");

            AdeğerleriniUret();
            for (int i = 0; i < temp.Count; i++)
            {
                Maxlar.Add(0);
            }

            for (int i = 0; i < Alar.Count; i++)
            {

                for (int j = 0; j < alfabe.Count; j++)
                {
                    int ensoneklenen = 0;
                    int skor = 0;
                    Puanlar.Add(0);
                    ab.Add(new int[2] { Alar[i], j });
                    for (int k = 0; k < temp.Count; k++)
                    {
                        skor += temp[k].ToString().Length;
                        string kelime = Desifrele(temp[k].ToString(), Alar[i], j);

                        string ilkHarf = kelime.Substring(0, 1);
                        bool a = true;
                        for (int c = 0; c < kelime.Length; c++)
                        {
                            if (sayilar.Contains(kelime[c].ToString()))
                            {
                                a = false;
                            }
                        }
                        if (k>=3&&Puanlar[Puanlar.Count-1]<skor/2)
                        {
                            break;
                        }
                        int dogruSayisi = 0;
                       if(alfabe.Contains(ilkHarf))
                        if (!sayilar.Contains( ilkHarf)&&a)
                        {

                            if (Maxlar[k] != -1)
                                for (int l = SozlukIndex[alfabe.IndexOf(ilkHarf.ToUpper())][0]; l <= SozlukIndex[alfabe.IndexOf(ilkHarf.ToUpper())][1]; l++)
                                {
                                    if (sozluk[l].ToString().ToLower() == kelime.ToLower())
                                    {
                                        Puanlar[Puanlar.Count - 1] += kelime.Length;
                                        Maxlar[k] = -1;
                                            dogruSayisi++;
                                            if (dogruSayisi>2)
                                            {
                                                EkranaYaz();
                                                return;
                                            }
                                        break;
                                    }
                                    else
                                    {
                                        for (int m = 1; m < sozluk[l].ToString().Length && m < kelime.ToString().Length; m++)
                                        {

                                            if (Maxlar[k] != 0 && (Maxlar[k] < sozluk[l].ToString().Length && Maxlar[k] < kelime.ToString().Length) && m == 0)
                                                if (sozluk[l].ToString().Substring(0, Maxlar[k]).ToLower() != kelime.Substring(0, Maxlar[k]).ToLower())
                                                    break;
                                                else
                                         {
                                                    m = Maxlar[k];
                                                }

                                            if (sozluk[l].ToString().ToLower()[m] != kelime.ToLower()[m])
                                            {
                                                Maxlar[k] = m;
                                                Puanlar[Puanlar.Count - 1] -= ensoneklenen;
                                                Puanlar[Puanlar.Count - 1] += m;
                                                    if (m>=kelime.Length/2)
                                                    {
                                                        if (dogruSayisi > 2)
                                                        {
                                                            dogruSayisi++;

                                                            EkranaYaz();
                                                            return;
                                                        }
                                                    }
                                                ensoneklenen = m;
                                                break;
                                            }


                                        }



                                    }

                                }
                        }

                    }
                }
            }


            EkranaYaz();
         

        }
        void EkranaYaz()
        {
            int max = 0;
            for (int i = 0; i < Puanlar.Count; i++)
            {
                if (Puanlar[max] <= Puanlar[i])
                {
                    max = i;
                }
            }


            for (int i = 0; i < temp.Count; i++)
            {
                richTextBox3.Text += Desifrele(temp[i].ToString(), ab[max][0], ab[max][1]) + " ";
            }
           label1.Text = "\na=" + ab[max][0].ToString() + " b=" + ab[max][1];

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        void AdeğerleriniUret()
        {
            Alar.Clear();
            Alar.Add(1);
            for (int i = 1; i < alfabe.Count + 1; i++)
            {
                for (int j = 2; j < i + 1; j++)
                {
                    if (alfabe.Count % i == 0)
                        break;
                    if (i % j == 0 && alfabe.Count % j == 0)
                    {
                        break;
                    }
                    else
                    {
                        if (j == i)
                            Alar.Add(i);

                    }
                }

            }
        }

    }
}
