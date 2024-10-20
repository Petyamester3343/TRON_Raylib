namespace TRON_RayLib
{
    partial class MainMenuForm
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
            VS_AI_BTN = new Button();
            VS_LOCAL_BTN = new Button();
            VS_ONLINE_BTN = new Button();
            EXIT_BTN = new Button();
            TITLE = new MaskedTextBox();
            SuspendLayout();
            // 
            // VS_AI_BTN
            // 
            VS_AI_BTN.BackColor = Color.Lime;
            VS_AI_BTN.Cursor = Cursors.Hand;
            VS_AI_BTN.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            VS_AI_BTN.Location = new Point(220, 179);
            VS_AI_BTN.Name = "VS_AI_BTN";
            VS_AI_BTN.Size = new Size(183, 23);
            VS_AI_BTN.TabIndex = 1;
            VS_AI_BTN.Text = "VERSUS AI";
            VS_AI_BTN.UseVisualStyleBackColor = false;
            VS_AI_BTN.Click += VS_AI_BTN_Click;
            // 
            // VS_LOCAL_BTN
            // 
            VS_LOCAL_BTN.BackColor = Color.FromArgb(255, 128, 0);
            VS_LOCAL_BTN.Cursor = Cursors.Hand;
            VS_LOCAL_BTN.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            VS_LOCAL_BTN.Location = new Point(220, 227);
            VS_LOCAL_BTN.Name = "VS_LOCAL_BTN";
            VS_LOCAL_BTN.Size = new Size(183, 23);
            VS_LOCAL_BTN.TabIndex = 2;
            VS_LOCAL_BTN.Text = "VERSUS LOCAL";
            VS_LOCAL_BTN.UseVisualStyleBackColor = false;
            VS_LOCAL_BTN.Click += VS_LOCAL_BTN_Click;
            // 
            // VS_ONLINE_BTN
            // 
            VS_ONLINE_BTN.BackColor = SystemColors.HotTrack;
            VS_ONLINE_BTN.Cursor = Cursors.Hand;
            VS_ONLINE_BTN.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            VS_ONLINE_BTN.Location = new Point(220, 277);
            VS_ONLINE_BTN.Name = "VS_ONLINE_BTN";
            VS_ONLINE_BTN.Size = new Size(183, 23);
            VS_ONLINE_BTN.TabIndex = 3;
            VS_ONLINE_BTN.Text = "VERSUS PEER 2 PEER CLIENT";
            VS_ONLINE_BTN.UseVisualStyleBackColor = false;
            VS_ONLINE_BTN.Click += VS_ONLINE_BTN_Click;
            // 
            // EXIT_BTN
            // 
            EXIT_BTN.BackColor = Color.Red;
            EXIT_BTN.Cursor = Cursors.Hand;
            EXIT_BTN.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            EXIT_BTN.Location = new Point(220, 406);
            EXIT_BTN.Name = "EXIT_BTN";
            EXIT_BTN.Size = new Size(183, 23);
            EXIT_BTN.TabIndex = 4;
            EXIT_BTN.Text = "EXIT GAME";
            EXIT_BTN.UseVisualStyleBackColor = false;
            EXIT_BTN.Click += EXIT_BTN_Click;
            // 
            // TITLE
            // 
            TITLE.AllowPromptAsInput = false;
            TITLE.BackColor = SystemColors.HotTrack;
            TITLE.BorderStyle = BorderStyle.None;
            TITLE.Font = new Font("Segoe UI", 48F, FontStyle.Bold);
            TITLE.ForeColor = Color.FromArgb(255, 128, 0);
            TITLE.Location = new Point(12, 12);
            TITLE.Name = "TITLE";
            TITLE.Size = new Size(600, 86);
            TITLE.TabIndex = 5;
            TITLE.Text = "PHANTASMATRON";
            TITLE.TextAlign = HorizontalAlignment.Center;
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(624, 441);
            Controls.Add(TITLE);
            Controls.Add(EXIT_BTN);
            Controls.Add(VS_ONLINE_BTN);
            Controls.Add(VS_LOCAL_BTN);
            Controls.Add(VS_AI_BTN);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainMenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PHANTASMATRON_LAUNCHER";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button VS_AI_BTN;
        private Button VS_LOCAL_BTN;
        private Button VS_ONLINE_BTN;
        private Button EXIT_BTN;
        private MaskedTextBox TITLE;
    }
}
