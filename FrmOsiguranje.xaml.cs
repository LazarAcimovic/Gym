using System;
using System.Collections.Generic;
using System.Data;
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

namespace WPFTeretana
{
    /// <summary>
    /// Interaction logic for FrmOsiguranje.xaml
    /// </summary>
    public partial class FrmOsiguranje : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private bool azuriraj;
        private DataRowView red;

        public FrmOsiguranje()
        {
            InitializeComponent();
           
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmOsiguranje(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtBrojPolise.Focus();
            this.azuriraj = azuriraj;
            this.red = red;
            konekcija = kon.KreirajKonekciju();
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiClana = @"select clanID, ime + ' ' + prezime as 'ime i prezime člana' 
                                          from tblČlan";
                DataTable dtClan = new DataTable();
                SqlDataAdapter daClan = new SqlDataAdapter(vratiClana, konekcija);
                daClan.Fill(dtClan);
                cbClan.ItemsSource = dtClan.DefaultView;
                dtClan.Dispose();
                daClan.Dispose();
            }
            catch
            {
                MessageBox.Show("Padajuce liste nisu popunjene!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
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

                cmd.Parameters.Add("@brPolise", SqlDbType.Int).Value = txtBrojPolise.Text;
                cmd.Parameters.Add("@tipOsiguranja", SqlDbType.NVarChar).Value = txtTip.Text;
                cmd.Parameters.Add("@clan", SqlDbType.Int).Value = cbClan.SelectedValue;
                if(azuriraj)
                {
                    DataRowView pomocniRed = red;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = pomocniRed["ID"];
                    cmd.CommandText = @"update tblOsiguranje 
                                        set brPolise=@brPolise, tipOsiguranja=@tipOsiguranja, clanID=@clan
                                        where osiguranjeID=@id";
                    red = null;
                        
                }
                else
                {
                    cmd.CommandText = @"insert into tblOsiguranje(brPolise, tipOsiguranja, clanID)
                                  values(@brPolise, @tipOsiguranja, @clan)";
                }
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Close();
            }
            catch
            {
                MessageBox.Show("Unos podataka nije validan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
