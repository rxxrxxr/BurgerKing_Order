namespace OrderForm.product_menu
{
    partial class Side
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // button3
            // 
            button3.BackgroundImageLayout = ImageLayout.None;
            button3.Location = new Point(265, 152);
            button3.Name = "button3";
            button3.Size = new Size(113, 67);
            button3.TabIndex = 10;
            button3.Text = "게살스틱";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.BackgroundImageLayout = ImageLayout.None;
            button2.Location = new Point(135, 152);
            button2.Name = "button2";
            button2.Size = new Size(124, 67);
            button2.TabIndex = 11;
            button2.Text = "치즈스틱";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.Location = new Point(6, 152);
            button1.Name = "button1";
            button1.Size = new Size(123, 67);
            button1.TabIndex = 12;
            button1.Text = "감자튀김";
            button1.UseVisualStyleBackColor = true;
            // 
            // side2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(386, 409);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "side2";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button3;
        private Button button2;
        private Button button1;
    }
}