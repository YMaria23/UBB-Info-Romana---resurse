using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using EFCoreSecondLevelCacheInterceptor;


namespace Laborator1
{
    public partial class Form1 : Form
    {
        int pageNumber = 0;
        int pageSize = 20;

        int firstId = 0;
        int lastId = 0;

        int cacheHit = 0;
        int cacheMiss = 0;

        private readonly IMemoryCache _cache;

        public TeatruNational GetTeatruNationalById(TeatruDbContext context,int id)
        {
            cacheHit++;

            if (_cache.TryGetValue("all_theaters_list", out List<TeatruNational> listaCompleta))
            {
                   var teatruDinLista = listaCompleta.FirstOrDefault(t => t.IdT == id);
                   if (teatruDinLista != null)
                   { 
                       return teatruDinLista;
                   }
            }

            return _cache.GetOrCreate($"teatru_{id}", entry =>
            {
                 cacheMiss++;
                 cacheHit--;
                 entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                 return context.TeatruNational.Find(id);
            });
        }

        public List<TeatruNational> GetAllTeatre(TeatruDbContext context)
        {

            string cacheKey = "all_theaters_list";
            cacheHit++;

            return _cache.GetOrCreate(cacheKey, entry =>
            {
                // daca intra aici, se poate considera un cache miss -> se da fetch din nou la inregistrari
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                cacheMiss++;
                cacheHit--;

                return context.TeatruNational.ToList();
            }) ?? new List<TeatruNational>(); // va returna lista goala daca este null
        }

        public Form1()
        {
            InitializeComponent();

            var options = new MemoryCacheOptions();
            _cache = new MemoryCache(options);
        }

        private void butonConectare_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeatruDbContext())
                {
                    // aduce toate teatrele din baza de date sub forma de lista
                    //var listaTeatre = GetAllTeatre(context);
                    var listaTeatre = context.TeatruNational
                                .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10)) // <--- Aici se întâmplă magia
                                .ToList();

                    dataGridView1.DataSource = listaTeatre;


                    // aduce toti angajatii intr-o lista paginata
                    textPageSize.Text = pageSize.ToString();
                    pageNumber = 0;
                    labelPageNumber.Text = "Page: 1";
                    lastId = 0;
                    firstId = 0;

                    //var rezultat = getAngajatiOffset(context,pageNumber, pageSize);
                    var rezultat = getAngajatiKeySet(context, lastId, pageSize,1);

                    lastId = rezultat.Items[rezultat.Items.Count - 1].IdAngajat;
                    firstId = rezultat.Items[0].IdAngajat;

                    dataGridAngajati.DataSource = rezultat.Items;

                    labelNrTotal.Text = "Nr total: " + rezultat.TotalCount.ToString();
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

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.CurrentRow != null &&
                   dataGridView1.CurrentRow.DataBoundItem is TeatruNational teatruI)
                {
                    int id = teatruI.IdT;

                    using (var db = new TeatruDbContext())
                    {
                        var teatru = GetTeatruNationalById(db,id);
                        if (teatru != null)
                        {
                            teatru.IdT = int.Parse(textIdTeatru.Text);
                            teatru.Denumire = textDenumireTeatru.Text;
                            teatru.Oras = textOrasTeatru.Text;

                            db.SaveChanges();

                            // se sterge inregistrarea actualizata din cache, pentru a forta reincarcarea atunci cand este ceruta din nou
                            _cache.Remove($"teatru_{id}");
                            _cache.Remove("all_theaters_list");

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

        private void incarcaAngajati(object sender, EventArgs args,int directie)
        {
            using (var db = new TeatruDbContext()) {
                labelPageNumber.Text = "Page: " + (pageNumber + 1);
                //var angajati = getAngajatiOffset(db, pageNumber, pageSize).Items;

                var angajati = new List<Angajati>();
                if (directie == 1)
                    angajati = getAngajatiKeySet(db, lastId, pageSize,directie).Items;
                else
                    angajati = getAngajatiKeySet(db, firstId, pageSize, directie).Items;

                lastId = angajati[angajati.Count - 1].IdAngajat;
                firstId = angajati[0].IdAngajat;

                dataGridAngajati.DataSource = angajati;
            }
        }

        private void buttonPreviousPage_Click(object sender, EventArgs e)
        {
            if (pageNumber == 0)
            {
                MessageBox.Show("This is the first page!");
                return;
            }
            lastId = firstId;
            pageNumber--;
            incarcaAngajati(sender, e,0);
        }

        private void buttonNextPage_Click(object sender, EventArgs e)
        {
            pageNumber++;
            incarcaAngajati(sender, e,1);
        }

        private void textPageSize_Leave(object sender, EventArgs e)
        {
            try
            {
                pageSize = int.Parse(textPageSize.Text);
                pageNumber = 0;
                lastId = 0;

                incarcaAngajati(sender, e,1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
        }


        private void butonRefresh_Click(object sender, EventArgs e)
        {
            butonConectare_Click(sender, e);
        }

        private void refreshSpectacole(object sender, EventArgs e)
        {
            dataGridView1_SelectionChanged(sender, e);
        }



        ////////////////////// LABORATORUL 4 ////////////////////////////////

        // Problema N+1 query
        private void buttonQueryProblem_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = Stopwatch.StartNew();

                // query 1
                List<TeatruNational> teatre = context.TeatruNational
                    .ToList();

                int interogari = 1;

                // next N queries
                foreach (TeatruNational teatru in teatre)
                {
                    // caut spectacolele pentru fiecare teatru
                    var spectacole = teatru.Spectacole;

                    interogari += 1;

                    Console.WriteLine("Teatrul " + teatru.Denumire + " are" + spectacole.Count + " spectacole.");
                }

                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;

                MessageBox.Show("Operation Completed!\nTotal time: " + totalTime + "ms");
                MessageBox.Show("Nr of theaters: " + teatre.Count + "\n" + "Nr of queries: " + interogari);
                refreshSpectacole(sender, e);
            }
        }

        // Rezolvarea problemei cu Eager Loading
        private void buttonSolutionQuery_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = Stopwatch.StartNew();

                var teatre = context.TeatruNational
                    .Include(x => x.Spectacole)
                    .ToList();

                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;

                int interogari = 1;

                MessageBox.Show("Operation Completed!\nTotal time: " + totalTime + "ms");
                MessageBox.Show("Nr of theaters: " + teatre.Count + "\n" + "Nr of queries: " + interogari);
            }
        }


        /////////////// BENCHMARKS WITH AND WITHOUT INDEX ///////////////////////////////

        //Performance Q1 -> equality on varchar
        private void buttonPerformanceQ1_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = Stopwatch.StartNew();

                for (int i = 0; i < 100; i++)
                {
                    var angajati = context.Angajati
                        .Where(a => a.Prenume == "Maria")
                        .Select(a => a.Prenume)
                        .ToList();
                }

                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var averageTimePerQuery = totalTime / 100;

                MessageBox.Show("Total time: " + totalTime + "ms\nAverage time per query: " + averageTimePerQuery + "ms");
            }
        }

        //Performance Q2 -> equality on int
        private void buttonPerformanceQ2_Click(Object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = Stopwatch.StartNew();

                for (int i = 0; i < 100; i++)
                {
                    var angajati = context.Angajati
                        .Where(a => a.IdT == 2)
                        .Select(a => a.Prenume)
                        .ToList();
                }

                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var averageTimePerQuery = totalTime / 100;

                MessageBox.Show("Total time: " + totalTime + "ms\nAverage time per query: " + averageTimePerQuery + "ms");
            }
        }


        //Performance Q3 -> interval
        private void buttonPerformanceQ3_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = Stopwatch.StartNew();

                for (int i = 0; i < 100; i++)
                {
                    var angajati = context.Angajati
                        .Where(a => a.Salariu >= 4000 && a.Salariu <= 5500)
                        .Select(a => a.Nume + a.Prenume)
                        .ToList();
                }

                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var averageTimePerQuery = totalTime / 100;

                MessageBox.Show("Total time: " + totalTime + "ms\nAverage time per query: " + averageTimePerQuery + "ms");
            }
        }


        //Performance Q4 -> based on 2 columns
        private void buttonPerformanceQ4_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = Stopwatch.StartNew();

                for (int i = 0; i < 100; i++)
                {
                    var angajati = context.Angajati
                        .Where(a => a.IdT == 1 && a.Salariu >= 6000)
                        .Select(a => a.Nume)
                        .ToList();
                }

                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var averageTimePerQuery = totalTime / 100;

                MessageBox.Show("Total time: " + totalTime + "ms\nAverage time per query: " + averageTimePerQuery + "ms");
            }
        }


        //////////////////////// PAGINARE /////////////////////////////

        // Metoda offset

        private PageResult<Angajati> getAngajatiOffset(TeatruDbContext context, int pageNumber, int pageSize)
        {
            int offset = pageNumber * pageSize;

            List<Angajati> angajati = context.Angajati
                    .Include(s => s.TeatruNational)
                    .OrderBy(a => a.IdAngajat)
                    .Skip(offset)
                    .Take(pageSize)
                    .ToList();
            
            int totalOfAngajati = context.Angajati.Count();

            return new PageResult<Angajati>(angajati, totalOfAngajati);

        }


        // Metoda KeySet
        private PageResult<Angajati> getAngajatiKeySet(TeatruDbContext context, int lastId, int pageSize, int directie)
        {
            List<Angajati> angajati = new List<Angajati>();
            if (directie == 1)
                angajati = context.Angajati
                        .Where(a => a.IdAngajat > lastId)
                        .Include(s => s.TeatruNational)
                        .OrderBy(a => a.IdAngajat)
                        .Take(pageSize)
                        .ToList();
            else
            {
                angajati = context.Angajati
                    .Where(a => a.IdAngajat < lastId)
                    .Include(a => a.TeatruNational)
                    .OrderByDescending(a => a.IdAngajat)
                    .Take(pageSize)
                    .ToList();

                angajati.Reverse();
            }


                int totalOfAngajati = context.Angajati.Count();

            return new PageResult<Angajati>(angajati, totalOfAngajati);

        }


        private void runOffset(TeatruDbContext context, int pageSizePrivate, int pageNumberPrivate)
        {
                int offsetPrivate = pageNumberPrivate * pageSizePrivate;

                List<Angajati> angajati = context.Angajati
                    .AsNoTracking()
                    .OrderBy(a => a.IdAngajat)
                    .Skip(offsetPrivate)
                    .Take(pageSizePrivate)
                    .ToList();
        }

        private void runKeyset(TeatruDbContext context, int pageSizePrivate, int lastId)
        {

            List<Angajati> angajati = context.Angajati
                .AsNoTracking()
                .Where(a => a.IdAngajat > lastId)
                .Include(s => s.TeatruNational)
                .OrderBy(a => a.IdAngajat)
                .Take(pageSizePrivate)
                .ToList();
        }


        private void buttonOffsetFirst_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; i++)
                {
                    runOffset(context,100,0);
                }
                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var timePerRun = totalTime / 50;
                MessageBox.Show("Total time: " + totalTime + "ms\nEstimated time per run: " + timePerRun + "ms");
            }
        }

        private void buttonKeysetFirst_Click(object sender, EventArgs args)
        {

            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; i++)
                {
                    runKeyset(context, 100, 0);
                }
                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var timePerRun = totalTime / 50;
                MessageBox.Show("Total time: " + totalTime + "ms\nEstimated time per run: " + timePerRun + "ms");
            }
        }

        private void buttonOffsetMid_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; i++)
                {
                    runOffset(context, 100, 50);
                }
                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var timePerRun = totalTime / 50;
                MessageBox.Show("Total time: " + totalTime + "ms\nEstimated time per run: " + timePerRun + "ms");
            }
        }

        private void buttonKeysetMid_Click(object sender, EventArgs args)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; i++)
                {
                    runKeyset(context, 100, 5000);
                }
                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var timePerRun = totalTime / 50;
                MessageBox.Show("Total time: " + totalTime + "ms\nEstimated time per run: " + timePerRun + "ms");
            }
        }

        private void buttonOffsetLast_Click(object sender, EventArgs e)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; i++)
                {
                    runOffset(context, 100, 100);
                }
                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var timePerRun = totalTime / 50;
                MessageBox.Show("Total time: " + totalTime + "ms\nEstimated time per run: " + timePerRun + "ms");
            }
        }

        private void buttonKeysetLast_Click(object sender, EventArgs args)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; i++)
                {
                    runKeyset(context, 100, 10000);
                }
                sw.Stop();
                var totalTime = sw.ElapsedMilliseconds;
                var timePerRun = totalTime / 50;
                MessageBox.Show("Total time: " + totalTime + "ms\nEstimated time per run: " + timePerRun + "ms");
            }
        }


        ////////////////////////// CACHE ////////////////////////////
        private void buttonCacheTest_Click(object sender,EventArgs a)
        {
            using (TeatruDbContext context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                var teatru = GetTeatruNationalById(context,1);

                sw.Stop();
                var missTime = sw.ElapsedMilliseconds;

                sw.Restart();
                teatru = GetTeatruNationalById(context,1);
                sw.Stop();

                var hitTime1 = sw.ElapsedMilliseconds;

                sw.Restart();
                teatru = GetTeatruNationalById(context,1);
                sw.Stop();

                var hitTime2 = sw.ElapsedMilliseconds;


                MessageBox.Show("Miss time: " + missTime + "ms\nHit Time (test 1): " + hitTime1 + "ms\nHit Time (test 2): " + hitTime2 + "ms");
            }

        }

        private void buttonStatisticaCaching_Click(object sender, EventArgs a)
        {
            double nrTotal = (double) cacheMiss + cacheHit;

            double cacheMissRate = cacheMiss / nrTotal;
            double cacheHitRate = cacheHit / nrTotal;

            MessageBox.Show("Total numer of usages: " + nrTotal + "\nNr of misses: " + cacheMiss + "\nNr of hits: " + cacheHit + "\n\nMiss Rate: " + cacheMissRate + "\nHitRate: " + cacheHitRate);
        }



        //////////////////////// UPDATE PERFORMANCE ////////////////////
        private void buttonIndividualUpdate_Click(object sender,EventArgs a)
        {
            using(var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start(); 

                List<Angajati> angajati = context.Angajati.ToList();
                foreach(Angajati angajat in angajati)
                {
                    //marire de salariu
                    angajat.Salariu += 100;
                    context.SaveChanges();
                }

                sw.Stop();
                var time = sw.ElapsedMilliseconds;
                MessageBox.Show("Total time: " + time + "ms");
            }

        }

        private void buttonMassUpdate_Click(object sender, EventArgs a)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                var angajati = context.Angajati
                    .Where(a => a.IdT == 2)
                    .ExecuteUpdate(s => s.SetProperty(a => a.Salariu, a => a.Salariu + 100));

                sw.Stop();
                var time = sw.ElapsedMilliseconds;
                MessageBox.Show("Total time: " + time + "ms");
            }


        }


        private void buttonBatchUpdate_Click(object sender, EventArgs a)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                List<Angajati> angajati = context.Angajati.ToList();
                foreach (Angajati angajat in angajati)
                {
                    //marire de salariu
                    angajat.Salariu += 100;
                }

                context.SaveChanges();

                sw.Stop();
                var time = sw.ElapsedMilliseconds;
                MessageBox.Show("Total time: " + time + "ms");
            }
        }

        //////////////////////////// CACHING STATEMENTS //////////////////////////
        private void buttonWithoutReusingStatement_Click(object sender, EventArgs a)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();

                for (int i = 1; i <= 1000; i++)
                {
                    var angajat = context.Angajati.FirstOrDefault(e => e.IdAngajat == i);
                }

                sw.Stop();
                var time = sw.ElapsedMilliseconds;

                MessageBox.Show("Total time: " + time + "ms");
            }
        }


        private static readonly Func<TeatruDbContext, int, Angajati> _getById =EF.CompileQuery((TeatruDbContext db, int id) =>db.Angajati.FirstOrDefault(e => e.IdAngajat == id));

        private void buttonReusingStatement_Click(object sender, EventArgs a)
        {
            using (var context = new TeatruDbContext())
            {
                var sw = new Stopwatch();
                sw.Start();


                for (int i = 1; i <= 1000; i++)
                {
                    var angajat = _getById(context, i);
                }

                sw.Stop();
                var time = sw.ElapsedMilliseconds;

                MessageBox.Show("Total time: " + time + "ms");
            }
        }
    }
}
