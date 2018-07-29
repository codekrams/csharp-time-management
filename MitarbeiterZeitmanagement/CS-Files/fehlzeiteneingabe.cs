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
    public partial class fehlzeiteneingabe : Form
    {
        List<Fehlzeit> fz = new List<Fehlzeit>();
        List<Mitarbeiter> mitarbeiter = new List<Mitarbeiter>();
        List<Fehlgrund> fg = new List<Fehlgrund>();

        Datenbank db = new Datenbank();

        public fehlzeiteneingabe()
        {
            InitializeComponent();
            fillList();
        }

        private void fehlzeiteneingabe_Load(object sender, EventArgs e)
        {
            foreach (Mitarbeiter m in mitarbeiter)
            {
                comboBox1.Items.Add(m.getNachname() + ", " + m.getVorname());
            }

            foreach (Fehlgrund f in fg)
            {
                comboBox2.Items.Add(f.getFehlgrund());
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int fid = fz[listBox1.SelectedIndex].getId();

                db.deleteFehlzeit(fid);
                MessageBox.Show("Fehlzeit gelöscht");
                fillList();
                showFehlzeiten(mitarbeiter[comboBox1.SelectedIndex].getMaid());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
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

        private void showFehlzeiten(int maid) {
            
            listBox1.Items.Clear();
            foreach (Fehlzeit f in fz) {
                if (maid == f.getMaid()) {
                    listBox1.Items.Add(f.getVonDatum().Substring(0, 10) + " - " + f.getBisDatum().Substring(0, 10));
 
                }
            }

        }

        private void fillList() {
            fz = db.getAllFehlzeiten();
            mitarbeiter = db.getAllMitarbeiter();
            fg = db.getAllFehlgruende();
        }

        private void safeUpdate() {
            if (listBox1.SelectedIndex > -1)
            {
                if (comboBox1.SelectedIndex > -1
                    && comboBox2.SelectedIndex > -1
                    && !String.IsNullOrEmpty(textBox1.Text)
                    )
                {
                    int id = fz[listBox1.SelectedIndex].getId();
                    
                    db.updateFehlzeit(mitarbeiter[comboBox1.SelectedIndex].getMaid(), dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), fg[comboBox2.SelectedIndex].getId(), textBox1.Text, id);
                    MessageBox.Show("Fehlzeiten geändert");
                    fillList();
                    showFehlzeiten(mitarbeiter[comboBox1.SelectedIndex].getMaid());
                }
                else
                {
                    MessageBox.Show("Bitte alle Angaben ausfüllen");
                }
            }
            else if (listBox1.SelectedIndex == -1)
            {
                if (comboBox1.SelectedIndex > -1
                    && comboBox2.SelectedIndex > -1
                    && !String.IsNullOrEmpty(textBox1.Text)
                    )
                {
                    db.safeFehlzeit(mitarbeiter[comboBox1.SelectedIndex].getMaid(), dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), fg[comboBox2.SelectedIndex].getId(), textBox1.Text);
                    MessageBox.Show("Fehlzeiten eingetragen");
                    fillList();
                    showFehlzeiten(mitarbeiter[comboBox1.SelectedIndex].getMaid());
                }
                else
                {
                    MessageBox.Show("Bitte alle Angaben ausfüllen");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            showFehlzeiten(mitarbeiter[comboBox1.SelectedIndex].getMaid());
        }

        private void fehlzeiteneingabe_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Text = "";
            listBox1.Items.Clear();
        }

    }
}
