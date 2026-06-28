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
    public partial class Form1
    {
     
        /// DIRTY READ
        private void Tranzactia1DR()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran1 = con.BeginTransaction())
                {

                    try
                    {
                        da.SelectCommand = new SqlCommand("INSERT INTO TeatruNational(Denumire,Oras,AnInfiintare) VALUES ('Teatrul National Craiova','Craiova',1900);", con, tran1);

                        da.SelectCommand.ExecuteNonQuery();
                        MessageBox.Show("S-a adaugat Teatrul National Craiova!");

                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);

                        //vom da rollback la tranzactie
                        tran1.Rollback();
                        MessageBox.Show("S-a efectuat rollback la tranzactia 1.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Tranzactia2DR()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran2 = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        SqlCommand comanda = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran2);

                        //ExecuteScalar -> returneaza prima coloana din primul rand (aka primul element)
                        int nr = (int)comanda.ExecuteScalar();

                        tran2.Commit();
                        MessageBox.Show("Nr de teatre existente este: " + nr);
                    }
                    catch (Exception ex)
                    {
                        tran2.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }

        //rezolvare DIRTY READS
        private void Tranzactia2DR1()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran2 = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        SqlCommand comanda = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran2);

                        //ExecuteScalar -> returneaza prima coloana din primul rand (aka primul element)
                        int nr = (int)comanda.ExecuteScalar();

                        tran2.Commit();

                        MessageBox.Show("Nr de teatre existente este: " + nr);
                    }
                    catch (Exception ex)
                    {
                        tran2.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }



        ///UNREPEATABLE READS
        private void Tranzactia1UR()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran1 = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        //se realizeaza primul select
                        using (SqlCommand comanda1 = new SqlCommand("SELECT * FROM TeatruNational WHERE IdT = 1", con, tran1))
                        {
                            using (SqlDataReader result1 = comanda1.ExecuteReader())

                                if (result1.Read())
                                {
                                    int id = result1.GetInt32(0);
                                    string denumire = result1.GetString(1);
                                    string oras = result1.GetString(2);
                                    int an = result1.GetInt32(3);

                                    MessageBox.Show("SELECT 1 \n" + "Id: " + id + "\nDenumire: " + denumire + "\nOras: " + oras + "\nAn de infiintare: " + an);
                                }
                        }


                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);

                        //se realizeaza al doilea select -> dupa ce tranzactia 2 a modificat inregistrarea
                        using (SqlCommand comanda2 = new SqlCommand("SELECT * FROM TeatruNational WHERE IdT = 1", con, tran1))
                        {
                            using (SqlDataReader result2 = comanda2.ExecuteReader())

                                if (result2.Read())
                                {
                                    int id = result2.GetInt32(0);
                                    string denumire = result2.GetString(1);
                                    string oras = result2.GetString(2);
                                    int an = result2.GetInt32(3);

                                    MessageBox.Show("SELECT 2 \n" + "Id: " + id + "\nDenumire: " + denumire + "\nOras: " + oras + "\nAn de infiintare: " + an);
                                }
                        }


                        tran1.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran1.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Tranzactia2UR()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran2 = con.BeginTransaction())
                {
                    try
                    {

                        da.SelectCommand = new SqlCommand("UPDATE TeatruNational SET Oras='Cluj-Napoca' WHERE IdT = 1", con, tran2);

                        da.SelectCommand.ExecuteNonQuery();
                        MessageBox.Show("S-a modificat orasul teatrului cu id-ul 1!");

                        tran2.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran2.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }


        //rezolvare UNREPEATABLE READS
        private void Tranzactia1UR1()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran1 = con.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    try
                    {

                        //se realizeaza primul select
                        using (SqlCommand comanda1 = new SqlCommand("SELECT * FROM TeatruNational WHERE IdT = 1", con, tran1))
                        {
                            using (SqlDataReader result1 = comanda1.ExecuteReader())

                                if (result1.Read())
                                {
                                    int id = result1.GetInt32(0);
                                    string denumire = result1.GetString(1);
                                    string oras = result1.GetString(2);
                                    int an = result1.GetInt32(3);

                                    MessageBox.Show("SELECT 1 \n" + "Id: " + id + "\nDenumire: " + denumire + "\nOras: " + oras + "\nAn de infiintare: " + an);
                                }
                        }


                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);

                        //se realizeaza al doilea select -> dupa ce tranzactia 2 a modificat inregistrarea
                        using (SqlCommand comanda2 = new SqlCommand("SELECT * FROM TeatruNational WHERE IdT = 1", con, tran1))
                        {
                            using (SqlDataReader result2 = comanda2.ExecuteReader())

                                if (result2.Read())
                                {
                                    int id = Convert.ToInt32(result2[0]);
                                    string denumire = result2.GetString(1);
                                    string oras = result2.GetString(2);
                                    int an = Convert.ToInt32(result2[3]);

                                    MessageBox.Show("SELECT 2 \n" + "Id: " + id + "\nDenumire: " + denumire + "\nOras: " + oras + "\nAn de infiintare: " + an);
                                }
                        }


                        tran1.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran1.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }




        ///PHANTOM READS
        private void Tranzactia1PR()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlTransaction tran1 = con.BeginTransaction(IsolationLevel.RepeatableRead))
                {

                    try
                    {

                        //se realizeaza primul select
                        SqlCommand comanda1 = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran1);

                        //ExecuteScalar -> returneaza prima coloana din primul rand (aka primul element)
                        int nr = (int)comanda1.ExecuteScalar();
                        MessageBox.Show("SELECT1: \n" + "S-a determinat un nr de " + nr + " teatre.");


                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);

                        //se realizeaza al doilea select -> dupa ce tranzactia 2 a modificat inregistrarea
                        SqlCommand comanda2 = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran1);

                        nr = (int)comanda2.ExecuteScalar();
                        MessageBox.Show("SELECT2: \n" + "S-a determinat un nr de " + nr + " teatre.");


                        tran1.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran1.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Tranzactia2PR()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran2 = con.BeginTransaction())
                {

                    try
                    {
                        da.SelectCommand = new SqlCommand("INSERT INTO TeatruNational(Denumire,Oras,AnInfiintare) values ('Teatrul National XYZ','Craiova',1800)", con, tran2);

                        da.SelectCommand.ExecuteNonQuery();
                        MessageBox.Show("S-a adaugat teatrul din Craiova");

                        tran2.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran2.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }


        private void Tranzactia1PR1()
        {
            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran1 = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        //se realizeaza primul select
                        SqlCommand comanda1 = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran1);

                        //ExecuteScalar -> returneaza prima coloana din primul rand (aka primul element)
                        int nr = (int)comanda1.ExecuteScalar();
                        MessageBox.Show("SELECT1: \n" + "S-a determinat un nr de " + nr + " teatre.");


                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);

                        //se realizeaza al doilea select -> dupa ce tranzactia 2 a modificat inregistrarea
                        SqlCommand comanda2 = new SqlCommand("SELECT COUNT(*) FROM TeatruNational", con, tran1);

                        nr = (int)comanda2.ExecuteScalar();
                        MessageBox.Show("SELECT2: \n" + "S-a determinat un nr de " + nr + " teatre.");


                        tran1.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran1.Rollback();
                        MessageBox.Show(ex.Message);
                    }
            
                }   
            }
        }




        ///LOST UPDATE
        private void Tranzactia1LU()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran1 = con.BeginTransaction())
                {
                    try
                    {

                        //se realizeaza selectul pe un Angajat pentru a-i modifica salariul
                        SqlCommand comanda1 = new SqlCommand("SELECT Salariu FROM Angajati WHERE IdAngajat = 1", con, tran1);
                        SqlDataReader result = comanda1.ExecuteReader();
                        int salariu = 0;
                        if (result.Read())
                        {
                            salariu = (int)result.GetInt32(0);
                            MessageBox.Show("Salariul initial al primului angajat este: " + salariu);

                            salariu += 100;
                            MessageBox.Show("S-a adaugat un bonus de 100 de lei");
                        }
                        result.Close();

                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);


                        //se executa update-ul final
                        SqlCommand cmd = new SqlCommand("UPDATE Angajati SET Salariu = @salariu WHERE IdAngajat = 1", con, tran1);
                        cmd.Parameters.Add("@salariu", SqlDbType.Int).Value = salariu;
                        cmd.ExecuteNonQuery();

                        tran1.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran1.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }


        private void Tranzactia2LU()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran2 = con.BeginTransaction())
                {
                    try
                    {

                        //se realizeaza selectul pe un Angajat pentru a-i modifica salariul
                        SqlCommand comanda1 = new SqlCommand("SELECT Salariu FROM Angajati WHERE IdAngajat = 1", con, tran2);
                        SqlDataReader result = comanda1.ExecuteReader();
                        int salariu = 0;
                        if (result.Read())
                        {
                            salariu = (int)result.GetInt32(0);

                            salariu += 1000;
                            MessageBox.Show("S-a adaugat un bonus de 1000 de lei");
                        }
                        result.Close();

                        //se executa update-ul
                        SqlCommand cmd = new SqlCommand("UPDATE Angajati SET Salariu = @salariu WHERE IdAngajat = 1", con, tran2);
                        cmd.Parameters.Add("@salariu", SqlDbType.Int).Value = salariu;
                        cmd.ExecuteNonQuery();

                        tran2.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran2.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        // rezolvare LOST UPDATES
        private void Tranzactia1LU1()
        {

            //aici incepe efectiv tranzactia
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction tran1 = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {

                        //se realizeaza selectul pe un Angajat pentru a-i modifica salariul
                        SqlCommand comanda1 = new SqlCommand("SELECT Salariu FROM Angajati WHERE IdAngajat = 1", con, tran1);
                        SqlDataReader result = comanda1.ExecuteReader();
                        int salariu = 0;
                        if (result.Read())
                        {
                            salariu = (int)result.GetInt32(0);
                            MessageBox.Show("Salariul initial al primului angajat este: " + salariu);

                            salariu += 100;
                            MessageBox.Show("S-a adaugat un bonus de 100 de lei");
                        }
                        result.Close();

                        //sleep -> timp in care se va executa cealalta tranzactie
                        Thread.Sleep(5000);


                        //se executa update-ul final
                        SqlCommand cmd = new SqlCommand("UPDATE Angajati SET Salariu = @salariu WHERE IdAngajat = 1", con, tran1);
                        cmd.Parameters.Add("@salariu", SqlDbType.Int).Value = salariu;
                        cmd.ExecuteNonQuery();

                        tran1.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran1.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

    }
}
