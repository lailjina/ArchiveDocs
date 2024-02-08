namespace ArchiveDocs
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
            button1 = new Button();
            groupBox1 = new GroupBox();
            button2 = new Button();
            txtPath = new TextBox();
            button3 = new Button();
            groupBox2 = new GroupBox();
            listBox1 = new ListBox();
            menuStrip1 = new MenuStrip();
            поискДокументаToolStripMenuItem = new ToolStripMenuItem();
            изменениеСтатусаОбъектовToolStripMenuItem = new ToolStripMenuItem();
            удалениеИстекшихДокументовToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            comboBox1 = new ComboBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 194);
            button1.Name = "button1";
            button1.Size = new Size(269, 57);
            button1.TabIndex = 0;
            button1.Text = "Генерация тестовых данных";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(txtPath);
            groupBox1.Location = new Point(12, 47);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(752, 78);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Директория с тестовыми файлами";
            // 
            // button2
            // 
            button2.Location = new Point(706, 26);
            button2.Name = "button2";
            button2.Size = new Size(38, 29);
            button2.TabIndex = 1;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // txtPath
            // 
            txtPath.Location = new Point(16, 26);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(684, 27);
            txtPath.TabIndex = 0;
            // 
            // button3
            // 
            button3.Location = new Point(487, 194);
            button3.Name = "button3";
            button3.Size = new Size(269, 57);
            button3.TabIndex = 2;
            button3.Text = "Загрузка в БД";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(listBox1);
            groupBox2.Dock = DockStyle.Bottom;
            groupBox2.Location = new Point(0, 257);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(778, 193);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Лог загрузки в БД";
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(3, 23);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(772, 167);
            listBox1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { поискДокументаToolStripMenuItem, изменениеСтатусаОбъектовToolStripMenuItem, удалениеИстекшихДокументовToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(778, 28);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // поискДокументаToolStripMenuItem
            // 
            поискДокументаToolStripMenuItem.Name = "поискДокументаToolStripMenuItem";
            поискДокументаToolStripMenuItem.Size = new Size(143, 24);
            поискДокументаToolStripMenuItem.Text = "Поиск документа";
            поискДокументаToolStripMenuItem.Click += поискДокументаToolStripMenuItem_Click;
            // 
            // изменениеСтатусаОбъектовToolStripMenuItem
            // 
            изменениеСтатусаОбъектовToolStripMenuItem.Name = "изменениеСтатусаОбъектовToolStripMenuItem";
            изменениеСтатусаОбъектовToolStripMenuItem.Size = new Size(225, 24);
            изменениеСтатусаОбъектовToolStripMenuItem.Text = "Изменение статуса объектов";
            изменениеСтатусаОбъектовToolStripMenuItem.Click += изменениеСтатусаОбъектовToolStripMenuItem_Click;
            // 
            // удалениеИстекшихДокументовToolStripMenuItem
            // 
            удалениеИстекшихДокументовToolStripMenuItem.Name = "удалениеИстекшихДокументовToolStripMenuItem";
            удалениеИстекшихДокументовToolStripMenuItem.Size = new Size(245, 24);
            удалениеИстекшихДокументовToolStripMenuItem.Text = "Удаление истекших документов";
            удалениеИстекшихДокументовToolStripMenuItem.Click += удалениеИстекшихДокументовToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 128);
            label1.Name = "label1";
            label1.Size = new Size(59, 20);
            label1.TabIndex = 5;
            label1.Text = "Объект";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(28, 151);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(728, 28);
            comboBox1.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 450);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(groupBox2);
            Controls.Add(button3);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Архивные документы";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private GroupBox groupBox1;
        private Button button2;
        private TextBox txtPath;
        private Button button3;
        private GroupBox groupBox2;
        private ListBox listBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem поискДокументаToolStripMenuItem;
        private ToolStripMenuItem изменениеСтатусаОбъектовToolStripMenuItem;
        private ToolStripMenuItem удалениеИстекшихДокументовToolStripMenuItem;
        private Label label1;
        private ComboBox comboBox1;
    }
}