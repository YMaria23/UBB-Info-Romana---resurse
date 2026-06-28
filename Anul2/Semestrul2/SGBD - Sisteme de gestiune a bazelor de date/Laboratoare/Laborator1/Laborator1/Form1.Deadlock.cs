using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1
{
    public partial class Form1
    {


        ///DEADLOCK BASIC
        private void Operatie1()
        {

            connectionTran1.Open();
            using SqlTransaction tran1 = connectionTran1.BeginTransaction();

            try
            {
                //vreau sa modific o inregistrare din Spectacole
                using (SqlCommand comanda = new SqlCommand("UPDATE Spectacole SET Denumire = 'deadlock1' WHERE IdS = 6", connectionTran1, tran1))
                {
                    comanda.ExecuteNonQuery();
                }

                Thread.Sleep(1000);

                //urmeaza sa modific o inregistrare din Angajati
                using (SqlCommand comanda = new SqlCommand("UPDATE Angajati SET Salariu = 2500 WHERE IdAngajat = 1", connectionTran1, tran1))
                {
                    comanda.ExecuteNonQuery();
                }

                tran1.Commit();
                connectionTran1.Close();
            }
            catch (Exception ex)
            {
                tran1.Rollback();
                MessageBox.Show(ex.Message);
                connectionTran1.Close();
            }
        }

        private void Operatie2()
        {
            connectionTran2.Open();
            using SqlTransaction tran2 = connectionTran2.BeginTransaction();

            try
            {
                //urmeaza sa modific o inregistrare din Angajati
                using (SqlCommand comanda = new SqlCommand("UPDATE Angajati SET Salariu = 3000 WHERE IdAngajat = 1", connectionTran2, tran2))
                {
                    comanda.ExecuteNonQuery();
                }

                Thread.Sleep(1000);

                //vreau sa modific o inregistrare din Spectacole
                using (SqlCommand comanda = new SqlCommand("UPDATE Spectacole SET Denumire = 'deadlock2' WHERE IdS = 6", connectionTran2, tran2))
                {
                    comanda.ExecuteNonQuery();
                }

                tran2.Commit();
                connectionTran2.Close();
            }
            catch (Exception ex)
            {
                tran2.Rollback();
                MessageBox.Show(ex.Message);
                connectionTran2.Close();
            }

        }



        ///SOLUTIE CAND APARE DEADLOCK
        ///se va seta prioritate high/low pe una din tranzactii
        private void Operatie11()
        {

            connectionTran1.Open();
            using SqlTransaction tran1 = connectionTran1.BeginTransaction();

            try
            {
                //se  va seta prioritatea la deadlock
                using (SqlCommand comanda = new SqlCommand("SET DEADLOCK_PRIORITY HIGH", connectionTran1, tran1))
                {
                    comanda.ExecuteNonQuery();
                }


                //vreau sa modific o inregistrare din Spectacole
                using (SqlCommand comanda = new SqlCommand("UPDATE Spectacole SET Denumire = 'deadlock1' WHERE IdS = 6", connectionTran1, tran1))
                {
                    comanda.ExecuteNonQuery();
                }

                Thread.Sleep(1000);

                //urmeaza sa modific o inregistrare din Angajati
                using (SqlCommand comanda = new SqlCommand("UPDATE Angajati SET Salariu = 2500 WHERE IdAngajat = 1", connectionTran1, tran1))
                {
                    comanda.ExecuteNonQuery();
                }

                tran1.Commit();
                connectionTran1.Close();
            }
            catch (Exception ex)
            {
                tran1.Rollback();
                MessageBox.Show(ex.Message);
                connectionTran1.Close();
            }
        }


        ///PREVENIRE DEADLOCK
        ///se vor accesa resursele in aceeasi ordine
        private void Operatie21()
        {
            connectionTran2.Open();
            using SqlTransaction tran2 = connectionTran2.BeginTransaction();

            try
            {

                //vreau sa modific o inregistrare din Spectacole
                using (SqlCommand comanda = new SqlCommand("UPDATE Spectacole SET Denumire = 'deadlock2' WHERE IdS = 6", connectionTran2, tran2))
                {
                    comanda.ExecuteNonQuery();
                }

                Thread.Sleep(1000);

                //urmeaza sa modific o inregistrare din Angajati
                using (SqlCommand comanda = new SqlCommand("UPDATE Angajati SET Salariu = 3000 WHERE IdAngajat = 1", connectionTran2, tran2))
                {
                    comanda.ExecuteNonQuery();
                }

                tran2.Commit();
                connectionTran2.Close();
            }
            catch (Exception ex)
            {
                tran2.Rollback();
                MessageBox.Show(ex.Message);
                connectionTran2.Close();
            }

        }

    }
}
