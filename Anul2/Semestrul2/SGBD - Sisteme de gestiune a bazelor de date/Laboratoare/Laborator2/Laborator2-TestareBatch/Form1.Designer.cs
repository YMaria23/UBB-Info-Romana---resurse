namespace Laborator2_TestareBatch
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
            butonRuleaza = new Button();
            label1 = new Label();
            panelChart = new Panel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // butonRuleaza
            // 
            butonRuleaza.BackColor = Color.MediumSeaGreen;
            butonRuleaza.Font = new Font("Segoe UI", 12F);
            butonRuleaza.Location = new Point(37, 127);
            butonRuleaza.Name = "butonRuleaza";
            butonRuleaza.Size = new Size(128, 46);
            butonRuleaza.TabIndex = 0;
            butonRuleaza.Text = "Ruleaza Test";
            butonRuleaza.UseVisualStyleBackColor = false;
            butonRuleaza.Click += butonRuleaza_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(239, 28);
            label1.Name = "label1";
            label1.Size = new Size(296, 41);
            label1.TabIndex = 1;
            label1.Text = "TEST PERFORMANTA";
            // 
            // panelChart
            // 
            panelChart.Location = new Point(460, 101);
            panelChart.Name = "panelChart";
            panelChart.Size = new Size(459, 318);
            panelChart.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(37, 191);
            label2.Name = "label2";
            label2.RightToLeft = RightToLeft.Yes;
            label2.Size = new Size(316, 28);
            label2.TabIndex = 3;
            label2.Text = "Dupa apasarea butonului, asteptati";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(37, 219);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.Yes;
            label3.Size = new Size(333, 28);
            label3.TabIndex = 4;
            label3.Text = "aprox 15 - 20 secunde pentru rularea";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(37, 247);
            label4.Name = "label4";
            label4.RightToLeft = RightToLeft.Yes;
            label4.Size = new Size(219, 28);
            label4.TabIndex = 5;
            label4.Text = "testelor de performanta";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(37, 319);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(360, 339);
            dataGridView1.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MintCream;
            ClientSize = new Size(1003, 741);
            Controls.Add(dataGridView1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(panelChart);
            Controls.Add(label1);
            Controls.Add(butonRuleaza);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button butonRuleaza;
        private Label label1;
        private Panel panelChart;
        private Label label2;
        private Label label3;
        private Label label4;
        private DataGridView dataGridView1;
    }
}
