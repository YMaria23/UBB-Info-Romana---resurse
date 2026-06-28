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
            textDenumireSpectacol = new TextBox();
            label8 = new Label();
            label9 = new Label();
            textDescriereSpectacol = new TextBox();
            butonInsert = new Button();
            butonUpdate = new Button();
            butonDelete = new Button();
            label10 = new Label();
            textIdTeatruSpectacol = new TextBox();
            label11 = new Label();
            label13 = new Label();
            textDenumireSpectacolCautare = new TextBox();
            butonCautare = new Button();
            butonRefresh = new Button();
            label12 = new Label();
            butonDirtyReads = new Button();
            butonUnrepeatableReads = new Button();
            butonPhantomReads = new Button();
            butonLostUpdate = new Button();
            dataGridViewAngajati = new DataGridView();
            label14 = new Label();
            butonDeadlock = new Button();
            butonDeadlockPriority = new Button();
            butonDeadlockPrevenire = new Button();
            butonDirtyReadsPrevenire = new Button();
            butonUnrepeatableReadsPrevenire = new Button();
            butonPhantomReadsPrevenire = new Button();
            butonLostUpdatesPrevenire = new Button();
            buttonTestPool = new Button();
            buttonTestWithoutPool = new Button();
            buttonTestWithoutClosingConnections = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridSpectacole).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAngajati).BeginInit();
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
            butonConectare.Location = new Point(637, 314);
            butonConectare.Name = "butonConectare";
            butonConectare.Size = new Size(411, 47);
            butonConectare.TabIndex = 1;
            butonConectare.Text = "Conectare";
            butonConectare.UseVisualStyleBackColor = false;
            butonConectare.Click += butonConectare_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 35);
            label1.Name = "label1";
            label1.Size = new Size(275, 25);
            label1.TabIndex = 2;
            label1.Text = "Tabel Parinte -> TeatruNational";
            // 
            // textIdTeatru
            // 
            textIdTeatru.Location = new Point(750, 140);
            textIdTeatru.Name = "textIdTeatru";
            textIdTeatru.Size = new Size(298, 33);
            textIdTeatru.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(637, 80);
            label2.Name = "label2";
            label2.Size = new Size(108, 25);
            label2.TabIndex = 4;
            label2.Text = "Date Teatru";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(637, 140);
            label3.Name = "label3";
            label3.Size = new Size(89, 25);
            label3.TabIndex = 5;
            label3.Text = "Id Teatru:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(637, 191);
            label4.Name = "label4";
            label4.Size = new Size(99, 25);
            label4.TabIndex = 6;
            label4.Text = "Denumire:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(637, 246);
            label5.Name = "label5";
            label5.Size = new Size(55, 25);
            label5.TabIndex = 7;
            label5.Text = "Oras:";
            // 
            // textDenumireTeatru
            // 
            textDenumireTeatru.Location = new Point(750, 191);
            textDenumireTeatru.Name = "textDenumireTeatru";
            textDenumireTeatru.Size = new Size(298, 33);
            textDenumireTeatru.TabIndex = 8;
            // 
            // textOrasTeatru
            // 
            textOrasTeatru.Location = new Point(750, 246);
            textOrasTeatru.Name = "textOrasTeatru";
            textOrasTeatru.Size = new Size(298, 33);
            textOrasTeatru.TabIndex = 9;
            // 
            // dataGridSpectacole
            // 
            dataGridSpectacole.BackgroundColor = SystemColors.Control;
            dataGridSpectacole.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridSpectacole.Location = new Point(23, 516);
            dataGridSpectacole.Name = "dataGridSpectacole";
            dataGridSpectacole.RowHeadersWidth = 62;
            dataGridSpectacole.Size = new Size(509, 368);
            dataGridSpectacole.TabIndex = 10;
            dataGridSpectacole.SelectionChanged += dataGridSpectacole_SelectionChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(23, 469);
            label6.Name = "label6";
            label6.Size = new Size(227, 25);
            label6.TabIndex = 11;
            label6.Text = "Tabel Copil -> Spectacole";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(637, 516);
            label7.Name = "label7";
            label7.Size = new Size(137, 25);
            label7.TabIndex = 12;
            label7.Text = "Date Spectacol";
            // 
            // textDenumireSpectacol
            // 
            textDenumireSpectacol.Location = new Point(750, 573);
            textDenumireSpectacol.Name = "textDenumireSpectacol";
            textDenumireSpectacol.Size = new Size(298, 33);
            textDenumireSpectacol.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(637, 576);
            label8.Name = "label8";
            label8.Size = new Size(99, 25);
            label8.TabIndex = 14;
            label8.Text = "Denumire:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(637, 627);
            label9.Name = "label9";
            label9.Size = new Size(95, 25);
            label9.TabIndex = 15;
            label9.Text = "Descriere:";
            // 
            // textDescriereSpectacol
            // 
            textDescriereSpectacol.Location = new Point(750, 627);
            textDescriereSpectacol.Name = "textDescriereSpectacol";
            textDescriereSpectacol.Size = new Size(298, 33);
            textDescriereSpectacol.TabIndex = 16;
            // 
            // butonInsert
            // 
            butonInsert.BackColor = Color.LightSalmon;
            butonInsert.Location = new Point(637, 777);
            butonInsert.Name = "butonInsert";
            butonInsert.Size = new Size(227, 47);
            butonInsert.TabIndex = 17;
            butonInsert.Text = "Insert";
            butonInsert.UseVisualStyleBackColor = false;
            butonInsert.Click += butonInsert_Click;
            // 
            // butonUpdate
            // 
            butonUpdate.BackColor = Color.Tomato;
            butonUpdate.Location = new Point(888, 777);
            butonUpdate.Name = "butonUpdate";
            butonUpdate.Size = new Size(227, 47);
            butonUpdate.TabIndex = 18;
            butonUpdate.Text = "Update";
            butonUpdate.UseVisualStyleBackColor = false;
            butonUpdate.Click += butonUpdate_Click;
            // 
            // butonDelete
            // 
            butonDelete.BackColor = Color.Red;
            butonDelete.Location = new Point(637, 837);
            butonDelete.Name = "butonDelete";
            butonDelete.Size = new Size(478, 47);
            butonDelete.TabIndex = 19;
            butonDelete.Text = "Delete";
            butonDelete.UseVisualStyleBackColor = false;
            butonDelete.Click += butonDelete_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(641, 680);
            label10.Name = "label10";
            label10.Size = new Size(89, 25);
            label10.TabIndex = 20;
            label10.Text = "Id Teatru:";
            // 
            // textIdTeatruSpectacol
            // 
            textIdTeatruSpectacol.Location = new Point(750, 677);
            textIdTeatruSpectacol.Name = "textIdTeatruSpectacol";
            textIdTeatruSpectacol.Size = new Size(298, 33);
            textIdTeatruSpectacol.TabIndex = 21;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(1188, 516);
            label11.Name = "label11";
            label11.Size = new Size(212, 25);
            label11.TabIndex = 22;
            label11.Text = "Cautare dupa denumire";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(1188, 573);
            label13.Name = "label13";
            label13.Size = new Size(95, 25);
            label13.TabIndex = 24;
            label13.Text = "Denumire";
            // 
            // textDenumireSpectacolCautare
            // 
            textDenumireSpectacolCautare.Location = new Point(1301, 570);
            textDenumireSpectacolCautare.Name = "textDenumireSpectacolCautare";
            textDenumireSpectacolCautare.Size = new Size(298, 33);
            textDenumireSpectacolCautare.TabIndex = 25;
            // 
            // butonCautare
            // 
            butonCautare.BackColor = Color.LightCoral;
            butonCautare.Location = new Point(1188, 627);
            butonCautare.Name = "butonCautare";
            butonCautare.Size = new Size(416, 47);
            butonCautare.TabIndex = 26;
            butonCautare.Text = "Cauta";
            butonCautare.UseVisualStyleBackColor = false;
            butonCautare.Click += butonCautare_Click;
            // 
            // butonRefresh
            // 
            butonRefresh.BackColor = Color.SkyBlue;
            butonRefresh.Location = new Point(1145, 314);
            butonRefresh.Name = "butonRefresh";
            butonRefresh.Size = new Size(416, 47);
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
            // butonDirtyReads
            // 
            butonDirtyReads.Location = new Point(0, 0);
            butonDirtyReads.Name = "butonDirtyReads";
            butonDirtyReads.Size = new Size(75, 23);
            butonDirtyReads.TabIndex = 0;
            // 
            // butonUnrepeatableReads
            // 
            butonUnrepeatableReads.Location = new Point(0, 0);
            butonUnrepeatableReads.Name = "butonUnrepeatableReads";
            butonUnrepeatableReads.Size = new Size(75, 23);
            butonUnrepeatableReads.TabIndex = 0;
            // 
            // butonPhantomReads
            // 
            butonPhantomReads.Location = new Point(0, 0);
            butonPhantomReads.Name = "butonPhantomReads";
            butonPhantomReads.Size = new Size(75, 23);
            butonPhantomReads.TabIndex = 0;
            // 
            // butonLostUpdate
            // 
            butonLostUpdate.Location = new Point(0, 0);
            butonLostUpdate.Name = "butonLostUpdate";
            butonLostUpdate.Size = new Size(75, 23);
            butonLostUpdate.TabIndex = 0;
            // 
            // dataGridViewAngajati
            // 
            dataGridViewAngajati.ColumnHeadersHeight = 34;
            dataGridViewAngajati.Location = new Point(0, 0);
            dataGridViewAngajati.Name = "dataGridViewAngajati";
            dataGridViewAngajati.RowHeadersWidth = 62;
            dataGridViewAngajati.Size = new Size(240, 150);
            dataGridViewAngajati.TabIndex = 0;
            // 
            // label14
            // 
            label14.Location = new Point(0, 0);
            label14.Name = "label14";
            label14.Size = new Size(100, 23);
            label14.TabIndex = 0;
            // 
            // butonDeadlock
            // 
            butonDeadlock.Location = new Point(0, 0);
            butonDeadlock.Name = "butonDeadlock";
            butonDeadlock.Size = new Size(75, 23);
            butonDeadlock.TabIndex = 0;
            // 
            // butonDeadlockPriority
            // 
            butonDeadlockPriority.Location = new Point(0, 0);
            butonDeadlockPriority.Name = "butonDeadlockPriority";
            butonDeadlockPriority.Size = new Size(75, 23);
            butonDeadlockPriority.TabIndex = 0;
            // 
            // butonDeadlockPrevenire
            // 
            butonDeadlockPrevenire.Location = new Point(0, 0);
            butonDeadlockPrevenire.Name = "butonDeadlockPrevenire";
            butonDeadlockPrevenire.Size = new Size(75, 23);
            butonDeadlockPrevenire.TabIndex = 0;
            // 
            // butonDirtyReadsPrevenire
            // 
            butonDirtyReadsPrevenire.Location = new Point(0, 0);
            butonDirtyReadsPrevenire.Name = "butonDirtyReadsPrevenire";
            butonDirtyReadsPrevenire.Size = new Size(75, 23);
            butonDirtyReadsPrevenire.TabIndex = 0;
            // 
            // butonUnrepeatableReadsPrevenire
            // 
            butonUnrepeatableReadsPrevenire.Location = new Point(0, 0);
            butonUnrepeatableReadsPrevenire.Name = "butonUnrepeatableReadsPrevenire";
            butonUnrepeatableReadsPrevenire.Size = new Size(75, 23);
            butonUnrepeatableReadsPrevenire.TabIndex = 0;
            // 
            // butonPhantomReadsPrevenire
            // 
            butonPhantomReadsPrevenire.Location = new Point(0, 0);
            butonPhantomReadsPrevenire.Name = "butonPhantomReadsPrevenire";
            butonPhantomReadsPrevenire.Size = new Size(75, 23);
            butonPhantomReadsPrevenire.TabIndex = 0;
            // 
            // butonLostUpdatesPrevenire
            // 
            butonLostUpdatesPrevenire.Location = new Point(0, 0);
            butonLostUpdatesPrevenire.Name = "butonLostUpdatesPrevenire";
            butonLostUpdatesPrevenire.Size = new Size(75, 23);
            butonLostUpdatesPrevenire.TabIndex = 0;
            // 
            // buttonTestPool
            // 
            buttonTestPool.BackColor = Color.SpringGreen;
            buttonTestPool.Location = new Point(1188, 768);
            buttonTestPool.Name = "buttonTestPool";
            buttonTestPool.Size = new Size(416, 47);
            buttonTestPool.TabIndex = 28;
            buttonTestPool.Text = "Test With Pool";
            buttonTestPool.UseVisualStyleBackColor = false;
            buttonTestPool.Click += testConnectionWithPool;
            // 
            // buttonTestWithoutPool
            // 
            buttonTestWithoutPool.BackColor = Color.MediumSeaGreen;
            buttonTestWithoutPool.Location = new Point(1188, 837);
            buttonTestWithoutPool.Name = "buttonTestWithoutPool";
            buttonTestWithoutPool.Size = new Size(416, 47);
            buttonTestWithoutPool.TabIndex = 29;
            buttonTestWithoutPool.Text = "Test Without Pool";
            buttonTestWithoutPool.UseVisualStyleBackColor = false;
            buttonTestWithoutPool.Click += testConnectionWithoutPool;
            // 
            // buttonTestWithoutClosingConnections
            // 
            buttonTestWithoutClosingConnections.BackColor = Color.MediumSeaGreen;
            buttonTestWithoutClosingConnections.Location = new Point(1188, 911);
            buttonTestWithoutClosingConnections.Name = "buttonTestWithoutClosingConnections";
            buttonTestWithoutClosingConnections.Size = new Size(416, 47);
            buttonTestWithoutClosingConnections.TabIndex = 30;
            buttonTestWithoutClosingConnections.Text = "Test Without Closing Connections";
            buttonTestWithoutClosingConnections.UseVisualStyleBackColor = false;
            buttonTestWithoutClosingConnections.Click += testWithPoolWrong;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1738, 984);
            Controls.Add(buttonTestWithoutClosingConnections);
            Controls.Add(buttonTestWithoutPool);
            Controls.Add(buttonTestPool);
            Controls.Add(butonRefresh);
            Controls.Add(butonCautare);
            Controls.Add(textDenumireSpectacolCautare);
            Controls.Add(label13);
            Controls.Add(label11);
            Controls.Add(textIdTeatruSpectacol);
            Controls.Add(label10);
            Controls.Add(butonDelete);
            Controls.Add(butonUpdate);
            Controls.Add(butonInsert);
            Controls.Add(textDescriereSpectacol);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(textDenumireSpectacol);
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
            ((System.ComponentModel.ISupportInitialize)dataGridViewAngajati).EndInit();
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
        private TextBox textDenumireSpectacol;
        private Label label8;
        private Label label9;
        private TextBox textDescriereSpectacol;
        private Button butonInsert;
        private Button butonUpdate;
        private Button butonDelete;
        private Label label10;
        private TextBox textIdTeatruSpectacol;
        private Label label11;
        private Label label13;
        private TextBox textDenumireSpectacolCautare;
        private Button butonCautare;
        private Button butonRefresh;
        private Label label12;
        private Button butonDirtyReads;
        private Button butonUnrepeatableReads;
        private Button butonPhantomReads;
        private Button butonLostUpdate;
        private DataGridView dataGridViewAngajati;
        private Label label14;
        private Button butonDeadlock;
        private Button butonDeadlockPriority;
        private Button butonDeadlockPrevenire;
        private Button butonDirtyReadsPrevenire;
        private Button butonUnrepeatableReadsPrevenire;
        private Button butonPhantomReadsPrevenire;
        private Button butonLostUpdatesPrevenire;
        private Button buttonTestPool;
        private Button buttonTestWithoutPool;
        private Button buttonTestWithoutClosingConnections;
    }
}
