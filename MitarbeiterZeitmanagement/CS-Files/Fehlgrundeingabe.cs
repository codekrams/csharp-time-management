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
    public partial class Fehlgrundeingabe : Form
    {
        List<Fehlgrund> fg = new List<Fehlgrund>();
        Datenbank db = new Datenbank();

        public Fehlgrundeingabe()
        {
            InitializeComponent();
            fg = db.getAllFehlgruende();
        }

        private void Fehlgrundeingabe_Load(object sender, EventArgs e)
        {
            foreach (Fehlgrund fehlg in fg) {
                comboBox1.Items.Add(fehlg.getFehlgrund());
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = fg[comboBox1.SelectedIndex].getFehlgrund();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void safeUpdate() {
            if (comboBox1.SelectedIndex == -1)
            {
                if (!String.IsNullOrEmpty(textBox1.Text))
                {
                    db.safeFehlgrund(textBox1.Text);
                    MessageBox.Show("Fehlgrund gespeichert");
                }
                else
                {
                    MessageBox.Show("Bitte Fehlgrund eintragen");
                }
            }

            if (comboBox1.SelectedIndex > -1)
            {
                if (!String.IsNullOrEmpty(textBox1.Text))
                {
                    int id = fg[comboBox1.SelectedIndex].getId();
                    db.updateFehlgrund(textBox1.Text, id);
                    MessageBox.Show("Fehlgrund geändert");
                }
                else
                {
                    MessageBox.Show("Bitte Änderung vornehmen");
                }
            }

            comboBox1.Items.Clear();

            fg = db.getAllFehlgruende();

            foreach (Fehlgrund fehlg in fg)
            {
                comboBox1.Items.Add(fehlg.getFehlgrund());
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                    int id = fg[comboBox1.SelectedIndex].getId();
                    db.deleteFehlgrund(id);
                    MessageBox.Show("Fehlgrund gelöscht");
                }
                else
                {
                    MessageBox.Show("Bitte Änderung vornehmen");
                }

            comboBox1.Items.Clear();

            fg = db.getAllFehlgruende();

            foreach (Fehlgrund fehlg in fg)
            {
                comboBox1.Items.Add(fehlg.getFehlgrund());
            }
        }
    }
}
