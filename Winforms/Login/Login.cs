using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fomularios;
using System.Configuration;


namespace Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (validarUsuario(sender)) {
                if (validarContraseña(sender)) {
                    String connString = ConfigurationManager.ConnectionStrings["Connection_String"].ConnectionString;
                    try {
                        WSConfiguraciones1.WebService ws = new WSConfiguraciones1.WebService();
                        String error = ws.LoguearUsuarioAdmin(txbUsuario.Text, txbContraseña.Text);
                        if (error == "No")
                        {
                            Fomularios.Configuraciones ventana = new Fomularios.Configuraciones();
                            this.Visible = false;
                            ventana.Show();
                        }
                        else {
                            MessageBox.Show(error);
                        }
                        
                    }
                    catch (Exception exLogueo) {
                        MessageBox.Show(exLogueo.Message);
                    }



                }
            }

        }
        private bool validarUsuario(Object sender) {
            bool retorno = false;
            if (String.IsNullOrEmpty(txbUsuario.Text))
            {
                errorProvider1.SetError(txbUsuario, "El usuario no puede ser vacío.");
            }
            else
            {
                errorProvider1.SetError(txbUsuario, String.Empty);
                retorno = true;
            }
            return retorno;
        }
        private bool validarContraseña(Object sender)
        {
            bool retorno = false;
            if (String.IsNullOrEmpty(txbContraseña.Text))
                errorProvider1.SetError(txbContraseña, "La contraseña no puede ser vacía.");
            else {
                errorProvider1.SetError(txbUsuario, String.Empty);
                retorno = true;
            }
            return retorno;
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
