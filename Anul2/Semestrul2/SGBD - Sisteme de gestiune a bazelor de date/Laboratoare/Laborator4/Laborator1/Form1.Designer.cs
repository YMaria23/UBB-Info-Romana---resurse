namespace Laborator1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            butonConectare = new Button();
            label1 = new Label();
            textIdTeatru = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            textDenumireTeatru = new TextBox();
            textOrasTeatru = new TextBox();
            dataGridSpectacole = new DataGridView();
            label6 = new Label();
            label7 = new Label();
            butonRefresh = new Button();
            label12 = new Label();
            label14 = new Label();
            buttonQueryProblem = new Button();
            buttonSolutionQuery = new Button();
            buttonPerformanceQ1 = new Button();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            buttonPerformanceQ2 = new Button();
            buttonPerformanceQ3 = new Button();
            label13 = new Label();
            buttonPerformanceQ4 = new Button();
            label15 = new Label();
            dataGridAngajati = new DataGridView();
            labelPageNumber = new Label();
            buttonNext = new Button();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            textPageSize = new TextBox();
            label20 = new Label();
            labelNrTotal = new Label();
            buttonPrevious = new Button();
            label21 = new Label();
            buttonOffsetFirst = new Button();
            label16 = new Label();
            buttonKeysetFirst = new Button();
            label22 = new Label();
            buttonKeysetMid = new Button();
            buttonOffsetMid = new Button();
            buttonOffsetLast = new Button();
            label23 = new Label();
            buttonKeysetLast = new Button();
            buttonUpdate = new Button();
            label24 = new Label();
            buttonCacheTest = new Button();
            label25 = new Label();
            buttonIndividualUpdate = new Button();
            buttonMassUpdate = new Button();
            buttonBatchUpdate = new Button();
            label26 = new Label();
            buttonWithoutReusingStatement = new Button();
            buttonReusingStatement = new Button();
            buttonStatisticaCaching = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridSpectacole).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridAngajati).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(23, 80);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(509, 281);
            dataGridView1.TabIndex = 0;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // butonConectare
            // 
            butonConectare.BackColor = SystemColors.ActiveCaption;
            butonConectare.Location = new Point(580, 304);
            butonConectare.Name = "butonConectare";
            butonConectare.Size = new Size(108, 47);
            butonConectare.TabIndex = 1;
            butonConectare.Text = "Conectare";
            butonConectare.UseVisualStyleBackColor = false;
            butonConectare.Click += butonConectare_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(23, 35);
            label1.Name = "label1";
            label1.Size = new Size(373, 32);
            label1.TabIndex = 2;
            label1.Text = "Tabel Parinte -> TeatruNational";
            // 
            // textIdTeatru
            // 
            textIdTeatru.Location = new Point(693, 136);
            textIdTeatru.Name = "textIdTeatru";
            textIdTeatru.Size = new Size(298, 33);
            textIdTeatru.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(580, 76);
            label2.Name = "label2";
            label2.Size = new Size(108, 25);
            label2.TabIndex = 4;
            label2.Text = "Date Teatru";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(580, 136);
            label3.Name = "label3";
            label3.Size = new Size(89, 25);
            label3.TabIndex = 5;
            label3.Text = "Id Teatru:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(580, 187);
            label4.Name = "label4";
            label4.Size = new Size(99, 25);
            label4.TabIndex = 6;
            label4.Text = "Denumire:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(580, 242);
            label5.Name = "label5";
            label5.Size = new Size(55, 25);
            label5.TabIndex = 7;
            label5.Text = "Oras:";
            // 
            // textDenumireTeatru
            // 
            textDenumireTeatru.Location = new Point(693, 187);
            textDenumireTeatru.Name = "textDenumireTeatru";
            textDenumireTeatru.Size = new Size(298, 33);
            textDenumireTeatru.TabIndex = 8;
            // 
            // textOrasTeatru
            // 
            textOrasTeatru.Location = new Point(693, 242);
            textOrasTeatru.Name = "textOrasTeatru";
            textOrasTeatru.Size = new Size(298, 33);
            textOrasTeatru.TabIndex = 9;
            // 
            // dataGridSpectacole
            // 
            dataGridSpectacole.BackgroundColor = SystemColors.Control;
            dataGridSpectacole.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridSpectacole.Location = new Point(1182, 80);
            dataGridSpectacole.Name = "dataGridSpectacole";
            dataGridSpectacole.RowHeadersWidth = 62;
            dataGridSpectacole.Size = new Size(509, 281);
            dataGridSpectacole.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(1182, 33);
            label6.Name = "label6";
            label6.Size = new Size(303, 32);
            label6.TabIndex = 11;
            label6.Text = "Tabel Copil -> Spectacole";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(637, 516);
            label7.Name = "label7";
            label7.Size = new Size(0, 25);
            label7.TabIndex = 12;
            // 
            // butonRefresh
            // 
            butonRefresh.BackColor = Color.SkyBlue;
            butonRefresh.Location = new Point(704, 304);
            butonRefresh.Name = "butonRefresh";
            butonRefresh.Size = new Size(97, 47);
            butonRefresh.TabIndex = 27;
            butonRefresh.Text = "Refresh";
            butonRefresh.UseVisualStyleBackColor = false;
            butonRefresh.Click += butonRefresh_Click;
            // 
            // label12
            // 
            label12.Location = new Point(0, 0);
            label12.Name = "label12";
            label12.Size = new Size(100, 23);
            label12.TabIndex = 0;
            // 
            // label14
            // 
            label14.Location = new Point(0, 0);
            label14.Name = "label14";
            label14.Size = new Size(100, 23);
            label14.TabIndex = 0;
            // 
            // buttonQueryProblem
            // 
            buttonQueryProblem.BackColor = Color.DarkCyan;
            buttonQueryProblem.Location = new Point(780, 486);
            buttonQueryProblem.Name = "buttonQueryProblem";
            buttonQueryProblem.Size = new Size(220, 47);
            buttonQueryProblem.TabIndex = 31;
            buttonQueryProblem.Text = "N + 1 query problem";
            buttonQueryProblem.UseVisualStyleBackColor = false;
            buttonQueryProblem.Click += buttonQueryProblem_Click;
            // 
            // buttonSolutionQuery
            // 
            buttonSolutionQuery.BackColor = Color.DarkCyan;
            buttonSolutionQuery.Location = new Point(780, 540);
            buttonSolutionQuery.Name = "buttonSolutionQuery";
            buttonSolutionQuery.Size = new Size(220, 47);
            buttonSolutionQuery.TabIndex = 32;
            buttonSolutionQuery.Text = "Solution";
            buttonSolutionQuery.UseVisualStyleBackColor = false;
            buttonSolutionQuery.Click += buttonSolutionQuery_Click;
            // 
            // buttonPerformanceQ1
            // 
            buttonPerformanceQ1.BackColor = Color.LightSeaGreen;
            buttonPerformanceQ1.Location = new Point(26, 557);
            buttonPerformanceQ1.Name = "buttonPerformanceQ1";
            buttonPerformanceQ1.Size = new Size(261, 47);
            buttonPerformanceQ1.TabIndex = 33;
            buttonPerformanceQ1.Text = "Run Performance Test";
            buttonPerformanceQ1.UseVisualStyleBackColor = false;
            buttonPerformanceQ1.Click += buttonPerformanceQ1_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(774, 439);
            label8.Name = "label8";
            label8.Size = new Size(259, 32);
            label8.TabIndex = 35;
            label8.Text = "N + 1 Query Problem";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(26, 439);
            label9.Name = "label9";
            label9.Size = new Size(440, 32);
            label9.TabIndex = 36;
            label9.Text = "Benchmarks - Peformance on queries";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label10.Location = new Point(26, 505);
            label10.Name = "label10";
            label10.Size = new Size(260, 28);
            label10.TabIndex = 37;
            label10.Text = "Query - equality on varchar";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label11.Location = new Point(354, 505);
            label11.Name = "label11";
            label11.Size = new Size(216, 28);
            label11.TabIndex = 38;
            label11.Text = "Query - equality on int";
            // 
            // buttonPerformanceQ2
            // 
            buttonPerformanceQ2.BackColor = Color.LightSeaGreen;
            buttonPerformanceQ2.Location = new Point(353, 559);
            buttonPerformanceQ2.Name = "buttonPerformanceQ2";
            buttonPerformanceQ2.Size = new Size(261, 47);
            buttonPerformanceQ2.TabIndex = 39;
            buttonPerformanceQ2.Text = "Run Performance Test";
            buttonPerformanceQ2.UseVisualStyleBackColor = false;
            buttonPerformanceQ2.Click += buttonPerformanceQ2_Click;
            // 
            // buttonPerformanceQ3
            // 
            buttonPerformanceQ3.BackColor = Color.LightSeaGreen;
            buttonPerformanceQ3.Location = new Point(26, 716);
            buttonPerformanceQ3.Name = "buttonPerformanceQ3";
            buttonPerformanceQ3.Size = new Size(261, 47);
            buttonPerformanceQ3.TabIndex = 42;
            buttonPerformanceQ3.Text = "Run Performance Test";
            buttonPerformanceQ3.UseVisualStyleBackColor = false;
            buttonPerformanceQ3.Click += buttonPerformanceQ3_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label13.Location = new Point(27, 662);
            label13.Name = "label13";
            label13.Size = new Size(176, 28);
            label13.TabIndex = 41;
            label13.Text = "Query - inequality";
            // 
            // buttonPerformanceQ4
            // 
            buttonPerformanceQ4.BackColor = Color.LightSeaGreen;
            buttonPerformanceQ4.Location = new Point(353, 716);
            buttonPerformanceQ4.Name = "buttonPerformanceQ4";
            buttonPerformanceQ4.Size = new Size(261, 47);
            buttonPerformanceQ4.TabIndex = 45;
            buttonPerformanceQ4.Text = "Run Performance Test";
            buttonPerformanceQ4.UseVisualStyleBackColor = false;
            buttonPerformanceQ4.Click += buttonPerformanceQ4_Click;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label15.Location = new Point(354, 662);
            label15.Name = "label15";
            label15.Size = new Size(350, 28);
            label15.TabIndex = 44;
            label15.Text = "Query - condition based on 2 columns";
            // 
            // dataGridAngajati
            // 
            dataGridAngajati.BackgroundColor = SystemColors.Control;
            dataGridAngajati.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridAngajati.Location = new Point(1173, 478);
            dataGridAngajati.Name = "dataGridAngajati";
            dataGridAngajati.RowHeadersWidth = 62;
            dataGridAngajati.Size = new Size(532, 510);
            dataGridAngajati.TabIndex = 49;
            // 
            // labelPageNumber
            // 
            labelPageNumber.AutoSize = true;
            labelPageNumber.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            labelPageNumber.Location = new Point(1465, 1002);
            labelPageNumber.Name = "labelPageNumber";
            labelPageNumber.Size = new Size(61, 28);
            labelPageNumber.TabIndex = 52;
            labelPageNumber.Text = "Page:";
            // 
            // buttonNext
            // 
            buttonNext.BackColor = Color.Silver;
            buttonNext.Location = new Point(1660, 994);
            buttonNext.Name = "buttonNext";
            buttonNext.Size = new Size(46, 47);
            buttonNext.TabIndex = 50;
            buttonNext.Text = ">";
            buttonNext.UseVisualStyleBackColor = false;
            buttonNext.UseWaitCursor = true;
            buttonNext.Click += buttonNextPage_Click;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label17.Location = new Point(1173, 434);
            label17.Name = "label17";
            label17.Size = new Size(177, 32);
            label17.TabIndex = 53;
            label17.Text = "Tabel Angajati";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(1168, 1137);
            label18.Name = "label18";
            label18.Size = new Size(317, 25);
            label18.TabIndex = 54;
            label18.Text = "Setarea nr de inregistrari per pagina";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(604, 674);
            label19.Name = "label19";
            label19.Size = new Size(0, 25);
            label19.TabIndex = 55;
            // 
            // textPageSize
            // 
            textPageSize.Location = new Point(1491, 1137);
            textPageSize.Name = "textPageSize";
            textPageSize.Size = new Size(215, 33);
            textPageSize.TabIndex = 56;
            textPageSize.Leave += textPageSize_Leave;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(1033, 929);
            label20.Name = "label20";
            label20.Size = new Size(0, 25);
            label20.TabIndex = 57;
            // 
            // labelNrTotal
            // 
            labelNrTotal.AutoSize = true;
            labelNrTotal.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            labelNrTotal.Location = new Point(1173, 1002);
            labelNrTotal.Name = "labelNrTotal";
            labelNrTotal.Size = new Size(92, 28);
            labelNrTotal.TabIndex = 58;
            labelNrTotal.Text = "Nr total: ";
            // 
            // buttonPrevious
            // 
            buttonPrevious.BackColor = Color.Silver;
            buttonPrevious.Location = new Point(1608, 994);
            buttonPrevious.Name = "buttonPrevious";
            buttonPrevious.Size = new Size(46, 47);
            buttonPrevious.TabIndex = 59;
            buttonPrevious.Text = "<";
            buttonPrevious.UseVisualStyleBackColor = false;
            buttonPrevious.UseWaitCursor = true;
            buttonPrevious.Click += buttonPreviousPage_Click;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label21.Location = new Point(27, 842);
            label21.Name = "label21";
            label21.Size = new Size(204, 32);
            label21.TabIndex = 60;
            label21.Text = "Test Page Access";
            // 
            // buttonOffsetFirst
            // 
            buttonOffsetFirst.BackColor = Color.LightSeaGreen;
            buttonOffsetFirst.Location = new Point(166, 885);
            buttonOffsetFirst.Name = "buttonOffsetFirst";
            buttonOffsetFirst.RightToLeft = RightToLeft.No;
            buttonOffsetFirst.Size = new Size(100, 47);
            buttonOffsetFirst.TabIndex = 61;
            buttonOffsetFirst.Text = "Offset";
            buttonOffsetFirst.UseVisualStyleBackColor = false;
            buttonOffsetFirst.Click += buttonOffsetFirst_Click;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label16.Location = new Point(36, 893);
            label16.Name = "label16";
            label16.Size = new Size(100, 28);
            label16.TabIndex = 62;
            label16.Text = "First Page";
            // 
            // buttonKeysetFirst
            // 
            buttonKeysetFirst.BackColor = Color.LightSeaGreen;
            buttonKeysetFirst.Location = new Point(296, 885);
            buttonKeysetFirst.Name = "buttonKeysetFirst";
            buttonKeysetFirst.RightToLeft = RightToLeft.No;
            buttonKeysetFirst.Size = new Size(100, 47);
            buttonKeysetFirst.TabIndex = 63;
            buttonKeysetFirst.Text = "Keyset";
            buttonKeysetFirst.UseVisualStyleBackColor = false;
            buttonKeysetFirst.Click += buttonKeysetFirst_Click;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label22.Location = new Point(36, 960);
            label22.Name = "label22";
            label22.Size = new Size(122, 28);
            label22.TabIndex = 64;
            label22.Text = "Middle Page";
            // 
            // buttonKeysetMid
            // 
            buttonKeysetMid.BackColor = Color.LightSeaGreen;
            buttonKeysetMid.Location = new Point(296, 952);
            buttonKeysetMid.Name = "buttonKeysetMid";
            buttonKeysetMid.RightToLeft = RightToLeft.No;
            buttonKeysetMid.Size = new Size(100, 47);
            buttonKeysetMid.TabIndex = 66;
            buttonKeysetMid.Text = "Keyset";
            buttonKeysetMid.UseVisualStyleBackColor = false;
            buttonKeysetMid.Click += buttonKeysetMid_Click;
            // 
            // buttonOffsetMid
            // 
            buttonOffsetMid.BackColor = Color.LightSeaGreen;
            buttonOffsetMid.Location = new Point(166, 952);
            buttonOffsetMid.Name = "buttonOffsetMid";
            buttonOffsetMid.RightToLeft = RightToLeft.No;
            buttonOffsetMid.Size = new Size(100, 47);
            buttonOffsetMid.TabIndex = 65;
            buttonOffsetMid.Text = "Offset";
            buttonOffsetMid.UseVisualStyleBackColor = false;
            buttonOffsetMid.Click += buttonOffsetMid_Click;
            // 
            // buttonOffsetLast
            // 
            buttonOffsetLast.BackColor = Color.LightSeaGreen;
            buttonOffsetLast.Location = new Point(166, 1017);
            buttonOffsetLast.Name = "buttonOffsetLast";
            buttonOffsetLast.RightToLeft = RightToLeft.No;
            buttonOffsetLast.Size = new Size(100, 47);
            buttonOffsetLast.TabIndex = 68;
            buttonOffsetLast.Text = "Offset";
            buttonOffsetLast.UseVisualStyleBackColor = false;
            buttonOffsetLast.Click += buttonOffsetLast_Click;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label23.Location = new Point(36, 1025);
            label23.Name = "label23";
            label23.Size = new Size(97, 28);
            label23.TabIndex = 67;
            label23.Text = "Last Page";
            // 
            // buttonKeysetLast
            // 
            buttonKeysetLast.BackColor = Color.LightSeaGreen;
            buttonKeysetLast.Location = new Point(296, 1017);
            buttonKeysetLast.Name = "buttonKeysetLast";
            buttonKeysetLast.RightToLeft = RightToLeft.No;
            buttonKeysetLast.Size = new Size(100, 47);
            buttonKeysetLast.TabIndex = 69;
            buttonKeysetLast.Text = "Keyset";
            buttonKeysetLast.UseVisualStyleBackColor = false;
            buttonKeysetLast.Click += buttonKeysetLast_Click;
            // 
            // buttonUpdate
            // 
            buttonUpdate.BackColor = Color.PaleTurquoise;
            buttonUpdate.Location = new Point(822, 304);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(169, 47);
            buttonUpdate.TabIndex = 70;
            buttonUpdate.Text = "Update";
            buttonUpdate.UseVisualStyleBackColor = false;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label24.Location = new Point(780, 661);
            label24.Name = "label24";
            label24.Size = new Size(235, 32);
            label24.TabIndex = 71;
            label24.Text = "Cache Performance";
            // 
            // buttonCacheTest
            // 
            buttonCacheTest.BackColor = Color.Turquoise;
            buttonCacheTest.Location = new Point(780, 718);
            buttonCacheTest.Name = "buttonCacheTest";
            buttonCacheTest.Size = new Size(137, 47);
            buttonCacheTest.TabIndex = 72;
            buttonCacheTest.Text = "Run Test";
            buttonCacheTest.UseVisualStyleBackColor = false;
            buttonCacheTest.Click += buttonCacheTest_Click;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label25.Location = new Point(491, 842);
            label25.Name = "label25";
            label25.Size = new Size(250, 32);
            label25.TabIndex = 73;
            label25.Text = "Update Performance";
            // 
            // buttonIndividualUpdate
            // 
            buttonIndividualUpdate.BackColor = Color.Turquoise;
            buttonIndividualUpdate.Location = new Point(500, 885);
            buttonIndividualUpdate.Name = "buttonIndividualUpdate";
            buttonIndividualUpdate.Size = new Size(220, 47);
            buttonIndividualUpdate.TabIndex = 74;
            buttonIndividualUpdate.Text = "Individual Update";
            buttonIndividualUpdate.UseVisualStyleBackColor = false;
            buttonIndividualUpdate.Click += buttonIndividualUpdate_Click;
            // 
            // buttonMassUpdate
            // 
            buttonMassUpdate.BackColor = Color.Turquoise;
            buttonMassUpdate.Location = new Point(500, 949);
            buttonMassUpdate.Name = "buttonMassUpdate";
            buttonMassUpdate.Size = new Size(220, 47);
            buttonMassUpdate.TabIndex = 75;
            buttonMassUpdate.Text = "Mass Update";
            buttonMassUpdate.UseVisualStyleBackColor = false;
            buttonMassUpdate.Click += buttonMassUpdate_Click;
            // 
            // buttonBatchUpdate
            // 
            buttonBatchUpdate.BackColor = Color.Turquoise;
            buttonBatchUpdate.Location = new Point(500, 1014);
            buttonBatchUpdate.Name = "buttonBatchUpdate";
            buttonBatchUpdate.Size = new Size(220, 47);
            buttonBatchUpdate.TabIndex = 76;
            buttonBatchUpdate.Text = "Batch Update";
            buttonBatchUpdate.UseVisualStyleBackColor = false;
            buttonBatchUpdate.Click += buttonBatchUpdate_Click;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label26.Location = new Point(838, 842);
            label26.Name = "label26";
            label26.Size = new Size(227, 32);
            label26.TabIndex = 77;
            label26.Text = "Caching Statement";
            // 
            // buttonWithoutReusingStatement
            // 
            buttonWithoutReusingStatement.BackColor = Color.PaleTurquoise;
            buttonWithoutReusingStatement.Location = new Point(845, 885);
            buttonWithoutReusingStatement.Name = "buttonWithoutReusingStatement";
            buttonWithoutReusingStatement.Size = new Size(220, 47);
            buttonWithoutReusingStatement.TabIndex = 78;
            buttonWithoutReusingStatement.Text = "Without Reusing";
            buttonWithoutReusingStatement.UseVisualStyleBackColor = false;
            buttonWithoutReusingStatement.Click += buttonWithoutReusingStatement_Click;
            // 
            // buttonReusingStatement
            // 
            buttonReusingStatement.BackColor = Color.PaleTurquoise;
            buttonReusingStatement.Location = new Point(845, 949);
            buttonReusingStatement.Name = "buttonReusingStatement";
            buttonReusingStatement.Size = new Size(220, 47);
            buttonReusingStatement.TabIndex = 79;
            buttonReusingStatement.Text = "Reusing";
            buttonReusingStatement.UseVisualStyleBackColor = false;
            buttonReusingStatement.Click += buttonReusingStatement_Click;
            // 
            // buttonStatisticaCaching
            // 
            buttonStatisticaCaching.BackColor = Color.Turquoise;
            buttonStatisticaCaching.Location = new Point(923, 716);
            buttonStatisticaCaching.Name = "buttonStatisticaCaching";
            buttonStatisticaCaching.Size = new Size(142, 47);
            buttonStatisticaCaching.TabIndex = 80;
            buttonStatisticaCaching.Text = "Statistici";
            buttonStatisticaCaching.UseVisualStyleBackColor = false;
            buttonStatisticaCaching.Click += buttonStatisticaCaching_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1804, 1278);
            Controls.Add(buttonStatisticaCaching);
            Controls.Add(buttonReusingStatement);
            Controls.Add(buttonWithoutReusingStatement);
            Controls.Add(label26);
            Controls.Add(buttonBatchUpdate);
            Controls.Add(buttonMassUpdate);
            Controls.Add(buttonIndividualUpdate);
            Controls.Add(label25);
            Controls.Add(buttonCacheTest);
            Controls.Add(label24);
            Controls.Add(buttonUpdate);
            Controls.Add(buttonKeysetLast);
            Controls.Add(buttonOffsetLast);
            Controls.Add(label23);
            Controls.Add(buttonKeysetMid);
            Controls.Add(buttonOffsetMid);
            Controls.Add(label22);
            Controls.Add(buttonKeysetFirst);
            Controls.Add(label16);
            Controls.Add(buttonOffsetFirst);
            Controls.Add(label21);
            Controls.Add(buttonPrevious);
            Controls.Add(labelNrTotal);
            Controls.Add(label20);
            Controls.Add(textPageSize);
            Controls.Add(label19);
            Controls.Add(label18);
            Controls.Add(label17);
            Controls.Add(labelPageNumber);
            Controls.Add(buttonNext);
            Controls.Add(dataGridAngajati);
            Controls.Add(buttonPerformanceQ4);
            Controls.Add(label15);
            Controls.Add(buttonPerformanceQ3);
            Controls.Add(label13);
            Controls.Add(buttonPerformanceQ2);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(buttonPerformanceQ1);
            Controls.Add(buttonSolutionQuery);
            Controls.Add(buttonQueryProblem);
            Controls.Add(butonRefresh);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(dataGridSpectacole);
            Controls.Add(textOrasTeatru);
            Controls.Add(textDenumireTeatru);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textIdTeatru);
            Controls.Add(label1);
            Controls.Add(butonConectare);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridSpectacole).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridAngajati).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private DataGridView dataGridView1;
        private Button butonConectare;
        private Label label1;
        private TextBox textIdTeatru;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox textDenumireTeatru;
        private TextBox textOrasTeatru;
        private DataGridView dataGridSpectacole;
        private Label label6;
        private Label label7;
        private Button butonRefresh;
        private Label label12;
        private Label label14;
        private Button buttonQueryProblem;
        private Button buttonSolutionQuery;
        private Button buttonPerformanceQ1;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Button buttonPerformanceQ2;
        private Button buttonPerformanceQ3;
        private Label label13;
        private Button buttonPerformanceQ4;
        private Label label15;
        private Button buttonNextPage;
        private Button buttonPreviousPage;
        private Label labelPageNumber;
        private DataGridView dataGridAngajati;
        private Label label16;
        private Button buttonKeysetFirst;
        private Button buttonNext;
        private Label label17;
        private Label label18;
        private Label label19;
        private TextBox textPageSize;
        private Label label20;
        private Label labelNrTotal;
        private Button buttonPrevious;
        private Label label21;
        private Button buttonOffsetFirst;
        private Label label22;
        private Button buttonKeysetMid;
        private Button buttonOffsetMid;
        private Button buttonOffsetLast;
        private Label label23;
        private Button buttonKeysetLast;
        private Button buttonUpdate;
        private Label label24;
        private Button buttonCacheTest;
        private Label label25;
        private Button buttonIndividualUpdate;
        private Button buttonMassUpdate;
        private Button buttonBatchUpdate;
        private Label label26;
        private Button buttonWithoutReusingStatement;
        private Button buttonReusingStatement;
        private Button buttonStatisticaCaching;
    }
}
