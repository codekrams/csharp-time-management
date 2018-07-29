using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitarbeiterZeitmanagement
{
    public partial class Einsatzzeiten : Form
    {
        List<Mitarbeiter> mitarbeiter = new List<Mitarbeiter>();
        List<Einsatz> einsatz = new List<Einsatz>();
        Datenbank db = new Datenbank();

        public Einsatzzeiten()
        {
            InitializeComponent();
            fillList();
        }

        private void Einsatzzeiten_Load(object sender, EventArgs e)
        {
            foreach (Mitarbeiter m in mitarbeiter)
            {
                comboBox1.Items.Add(m.getNachname() + ", " + m.getVorname());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                safeUpdate();
            }
            catch (Exception ex) {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int eid = einsatz[listBox1.SelectedIndex].getId();
                db.deleteEinsatz(eid);
                MessageBox.Show("Einsatz gelöscht");
                fillList();
                showEinsaetze(eid);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
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
            }
        }

        private void fillList() {
            mitarbeiter = db.getAllMitarbeiter();
            einsatz = db.getAllEinsaetze();
        }

        private void showEinsaetze(int maid)
        {
            listBox1.Items.Clear();
            foreach (Einsatz e in einsatz)
            {
                if (maid == e.getMaid())
                {
                    listBox1.Items.Add(e.getDatum().Substring(0, 10) + ": " + e.getEinsatzvon() + " - " + e.getEinsatzbis());
                }
            }
        }

        private void safeUpdate() {
            if (listBox1.SelectedIndex == -1)
            {
                if (comboBox1.SelectedIndex > -1
                    && !String.IsNullOrEmpty(textBox1.Text)
                    && !String.IsNullOrEmpty(textBox2.Text)
                    )
                {
                    db.safeEinsatz(mitarbeiter[comboBox1.SelectedIndex].getMaid(), dateTimePicker1.Value.ToString("yyyy-MM-dd"), textBox1.Text, textBox2.Text);
                    MessageBox.Show("Einsatz eingetragen");
                    fillList();
                    showEinsaetze(mitarbeiter[comboBox1.SelectedIndex].getMaid());
                }
                else
                {
                    MessageBox.Show("Bitte alle Angaben eintragen");
                }
            }
            else if (listBox1.SelectedIndex > -1)
            {
                if (comboBox1.SelectedIndex > -1
                    && !String.IsNullOrEmpty(textBox1.Text)
                    && !String.IsNullOrEmpty(textBox2.Text)
                    )
                {
                    int id = einsatz[listBox1.SelectedIndex].getId();
                    db.updateEinsatz(mitarbeiter[comboBox1.SelectedIndex].getMaid(), dateTimePicker1.Value.ToString("yyyy-MM-dd"), textBox1.Text, textBox2.Text, id);
                    MessageBox.Show("Einsatz geändert");
                    fillList();
                    showEinsaetze(mitarbeiter[comboBox1.SelectedIndex].getMaid());
                }
                else
                {
                    MessageBox.Show("Bitte alle Angaben eintragen");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            showEinsaetze(mitarbeiter[comboBox1.SelectedIndex].getMaid());
        }

        private void Einsatzzeiten_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Text = "";
            textBox2.Text = "";
            listBox1.Items.Clear();
        }

        
    }
}
