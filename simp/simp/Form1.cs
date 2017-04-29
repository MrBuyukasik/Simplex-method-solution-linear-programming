using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace simp
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

       
        private void metroTile1_Click(object sender, EventArgs e)
        {

            //kontroller
            
            if (string.IsNullOrEmpty(kisit.Text) && string.IsNullOrEmpty(degisken.Text))
            {
                MetroMessageBox.Show(this, "Kısıt veya Değişken Değeri Giriniz", "Uyarı");
            }
            else
            {
                try
                {
                    int kis = Convert.ToInt32(kisit.Text);
                    int deg = Convert.ToInt32(degisken.Text);

                    metroGrid1.RowHeadersWidth = 60;


                    for (int i = 0; i < deg+kis;i++)
                    {
                        if (i < deg)
                            metroGrid1.Columns.Add("x" + (i + 1).ToString(), "x" + (i + 1).ToString());
                        else
                        {
                            metroGrid1.Columns.Add("s" + (i + 1 - deg).ToString(), "s" + (i + 1 - deg).ToString());
                        }

                        metroGrid1.Columns[i].Width = 60;
                    }
                    
                    metroGrid1.Columns.Add("Koşul", "Koşul");
                    metroGrid1.Columns["Koşul"].Width=60;
                    metroGrid1.Columns.Add("STS","STS"); 
                    metroGrid1.Columns["STS"].Width=60;
                    
                    for (int i = 0; i < deg+2; i++)
                    {
                        metroGrid1.Columns[i].Width = 60;

                    }
                    metroGrid1.Rows.Add("");
                    metroGrid1.Rows[0].HeaderCell.Value = "Z";
                    metroGrid1.Rows[0].Cells["Koşul"].Value = "=";
                    
                    for (int i = 1; i <= kis;i++ )
                    {
                        metroGrid1.Rows.Add("");
                        metroGrid1.TopLeftHeaderCell.Value = "Kısıtlar";
                        metroGrid1.Rows[i].HeaderCell.Value = "K " + i.ToString();
                        metroGrid1.Rows[i].Cells["Koşul"].Value = "<=";

                    }

                }
                catch
                {
                    MetroMessageBox.Show(this, "Değerleri Kontrol Ediniz", "Uyarı");
                }
            }
            //Kontrollerbitiş

        

        }
        bool ilk = true;
        Simpleks[] satirlar;
        Simpleks z;
        private void uygula_Click(object sender, EventArgs e)
        {
                if (!maxradio.Checked && !minradio.Checked)
                {
                    MetroMessageBox.Show(this, "max/min seçimi yapınız", "Uyarı");
                }
                else
                {
                    try
                    {
                        for (int i = 0; i < metroGrid1.Rows.Count - 1; i++)
                        {
                            for (int j=0; j<metroGrid1.Columns.Count; j++)
                            {
                                if (metroGrid1.Rows[i].Cells[j] != metroGrid1.Rows[i].Cells["Koşul"])
                                {
                                    Convert.ToDouble(metroGrid1.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                        
                        if (ilk)
                        {
                            satirlar = new Simpleks[Convert.ToInt32(kisit.Text)];
                            Simpleks.set_Kosul(metroGrid1.Columns["Koşul"].Index);
                            for (int i = 0; i < satirlar.Length; i++)
                            {
                                satirlar[i] = new Simpleks(metroGrid1.Columns.Count, metroGrid1.Rows[i + 1], minradio.Checked);
                            }
                            z = new Simpleks(metroGrid1.Columns.Count, metroGrid1.Rows[0], minradio.Checked);
                            ilk = false;
                        }
                        if(minradio.Checked)
                            if(Simpleks.min_kontrol(z))
                            {
                                Simpleks.iterasyon(satirlar, z);
                                metroGrid1.Rows[Simpleks.get_Cikan() + 1].HeaderCell.Value = metroGrid1.Columns[Simpleks.get_Giren()].HeaderCell.Value;

                            }
                        if(!minradio.Checked)
                            if(Simpleks.max_kontrol(z))
                            {
                                Simpleks.iterasyon(satirlar, z);
                                metroGrid1.Rows[Simpleks.get_Cikan() + 1].HeaderCell.Value = metroGrid1.Columns[Simpleks.get_Giren()].HeaderCell.Value;

                            }

                        
                        for (int j = 0; j < metroGrid1.Columns.Count;j++ )
                        {
                            if (metroGrid1.Rows[0].Cells["Koşul"] != metroGrid1.Rows[0].Cells[j])
                                metroGrid1.Rows[0].Cells[j].Value = z.satir[j].ToString();
                            else metroGrid1.Rows[0].Cells["Koşul"].Value = "<=";
                        }
                            for (int i = 1; i < satirlar.Length + 1; i++)
                                for (int j = 0; j < metroGrid1.Columns.Count; j++)
                                {

                                    if (metroGrid1.Rows[i].Cells["Koşul"] != metroGrid1.Rows[i].Cells[j])
                                        metroGrid1.Rows[i].Cells[j].Value = satirlar[i - 1].satir[j].ToString();
                                   

                                }
                    }
                    catch
                    {
                        MetroMessageBox.Show(this, "Girilen Değerleri Kontrol Ediniz", "Uyarı");
                    }
                } 
           
             
        }
        
        class Simpleks
        {
            public double[] satir; //new int[];
            static int giren;
            static int cikan;
            static int kosul = -1;
            static bool minimizasyon;
            public static void set_Kosul(int n)
            {
                kosul=n;
            }
            public Simpleks(int sutun_say,DataGridViewRow row,bool m)
            {
                satir = new double[sutun_say];
                minimizasyon = m;
                for (int i = 0; i < sutun_say; i++)
                {
                    if(kosul!=i)
                        satir[i] = Convert.ToDouble(row.Cells[i].Value);
                    
                }

            }
            public static int get_Giren()
            {
                return giren;
            }
            public static int get_Cikan()
            {
                return cikan;
            }

            static int max_bul(Simpleks z) //maximize problem
            {
                double m = double.MaxValue;
                int indis = -1;
                for (int i=0; i<z.satir.Length;i++)
                {
                    if(kosul!=i)
                    if(z.satir[i] < m) 
                    {
                        m = z.satir[i];
                        indis = i;
                    }
                }

                return indis;
            }

            static int min_bul(Simpleks z) // minimize problem
            {
                double n = double.MinValue;
                int indis = -1;

                for (int i=0; i<z.satir.Length;i++)
                {
                    if(kosul!=i)
                    if (z.satir[i] > n)
                    {
                        n = z.satir[i];
                        indis = i;
                    }
                }
                return indis;
            }

            public static bool max_kontrol(Simpleks z)//maximize_kontrol
            {
                 for (int i=0; i<z.satir.Length;i++)
                {
                        if (z.satir[i] < 0)
                            return true;
                }
                return false;
            }

            public static bool min_kontrol(Simpleks z)
            {
             for (int i=0; i<z.satir.Length;i++)
                {
                    if (z.satir[i] > 0)
                        return true;
                }
                return false;
            }//minimize_kontrol iterasyon devam kontrol

            static int cikan_bul(Simpleks[] satirlar,Simpleks z)
            {
                double min = double.MaxValue;
                int indis = -1;
                if (minimizasyon)
                    giren=Simpleks.min_bul(z);
                else
                    giren=Simpleks.max_bul(z);

                    for (int i = 0; i < satirlar.Length; i++)
                    {
                        if (satirlar[i].satir[giren] > 0)
                            if (satirlar[i].satir[satirlar[i].satir.Length - 1] / satirlar[i].satir[giren] > 0 && satirlar[i].satir[satirlar[i].satir.Length - 1] / satirlar[i].satir[giren] < min)
                            {
                                min = satirlar[i].satir[satirlar[i].satir.Length - 1] / satirlar[i].satir[giren];
                                indis = i;
                            }
                    }
   
                return indis;
            }

            public static void iterasyon(Simpleks[] satirlar, Simpleks z)
            {
                cikan = cikan_bul(satirlar, z);
                double anahtar_eleman = satirlar[cikan].satir[giren];
                double carpan;
                if (z.satir[giren] != 0)
                {
                    carpan = z.satir[giren] / satirlar[cikan].satir[giren];//anahtar eleman

                    for (int j = 0; j < satirlar[0].satir.Length; j++)
                       z.satir[j] -= carpan * satirlar[cikan].satir[j];
                }
                for (int i = 0; i < satirlar.Length; i++)
                {
                    
                    if(i!=cikan&&satirlar[i].satir[giren]!=0)
                    {
                     carpan=satirlar[i].satir[giren]/satirlar[cikan].satir[giren];

                    for (int j = 0; j < satirlar[0].satir.Length; j++)
                        if(kosul!=j)
                        satirlar[i].satir[j] -= carpan*satirlar[cikan].satir[j];
                        
                    }
                }
                
                for (int i = 0; i < satirlar[cikan].satir.Length; i++)
                    if(kosul!=i)
                        satirlar[cikan].satir[i] /=  anahtar_eleman;
            

            }

        }


        private void temizle_Click(object sender, EventArgs e)
        {
            ilk = true;
            minradio.Checked = false;
            maxradio.Checked = false;
            metroGrid1.Rows.Clear();
            metroGrid1.Columns.Clear();
            kisit.Text = "";
            degisken.Text = "";
        }
    }
}
