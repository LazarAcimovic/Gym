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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTeretana
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ucitanaTabela;
        bool azuriraj;



        #region Select upiti
        static string trenerSelect = @"select trenerID as 'ID', __Ime as 'Ime', __Prezime as 'Prezime', kontakt as 'Kontakt', adresa as 'Adresa' from tblTrener";
        static string clanSelect = @"select clanID as 'ID', ime as 'Ime', prezime as 'Prezime', jbmg as 'JMBG',_kontakt_ as 'Kontakt', datumRodjenja, grad as 'Grad', vrstaTreninga as 'Vrsta treninga', __Ime + ' ' + __Prezime as 'Ime i prezime trenera', nazivSprave as 'Naziv sprave'
                                   from tblČlan join tblTipTreninga on tblČlan.treningID = tblTipTreninga.treningID 
			                        join tblTrener on tblČlan.trenerID = tblTrener.trenerID 
			                         join tblSprava on tblČlan.spravaID = tblSprava.spravaID";
        static string registracijaSelect = @"select registracijaID as 'ID', _Ime + ' ' + _Prezime as 'Ime i prezime zaposlenog', ime + ' ' + prezime as 'Ime i prezime člana',datumRegistracije as 'Datum registracije', cenaRegistracije as 'Cena registracije'
                                              from tblRegistracija join tblZaposleni on tblRegistracija.zaposleniID = tblZaposleni.zaposleniID
					                            join tblČlan on tblRegistracija.clanID = tblČlan.clanID";

        static string tipTreningaSelect = @"select treningID as 'ID', vrstaTreninga as 'Vrsta treninga' from tblTipTreninga";
        static string osiguranjeSelect = @"select osiguranjeID as 'ID', brPolise as 'Broj polise', tipOsiguranja as 'Tip osiguranja', ime + ' ' + prezime as 'Ime i prezime člana' 
                                          from tblOsiguranje join tblČlan on tblOsiguranje.clanID = tblČlan.clanID";

        static string spravaSelect = @"select spravaID as 'ID', nazivSprave as 'Naziv sprave' from tblSprava";
        #endregion

        #region SelectSaUslovom
        string selectUslovTreneri = @"select * from tblTrener where trenerID=";
        string selectUslovClanovi = @"select * from tblČlan where clanID=";
        string selectUslovRegistracije = @"select * from tblRegistracija where registracijaID=";
        string selectUslovTipTreninga = @"select * from tblTipTreninga where treningID=";
        string selectUslovOsiguranje = @"select * from tblOsiguranje where osiguranjeID=";
        string selectUslovSprave = @"select * from tblSprava where spravaID=";
        #endregion


        #region 
        string treneriDelete = @"delete from tblTrener where trenerID=";
        string clanoviDelete = @"delete from tblČlan where clanID=";
        string registracijaDelete = @"delete from tblRegistracija where registracijaID=";
        string tipTreningaDelete = @"delete from tblTipTreninga where treningID=";
        string osiguranjeDelete = @"delete from tblOsiguranje where osiguranjeID=";
        string spravaDelete = @"delete from tblSprava where spravaID=";
        #endregion
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(dataGridCentralni, trenerSelect);
        }


        private void UcitajPodatke(DataGrid grid, string SelectUpit)
        {
            try
            {
                konekcija.Open(); // konekcija sa bazon
                //SqlDataAdapter - prosleđujemo mu konekciju tj bazu sa kojom radimo i upit 
                SqlDataAdapter dataAdapter = new SqlDataAdapter(SelectUpit, konekcija);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (grid != null) // ako grid postoji
                {
                    grid.ItemsSource = dt.DefaultView; // ovo znači da će se ta tabela prikazati kao ui tj u gridu
                }
                ucitanaTabela = SelectUpit;
                dt.Dispose();
                dataAdapter.Dispose();//ovo i ovo iznad predstavlja dereferenciranje objekata
            }
            catch (SqlException)
            {
                MessageBox.Show("Neuspesno ucitani podaci", "greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close(); //konekcija se zatvara 
                }
            }
        }

        private void btnTrener_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, trenerSelect);
        }

        private void btnČlanovi_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, clanSelect);
        }

        private void btnRegistracija_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, registracijaSelect);
        }

        private void btnTipTreninga_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, tipTreningaSelect);
        }

        private void btnOsiguranje_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, osiguranjeSelect);
        }

        private void btnSprava_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, spravaSelect);
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;
            if (ucitanaTabela.Equals(trenerSelect))
            {
                prozor = new FrmTrener();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, trenerSelect);
            }
            else if (ucitanaTabela.Equals(clanSelect))
            {
                prozor = new FrmClanovi();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, clanSelect);
            }
            else if (ucitanaTabela.Equals(registracijaSelect))
            {
                prozor = new FrmRegistracija();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, registracijaSelect);
            }
            else if (ucitanaTabela.Equals(tipTreningaSelect))
            {
                prozor = new FrmTipTreninga();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, tipTreningaSelect);
            }
            else if (ucitanaTabela.Equals(osiguranjeSelect))
            {
                prozor = new FrmOsiguranje();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, osiguranjeSelect);
            }
            else if (ucitanaTabela.Equals(spravaSelect))
            {
                prozor = new FrmSprava();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, spravaSelect);
            }
        }


        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(trenerSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovTreneri); // selecUslovTreneri a ne trenerSelect, jer hoćemo da nam prikaže samo selektovan red;
                UcitajPodatke(dataGridCentralni, trenerSelect); //nakon što smo kliknuni na izmeni, i nakon što smo izmenili selektovan red i kliknuli sačuvaj, onda treba da opet prikažemo sve trenere u bazi
            }
            else if (ucitanaTabela.Equals(clanSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovClanovi);
                UcitajPodatke(dataGridCentralni, clanSelect);
            }
            else if (ucitanaTabela.Equals(registracijaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovRegistracije);
                UcitajPodatke(dataGridCentralni, registracijaSelect);
            }
            else if (ucitanaTabela.Equals(tipTreningaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovTipTreninga);
                UcitajPodatke(dataGridCentralni, tipTreningaSelect);
            }
            else if (ucitanaTabela.Equals(osiguranjeSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovOsiguranje);
                UcitajPodatke(dataGridCentralni, osiguranjeSelect);
            }
            else if (ucitanaTabela.Equals(spravaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovSprave);
                UcitajPodatke(dataGridCentralni, spravaSelect);
            }
        }

        private void PopuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0]; // Taj red koji je selektovan zelimo da izvucemo
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"]; // ovako pristupamo koloni koju smo selektovali
                cmd.CommandText = selectUslov + "@id";
                //sada čitamo iz baze- SqlDataReader služi za čitanje iz baze
                //čita vrednosti iz baze za upit koji mu prosledimo, čita ceo red, ne samo jednu kolonu reda
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();

                if (citac.Read()) //citac.Read()- čita ceo red
                {
                    if (ucitanaTabela.Equals(trenerSelect))
                    {
                        //sada otvaravamo formu i popunjavamo je podacima
                        //treba da mu prosledimo red koji je selektovan i azuriraj da bismo tamo u okviru metod btn sačiuvaj mogli da proverimo da li se radi o azuriranju ili insertu
                        FrmTrener prozorTrener = new FrmTrener(azuriraj, red);
                        prozorTrener.txtIme.Text = (string)citac["__Ime"]; //kod labele Ime fillujemo čime će da se popuni tj kod koga je selektovao
                        prozorTrener.txtPrezime.Text = (string)citac["__Prezime"];
                        prozorTrener.txtKontakt.Text = (string)citac["kontakt"];
                        prozorTrener.txtAdresa.Text = (string)citac["adresa"];
                        prozorTrener.ShowDialog();//otvaramo prozor FrmTrener
                    }
                    else if (ucitanaTabela.Equals(clanSelect))
                    {
                        FrmClanovi prozorClan = new FrmClanovi(azuriraj, red);
                        prozorClan.txtIme.Text = (string)citac["ime"];
                        prozorClan.txtPrezime.Text = (string)citac["prezime"];
                        prozorClan.txtJbmg.Text = (string)citac["jbmg"];
                        prozorClan.txtKontakt.Text = (string)citac["_kontakt_"];
                        prozorClan.dpDatum.SelectedDate = (DateTime)citac["datumRodjenja"];
                        prozorClan.txtGrad.Text = (string)citac["grad"];
                        prozorClan.cbTip.SelectedValue = citac["treningID"];
                        prozorClan.cbTrener.SelectedValue = citac["trenerID"];
                        prozorClan.cbSprava.SelectedValue = citac["spravaID"];
                        prozorClan.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(registracijaSelect))
                    {
                        FrmRegistracija prozorRegistracija = new FrmRegistracija(azuriraj, red);
                        prozorRegistracija.cbZaposleni.SelectedValue = citac["zaposleniID"];
                        prozorRegistracija.dpDatum.SelectedDate = (DateTime)citac["datumRegistracije"];
                        prozorRegistracija.cbClan.SelectedValue = citac["clanID"];
                        prozorRegistracija.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(tipTreningaSelect))
                    {
                        FrmTipTreninga prozorTip = new FrmTipTreninga(azuriraj, red);
                        prozorTip.txtTipTreninga.Text = (string)citac["vrstaTreninga"];
                        prozorTip.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(osiguranjeSelect))
                    {
                        FrmOsiguranje prozorOsiguranje = new FrmOsiguranje(azuriraj, red);
                        prozorOsiguranje.txtBrojPolise.Text = citac["brPolise"].ToString();
                        prozorOsiguranje.txtTip.Text = (string)citac["tipOsiguranja"];
                        prozorOsiguranje.cbClan.SelectedValue = citac["clanID"];
                        prozorOsiguranje.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(spravaSelect))
                    {
                        FrmSprava prozorSprava = new FrmSprava(azuriraj, red);
                        prozorSprava.txtNazivSprave.Text = (string)citac["nazivSprave"];
                        prozorSprava.ShowDialog();

                    }
                }
            }

            //ispod catch - ako neko proba da izmeni nešto a nije selektovao red
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                    azuriraj = false;
                }

            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(trenerSelect))
            {
                obrisiZapis(treneriDelete);
                UcitajPodatke(dataGridCentralni, trenerSelect);
            }
            else if (ucitanaTabela.Equals(clanSelect))
            {
                obrisiZapis(clanoviDelete);
                UcitajPodatke(dataGridCentralni, clanSelect);
            }
            else if (ucitanaTabela.Equals(registracijaSelect))
            {
                obrisiZapis(registracijaDelete);
                UcitajPodatke(dataGridCentralni, registracijaSelect);
            }
            else if (ucitanaTabela.Equals(tipTreningaSelect))
            {
                obrisiZapis(tipTreningaDelete);
                UcitajPodatke(dataGridCentralni, tipTreningaSelect);
            }
            else if (ucitanaTabela.Equals(osiguranjeSelect))
            {
                obrisiZapis(osiguranjeDelete);
                UcitajPodatke(dataGridCentralni, osiguranjeSelect);
            }
            else if (ucitanaTabela.Equals(spravaSelect))
            {
                obrisiZapis(spravaDelete);
                UcitajPodatke(dataGridCentralni, spravaSelect);
            }
        }

        private void obrisiZapis(string deleteUpit)
        {
            try
            {
                konekcija.Open();
                DataRowView red = (DataRowView)dataGridCentralni.SelectedItems[0];
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Obaveštenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = deleteUpit + "@id";
                    cmd.ExecuteNonQuery(); // ova linija koda šalje sve do sad bazi da izvrši;
                    cmd.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (SqlException)//ako brišemo strani ključ
            {
                MessageBox.Show("Postoje povezani podaci u tabelama", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }
    }
}