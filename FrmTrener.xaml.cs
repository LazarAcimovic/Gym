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
    /// Interaction logic for FrmTrener.xaml
    /// </summary>
    public partial class FrmTrener : Window
    {

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        public FrmTrener()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
        }

        public FrmTrener(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.red = red;
            konekcija = kon.KreirajKonekciju();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };

                cmd.Parameters.Add("@ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@kontakt", SqlDbType.NVarChar).Value = txtKontakt.Text;
                cmd.Parameters.Add("@adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;

                if(azuriraj)
                {
                    DataRowView pomocniRed = red;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = pomocniRed["ID"];
                    cmd.CommandText = @"update tblTrener
                                      set __Ime=@ime, __Prezime=@prezime, Kontakt=@kontakt, adresa=@adresa
                                        where trenerID=@id";

                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblTrener(__Ime, __Prezime, kontakt, adresa)
                                       values(@ime, @prezime, @kontakt, @adresa)";
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
