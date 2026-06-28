using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Windows.Forms.DataVisualization.Charting;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;  

namespace Laborator2_TestareBatch
{
    public partial class Form1 : Form
    {

        SqlConnection connection = new SqlConnection("Data Source=MARIA\\SQLEXPRESS; Initial Catalog=Teatru2;Integrated Security=True");

        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        BindingSource bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void afiseazaGrafic(double autoCommit, double commitLots, double tranzactieUnica)
        {
            panelChart.Controls.Clear(); // sterge DOAR graficul vechi

            //se creaza graficul manual -> in versiuni mai vechi poate aparea si direct in toolbox
            Chart chart = new Chart();
            chart.Dock = DockStyle.Fill;

            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.Title = "Metoda";
            chartArea.AxisY.Title = "Timp (ms)";
            chart.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.ChartType = SeriesChartType.Column;

            series.Points.AddXY("Insert simplu", autoCommit);
            series.Points.AddXY("Commit 100", commitLots);
            series.Points.AddXY("BulkCopy", tranzactieUnica);

            chart.Series.Add(series);

            panelChart.Controls.Add(chart);
        }



        //Abordarea AUTO-COMMIT -> o tranzactie separata pentru fiecare inserare
        private void autoCommit()
        {
            connection.Open();

            SqlCommand comanda = new SqlCommand("INSERT INTO AngajatiBenchmark(Nume, Prenume) VALUES (@nume, @prenume);",connection);

            comanda.Parameters.Add("@nume", SqlDbType.VarChar);
            comanda.Parameters.Add("@prenume", SqlDbType.VarChar);

            for (int i = 0; i < 5000; i++)
            {
                try
                {
                    comanda.Parameters["@nume"].Value = "Nume " + i;
                    comanda.Parameters["@prenume"].Value = "Prenume " + i;

                    //se executa automat salvarea pe disc la fiecare operatie
                    comanda.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    connection.Close();
                    throw;
                }

            }
            connection.Close();
        }



        //Abordarea Commit-urilor in loturi
        private void commitLots()
        {
            connection.Open();

            SqlTransaction tran1 = connection.BeginTransaction();
            try
            {
                SqlCommand comanda = new SqlCommand("INSERT INTO AngajatiBenchmark(Nume, Prenume) VALUES (@nume, @prenume);", connection, tran1);

                comanda.Parameters.Add("@nume", SqlDbType.VarChar);
                comanda.Parameters.Add("@prenume", SqlDbType.VarChar);

                for (int i = 0; i < 5000; i++)
                {
                    comanda.Parameters["@nume"].Value = "Nume " + i;
                    comanda.Parameters["@prenume"].Value = "Prenume " + i;

                    comanda.ExecuteNonQuery();

                    //salvarea se realizeaza abia la finalizarea unei tranzactii
                    //o tranzactie este incheiata dupa inserarea a 100 de inregistrari

                    //ulterior, se creaza o noua tranzactie
                    if((i+1)%100 == 0)
                    {
                        tran1.Commit();
                        tran1 = connection.BeginTransaction();
                        comanda.Transaction = tran1;
                    }

                }

                //se realizeaza commit-ul si pentru ultima tranzactie deschisa
                tran1.Commit();
                connection.Close();
            }
            catch(Exception ex) { 
                //se va face rollback in caz de eroare
                tran1.Rollback();
                connection.Close();
                throw;
            }
        }


        //Abordarea cu tranzactie unica
        public void tranzactieUnica()
        {
            connection.Open();

            using SqlTransaction tran1 = connection.BeginTransaction();
            try
            {
                SqlCommand comanda = new SqlCommand("INSERT INTO AngajatiBenchmark(Nume, Prenume) VALUES (@nume, @prenume);", connection, tran1);

                comanda.Parameters.Add("@nume", SqlDbType.VarChar);
                comanda.Parameters.Add("@prenume", SqlDbType.VarChar);

                for (int i = 0; i < 5000; i++)
                {
                    comanda.Parameters["@nume"].Value = "Nume " + i;
                    comanda.Parameters["@prenume"].Value = "Prenume " + i;

                    comanda.ExecuteNonQuery();
                }

                //se realizeaza commit-ul pentru intreaga tranzactie
                tran1.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                //se va face rollback in caz de eroare
                tran1.Rollback();
                connection.Close();
                throw;
            }
        }


        //masoara timpii de executie in milisecunde
        private long timpExecutie(Action metoda)
        {
            var sw = Stopwatch.StartNew();
            metoda();
            sw.Stop();

            //partea de resetare a tabelei dupa test
            SqlCommand comanda = new SqlCommand("TRUNCATE TABLE AngajatiBenchmark;", connection);

            connection.Open();
            comanda.ExecuteNonQuery();
            connection.Close();

            return sw.ElapsedMilliseconds;
        }

        //masoara media timpilor de executie
        private double timpExecutieMediu(Action metoda)
        {
            long medie = 0;
            for (int i = 0; i < 3; i++)
                medie += timpExecutie(metoda);

            return medie / 3;
        }

        private void afiseazaTabel(string metoda, long r1, long r2, long r3,double medie)
        {
            // cazul in care nu este configurat tabelul
            if (dataGridView1.ColumnCount == 0)
            {
                dataGridView1.Columns.Add("Metoda", "Metoda Inserare");
                dataGridView1.Columns.Add("Rularea1", "Rularea 1 (ms)");
                dataGridView1.Columns.Add("Rularea2", "Rularea 2 (ms)");
                dataGridView1.Columns.Add("Rularea3", "Rularea 3 (ms)");
                dataGridView1.Columns.Add("Media", "Medie (ms)");
            }

            // se adauga un rand nou
            dataGridView1.Rows.Add(metoda, r1, r2, r3,medie);
        }

  
        private List<long> masoaraToateRularile(Action metoda)
        {
            List<long> timpi = new List<long>();
            for (int i = 0; i < 3; i++)
            {
                timpi.Add(timpExecutie(metoda));
            }
            return timpi;
        }

        private void butonRuleaza_Click(object sender,EventArgs arg)
        {
            //double t1 = timpExecutieMediu(autoCommit);
            //double t2 = timpExecutieMediu(commitLots);
            //double t3 = timpExecutieMediu(tranzactieUnica);
     
            dataGridView1.Rows.Clear(); // se curata tabelul pentru o rulare noua

            // auto-commit
            var timpiT1 = timpExecutie(autoCommit);
            var timpiT2 = timpExecutie(autoCommit);
            var timpiT3 = timpExecutie(autoCommit);
            double medieT1 = (timpiT1 + timpiT2 + timpiT3) / 3;
            afiseazaTabel("Insert Simplu (Auto-Commit)", timpiT1, timpiT2, timpiT3, medieT1);

            // commitLots
            timpiT1 = timpExecutie(commitLots);
            timpiT2 = timpExecutie(commitLots);
            timpiT3 = timpExecutie(commitLots);
            double medieT2 = (timpiT1 + timpiT2 + timpiT3) / 3;
            afiseazaTabel("Commit in Loturi (100)", timpiT1, timpiT2, timpiT3, medieT2);

            // tranzactieUnica
            timpiT1 = timpExecutie(tranzactieUnica);
            timpiT2 = timpExecutie(tranzactieUnica);
            timpiT3 = timpExecutie(tranzactieUnica);
            double medieT3 = (timpiT1 + timpiT2 + timpiT3) / 3;
            afiseazaTabel("Tranzactie Unica", timpiT1, timpiT2, timpiT3, medieT3);

            // se actualizeaza graficul
            afiseazaGrafic(medieT1, medieT2, medieT3);

            MessageBox.Show("S-au terminat masuratorile!");

            afiseazaGrafic(medieT1, medieT2, medieT3);
        }

    }
}
