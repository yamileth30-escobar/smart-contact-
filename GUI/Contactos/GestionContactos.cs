using AgendaContactos.BLL; 
using AgendaContactos.EL; 
using System;
using System.Data;
using System.Windows.Forms;

namespace AgendaContactos.GUI.Contactos
{
    public partial class GestionContactos : Form
    {
        // Usamos la BLL, no la conexión directa
        private readonly ContactoBLL contactoBll = new ContactoBLL();

        public GestionContactos()
        {
            InitializeComponent();
            CargarDatos();
        }

        // --- 1. CARGAR DATOS USANDO BLL ---
        private void CargarDatos()
        {
            try
            {
                // El DataGridView se llena con lo que devuelve la BLL
                dvListadoClientes.DataSource = contactoBll.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        // --- 2. GUARDAR USANDO BLL Y ENTIDADES ---
        private void GuardarContacto()
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Por favor, completa Nombre y Apellido.");
                return;
            }

            try
            {
                // Llenamos la entidad con lo que hay en los TextBox
                ContactoEL contacto = new ContactoEL();
                contacto.Nombre = txtID.Text;    // Si txtID es para el nombre
                contacto.Apellido = txtNombre.Text; // Si txtNombre es para el apellido
                contacto.Correo = txtCorreo.Text;
                contacto.Telefono = txtTelefono.Text;

                // Estos valores deben venir de tu lógica de sesión o combos
                contacto.CategoriaId = 1;
                contacto.UsuarioId = 1;

                // Enviamos a la BLL
                contactoBll.Guardar(contacto);

                MessageBox.Show("¡Contacto guardado con éxito!");
                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        // --- 3. BOTONES ---

        private void button2_Click(object sender, EventArgs e) // Botón Guardar
        {
            GuardarContacto();
        }

        private void button1_Click(object sender, EventArgs e) // Botón Eliminar
        {
            if (dvListadoClientes.CurrentRow != null)
            {
                // Obtenemos el ID de la fila seleccionada
                int id = Convert.ToInt32(dvListadoClientes.CurrentRow.Cells["Id"].Value);

                if (MessageBox.Show("¿Eliminar?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    contactoBll.Eliminar(id);
                    CargarDatos();
                }
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                dvListadoClientes.DataSource = contactoBll.Buscar(textBox2.Text);
            }
            else
            {
                CargarDatos();
            }
        }

        private void LimpiarCampos()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
            txtID.Focus();
        }
    }
}