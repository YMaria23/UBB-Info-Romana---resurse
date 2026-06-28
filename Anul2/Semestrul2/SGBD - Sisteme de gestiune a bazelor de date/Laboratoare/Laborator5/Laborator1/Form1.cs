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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;


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
                    // aduce toate teatrele din baza de date sub forma de lista
                    var listaTeatre = context.TeatruNational.ToList();
                    dataGridView1.DataSource = listaTeatre;


                    // aduce toate spectacolele sterse (marcate ca fiind sterse) intr-o lista
                    var listaSpectacoleSterse = context.Spectacole
                        .Where(x => x.IsDeleted)
                        .ToList();
                    dataGridViewDeleted.DataSource = listaSpectacoleSterse;

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
                            .Where(s => s.IsDeleted == false)
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
                        IdT = int.Parse(textIdTeatruSpectacol.Text),
                        DeletedBy = "-"
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
                string errorMessage = "Eroare principală: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\n\nEroare internă (MOTIVUL REAL):\n" + ex.InnerException.Message;

                    // Uneori mai există încă o eroare și mai adâncă
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += "\n\nDetalii SQL:\n" + ex.InnerException.InnerException.Message;
                    }
                }
                MessageBox.Show(errorMessage);
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
                                spectacol.IsDeleted = true;
                                spectacol.DeletedAt = DateTime.Now;
                                spectacol.DeletedBy = "admin";

                                db.SaveChanges();
                                MessageBox.Show("Deleted!");

                                refreshSpectacole(sender, e);
                                refreshSpectacoleDeleted(sender, e);
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

        private void refreshSpectacoleDeleted(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext()) {
                var spectacole = context.Spectacole
                    .Where(s => s.IsDeleted)
                    .ToList();

                dataGridViewDeleted.DataSource = spectacole;
            } 
        }

        private void testConnectionWithoutPool(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
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




        //////////////////////// LABORATORUL 5 //////////////////////////////////


        // scenariu tranzactii pentru implementarea locking-ului optimist
        private void tranzactieA()
        {
            using (var context = new TeatruDbContext())
            {
                // se incarca spectacolul cu id-ul 1
                var spectacol = context.Spectacole.Find(1);

                Thread.Sleep(3000);

                // se actualizeaza spectacolul
                spectacol.Descriere = "romantic";
                context.SaveChanges();
            }
        }

        private void tranzactieB()
        {
            using (var context = new TeatruDbContext())
            {
                try
                {
                    Thread.Sleep(3000);

                    // se incarca spectacolul cu id-ul 1
                    var spectacol = context.Spectacole.Find(1);

                    Thread.Sleep(5000);

                    // se actualizeaza spectacolul
                    spectacol.Descriere = "descriere";
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var decizie = MessageBox.Show("Salvarea datelor nu a putut fi modificata! Un alt utilizator a modificat" +
                        " aceeasi inregistrare in acelasi timp cu dvs. Uitati ce puteti face mai departe:\n" +
                        "YES -> Reincarcarea datelor (modificarile dvs se anuleaza)\n" +
                        "NO -> Se forteaza actualizarea datelor\n" +
                        "Cancel -> Se renunta complet la tranzactie", "Conflict de date", MessageBoxButtons.YesNoCancel);

                    var entry = ex.Entries.Single();

                    if (decizie == DialogResult.Yes)
                    {
                        // inseamna ca trebuie reincarcate datele
                        entry.Reload();
                        var dbValues = entry.GetDatabaseValues();

                        if (dbValues != null)
                        {
                            // construieste un string din mers care uneste numele coloanei cu valoarea
                            string rezultat = string.Join(Environment.NewLine, dbValues.Properties
                                .Select(p => $"{p.Name}: {dbValues[p.Name]}"));

                            MessageBox.Show("Valoarea actuala este:" + rezultat);
                        }
                    }
                    else if (decizie == DialogResult.No)
                    {
                        var databaseValues = entry.GetDatabaseValues();
                        if (databaseValues == null)
                        {
                            MessageBox.Show("Eroare: Spectacolul a fost sters.");
                            return;
                        }

                        // setam valorile initiale ca fiind cele actuale (pentru a parea ca am citit valorile noi actuale)
                        // acum putem sa le modificam
                        entry.OriginalValues.SetValues(databaseValues);
                        context.SaveChanges();

                        MessageBox.Show("S-a actualizat cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Ati renuntat la tranzactie!");
                    }
                }
            }
        }

        public async void buttonOptimistLocking_Click(object sender, EventArgs arg)
        {
            try
            {
                var t1 = Task.Run(() => tranzactieA());
                var t2 = Task.Run(() => tranzactieB());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        // soft delete -> functii de restaurare si stergere definitiva
        private void butonDeleteDefinitiv_Click(object sender, EventArgs e)
        {
            try
            {
                //dialogul de confirmare
                DialogResult dr = MessageBox.Show("Sigur vreti sa stergeti definitiv aceasta inregistrare?", "Se confirma stergerea!", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    if (dataGridViewDeleted.CurrentRow != null &&
                   dataGridViewDeleted.CurrentRow.DataBoundItem is Spectacol spectacolI)
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

                                refreshSpectacoleDeleted(sender, e);
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

        private void butonRestaurare_Click(object sender, EventArgs e)
        {
            try
            {
                //dialogul de confirmare
                DialogResult dr = MessageBox.Show("Sigur vreti sa resaurati aceasta inregistrare?", "Se confirma restaurarea!", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    if (dataGridViewDeleted.CurrentRow != null &&
                   dataGridViewDeleted.CurrentRow.DataBoundItem is Spectacol spectacolI)
                    {
                        int id = spectacolI.IdS;

                        using (var db = new TeatruDbContext())
                        {
                            var spectacol = db.Spectacole.Find(id);
                            if (spectacol != null)
                            {
                                spectacol.IsDeleted = false;
                                spectacol.DeletedBy = "";
                                spectacol.DeletedAt = null;

                                db.SaveChanges();
                                MessageBox.Show("Restored!");

                                refreshSpectacole(sender, e);
                                refreshSpectacoleDeleted(sender, e);
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


    //}


    /// practic
    //tranzactii

    ///PHANTOM READS
        private void Tranzactia1PR()
        {

            //aici incepe efectiv tranzactia
            using (var context = new TeatruDbContext())
            {
                //con.Open();

                //using (SqlTransaction tran1 = con.BeginTransaction(IsolationLevel.RepeatableRead))
                //{

                    try
                    {

                    /*
                        //se realizeaza primul select
                        SqlCommand comanda1 = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran1);

                        //ExecuteScalar -> returneaza prima coloana din primul rand (aka primul element)
                        int nr = (int)comanda1.ExecuteScalar();
                        MessageBox.Show("SELECT1: \n" + "S-a determinat un nr de " + nr + " teatre.");
                    */

                        // se realizeaza un select
                        var manageri = context.Managers.ToList();
                        MessageBox.Show("SELECT1: \n" + "S-a terminat cu un nr de "+ manageri.Count+ " inregistrari");

                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);


                    /*
                        //se realizeaza al doilea select -> dupa ce tranzactia 2 a modificat inregistrarea
                        SqlCommand comanda2 = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran1);

                        nr = (int)comanda2.ExecuteScalar();
                        MessageBox.Show("SELECT2: \n" + "S-a determinat un nr de " + nr + " teatre.");
                    */

                        var manageri2 = context.Managers.ToList();
                        MessageBox.Show("SELECT2: \n" + "S-a terminat cu un nr de " + manageri2.Count + " inregistrari");


                        //tran1.Commit();
                    }
                    catch (Exception ex)
                    {
                        //tran1.Rollback();
                        MessageBox.Show(ex.Message);
                    //}
                }
            }
        }

        private void Tranzactia2PR()
        {

            //aici incepe efectiv tranzactia
            using (var context = new TeatruDbContext())
            {
                //con.Open();
                //using (SqlTransaction tran2 = con.BeginTransaction())
                //{

                    try
                    {
                    /*
                        da.SelectCommand = new SqlCommand("INSERT INTO TeatruNational(Denumire,Oras,AnInfiintare) values ('Teatrul National XYZ','Craiova',1800)", con, tran2);

                        da.SelectCommand.ExecuteNonQuery();
                        MessageBox.Show("S-a adaugat teatrul din Craiova");

                        tran2.Commit();
                    */
                    //se insereaza un manager nou
                        var manager = new Manager
                        {
                            Nume = "Popescu",
                            Prenume = "Vasile",
                            IdT = 1
                        }
                    ;
                        context.Managers.Add(manager);
                        context.SaveChanges();
                }
                    catch (Exception ex)
                    {
                        //tran2.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            //}
        }

        //simularea rularii tranzactiilor

        private async void butonPhantomReads_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1PR());
                var t2 = Task.Run(() => Tranzactia2PR());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // DIRTY READS

        private void Tranzactia1DR()
        {

            //aici incepe efectiv tranzactia
            using (var context = new TeatruDbContext())
            {
                //con.Open();
                //using (SqlTransaction tran1 = con.BeginTransaction())
                //{

                    try
                    {
                    /*
                        da.SelectCommand = new SqlCommand("INSERT INTO TeatruNational(Denumire,Oras,AnInfiintare) VALUES ('Teatrul National Craiova','Craiova',1900);", con, tran1);

                        da.SelectCommand.ExecuteNonQuery();
                        MessageBox.Show("S-a adaugat Teatrul National Craiova!");

                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);

                        //vom da rollback la tranzactie
                        tran1.Rollback();
                        MessageBox.Show("S-a efectuat rollback la tranzactia 1.");
                    */

                    // se insereaza un nou manager
                    var manager = new Manager
                    {
                        Nume = "Popescu",
                        Prenume = "Vasile",
                        IdT = 2
                    };

                    context.Managers.Add(manager);
                    context.SaveChanges();

                    Thread.Sleep(5000);

                    throw new Exception("Se face rollback");
                    MessageBox.Show("S-a efectuat rollback la tranzactia 1.");
                }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        private void Tranzactia2DR()
        {

            //aici incepe efectiv tranzactia
            using (var context = new TeatruDbContext())
            {
                //con.Open();
                //using (SqlTransaction tran2 = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                //{
                    try
                    {
                    /*
                        SqlCommand comanda = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran2);

                        //ExecuteScalar -> returneaza prima coloana din primul rand (aka primul element)
                        int nr = (int)comanda.ExecuteScalar();

                        tran2.Commit();
                        MessageBox.Show("Nr de teatre existente este: " + nr);
                    */
                    var managers = context.Managers.ToList();
                    MessageBox.Show("Nr de manageri existenti este: " + managers.Count);

                }
                    catch (Exception ex)
                    {
                        //tran2.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        private async void butonDirtyReads_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1DR());
                var t2 = Task.Run(() => Tranzactia2DR());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }

}
