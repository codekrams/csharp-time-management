using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitarbeiterZeitmanagement
{
    
    public partial class Form1 : Form
    {
        List<Auswahl> auswahl = new List<Auswahl>();
      
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                checkLogin();
            }
            catch (Exception ex) {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            label1.Visible = false;
            button5.Visible = false;

            textBox1.Text = "";
            textBox2.Text = "";

            Benutzerlogin.benutzer = "";
            Benutzerlogin.password = "";

            MessageBox.Show("Logout erfolgreich");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try {
                getAuswahl();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            } 
        }       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    checkLogin();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
                }
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    getAuswahl();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
                }
            }
        }


        private string passwortVerschluesseln(string eingabe)
        {
            char[] passwortarr = eingabe.ToCharArray();


            for (int i = 0; i < passwortarr.Length; i++)
            {
                int wert = Convert.ToInt32(passwortarr[i]) + 1;
                passwortarr[i] = Convert.ToChar(wert);
            }

            string passwort = new string(passwortarr);
            return passwort;
        }

        private string passwortEntschluesseln(string eingabe)
        {
            char[] passwortarr = eingabe.ToCharArray();


            for (int i = 0; i < passwortarr.Length; i++)
            {
                int wert = Convert.ToInt32(passwortarr[i]) - 1;
                passwortarr[i] = Convert.ToChar(wert);
            }

            string passwort = new string(passwortarr);
            return passwort;
        }

        private string passwortImportieren(string benutzer)
        {
            string passwort = null;
            FileStream fs = new FileStream("geheim.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);


            while (sr.Peek() != -1)
            {
                if (sr.ReadLine() == benutzer)
                {
                    passwort = sr.ReadLine();
                }
            }

            sr.Close();
            fs.Close();
            return passwort;
        }

        private bool passwortVergleichen(string eingabe, string passwort)
        {

            if (eingabe == passwort)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void checkLogin() {
            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text))
            {
                string passwort = passwortImportieren(textBox1.Text);
                string eingabe = passwortVerschluesseln(textBox2.Text);

                bool vergleich = passwortVergleichen(eingabe, passwort);

                if (vergleich == true)
                {
                    label1.Visible = true;
                    comboBox1.Visible = true;
                    button5.Visible = true;
                    Benutzerlogin.benutzer = textBox1.Text;
                    Benutzerlogin.password = textBox2.Text;
                    listeErstellen(Benutzerlogin.benutzer);
                    MessageBox.Show("Login erfolgreich");
                }
                else
                {
                    MessageBox.Show("Passwort oder Benutzername nicht korrekt");
                }
            }
            else
            {
                MessageBox.Show("Bitte alle Felder ausfüllen");
            }
        }

        private void getAuswahl() {
            if (comboBox1.Text == "Mitarbeiter eintragen, loeschen oder aendern")
            {
                mitarbeitereingabe f2 = new mitarbeitereingabe();
                f2.ShowDialog();

            }
            else if (comboBox1.Text == "Einsatzzeiten eintragen, loeschen oder aendern")
            {
                Einsatzzeiten f2 = new Einsatzzeiten();
                f2.ShowDialog();
            }
            else if (comboBox1.Text == "Fehlzeiten eintragen, loeschen oder aendern")
            {
                fehlzeiteneingabe f2 = new fehlzeiteneingabe();
                f2.ShowDialog();
            }
            else if (comboBox1.Text == "Fehlgrund eintragen, loeschen oder aendern")
            {
                Fehlgrundeingabe f2 = new Fehlgrundeingabe();
                f2.ShowDialog();
            }
            else if (String.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Bitte Aktion auswählen");
            }
        }

        private void listeErstellen(string benutzer)
        {
            comboBox1.Items.Clear();

            FileStream fs = new FileStream("Auswahl.dat", FileMode.OpenOrCreate, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);

            while (sr.Peek() != -1)
            {
                if (sr.ReadLine() == benutzer) {
                    Auswahl a1 = new Auswahl (sr.ReadLine());
                    auswahl.Add(a1);
                    Auswahl a2 = new Auswahl(sr.ReadLine());
                    auswahl.Add(a2);
                    Auswahl a3 = new Auswahl(sr.ReadLine());
                    auswahl.Add(a3);
                    Auswahl a4 = new Auswahl(sr.ReadLine());
                    auswahl.Add(a4);
                }
            }
            sr.Close();
            fs.Close();

            listeAnzeigen();

        }

        private void listeAnzeigen()
        {
            comboBox1.Items.Clear();

            foreach (Auswahl a in auswahl)
            {
                comboBox1.Items.Add(a.auswahl);
            }
        }
    }
}
