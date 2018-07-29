using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MitarbeiterZeitmanagement
{
    public partial class mitarbeitereingabe : Form
    {
        List<Mitarbeiter> mitarbeiter = new List<Mitarbeiter>();
        Datenbank db = new Datenbank();
        string picturepath = "0";
        string datenbankpicture = "0";

        public mitarbeitereingabe()
        {
            InitializeComponent();
            mitarbeiter = db.getAllMitarbeiter();
            pictureBox2.AllowDrop = true;
            pictureBox1.AllowDrop = true;
        }

        private void mitarbeitereingabe_Load(object sender, EventArgs e)
        {
            showMitarbeiter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                safeUpdate();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
            mitarbeiter = db.getAllMitarbeiter();

            showMitarbeiter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    int id = mitarbeiter[comboBox1.SelectedIndex].getMaid();
                    db.deleteMitarbeiter(id);
                    MessageBox.Show("Mitarbeiter gelöscht");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }

            comboBox1.Items.Clear();

            mitarbeiter = db.getAllMitarbeiter();

            showMitarbeiter();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MitarbeiterFehlzeiten f2 = new MitarbeiterFehlzeiten();
            f2.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MitarbeiterEinsätze f2 = new MitarbeiterEinsätze();
            f2.ShowDialog();
        }      
          
        private void button6_Click(object sender, EventArgs e)
        {
            
            xmlInterface(textBox3.Text);
            MessageBox.Show("XML erfolgreich eingetragen");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                anzeigenMitarbeiter(comboBox1.SelectedIndex);
                button4.Visible = true;
                button5.Visible = true;
            }
            else
            {
                MessageBox.Show("Bitte Mitarbeiter auswählen");
            }
        }


        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    safeUpdate();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
                }

                comboBox1.Items.Clear();

                mitarbeiter = db.getAllMitarbeiter();

                showMitarbeiter();

            }
        }

        private void label8_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void label8_DragDrop(object sender, DragEventArgs e)
        {
            string[] filesNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (filesNames.Count() > 1)
                {
                    MessageBox.Show("Immer nur eine Datei");
                }
                else
                {
                    string[] help = filesNames[0].Split('\\');
                    label9.Text = help[help.Count() - 1];
                    xmlInterface(filesNames[0].ToString());
                    MessageBox.Show("XML erfolgreich eingetragen");
                    showMitarbeiter();
                }
            }
        }

        private void label10_DragDrop(object sender, DragEventArgs e)
        {
            string[] filesNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (comboBox1.SelectedIndex > -1) {
                    if (filesNames.Count() > 1)
                    {
                        MessageBox.Show("Immer nur eine Datei");
                    }
                    else
                    {

                        picturepath = filesNames[0].ToString();
                        pictureBox1.ImageLocation = picturepath;
                        datenbankpicture = filesNames[0].Replace("\\", "\\\\\\\\");
                        int maid = mitarbeiter[comboBox1.SelectedIndex].getMaid();
                        db.safePicture(datenbankpicture, maid);
                        MessageBox.Show("Bild hochgeladen");
                    }
                 }
                else
                {
                    MessageBox.Show("Bitte Mitarbeiter auswählen");
                }
            }
        }

        private void mitarbeitereingabe_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            richTextBox1.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void showMitarbeiter() {
            comboBox1.Items.Clear();
            foreach (Mitarbeiter m in mitarbeiter)
            {
                comboBox1.Items.Add(m.getNachname() + ", " + m.getVorname());
            }
        }

        private void label10_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void xmlInterface(string dateiname)
        {
            try
            {
                string nachname = "0";
                string vorname = "0";
                string geb = "2000-01-01";
                string arbeitszeit = "0";
                string urlaub = "0";
                string bild = "0";
                string bewertung = "0";
                XmlTextReader reader = new XmlTextReader(dateiname);
                while (reader.Read())
                {
                    if (reader.Name != "")
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            if (reader.Name == "Mitarbeiter")
                            {
                                reader.MoveToAttribute("name");
                                nachname = reader.Value;

                                reader.MoveToAttribute("vorname");
                                vorname = reader.Value;

                                reader.MoveToAttribute("geb");
                                geb = reader.Value;

                                reader.MoveToAttribute("arbeitszeit");
                                arbeitszeit = reader.Value;

                                reader.MoveToAttribute("urlaub");
                                urlaub = reader.Value;

                                reader.MoveToAttribute("bild");
                                bild = reader.Value;

                                reader.MoveToAttribute("bewertung");
                                bewertung = reader.Value;

                                db.safeMitarbeiter(nachname, vorname, geb, arbeitszeit, urlaub, bild, bewertung);
                            }
                            reader.MoveToNextAttribute();
                        }

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
        }

        private string setBewertung()
        {
            string bew = "0";
            if (radioButton1.Checked == true)
            {
                bew = "1";
            }
            else if (radioButton2.Checked == true)
            {
                bew = "2";
            }
            else if (radioButton3.Checked == true)
            {
                bew = "3";
            }

            return bew;
        }

        private void safeUpdate()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                if (!String.IsNullOrEmpty(textBox1.Text)
                     && !String.IsNullOrEmpty(textBox2.Text)
                     && !String.IsNullOrEmpty(textBox4.Text)
                     && !String.IsNullOrEmpty(textBox5.Text))
                {
                    string bew = setBewertung();
                    db.safeMitarbeiter(textBox1.Text, textBox2.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"), textBox4.Text, textBox5.Text, datenbankpicture, bew);
                    MessageBox.Show("Mitarbeiter eingetragen");
                }
                else
                {
                    MessageBox.Show("Bitte alle Angaben eintragen");
                }
            }

            if (comboBox1.SelectedIndex > -1)
            {
                if (!String.IsNullOrEmpty(textBox1.Text)
                    && !String.IsNullOrEmpty(textBox2.Text)
                    && !String.IsNullOrEmpty(textBox4.Text)
                    && !String.IsNullOrEmpty(textBox5.Text))
                {

                    int id = mitarbeiter[comboBox1.SelectedIndex].getMaid();
                    string bew = setBewertung();
                    
                    db.updateMitarbeiter(textBox1.Text, textBox2.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"), textBox4.Text, textBox5.Text, bew, id);
                    MessageBox.Show("Mitarbeiterdaten aktualisiert");
                }
                else
                {
                    MessageBox.Show("Bitte alle Angaben eintragen");
                }
            }
            mitarbeiter = db.getAllMitarbeiter();
        }

        private void anzeigenMitarbeiter(int which)
        {
            mitarbeiter = db.getAllMitarbeiter();
            richTextBox1.Text = "";
            richTextBox1.Text = mitarbeiter[which].getMaid() + Environment.NewLine + mitarbeiter[which].getNachname() + ", " + mitarbeiter[which].getVorname() + Environment.NewLine + "Geburtsdatum: " + mitarbeiter[which].getGebdat().Substring(0, 10) + Environment.NewLine + "Arbeitszeit: " + mitarbeiter[which].getZeit() + Environment.NewLine + "Urlaubsanspruch: " + mitarbeiter[which].getUrlaub();

            textBox1.Text = mitarbeiter[which].getNachname();
            textBox2.Text = mitarbeiter[which].getVorname();

            textBox4.Text = mitarbeiter[which].getZeit().ToString();
            textBox5.Text = mitarbeiter[which].getUrlaub().ToString();

            Benutzerlogin.mitarbeiter = mitarbeiter[which].getMaid();

            foreach (Mitarbeiter m in mitarbeiter)
            {
                if (Benutzerlogin.mitarbeiter == m.getMaid())
                {
                    picturepath = m.getBild();
                    pictureBox1.ImageLocation = picturepath;


                    if (m.getBewertung() == 1)
                    {
                        radioButton1.Checked = true;
                    }
                    if (m.getBewertung() == 2)
                    {
                        radioButton2.Checked = true;
                    }
                    if (m.getBewertung() == 3)
                    {
                        radioButton3.Checked = true;
                    }
                    break;
                }
            }
        }

    }
}