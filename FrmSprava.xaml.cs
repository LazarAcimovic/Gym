using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace WPFTeretana
{
    /// <summary>
    /// Interaction logic for FrmSprava.xaml
    /// </summary>
    public partial class FrmSprava : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private bool azuriraj;
        private DataRowView red;

        public FrmSprava()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
        }

        public FrmSprava(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtNazivSprave.Focus();
            this.azuriraj = azuriraj;
            this.red = red;
            konekcija = kon.KreirajKonekciju();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija = kon.KreirajKonekciju();
                konekcija.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };

                cmd.Parameters.Add("@nazivSprave", SqlDbType.NVarChar).Value = txtNazivSprave.Text;
                if(azuriraj)
                {
                    DataRowView pomocniRed = red;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = pomocniRed["ID"];
                    cmd.CommandText = @"update tblSprava
                                set nazivSprave=@nazivSprave where spravaID=@id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblSprava(nazivSprave)
                                       values(@nazivSprave)";
                }
               
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Close();
            }
            catch
            {
                MessageBox.Show("Unos podataka nije validan", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
