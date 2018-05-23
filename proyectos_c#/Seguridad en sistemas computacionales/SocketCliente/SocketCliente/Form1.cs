﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace SocketCliente
{
    public partial class Form1 : Form
    {
        private bool bandera = false;
        string _mensajeChat;
        NetworkStream streamServidor;
        TcpClient cliente;
        private static Hashtable clientes_conectados;
        private String mensajeCliente;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {

                this.Text = string.Format("Cliente: {0}", txtUsuario.Text);

                _mensajeChat = "Conectando al servidor...";
                Mensaje();

                cliente = new TcpClient(txtServidor.Text, int.Parse(txtPuerto.Text));

                streamServidor = cliente.GetStream();

                Byte[] datos = System.Text.Encoding.ASCII.GetBytes(txtUsuario.Text + "$");

                streamServidor.Write(datos, 0, datos.Length);
                streamServidor.Flush();

                Thread ctThread = new Thread(Chat);
                ctThread.Start();

                btnConectar.Enabled = false;

                btnEnviar.Enabled = true;
            }
            catch (Exception ex)
            {
                txtChat.Text = ex.ToString();

                if (MessageBox.Show("¿Conectar de nuevo?") == System.Windows.Forms.DialogResult.Yes)
                    btnConectar_Click(null, null);
                else
                    this.Close();
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                streamServidor = cliente.GetStream();

                Byte[] datos = Encoding.ASCII.GetBytes(txtMensaje.Text + "$" + txtUsuario.Text);

                streamServidor.Write(datos, 0, datos.Length);
                streamServidor.Flush();
            }
            catch (Exception ex)
            {
                txtChat.Text = ex.ToString();
            }
        }
        public void Mensaje()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(Mensaje));
            else
                txtChat.Text = txtChat.Text + Environment.NewLine + " -> " + _mensajeChat;
        }

        private void Chat()
        {
            while (true)
            {
                streamServidor = cliente.GetStream();

                byte[] bytes = new byte[256];

                streamServidor.Read(bytes, 0, bytes.Length);

                _mensajeChat = Encoding.ASCII.GetString(bytes);
                Mensaje();
            }
        }
    }
    
}

/*
 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blowfish
{
    public partial class Form1 : Form
    {
        private ClaseBlowfish b12=null;
        private string cipherText;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            b12= new ClaseBlowfish("04B915BA43FEB5B6");
            string mensajebox = this.textBox1.Text;
            cipherText = b12.Encrypt_CBC(mensajebox);
            this.textBox3.Text = cipherText;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (b12 != null)
            {
                string textobox = b12.Decrypt_CBC(cipherText);
                this.textBox2.Text = textobox;
            }
            else {
                MessageBox.Show("Ingrese una nueva palabra");
            }
            b12 = null;
        }
    }
}

 */
