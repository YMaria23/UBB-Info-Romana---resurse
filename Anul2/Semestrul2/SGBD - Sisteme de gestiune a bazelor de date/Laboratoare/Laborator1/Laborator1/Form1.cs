using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Laborator1
{
    public partial class Form1 : Form
    {

        string connectionString = "Data Source=MARIA\\SQLEXPRESS; Initial Catalog=Teatru2;Integrated Security=True";
        SqlConnection connection = new SqlConnection("Data Source=MARIA\\SQLEXPRESS; Initial Catalog=Teatru2;Integrated Security=True");

        SqlConnection connectionTran1 = new SqlConnection("Data Source=MARIA\\SQLEXPRESS; Initial Catalog=Teatru2;Integrated Security=True");
        SqlConnection connectionTran2 = new SqlConnection("Data Source=MARIA\\SQLEXPRESS; Initial Catalog=Teatru2;Integrated Security=True");

        SqlDataAdapter da = new SqlDataAdapter();
        SqlDataAdapter daSpectacole = new SqlDataAdapter();
        SqlDataAdapter daAngajati = new SqlDataAdapter();

        DataSet ds = new DataSet();

        BindingSource bs = new BindingSource();
        BindingSource bsSpectacole = new BindingSource();
        BindingSource bsAngajati = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void validare()
        {
            //campul obligatoriu este denumirea -> intotdeauna va trebui sa aiba o valoare
            if (textDenumireSpectacol.Text == "")
                throw new Exception("Field-ul Descriere nu trebuie sa fie gol!");
        }

        private void validare_id()
        {
            String id = textIdTeatruSpectacol.Text;
            if (Convert.ToInt32(id) < 1)
                throw new Exception("Id-ul nu poate sa fie mai mic fata de 1!");
        }

        private void butonConectare_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new SqlCommand("SELECT * FROM TeatruNational", connection);
            ds.Clear();
            da.Fill(ds, "TeatruNational");

            //display pentru spectacolele tinute in acel teatru

            //se creaza un dataAdapter nou pentru spectacole + binding source
            daSpectacole.SelectCommand = new SqlCommand("SELECT * FROM Spectacole", connection);

            daSpectacole.Fill(ds, "Spectacole");

            //ne vom folosi de DataRelation pentru a selecta numai spectacolele cu o anumita cheie straina (aka cea selectata in tabelul cu teatru)
            DataRelation dataRelation1 = new DataRelation("FK__Spectacole__IdT__4CA06362", ds.Tables["TeatruNational"].Columns["IdT"], ds.Tables["Spectacole"].Columns["IdT"]);
            ds.Relations.Add(dataRelation1);


            daAngajati.SelectCommand = new SqlCommand("SELECT * FROM Angajati", connection);
            daAngajati.Fill(ds, "Angajati");

            DataRelation dataRelation2 = new DataRelation("FK__Angajati__IdT__41B8C09B", ds.Tables["TeatruNational"].Columns["IdT"], ds.Tables["Angajati"].Columns["IdT"]);
            ds.Relations.Add(dataRelation2);

            //se seteaza datele pentru binding
            bs.DataSource = ds;
            bs.DataMember = "TeatruNational";

            bsSpectacole.DataSource = bs;
            bsSpectacole.DataMember = "FK__Spectacole__IdT__4CA06362";

            bsAngajati.DataSource = bs;
            bsAngajati.DataMember = "FK__Angajati__IdT__41B8C09B";

            dataGridSpectacole.DataSource = bsSpectacole;
            dataGridView1.DataSource = bs;
            dataGridViewAngajati.DataSource = bsAngajati;


            //display informatii despre teatrul selectat din lista

            //pentru siguranta
            textIdTeatru.DataBindings.Clear();
            textDenumireTeatru.DataBindings.Clear();
            textOrasTeatru.DataBindings.Clear();

            textIdTeatru.DataBindings.Add("Text", bs, "IdT");
            textDenumireTeatru.DataBindings.Add("Text", bs, "Denumire");
            textOrasTeatru.DataBindings.Add("Text", bs, "Oras");


            //display informatii despre spectacolul selectat din lista
            textDenumireSpectacol.DataBindings.Add("Text", bsSpectacole, "Denumire");
            textDescriereSpectacol.DataBindings.Add("Text", bsSpectacole, "Descriere");
            textIdTeatruSpectacol.DataBindings.Add("Text", bsSpectacole, "IdT");
        }

        private void butonInsert_Click(object sender, EventArgs e)
        {
            try
            {
                validare();
                validare_id();

                //comanda propriu-zisa -> nu se cunosc parametrii
                daSpectacole.InsertCommand = new SqlCommand("INSERT INTO Spectacole VALUES (@denumire ,@descriere,@idTeatru)", connection);

                //se completeaza parametrii pentru comanda anterioara
                daSpectacole.InsertCommand.Parameters.Add("@denumire", SqlDbType.VarChar).Value = textDenumireSpectacol.Text;
                daSpectacole.InsertCommand.Parameters.Add("@descriere", SqlDbType.VarChar).Value = textDescriereSpectacol.Text;
                daSpectacole.InsertCommand.Parameters.Add("@idTeatru", SqlDbType.Int).Value = textIdTeatru.Text;

                //este important sa inchidem conexiunea cat mai rapid (altfel riscam sa uitam si sa o lasam deschisa)
                connection.Open();
                daSpectacole.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Inserted Succesfull to the Database");
                connection.Close();

                //partea de update (pentru a vedea informatiile in tabelul de spectacole)
                ds.Tables["Spectacole"].Clear();
                daSpectacole.Fill(ds, "Spectacole");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }

        private void butonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                validare();
                validare_id();

                //se scrie comanda de update propriu-zisa
                daSpectacole.UpdateCommand = new SqlCommand("UPDATE Spectacole SET Denumire = @denumire, Descriere = @descriere, IdT = @idTeatru WHERE IdS = @id", connection);

                //se dau valori parametrilor necunoscuti
                daSpectacole.UpdateCommand.Parameters.Add("@denumire", SqlDbType.VarChar).Value = textDenumireSpectacol.Text;
                daSpectacole.UpdateCommand.Parameters.Add("@descriere", SqlDbType.VarChar).Value = textDescriereSpectacol.Text;
                daSpectacole.UpdateCommand.Parameters.Add("@idTeatru", SqlDbType.Int).Value = textIdTeatruSpectacol.Text;

                //se determina randul curent + id-ul spectacolului de pe acel rand
                DataRowView currentRow = (DataRowView)bsSpectacole.Current;
                int idSpectacol = Convert.ToInt32(currentRow["IdS"]);
                daSpectacole.UpdateCommand.Parameters.Add("@id", SqlDbType.Int).Value = idSpectacol;

                //se efectueaza comanda
                connection.Open();
                daSpectacole.UpdateCommand.ExecuteNonQuery();
                MessageBox.Show("The update was succesfull");
                connection.Close();

                //update pentru ui
                ds.Tables["Spectacole"].Clear();
                daSpectacole.Fill(ds, "Spectacole");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
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

                    //se scrie comanda propriu-zisa
                    daSpectacole.DeleteCommand = new SqlCommand("DELETE FROM Spectacole WHERE IdS = @id", connection);

                    //se adauga parametrul (se identifica inregistrarea curenta si se utilizeaza id-ul acesteia)
                    DataRowView currentRow = (DataRowView)bsSpectacole.Current;
                    int idSpectacol = Convert.ToInt32(currentRow["IdS"]);
                    daSpectacole.DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = idSpectacol;

                    //se executa comanda
                    connection.Open();
                    daSpectacole.DeleteCommand.ExecuteNonQuery();
                    connection.Close();

                    //update in ui
                    ds.Tables["Spectacole"].Clear();
                    daSpectacole.Fill(ds, "Spectacole");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
        }


        private void butonCautare_Click(object sender, EventArgs e)
        {
            try
            {
                //ne asiguram ca exista o denumire dupa care vrem sa cautam in tabel
                validare();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            //se realizeaza comanda de cautare
            daSpectacole.SelectCommand = new SqlCommand("SELECT * FROM Spectacole WHERE Denumire = @denumire", connection);

            //se adauga parametrii
            daSpectacole.SelectCommand.Parameters.Add("@denumire", SqlDbType.VarChar).Value = textDenumireSpectacolCautare.Text;

            ds.Tables["Spectacole"].Clear();
            daSpectacole.Fill(ds, "Spectacole");
        }


        private void butonRefresh_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new SqlCommand("SELECT * FROM TeatruNational", connection);
            daSpectacole.SelectCommand = new SqlCommand("SELECT * FROM Spectacole", connection);
            daAngajati.SelectCommand = new SqlCommand("SELECT * FROM Angajati", connection);

            ds.Clear();
            da.Fill(ds, "TeatruNational");
            daSpectacole.Fill(ds, "Spectacole");
            daAngajati.Fill(ds, "Angajati");
        }


        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            //se sorteaza dupa coloana respectiva
            daSpectacole.SelectCommand = new SqlCommand("SELECT * FROM Spectacole ORDER BY @column", connection);

            //se seteaza parametrul necunoscut
            daSpectacole.SelectCommand.Parameters.Add(Text, SqlDbType.VarChar).Value = columnName;

            //se realizeaza comanda
            ds.Tables["Spectacole"].Clear();
            daSpectacole.Fill(ds, "Spectacole");
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }


        ////////////// LABORATOR 2 /////////////////////

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


        private async void butonUnrepeatableReads_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1UR());
                var t2 = Task.Run(() => Tranzactia2UR());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

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


        private async void butonLostUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1LU());
                var t2 = Task.Run(() => Tranzactia2LU());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private async void butonDeadlock_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Operatie1());
                var t2 = Task.Run(() => Operatie2());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void butonDeadlockPrevenire_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Operatie1());
                var t2 = Task.Run(() => Operatie21());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void butonDeadlockPriority_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Operatie11());
                var t2 = Task.Run(() => Operatie2());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void butonDirtyReadsPrevenire_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1DR());
                var t2 = Task.Run(() => Tranzactia2DR1());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void butonUnrepeatableReadsPrevenire_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1UR1());
                var t2 = Task.Run(() => Tranzactia2UR());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void butonPhantomReadsPrevenire_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1PR1());
                var t2 = Task.Run(() => Tranzactia2PR());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void butonLostUpdatesPrevenire_Click(object sender, EventArgs e)
        {
            try
            {
                var t1 = Task.Run(() => Tranzactia1LU1());
                var t2 = Task.Run(() => Tranzactia2LU());

                await Task.WhenAll(t1, t2);
                MessageBox.Show("S-au finalizat ambele tranzactii!");

                //update pentru ui
                butonRefresh_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
