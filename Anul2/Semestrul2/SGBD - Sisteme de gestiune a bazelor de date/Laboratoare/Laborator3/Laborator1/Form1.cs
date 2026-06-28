using Microsoft.EntityFrameworkCore;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Laborator1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void validare()
        {
            //campul obligatoriu este denumirea -> intotdeauna va trebui sa aiba o valoare
            if (textDenumireSpectacol.Text == "")
                throw new Exception("Field-ul Denumire nu trebuie sa fie gol!");
        }

        private void validare_id()
        {
            String id = textIdTeatruSpectacol.Text;
            if (Convert.ToInt32(id) < 1)
                throw new Exception("Id-ul nu poate sa fie mai mic fata de 1!");
        }

        private void butonConectare_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeatruDbContext())
                {
                    // aduce toate teatrele din baza de date sub forma de listă
                    var listaTeatre = context.TeatruNational.ToList();
                    dataGridView1.DataSource = listaTeatre;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // se verifica selectarea randului si validitatea obiectului din spate
                if (dataGridView1.CurrentRow != null &&
                    dataGridView1.CurrentRow.DataBoundItem is TeatruNational teatru)
                {
                    int idSelectat = teatru.IdT;

                    // actualizeaza textBox-urile
                    textIdTeatru.Text = teatru.IdT.ToString();
                    textDenumireTeatru.Text = teatru.Denumire;
                    textOrasTeatru.Text = teatru.Oras;

                    using (var db = new TeatruDbContext())
                    {
                        var spectacole = db.Spectacole
                            .Where(p => p.IdT == idSelectat)
                            .ToList();

                        dataGridSpectacole.DataSource = spectacole;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dataGridSpectacole_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // se verifica selectarea randului si validitatea obiectului din spate
                if (dataGridSpectacole.CurrentRow != null &&
                    dataGridSpectacole.CurrentRow.DataBoundItem is Spectacol spectacol)
                {

                    // actualizeaza textBox-urile
                    textDenumireSpectacol.Text = spectacol.Denumire;
                    textDescriereSpectacol.Text = spectacol?.Descriere;
                    textIdTeatruSpectacol.Text = spectacol.IdT.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butonInsert_Click(object sender, EventArgs e)
        {
            try
            {
                validare();
                validare_id();

                using (var context = new TeatruDbContext())
                {
                    var spectacol = new Spectacol
                    {
                        Denumire = textDenumireSpectacol.Text,
                        Descriere = textDescriereSpectacol.Text,
                        IdT = int.Parse(textIdTeatruSpectacol.Text)
                    }
                    ;
                    context.Spectacole.Add(spectacol);
                    context.SaveChanges();

                    MessageBox.Show("Inserted successfully!");
                    refreshSpectacole(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                validare();
                validare_id();

                if (dataGridSpectacole.CurrentRow != null &&
                   dataGridSpectacole.CurrentRow.DataBoundItem is Spectacol spectacolI)
                {
                    int id = spectacolI.IdS;

                    using (var db = new TeatruDbContext())
                    {
                        var spectacol = db.Spectacole.Find(id);
                        if (spectacol != null)
                        {
                            spectacol.Denumire = textDenumireSpectacol.Text;
                            spectacol.Descriere = textDescriereSpectacol.Text;
                            spectacol.IdT = int.Parse(textIdTeatruSpectacol.Text);

                            db.SaveChanges();
                            MessageBox.Show("Updated!");

                            refreshSpectacole(sender, e);
                        }
                    }
                }
                else
                {
                    throw new Exception("There wasn't any selection!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void butonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //dialogul de confirmare
                DialogResult dr = MessageBox.Show("Sigur vreti sa stergeti aceasta inregistrare?", "Se confirma stergerea!", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    if (dataGridSpectacole.CurrentRow != null &&
                   dataGridSpectacole.CurrentRow.DataBoundItem is Spectacol spectacolI)
                    {
                        int id = spectacolI.IdS;

                        using (var db = new TeatruDbContext())
                        {
                            var spectacol = db.Spectacole.Find(id);
                            if (spectacol != null)                                
                            {
                                db.Spectacole.Remove(spectacol);
                                db.SaveChanges();
                                MessageBox.Show("Deleted!");

                                refreshSpectacole(sender, e);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void butonCautare_Click(object sender, EventArgs e)
        {
            try
            {
                //ne asiguram ca exista o denumire dupa care vrem sa cautam in tabel
                var denumire = textDenumireSpectacolCautare.Text;
                if (denumire == "")
                    throw new Exception("Choose a name first!");

                using (var context = new TeatruDbContext())
                {
                    var listaSpectacole = context.Spectacole
                                            .Where(s => s.Denumire == denumire)
                                            .ToList();
                    dataGridSpectacole.DataSource = listaSpectacole;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }


        private void butonRefresh_Click(object sender, EventArgs e)
        {
            butonConectare_Click(sender, e);
        }

        private void refreshSpectacole(object sender, EventArgs e)
        {
            dataGridView1_SelectionChanged(sender, e);
        }

        private void testConnectionWithoutPool(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++) {
                using (var context = new TeatruDbContext(false))
                {
                    //se forteaza deschiderea conexiunii (nu se deschide altfel -> procesul este unul implicit lazy)
                    context.Database.OpenConnection();
                }
            }

            sw.Stop();
            var totalTime = sw.ElapsedMilliseconds;
            var averageTimePerConnection = totalTime / 100;
            MessageBox.Show("Time used - without pool: " + totalTime + " ms\n" + "Average Time per connection: " + averageTimePerConnection + " ms");
            Debug.WriteLine("Time used - without pool: " + totalTime + " ms\n" + "Average Time per connection: " + averageTimePerConnection + " ms");
        }

        private void testConnectionWithPool(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
                using (var context = new TeatruDbContext())
                {
                    //se forteaza deschiderea conexiunii (nu se deschide altfel -> procesul este unul implicit lazy)
                    context.Database.OpenConnection();
                }
            }

            sw.Stop();
            var totalTime = sw.ElapsedMilliseconds;
            var averageTimePerConnection = totalTime / 100;
            MessageBox.Show("Time used - with pool: " + sw.ElapsedMilliseconds.ToString() + " ms\n" + "Average Time per connection: " + averageTimePerConnection + " ms");
            Debug.WriteLine("Time used - with pool: " + sw.ElapsedMilliseconds.ToString() + " ms\n" + "Average Time per connection: " + averageTimePerConnection + " ms");
        }

        private void testWithPoolWrong(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                var context = new TeatruDbContext();
                //se forteaza deschiderea conexiunii (nu se deschide altfel -> procesul este unul implicit lazy)
                context.Database.OpenConnection();
                //nu se inchide conexiunea => pool-ul va fi epuizat
            }

            MessageBox.Show("Task Ended");
        }

    }
}
