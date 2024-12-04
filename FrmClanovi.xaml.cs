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
    /// Interaction logic for FrmClanovi.xaml
    /// </summary>
    public partial class FrmClanovi : Window
    {
        SqlConnection konekcija = new SqlConnection();
        Konekcija kon = new Konekcija();
        private bool azuriraj;
        private DataRowView red;

        public FrmClanovi()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        public FrmClanovi(bool azuriraj, DataRowView red)
        {
            InitializeComponent();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.red = red;
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
        }

        private void PopuniPadajuceListe ()
        {
            try
            { 
            konekcija.Open();
            string vratiTipTreninga = @"select treningID, vrstaTreninga from tblTipTreninga";
            DataTable dtTipTreninga = new DataTable();
            SqlDataAdapter daTipTreninga= new SqlDataAdapter(vratiTipTreninga, konekcija);
            daTipTreninga.Fill(dtTipTreninga);
            cbTip.ItemsSource = dtTipTreninga.DefaultView;
            dtTipTreninga.Dispose();
            daTipTreninga.Dispose();


            string vratiImePrezime = @"select trenerID, __Ime + ' ' + __Prezime as 'Ime i prezime trenera' 
                                   from tblTrener";
                DataTable dtImePrezimeTrenera = new DataTable();
                SqlDataAdapter daImePrezimeTrenera = new SqlDataAdapter(vratiImePrezime, konekcija);
                daImePrezimeTrenera.Fill(dtImePrezimeTrenera);
                cbTrener.ItemsSource = dtImePrezimeTrenera.DefaultView;
                dtImePrezimeTrenera.Dispose();
                daImePrezimeTrenera.Dispose();

                string vratiSpravu = @"select spravaID, nazivSprave from tblSprava";
                DataTable dtNazivSprave = new DataTable();
                SqlDataAdapter daNazivSprave = new SqlDataAdapter(vratiSpravu, konekcija);
                daNazivSprave.Fill(dtNazivSprave);
                cbSprava.ItemsSource = dtNazivSprave.DefaultView;
                dtNazivSprave.Dispose();
                daNazivSprave.Dispose();
            }
            catch
            {
                MessageBox.Show("Padajuce liste nisu popunjene!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if(konekcija != null)
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
                Connection = konekcija //ovo pitaj Mareta
            };

                DateTime date = (DateTime)dpDatum.SelectedDate; // što ovo (DateTime)
                string datum = date.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@jbmg", SqlDbType.NVarChar).Value = txtJbmg.Text;
                cmd.Parameters.Add("@kontakt", SqlDbType.NVarChar).Value = txtKontakt.Text;
                cmd.Parameters.Add("@datumRodjenja", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@grad", SqlDbType.NVarChar).Value = txtGrad.Text;
                cmd.Parameters.Add("@nazivTreninga", SqlDbType.Int).Value = cbTip.SelectedValue;
                cmd.Parameters.Add("@trener", SqlDbType.Int).Value = cbTrener.SelectedValue;
                cmd.Parameters.Add("@nazivSprave", SqlDbType.Int).Value = cbSprava.SelectedValue;

                if(azuriraj)
                {
                    DataRowView pomocniRed = red;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = pomocniRed["ID"];
                    cmd.CommandText = @"update tblČlan
                                    set ime=@ime, prezime=@prezime, jbmg=@jbmg, _kontakt_=@kontakt, datumRodjenja=@datumRodjenja, grad=@grad, treningID=nazivTipaTreninga, trenerID=@trener, spravaID=@nazivSprave
                                    where clanID=@id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into tblČlan(ime, prezime, jbmg, _kontakt_, datumRodjenja, grad, treningID, trenerID, spravaID)
                                    values(@ime, @prezime, @jbmg, @kontakt, @datumRodjenja, @grad, @nazivTreninga, @trener, @nazivSprave)";
                }
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();


            }
          
            catch (SqlException)
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
