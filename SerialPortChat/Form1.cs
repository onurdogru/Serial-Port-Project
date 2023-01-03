using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPortChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var seriPort in SerialPort.GetPortNames())
            {
                comboBoxPort.Items.Add(seriPort);
            }
            comboBoxPort.SelectedIndex = 0; //sıralı dizilim istersek
            buttonDisconnect.Enabled = false;
        }



        private void buttonConnect_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBoxPort.Text; //hani combobox verisi seçilirse, o seri port'a bağla
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = Parity.Even;
            serialPort1.StopBits = StopBits.One;
            serialPort1.DataBits = 8;

            try 
            {
                serialPort1.Open();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"Seri Port bağlantısı yapılamadı \n Hata: {ex.Message}","Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("Lütfen Düzeltin", "Hata Oldu");
            }

            if (serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = false;
                this.buttonConnect.BackColor = Color.Red;
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Write(textBoxSend.Text);
                textBoxSend.Clear();
            }
        }

        public delegate void veriGoster(String s); //karşılıklı iletişim.

        public void textBoxYaz(String s)
        {
            textBoxReceived.Text += s;
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string gelenVeri = serialPort1.ReadExisting(); //bu kod satırı örneğin Merhaba yazdık, Merhabayı string olarak alır.
            textBoxReceived.Invoke(new veriGoster(textBoxYaz), gelenVeri); //?
        }
    }
}
