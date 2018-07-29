using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace MitarbeiterZeitmanagement
{
    public partial class MitarbeiterFehlzeiten : Form
    {
        Datenbank db = new Datenbank();
        List<Fehlzeit> fz = new List<Fehlzeit>();
        List<Mitarbeiter> mitarbeiter = new List<Mitarbeiter>();
        List<Fehlgrund> fg = new List<Fehlgrund>();
        List<Monatsauswahl> monate = new List<Monatsauswahl>();
        int summe =0;

        public MitarbeiterFehlzeiten()
        {
            InitializeComponent();
            fz = db.getAllFehlzeiten();
            mitarbeiter = db.getAllMitarbeiter();
            fg = db.getAllFehlgruende();
            monate = getMonate();
        }

        private void MitarbeiterFehlzeiten_Load(object sender, EventArgs e)
        {
            
            foreach (Mitarbeiter m in mitarbeiter) {
                if (Benutzerlogin.mitarbeiter == m.getMaid()) {
                    label1.Text = m.getNachname() + ", " + m.getVorname();
                    pictureBox1.ImageLocation = m.getBild();

                        if (m.getBewertung() == 1) {
                            checkBox1.Checked=true;
                        }
                        if (m.getBewertung() == 2)
                        {
                        checkBox2.Checked = true;
                        }
                        if (m.getBewertung() == 3)
                        {
                        checkBox3.Checked = true;
                        }
                    break;
                }
            }

            foreach (Fehlgrund fehlgrund in fg) {
                comboBox1.Items.Add(fehlgrund.getFehlgrund());
            }

            foreach (Monatsauswahl m in monate)
            {
                comboBox2.Items.Add(m.getMonat());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
 
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    showAllFehlzeiten();
                }
                else if (radioButton2.Checked)
                {
                    if (!String.IsNullOrEmpty(textBox1.Text))
                    {
                        showJahrFehlzeiten(textBox1.Text);
                    }
                    else
                    {
                        MessageBox.Show("Bitte Jahr eingeben");
                    }
                }
                else if (radioButton3.Checked)
                {
                    if (comboBox2.SelectedIndex > -1)
                    {
                        string monat = monate[comboBox2.SelectedIndex].getZahl();
                        showMonatsFehlzeiten(monat);
                    }
                    else
                    {
                        MessageBox.Show("Bitte Monat eingeben");
                    }
                }
                else if (radioButton4.Checked)
                {
                    int fehlgrund = findFehlgrundid(comboBox1.Text);
                    showAllFehlzeitennachGrund(fehlgrund);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Schwerer Fehler aufgetreten: " + ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Text = "";
            comboBox2.SelectedIndex = -1;
            richTextBox1.Text = "";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

            textBox1.Text = "";
            comboBox2.SelectedIndex = -1;
            richTextBox1.Text = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            richTextBox1.Text = "";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Text = "";
            richTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExcelEintragen(@"F:\GPB\C#\CSharp-Projekte\MitarbeiterZeitmanagement\bin\Debug\AuswertungFehlzeit.xlsx");
        }

        private void showAllFehlzeiten() {
            richTextBox1.Clear();
          

            foreach (Fehlzeit fehlzeit in fz) {
                if (Benutzerlogin.mitarbeiter == fehlzeit.getMaid()) {
                    richTextBox1.AppendText(fehlzeit.getVonDatum().Substring(0, 10) + " - " + fehlzeit.getBisDatum().Substring(0, 10) + ", Fehltage: " + fehlzeit.getFehltage() + Environment.NewLine);
                }
            }
        }

        private void showAllFehlzeitennachGrund(int fehlgrundid)
        {
            richTextBox1.Clear();
            

            foreach (Fehlzeit fehlzeit in fz)
            {
                if (Benutzerlogin.mitarbeiter == fehlzeit.getMaid() && fehlgrundid == fehlzeit.getFehlid())
                {
                    richTextBox1.AppendText(fehlzeit.getVonDatum().Substring(0, 10) + " - " + fehlzeit.getBisDatum().Substring(0, 10) + ", Fehltage: " + fehlzeit.getFehltage() + Environment.NewLine);
                }
            }
        }

        private void showMonatsFehlzeiten(string monat)
        {
            richTextBox1.Clear();
          

            foreach (Fehlzeit fehlzeit in fz)
            {
                if (Benutzerlogin.mitarbeiter == fehlzeit.getMaid() && (fehlzeit.getVonDatum().Contains(monat) || fehlzeit.getBisDatum().Contains(monat)))
                {
                    richTextBox1.AppendText(fehlzeit.getVonDatum().Substring(0, 10) + " - " + fehlzeit.getBisDatum().Substring(0, 10) + ", Fehltage: " + fehlzeit.getFehltage() + Environment.NewLine);
                }
            }
        }
        

        private void showJahrFehlzeiten(string jahr)
        {
            richTextBox1.Clear();
           

            foreach (Fehlzeit fehlzeit in fz)
            {
                if (Benutzerlogin.mitarbeiter == fehlzeit.getMaid() && (fehlzeit.getVonDatum().Contains(jahr) || fehlzeit.getBisDatum().Contains(jahr)))
                {
                    richTextBox1.AppendText(fehlzeit.getVonDatum().Substring(0, 10) + " - " + fehlzeit.getBisDatum().Substring(0, 10) + ", Fehltage: " + fehlzeit.getFehltage() + Environment.NewLine);
                }
            }
        }


        private int findFehlgrundid(string fehlgrund) {
            int id = 0;

            foreach (Fehlgrund f in fg){
                if (f.getFehlgrund() == fehlgrund) {
                    id = f.getId();
                } 
            }
            return id;
        }

        private List<Monatsauswahl> getMonate()
        {
            List<Monatsauswahl> liste = new List<Monatsauswahl>();
            liste.Add(new Monatsauswahl("Januar", "01"));
            liste.Add(new Monatsauswahl("Feburar", "02"));
            liste.Add(new Monatsauswahl("März", "03"));
            liste.Add(new Monatsauswahl("April", "04"));
            liste.Add(new Monatsauswahl("Mai", "05"));
            liste.Add(new Monatsauswahl("Juni", "06"));
            liste.Add(new Monatsauswahl("Juli", "07"));
            liste.Add(new Monatsauswahl("August", "08"));
            liste.Add(new Monatsauswahl("September", "09"));
            liste.Add(new Monatsauswahl("Oktober", "10"));
            liste.Add(new Monatsauswahl("November", "11"));
            liste.Add(new Monatsauswahl("Dezember", "12"));

            return liste;
        }

        private void ExcelEintragen(string fn)
        {
            try
            {

                Microsoft.Office.Interop.Excel.Application excel =
                    new Microsoft.Office.Interop.Excel.Application();

                excel.Visible = true;

                
                string fileName = fn;
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Open(fileName);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets["Tabelle1"];

                Microsoft.Office.Interop.Excel.Range bRange = worksheet.get_Range("B2:B2");
                bRange.Value = "Fehlzeit-Auswertung für " + label1.Text;

                bRange = worksheet.get_Range("B4:B4");
                bRange.Value = "Datum von";
                bRange = worksheet.get_Range("C4:C4");
                bRange.Value = "Datum bis";
                bRange = worksheet.get_Range("D4:D4");
                bRange.Value = "Tage";
                bRange = worksheet.get_Range("E4:E4");
                bRange.Value = "Grund";

                if (radioButton1.Checked == true)
                {
                    for (int i = 0; i < fz.Count; i++) {
                       
                        if (Benutzerlogin.mitarbeiter == fz[i].getMaid())
                        {
                            string fehlgrund = fg[fz[i].getFehlid()].getFehlgrund();

                            bRange = worksheet.get_Range("B" + (i+5) + ":B" + (i + 5));
                            bRange.Value = fz[i].getVonDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("C" + (i + 5) + ":C" + (i + 5));
                            bRange.Value = fz[i].getBisDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("D" + (i + 5) + ":D" + (i + 5));
                            bRange.Value = fz[i].getFehltage();
                            bRange = worksheet.get_Range("E" + (i + 5) + ":E" + (i + 5));
                            bRange.Value = fehlgrund;
                            summe += fz[i].getFehltage();
                        }
                    }
                    
                }
                else if (radioButton2.Checked == true)
                {
                    for (int j = 0; j < fz.Count; j++)
                    {
                        if (Benutzerlogin.mitarbeiter == fz[j].getMaid() && fz[j].getVonDatum().Contains(textBox1.Text))
                        {
                            int fehlid = fz[j].getFehlid();
                            string fehlgrund = fg[fehlid].getFehlgrund();

                            bRange = worksheet.get_Range("B" + (j + 5) + ":B" + (j + 5));
                            bRange.Value = fz[j].getVonDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("C" + (j + 5) + ":C" + (j + 5));
                            bRange.Value = fz[j].getBisDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("D" + (j + 5) + ":D" + (j + 5));
                            bRange.Value = fz[j].getFehltage();
                            bRange = worksheet.get_Range("E" + (j + 5) + ":E" + (j + 5));
                            bRange.Value = fehlgrund;
                            summe += fz[j].getFehltage();
                        }
                    }
                }
                else if (radioButton3.Checked == true)
                {
                    for (int k = 0; k < fz.Count; k++)
                    {
                        string monat = monate[comboBox2.SelectedIndex].getZahl();
                        if (Benutzerlogin.mitarbeiter == fz[k].getMaid() && (fz[k].getVonDatum().Contains(monat) || fz[k].getBisDatum().Contains(monat)))
                        {
                            int fehlid = fz[k].getFehlid();
                            string fehlgrund = fg[fehlid].getFehlgrund();

                            bRange = worksheet.get_Range("B" + (k + 5) + ":B" + (k + 5));
                            bRange.Value = fz[k].getVonDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("C" + (k + 5) + ":C" + (k + 5));
                            bRange.Value = fz[k].getBisDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("D" + (k + 5) + ":D" + (k + 5));
                            bRange.Value = fz[k].getFehltage();
                            bRange = worksheet.get_Range("E" + (k + 5) + ":E" + (k + 5));
                            bRange.Value = fehlgrund;
                            summe += fz[k].getFehltage();
                        }
                    }
                }


                else if (radioButton4.Checked == true)
                {
                    int fehlgrundid = findFehlgrundid(comboBox1.Text);
                    for (int k = 0; k < fz.Count; k++)
                    {
                        if (Benutzerlogin.mitarbeiter == fz[k].getMaid() && fehlgrundid == fz[k].getFehlid())
                            {

                            bRange = worksheet.get_Range("B" + (k + 5) + ":B" + (k + 5));
                            bRange.Value = fz[k].getVonDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("C" + (k + 5) + ":C" + (k + 5));
                            bRange.Value = fz[k].getBisDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("D" + (k + 5) + ":D" + (k + 5));
                            bRange.Value = fz[k].getFehltage();
                            bRange = worksheet.get_Range("E" + (k + 5) + ":E" + (k + 5));
                            bRange.Value = comboBox1.Text;
                            summe += fz[k].getFehltage();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bitte Auswahl festlegen");
                }

                bRange = worksheet.get_Range("B" + (fz.Count+5) + ":B" + (fz.Count + 5));
                bRange.Value = "Summe";
                bRange = worksheet.get_Range("D" + (fz.Count + 5) + ":D" + (fz.Count + 5));
                bRange.Value = summe;

                workbook.SaveAs(@"F:\GPB\C#\CSharp-Projekte\MitarbeiterZeitmanagement\bin\Debug\AuswertungFehlzeit-" + label1.Text +".xlsx");
                excel.Quit();
                MessageBox.Show("File gespeichert");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
