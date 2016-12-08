using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Fomularios;
using System.IO;

namespace Fomularios
{
    public partial class Configuraciones : Form
    {
        private string carpetacliente = "";
        private WSConfiguraciones.WebService ws = new WSConfiguraciones.WebService();

        public Configuraciones()
        {
            InitializeComponent();
            CargarRoles();
            CargarUsuarios();
            this.tabControlAcciones.Visible = true;

        }



        private void Acciones_Load(object sender, EventArgs e)
        {

            switch (tabControlAcciones.SelectedIndex)
            {
                case 0:
                    CargarUsuarios();
                    break;
                case 1:
                    CargarRoles();
                    CargarUsuariosPorRol();
                    break;
                case 2:
                    CargarConexiones();
                    break;
                case 3:
                    CargarRutas();
                    break;
                case 4:
                    CargarClientes();
                    break;
            }

        }

        private void btnSeleccionarNuevaRuta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdb = new FolderBrowserDialog();
            if (fdb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(fdb.SelectedPath))
                    lblRutaRepositorioNueva.Text = fdb.SelectedPath;
            }

        }

        private void btnSeleccionarLicencia_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(ofd.FileName.ToString()))
                    lblRutaNuevaLicencia.Text = ofd.FileName.ToString();
            }
        }

        private void btnSeleccionarOI_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(ofd.FileName.ToString()))
                    lblRutaNuevaOI.Text = ofd.FileName.ToString();
            }
        }



        private void btnSeleccionarCarpetaCliente_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdb = new FolderBrowserDialog();
            if (fdb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(fdb.SelectedPath);
                this.carpetacliente = fdb.SelectedPath;


            }

        }

        private void CargarRoles()
        {
            /*Carga de combos para seleccion de roles*/
                WSConfiguraciones.Roles[] roles = ws.ListRoles();

            this.cmbroles.DataSource = roles;
            this.cmbroles.DisplayMember = "Descripcion";
            this.cmbroles.ValueMember = "IdRol";
            this.cmbroles.SelectedIndex = 0;


            this.cmbrolusuario.DataSource = roles;
            this.cmbrolusuario.DisplayMember = "Descripcion";
            this.cmbrolusuario.ValueMember = "IdRol";
            this.cmbrolusuario.SelectedIndex = 0;

        }

        private void CargarConexiones()
        {
            /*Carga de datos para la pestaña de conexiones*/
            WSConfiguraciones.Configuraciones[] configuraciones = ws.ListConfig();
            txbServidorCliente.Text = configuraciones[4].Valor;
            txbPuerto.Text = configuraciones[3].Valor;
           // chkSSL.Checked = configuraciones[5].Valor;
            cbxAutenticacion.ValueMember = configuraciones[0].Valor;
            txbUsuarioMail.Text = configuraciones[6].Valor;
            txbContraseñaMail.Text = configuraciones[1].Valor;
            txbMensajeMail.Text = "";
        }

        private void CargarRutas()
        {
            /*Carga de datos para la pestaña de rutas*/
            WSConfiguraciones.Configuraciones[] configuraciones = ws.ListConfig();
            lblRutaRepositorio.Text = configuraciones[9].Valor;
            lblRutaOI.Text = configuraciones[8].Valor;
            lblRutaLicencia.Text = configuraciones[7].Valor;
        }

        private void RegistrarUsuario()
        {


            ws.AddUsuario(this.txtnombreusuario.Text, txtloginusuario.Text, txtcontraseñausuario.Text, txtcorreousuario.Text, int.Parse(cmbroles.SelectedValue.ToString()));

            DialogResult res = MessageBox.Show("Usuario se registro con exito!", "Configuracion", MessageBoxButtons.OK);


        }

        private void CargarUsuarios()
        {
            txtnombreusuario.Text = "";
            txtcorreousuario.Text = "";
            txtloginusuario.Text = "";
            txtcontraseñausuario.Text = "";


            this.dataGridViewUsuarios.Enabled = true;

            WSConfiguraciones.Usuario[] usuarios = ws.ListUsuarios();
            this.dataGridViewUsuarios.Rows.Clear();

            foreach (WSConfiguraciones.Usuario usuario in usuarios)
            {
                //Console.WriteLine("numero " + usuario.IdUsuario);
                WSConfiguraciones.Roles[] roles = ws.ListRol(usuario.IdUsuario);

                foreach (WSConfiguraciones.Roles rol in roles)
                {
                    this.dataGridViewUsuarios.Rows.Insert(this.dataGridViewUsuarios.NewRowIndex, usuario.IdUsuario, usuario.Nombre, usuario.Login, rol.Descripcion, usuario.Correo);

                }


            }

        }


        private void CargarClientes()
        {
            this.dataGridViewUsuarios.Enabled = true;

            WSConfiguraciones.Clientes[] clientes = ws.ListClientes();
            this.dataGridViewregistrados.Rows.Clear();

            foreach (WSConfiguraciones.Clientes cliente in clientes)
            {

                this.dataGridViewregistrados.Rows.Insert(this.dataGridViewregistrados.NewRowIndex, cliente.Nombre, cliente.Carpeta);


            }

        }

        private void CargarUsuariosPorRol()
        {
            this.dataGridViewUsuarios.Enabled = true;

            WSConfiguraciones.Usuario[] usuarios = ws.ListUsuarios();
            this.lbxusuariosasignados.Items.Clear();
            this.lbxusuariosinrol.Items.Clear();

            foreach (WSConfiguraciones.Usuario usuario in usuarios)
            {
                Console.WriteLine("numero " + usuario.IdUsuario);
                WSConfiguraciones.Roles[] roles = ws.ListRol(usuario.IdUsuario);


                foreach (WSConfiguraciones.Roles rol in roles)
                {

                    Console.WriteLine("this.cmbroles.SelectedValue.ToString() " + this.cmbroles.SelectedValue.ToString() + "rol " + rol.IdRol);
                    if (this.cmbroles.SelectedValue.ToString().Equals(rol.IdRol.ToString()))
                    {
                        Console.WriteLine("numeroa " + usuario.IdUsuario);
                        this.lbxusuariosasignados.Items.Add(usuario);
                        this.lbxusuariosasignados.DisplayMember = "Nombre";
                        this.lbxusuariosasignados.ValueMember = "IdUsuario";
                    }

                }


                if (roles.Length == 0)
                {
                    Console.WriteLine("numerob " + usuario.IdUsuario);
                    this.lbxusuariosinrol.Items.Add(usuario);
                    this.lbxusuariosinrol.DisplayMember = "Nombre";
                    this.lbxusuariosinrol.ValueMember = "IdUsuario";
                }

            }


        }


        private void AsignarRolUsuario()
        {


            int idrol = int.Parse(this.cmbroles.SelectedValue.ToString());

            if (this.lbxusuariosinrol.SelectedItem != null)
            {

                WSConfiguraciones.Usuario usuario = (WSConfiguraciones.Usuario)this.lbxusuariosinrol.SelectedItem;
                int idusuario = usuario.IdUsuario;
                Console.WriteLine("id usuario " + usuario.IdUsuario + " idrol " + idrol);

                ws.AddRol(idrol, idusuario);

                this.lbxusuariosinrol.Items.Remove(usuario);
                this.lbxusuariosasignados.Items.Add(usuario);
                this.lbxusuariosasignados.DisplayMember = "Nombre";
                this.lbxusuariosasignados.ValueMember = "IdUsuario";


                DialogResult res = MessageBox.Show("Se asigno rol al usuario exito!", "Configuracion", MessageBoxButtons.OK);
            }
            else {
                DialogResult res = MessageBox.Show("Seleccione un usuario!", "Configuracion", MessageBoxButtons.OK);
            }


        }

        private void RemoverRolUsuario()
        {


            int idrol = int.Parse(this.cmbroles.SelectedValue.ToString());

            if (this.lbxusuariosasignados.SelectedItem!= null)
            {
                WSConfiguraciones.Usuario usuario = (WSConfiguraciones.Usuario)this.lbxusuariosasignados.SelectedItem;
                int idusuario = usuario.IdUsuario;
                Console.WriteLine("id usuario " + usuario.IdUsuario + " idrol " + idrol);

                ws.RemoveRol(idrol, idusuario);

                this.lbxusuariosasignados.Items.Remove(usuario);
                this.lbxusuariosinrol.Items.Add(usuario);
                this.lbxusuariosinrol.DisplayMember = "Nombre";
                this.lbxusuariosinrol.ValueMember = "IdUsuario";



                DialogResult res = MessageBox.Show("Seleccione un usuario!", "Configuracion", MessageBoxButtons.OK);
            }
            else {
                DialogResult res = MessageBox.Show("Seleccione un usuario!", "Configuracion", MessageBoxButtons.OK);
            }


        }

        private void RegistrarCliente()
        {


            if (this.txtnombrecliente.Text.Equals(String.Empty) || this.carpetacliente.Equals(String.Empty))
            {
                DialogResult res = MessageBox.Show("Los campos no pueden estar vacios,verifique por por favor!", "Configuracion", MessageBoxButtons.OK);
            }
            else
            {
                ws.AddClientes(this.txtnombrecliente.Text, this.carpetacliente);


                DialogResult res = MessageBox.Show("Se registro el cliente con exito!", "Configuracion", MessageBoxButtons.OK);
            }

        }



        
     
        private void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            RegistrarUsuario();
            CargarUsuarios();

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Desea cerrar el programa ?", "Configuracion", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
                this.Close();
        }

        private void cmbroles_SelectedIndexChanged(object sender, EventArgs e)
        {

            CargarUsuariosPorRol();


        }

        private void btnasignarrol_Click(object sender, EventArgs e)
        {
            AsignarRolUsuario();
        }


        private void btnconfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                RegistrarCliente();
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnConfirmarNuevaRuta_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(lblRutaRepositorioNueva.Text))
                    MessageBox.Show("Seleccione una ruta antes de continuar.");
                else
                {

                    String clave = @"Ruta\|Repositorio";
                    ws.UpdateConfig(clave, lblRutaRepositorioNueva.Text);
                    MessageBox.Show("Actualización exitosa.");
                    lblRutaRepositorioNueva.Text = "";
                    CargarRutas();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConfirmarLicencia_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(lblRutaNuevaLicencia.Text))
                    MessageBox.Show("Seleccione un archivo antes de continuar.");
                else
                {
                    if (String.IsNullOrEmpty(lblRutaRepositorio.Text))
                    {
                        MessageBox.Show("Por favor, indicar la ruta del repositorio.");
                    }
                    else
                    {
                        String clave = @"Ruta\|Licencia";
                        //CargarRutas();   Hay que mejorarlo porque esta chancho
                        string direccionOrigen = lblRutaNuevaLicencia.Text;
                        string direccionDestino = String.Format("{0}{1}{2}", lblRutaRepositorio.Text, @"\", lblRutaNuevaLicencia.Text.Substring(lblRutaNuevaLicencia.Text.LastIndexOf(@"\")).Replace(@"\", ""));
                        ws.UpdateConfig(clave, direccionDestino);
                        File.Copy(direccionOrigen, direccionDestino, true);
                        MessageBox.Show("La licencia por defecto fue actualizada.");
                        lblRutaNuevaLicencia.Text = "";
                        CargarRutas();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConfirmarOI_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(lblRutaNuevaOI.Text))
                    MessageBox.Show("Seleccione una archivo antes de continuar.");
                else
                {
                    if (String.IsNullOrEmpty(lblRutaRepositorio.Text))
                    {
                        MessageBox.Show("Por favor, indicar la ruta del repositorio.");
                    }
                    else
                    {
                        String clave = @"Ruta\|OI";


                        string direccionOrigen = lblRutaNuevaOI.Text;
                        string direccionDestino = String.Format("{0}{1}{2}", lblRutaRepositorio.Text, @"\", lblRutaNuevaOI.Text.Substring(lblRutaNuevaOI.Text.LastIndexOf(@"\")).Replace(@"\", ""));
                        ws.UpdateConfig(clave, direccionDestino);
                        File.Copy(direccionOrigen, direccionDestino, true);
                        MessageBox.Show("La licencia por defecto fue actualizada.");
                        lblRutaNuevaOI.Text = "";
                        CargarRutas();


                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGuardarMail_Click(object sender, EventArgs e)
        {

        }

        private void btnquitarrol_Click(object sender, EventArgs e)
        {
            RemoverRolUsuario();
        }

        private void btnGuardarMail_Click_1(object sender, EventArgs e)
        {
            try {
                ws.UpdateConfig(@"Mail\|ServidorMail", txbServidorCliente.Text);
                ws.UpdateConfig(@"Mail\|Puerto", txbPuerto.Text);
                ws.UpdateConfig(@"Mail\|SSL", chkSSL.Checked.ToString());
                ws.UpdateConfig(@"Mail\|Autenticacion", cbxAutenticacion.SelectedText);
                ws.UpdateConfig(@"Mail\|Usuario", txbUsuarioMail.Text);
                ws.UpdateConfig(@"Mail\|Contraseña", txbContraseñaMail.Text);


            } catch (Exception exGuardarMail) {
                MessageBox.Show(exGuardarMail.Message);
                txbMensajeMail.Text = "Guardado";
            }
        }

        private void btnProbarMail_Click(object sender, EventArgs e)
        {
            //Si la comprobacion contra servidor de correo es ok ejecuta lo siguiente, en caso contrario carga el error
            txbMensajeMail.Text = "ok";
        }
    }
}
