namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnTstart = new System.Windows.Forms.Button();
            this.cbAngle = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLaser = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelAngle = new System.Windows.Forms.Label();
            this.labelLaser = new System.Windows.Forms.Label();
            this.labelHight = new System.Windows.Forms.Label();
            this.btnTstop = new System.Windows.Forms.Button();
            this.labelSec = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelTemp = new System.Windows.Forms.Label();
            this.btnBladeStart = new System.Windows.Forms.Button();
            this.btnBladeStop = new System.Windows.Forms.Button();
            this.labelRPM = new System.Windows.Forms.Label();
            this.textBoxTA = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTH = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxBA = new System.Windows.Forms.TextBox();
            this.textBoxBH = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCalc = new System.Windows.Forms.Button();
            this.labelB3 = new System.Windows.Forms.Label();
            this.labelB2 = new System.Windows.Forms.Label();
            this.labelB1 = new System.Windows.Forms.Label();
            this.labelB1W = new System.Windows.Forms.Label();
            this.labelB2W = new System.Windows.Forms.Label();
            this.labelB3W = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTstart
            // 
            this.btnTstart.Location = new System.Drawing.Point(320, 10);
            this.btnTstart.Name = "btnTstart";
            this.btnTstart.Size = new System.Drawing.Size(102, 23);
            this.btnTstart.TabIndex = 0;
            this.btnTstart.Text = "Tower - Start";
            this.btnTstart.UseVisualStyleBackColor = true;
            this.btnTstart.Click += new System.EventHandler(this.btnTstart_Click);
            // 
            // cbAngle
            // 
            this.cbAngle.FormattingEnabled = true;
            this.cbAngle.Location = new System.Drawing.Point(73, 12);
            this.cbAngle.Name = "cbAngle";
            this.cbAngle.Size = new System.Drawing.Size(72, 20);
            this.cbAngle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Angle：";
            // 
            // cbLaser
            // 
            this.cbLaser.FormattingEnabled = true;
            this.cbLaser.Location = new System.Drawing.Point(223, 12);
            this.cbLaser.Name = "cbLaser";
            this.cbLaser.Size = new System.Drawing.Size(72, 20);
            this.cbLaser.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Laser：";
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelAngle.Location = new System.Drawing.Point(254, 50);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.Size = new System.Drawing.Size(70, 24);
            this.labelAngle.TabIndex = 3;
            this.labelAngle.Text = "alpha";
            // 
            // labelLaser
            // 
            this.labelLaser.AutoSize = true;
            this.labelLaser.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLaser.Location = new System.Drawing.Point(344, 50);
            this.labelLaser.Name = "labelLaser";
            this.labelLaser.Size = new System.Drawing.Size(94, 24);
            this.labelLaser.TabIndex = 3;
            this.labelLaser.Text = "100.000";
            // 
            // labelHight
            // 
            this.labelHight.AutoSize = true;
            this.labelHight.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelHight.Location = new System.Drawing.Point(463, 50);
            this.labelHight.Name = "labelHight";
            this.labelHight.Size = new System.Drawing.Size(94, 24);
            this.labelHight.TabIndex = 3;
            this.labelHight.Text = "100.000";
            // 
            // btnTstop
            // 
            this.btnTstop.Location = new System.Drawing.Point(455, 9);
            this.btnTstop.Name = "btnTstop";
            this.btnTstop.Size = new System.Drawing.Size(102, 23);
            this.btnTstop.TabIndex = 0;
            this.btnTstop.Text = "Tower - Stop";
            this.btnTstop.UseVisualStyleBackColor = true;
            this.btnTstop.Click += new System.EventHandler(this.btnTstop_Click);
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSec.Location = new System.Drawing.Point(159, 50);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(58, 24);
            this.labelSec.TabIndex = 3;
            this.labelSec.Text = "Time";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelTemp
            // 
            this.labelTemp.AutoSize = true;
            this.labelTemp.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTemp.Location = new System.Drawing.Point(12, 50);
            this.labelTemp.Name = "labelTemp";
            this.labelTemp.Size = new System.Drawing.Size(34, 24);
            this.labelTemp.TabIndex = 3;
            this.labelTemp.Text = "TE";
            // 
            // btnBladeStart
            // 
            this.btnBladeStart.Location = new System.Drawing.Point(320, 97);
            this.btnBladeStart.Name = "btnBladeStart";
            this.btnBladeStart.Size = new System.Drawing.Size(102, 23);
            this.btnBladeStart.TabIndex = 4;
            this.btnBladeStart.Text = "Blade - Start";
            this.btnBladeStart.UseVisualStyleBackColor = true;
            this.btnBladeStart.Click += new System.EventHandler(this.btnBladeStart_Click);
            // 
            // btnBladeStop
            // 
            this.btnBladeStop.Location = new System.Drawing.Point(455, 97);
            this.btnBladeStop.Name = "btnBladeStop";
            this.btnBladeStop.Size = new System.Drawing.Size(102, 23);
            this.btnBladeStop.TabIndex = 4;
            this.btnBladeStop.Text = "Blade - Stop";
            this.btnBladeStop.UseVisualStyleBackColor = true;
            this.btnBladeStop.Click += new System.EventHandler(this.btnBladeStop_Click);
            // 
            // labelRPM
            // 
            this.labelRPM.AutoSize = true;
            this.labelRPM.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelRPM.Location = new System.Drawing.Point(18, 96);
            this.labelRPM.Name = "labelRPM";
            this.labelRPM.Size = new System.Drawing.Size(46, 24);
            this.labelRPM.TabIndex = 3;
            this.labelRPM.Text = "RPM";
            // 
            // textBoxTA
            // 
            this.textBoxTA.Location = new System.Drawing.Point(53, 20);
            this.textBoxTA.Name = "textBoxTA";
            this.textBoxTA.Size = new System.Drawing.Size(72, 21);
            this.textBoxTA.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "TA：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "TH：";
            // 
            // textBoxTH
            // 
            this.textBoxTH.Location = new System.Drawing.Point(179, 20);
            this.textBoxTH.Name = "textBoxTH";
            this.textBoxTH.Size = new System.Drawing.Size(72, 21);
            this.textBoxTH.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(278, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "BA：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(404, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "BH：";
            // 
            // textBoxBA
            // 
            this.textBoxBA.Location = new System.Drawing.Point(313, 20);
            this.textBoxBA.Name = "textBoxBA";
            this.textBoxBA.Size = new System.Drawing.Size(72, 21);
            this.textBoxBA.TabIndex = 5;
            // 
            // textBoxBH
            // 
            this.textBoxBH.Location = new System.Drawing.Point(439, 20);
            this.textBoxBH.Name = "textBoxBH";
            this.textBoxBH.Size = new System.Drawing.Size(72, 21);
            this.textBoxBH.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCalc);
            this.groupBox1.Controls.Add(this.textBoxBA);
            this.groupBox1.Controls.Add(this.textBoxBH);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxTH);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxTA);
            this.groupBox1.Controls.Add(this.labelB3W);
            this.groupBox1.Controls.Add(this.labelB2W);
            this.groupBox1.Controls.Add(this.labelB3);
            this.groupBox1.Controls.Add(this.labelB1W);
            this.groupBox1.Controls.Add(this.labelB2);
            this.groupBox1.Controls.Add(this.labelB1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(22, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 200);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // buttonCalc
            // 
            this.buttonCalc.Location = new System.Drawing.Point(433, 156);
            this.buttonCalc.Name = "buttonCalc";
            this.buttonCalc.Size = new System.Drawing.Size(75, 23);
            this.buttonCalc.TabIndex = 6;
            this.buttonCalc.Text = "Calculate";
            this.buttonCalc.UseVisualStyleBackColor = true;
            this.buttonCalc.Click += new System.EventHandler(this.buttonCalc_Click);
            // 
            // labelB3
            // 
            this.labelB3.AutoSize = true;
            this.labelB3.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelB3.Location = new System.Drawing.Point(369, 69);
            this.labelB3.Name = "labelB3";
            this.labelB3.Size = new System.Drawing.Size(106, 24);
            this.labelB3.TabIndex = 3;
            this.labelB3.Text = "B3_alpha";
            // 
            // labelB2
            // 
            this.labelB2.AutoSize = true;
            this.labelB2.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelB2.Location = new System.Drawing.Point(210, 69);
            this.labelB2.Name = "labelB2";
            this.labelB2.Size = new System.Drawing.Size(106, 24);
            this.labelB2.TabIndex = 3;
            this.labelB2.Text = "B2_alpha";
            // 
            // labelB1
            // 
            this.labelB1.AutoSize = true;
            this.labelB1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelB1.Location = new System.Drawing.Point(51, 69);
            this.labelB1.Name = "labelB1";
            this.labelB1.Size = new System.Drawing.Size(106, 24);
            this.labelB1.TabIndex = 3;
            this.labelB1.Text = "B1_alpha";
            // 
            // labelB1W
            // 
            this.labelB1W.AutoSize = true;
            this.labelB1W.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelB1W.Location = new System.Drawing.Point(51, 113);
            this.labelB1W.Name = "labelB1W";
            this.labelB1W.Size = new System.Drawing.Size(106, 24);
            this.labelB1W.TabIndex = 3;
            this.labelB1W.Text = "B1_Width";
            // 
            // labelB2W
            // 
            this.labelB2W.AutoSize = true;
            this.labelB2W.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelB2W.Location = new System.Drawing.Point(210, 113);
            this.labelB2W.Name = "labelB2W";
            this.labelB2W.Size = new System.Drawing.Size(106, 24);
            this.labelB2W.TabIndex = 3;
            this.labelB2W.Text = "B2_Width";
            // 
            // labelB3W
            // 
            this.labelB3W.AutoSize = true;
            this.labelB3W.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelB3W.Location = new System.Drawing.Point(369, 113);
            this.labelB3W.Name = "labelB3W";
            this.labelB3W.Size = new System.Drawing.Size(106, 24);
            this.labelB3W.TabIndex = 3;
            this.labelB3W.Text = "B3_Width";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 352);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBladeStop);
            this.Controls.Add(this.btnBladeStart);
            this.Controls.Add(this.labelHight);
            this.Controls.Add(this.labelLaser);
            this.Controls.Add(this.labelTemp);
            this.Controls.Add(this.labelRPM);
            this.Controls.Add(this.labelSec);
            this.Controls.Add(this.labelAngle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLaser);
            this.Controls.Add(this.cbAngle);
            this.Controls.Add(this.btnTstop);
            this.Controls.Add(this.btnTstart);
            this.Name = "Form1";
            this.Text = "Alpha Meas Blade Pitch Meas V1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTstart;
        private System.Windows.Forms.ComboBox cbAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLaser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelAngle;
        private System.Windows.Forms.Label labelLaser;
        private System.Windows.Forms.Label labelHight;
        private System.Windows.Forms.Button btnTstop;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelTemp;
        private System.Windows.Forms.Button btnBladeStart;
        private System.Windows.Forms.Button btnBladeStop;
        private System.Windows.Forms.Label labelRPM;
        private System.Windows.Forms.TextBox textBoxTA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxBA;
        private System.Windows.Forms.TextBox textBoxBH;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonCalc;
        private System.Windows.Forms.Label labelB3;
        private System.Windows.Forms.Label labelB2;
        private System.Windows.Forms.Label labelB1;
        private System.Windows.Forms.Label labelB3W;
        private System.Windows.Forms.Label labelB2W;
        private System.Windows.Forms.Label labelB1W;
    }
}

