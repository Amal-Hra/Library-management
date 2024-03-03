using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Runtime.Remoting.Messaging;
using static UA2_Amal_Hra_2710319.MainWindow;

namespace UA2_Amal_Hra_2710319
{
    public partial class MainWindow : Window
    {
        //Getters and Setters
        public class auteur
        {
            public int auteurID { get; set; }
            public string auteurName { get; set; }
            public string auteuryr { get; set; }
            public string auteurgender { get; set; }
        }

        public class livre
        {
            public int livreID { get; set; }
            public string livretitre { get; set; }
            public string livreyr { get; set; }
            public string livreauteur { get; set; }
        }
        //---------------------------------------------------------------------------------------------
        private void AjouterAuteur_Click(object sender, RoutedEventArgs e)
        {
            auteur ecrivain = new auteur();

            // Verification du champ ID de l'auteur
            if (!int.TryParse(num_auteur.Text, out int nouveauAuteurID) || nouveauAuteurID <= 0)
            {
                MessageBox.Show("Veuillez saisir un ID d'écrivain valide.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verification si un ecrivain avec le meme ID existe deja
            if (dataGridAuteurs.Items.OfType<auteur>().Any(a => a.auteurID == nouveauAuteurID))
            {
                MessageBox.Show("Un écrivain avec le même ID existe déjà.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérification du nom et prénom de l'écrivain
            string nouveauNomPrenom = nom_prenom.Text.Trim();
            if (string.IsNullOrWhiteSpace(nouveauNomPrenom))
            {
                MessageBox.Show("Veuillez saisir un nom et prénom d'écrivain.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérification de la date de naissance de l'écrivain
            DateTime nouvelleDateNaissance;
            if (!DateTime.TryParse(date_naissance.Text, out nouvelleDateNaissance))
            {
                MessageBox.Show("Veuillez saisir une date de naissance valide.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérification du genre de l'écrivain
            string nouveauGenre = SexeComboBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(nouveauGenre))
            {
                MessageBox.Show("Veuillez sélectionner un genre d'écrivain.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Création du nouvel objet écrivain après les vérifications
            ecrivain.auteurID = nouveauAuteurID;
            ecrivain.auteurName = nouveauNomPrenom;
            ecrivain.auteuryr = nouvelleDateNaissance.ToString("yyyy-MM-dd");
            ecrivain.auteurgender = nouveauGenre;

            // Ajout de l'écrivain à la liste
            dataGridAuteurs.Items.Add(ecrivain);
            AuteurComboBox.Items.Add(ecrivain.auteurName);
            RechercheTextBox.Items.Add(ecrivain.auteurName);
        }
        //------------------------------------------------------------------------------------------------------
        private void AjouterLivre_Click(object sender, RoutedEventArgs e)
        {
            // Vérification du champ ID du livre
            if (!int.TryParse(num_livre.Text, out int nouveauLivreID) || nouveauLivreID <= 0)
            {
                MessageBox.Show("Veuillez saisir un ID de livre valide.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérification si un livre avec le même ID existe déjà
            if (dataGridLivre.Items.OfType<livre>().Any(l => l.livreID == nouveauLivreID))
            {
                MessageBox.Show("Un livre avec le même ID existe déjà.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérification des autres champs du livre
            string nouveauTitre = titre_livre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nouveauTitre))
            {
                MessageBox.Show("Veuillez saisir un titre de livre.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérification si une date de parution est sélectionnée
            if (string.IsNullOrWhiteSpace(Date_Parution.Text))
            {
                MessageBox.Show("Veuillez sélectionner une date de parution.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérification si un auteur est sélectionné
            if (string.IsNullOrWhiteSpace(AuteurComboBox.Text))
            {
                MessageBox.Show("Veuillez sélectionner un auteur.", "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Création du nouvel objet livre après les vérifications
            livre livre = new livre();

            livre.livreID = nouveauLivreID;
            livre.livretitre = nouveauTitre;
            livre.livreyr = Date_Parution.Text.Trim();
            livre.livreauteur = AuteurComboBox.Text.Trim();

            // Ajout du livre à la liste
            dataGridLivre.Items.Add(livre);
        }
        //-----------------------------------------------------------------------------------------------------------
        private void RechercheTextBox_SelectionChanger(object sender, SelectionChangedEventArgs e)
        {
            if (RechercheTextBox.SelectedItem != null)
            {
                string nomAuteur = RechercheTextBox.SelectedItem.ToString();
                FiltrerLivresParAuteur(nomAuteur);
            }
        }

        private void FiltrerLivresParAuteur(string nomAuteur)
        {
            var livresDeLAuteur = from livre in dataGridLivre.Items.OfType<livre>()
                                  where livre.livreauteur == nomAuteur
                                  select new { NomPrenom = livre.livreauteur, NumeroAuteur = livre.livreID, Sexe = livre.livretitre };

            dataGridConsulter.ItemsSource = livresDeLAuteur.ToList();
        }
    }
}