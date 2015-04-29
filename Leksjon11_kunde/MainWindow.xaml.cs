using System;
using System.Collections.Generic;
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

namespace Leksjon11_kunde
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            visAlleKunder();
        }

        private void visAlleKunder()
        {
            KundeDataContext ds = new KundeDataContext();

            var resultat = from k in ds.kundes orderby k.kundenr select k;
            gridView1.ItemsSource = resultat;
        }
        private void clear()
        {
            txtKnr.Text = "";
            txtNavn.Text = "";
            txtAdresse.Text = "";
            txtEpost.Text = "";

        }

        private void btnSok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int knr = Int32.Parse(txtKnr.Text.Trim());
                KundeDataContext ds = new KundeDataContext();
                kunde kunde = ds.kundes.Single(k => k.kundenr == knr);
                txtNavn.Text = kunde.kundenavn;
                txtAdresse.Text = kunde.kundeadresse;
                txtEpost.Text = kunde.epost;
                lblError.Visibility = System.Windows.Visibility.Hidden;

            }
            catch (Exception)
            {
                lblError.Content = "Kunde ikke funnet";
                lblError.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int knr = Int32.Parse(txtKnr.Text.Trim());
                String navn = txtNavn.Text.Trim();
                String adresse = txtAdresse.Text.Trim();
                String epost = txtEpost.Text.Trim();

                kunde kunde = new kunde();
                kunde.kundenr = knr;
                kunde.kundenavn = navn;
                kunde.kundeadresse = adresse;
                kunde.epost = epost;

                KundeDataContext ds = new KundeDataContext();
                ds.kundes.InsertOnSubmit(kunde);
                ds.SubmitChanges(); // oppdaterer i databasen
                visAlleKunder();
            }
            catch (Exception)
            {
                lblError.Content = "Feil under registrering av ny kunde";
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                KundeDataContext ds = new KundeDataContext();
                int knr = Int32.Parse(txtKnr.Text.Trim());
                kunde kunde = ds.kundes.Single(k => k.kundenr == knr);
                kunde.kundenavn = txtNavn.Text.Trim();
                kunde.kundeadresse = txtAdresse.Text.Trim();
                kunde.epost = txtEpost.Text.Trim();

                ds.SubmitChanges(); // oppdater i datbasen          
                visAlleKunder();
            }
            catch (Exception)
            {
                lblError.Content = "Feil under endring av kunde";
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                KundeDataContext ds = new KundeDataContext();
                int knr = Int32.Parse(txtKnr.Text.Trim());
                kunde kunde = ds.kundes.Single(k => k.kundenr == knr);
                ds.kundes.DeleteOnSubmit(kunde);
                ds.SubmitChanges();
                visAlleKunder();
            }
            catch (Exception)
            {
                lblError.Content = "Feil ved sletting av kunde";
            }
        }

        private void btnNavn_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = "";
            try
            {
                KundeDataContext ds = new KundeDataContext();

                char[] tegn = { '-', ' ' };
                String[] navn = txtNavn.Text.Trim().Split(tegn);

                // b)	Søke på navn og deler av navn (for eksempel hente ut alle som har etternavn som starter med O)
                var resultat = from k in ds.kundes
                               where k.kundenavn.Substring(k.kundenavn.IndexOf(' ') + 1).ToUpper().StartsWith(txtNavn.Text.Trim())
                               orderby k.kundenr
                               select k;

                if (resultat.Count() > 0)
                {
                    gridView1.ItemsSource = resultat;
                }
                else
                {
                    lblError.Content = "Ingen kunder passer søkekriteriet";
                    lblError.Visibility = System.Windows.Visibility.Visible;
                    clear();

                }

            }
            catch (Exception)
            {
                lblError.Content = "Feil ved søk etter navn";
                clear();
            }
        }

        private void btnAdresse_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = "";
            try
            {
                KundeDataContext ds = new KundeDataContext();

                char[] tegn = { '-', ' ' };
                String[] navn = txtNavn.Text.Trim().Split(tegn);

                var resultat = from k in ds.kundes
                               where k.kundeadresse.Substring(k.kundeadresse.IndexOf(' ') + 1).ToUpper().StartsWith(txtAdresse.Text.Trim())
                               orderby k.kundenr
                               select k;

                if (resultat.Count() > 0)
                {
                    gridView1.ItemsSource = resultat;
                }
                else
                {
                    lblError.Content = "Ingen kunder har denne adressen";
                    lblError.Visibility = System.Windows.Visibility.Visible;
                    clear();
                }

            }
            catch (Exception)
            {
                lblError.Content = "Feil ved søk etter adresse";
                lblError.Visibility = System.Windows.Visibility.Visible;
                clear();
            }

        }

        private void btnEpost_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = "";
            try
            {
                KundeDataContext ds = new KundeDataContext();

                char[] tegn = { '-', ' ' };
                String[] navn = txtEpost.Text.Trim().Split(tegn);

                var resultat = from k in ds.kundes
                               where k.epost.Substring(k.epost.IndexOf(' ') + 1).ToUpper().Contains(txtEpost.Text.Trim())
                               orderby k.kundenr
                               select k;

                if (resultat.Count() > 0)
                {
                    gridView1.ItemsSource = resultat;
                }
                else
                {
                    lblError.Content = "Ingen kunder har denne adressen";
                    lblError.Visibility = System.Windows.Visibility.Visible;
                    clear();
                }

            }
            catch (Exception)
            {
                lblError.Content = "Feil ved søk etter adresse";
                lblError.Visibility = System.Windows.Visibility.Visible;
                clear();
            }
        }

        private void btnIntervall_Click(object sender, RoutedEventArgs e)
        {
            
            lblError.Content = "";
            try
            {
                KundeDataContext ds = new KundeDataContext();

                char[] tegn = { '-', ' ' };
                String[] navn = txtNavn.Text.Trim().Split(tegn);

                var resultat = from k in ds.kundes
                               where k.kundenavn.Substring(k.kundenavn.IndexOf(' ')+1).ToUpper().CompareTo(navn[0].Trim()) >=0 &&
                               k.kundenavn.Substring(k.kundenavn.IndexOf(' ')+1).ToUpper().CompareTo(navn[1].Trim()) <=0
                               orderby k.kundenavn.Substring(k.kundenavn.IndexOf(' ')+1)
                               select k;

                if (resultat.Count() > 0)
                {
                    gridView1.ItemsSource = resultat;
                }
                else
                {
                    lblError.Content = "Ingen kunder passer søkekriteriet";
                    lblError.Visibility = System.Windows.Visibility.Visible;
                    
                }

            }
            catch (Exception)
            {
                lblError.Content = "Feil ved søk etter intervall";
                lblError.Visibility = System.Windows.Visibility.Visible;
                
            }
        }

        private void btnEpostleverand_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = "";
            try
            {
                KundeDataContext ds = new KundeDataContext();

                char[] tegn = { '@' };
                String[] navn = txtEpost.Text.Trim().Split(tegn);



                var resultat = from k in ds.kundes
                               where k.epost.Substring(k.epost.IndexOf(' ') + 1).ToUpper().Contains(txtEpost.Text.Trim())
                               orderby k.kundenr
                               select k;

                if (resultat.Count() > 0)
                {
                    gridView1.ItemsSource = resultat;
                }
                else
                {
                    lblError.Content = "Ingen kunder har denne adressen";
                    lblError.Visibility = System.Windows.Visibility.Visible;
                    clear();
                }

            }
            catch (Exception)
            {
                lblError.Content = "Feil ved søk etter adresse";
                lblError.Visibility = System.Windows.Visibility.Visible;
                clear();
            }
        }
        }
    }


