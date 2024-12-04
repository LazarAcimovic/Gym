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
    /// Interaction logic for FrmRegistracija.xaml
    /// </summary>
    public partial class FrmRegistracija : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private bool azuriraj;
        private DataRowView red;

        public FrmRegistracija()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmRegistracija(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            this.azuriraj = azuriraj;
            this.red = red;
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiNazivZaposlenog = @"select zaposleniID, _Ime + ' ' + _Prezime as 'ime i prezime zaposlenog' 
                                     from tblZaposleni";
                DataTable dtRegistracija = new DataTable();
                SqlDataAdapter daNazivZaposlenog = new SqlDataAdapter(vratiNazivZaposlenog, konekcija);
                daNazivZaposlenog.Fill(dtRegistracija);
                cbZaposleni.ItemsSource = dtRegistracija.DefaultView;
                dtRegistracija.Dispose();
                daNazivZaposlenog.Dispose();

                string vratiNazivClana = @"select clanID, ime + ' ' + prezime as 'ime i prezime člana'
                          from tblČlan";
                DataTable dtClan = new DataTable();
                SqlDataAdapter daClan = new SqlDataAdapter(vratiNazivClana, konekcija);
                daClan.Fill(dtClan);
                cbClan.ItemsSource = dtClan.DefaultView;
                daClan.Dispose();
                dtClan.Dispose();

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
                DateTime date = (DateTime)dpDatum.SelectedDate;
                string datum = date.ToString("yyyy-MM-dd");

                cmd.Parameters.Add("@zaposleni", SqlDbType.Int).Value = cbZaposleni.SelectedValue;
                cmd.Parameters.Add("@nazivClana", SqlDbType.Int).Value = cbClan.SelectedValue;
                cmd.Parameters.Add("@datum", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@cenaRegistracije", SqlDbType.Int).Value = txtCena.Text;
                
                if(azuriraj)
                {
                    DataRowView pomocniRed = red;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"update tblRegistracija
                                       set zaposleniID=@zaposleni, clanID=@nazivClana, datumRegistracije=@datum, cenaRegistracije=@cenaRegistracije
                                        where registracijaID=@id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblRegistracija(zaposleniID, clanID, datumRegistracije, cenaRegistracije)
                                    values(@zaposleni, @nazivClana, @datum, @cenaRegistracije)";
                }
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Close();

            }
            catch(SqlException)
            {
                MessageBox.Show("Unos podataka nije validan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Odaberite datum!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Greška prilikom konverzije podataka!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
