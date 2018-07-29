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
    public partial class MitarbeiterEinsätze : Form
    {
        Datenbank db = new Datenbank();

        List<Mitarbeiter> mitarbeiter = new List<Mitarbeiter>();
        List<Einsatz> einsatz = new List<Einsatz>();
        List<Monatsauswahl> monate = new List<Monatsauswahl>();

        public MitarbeiterEinsätze()
        {
            InitializeComponent();
            mitarbeiter = db.getAllMitarbeiter();
            einsatz = db.getAllEinsaetze();
            monate = getMonate();
        }

        private void MitarbeiterEinsätze_Load(object sender, EventArgs e)
        {
            foreach (Mitarbeiter m in mitarbeiter)
            {
                if (Benutzerlogin.mitarbeiter == m.getMaid())
                {
                    label1.Text = m.getNachname() + ", " + m.getVorname();
                    pictureBox1.ImageLocation = m.getBild();

                    if (m.getBewertung() == 1)
                    {
                        checkBox1.Checked = true;
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

            foreach (Monatsauswahl m in monate) {
                comboBox1.Items.Add(m.getMonat());
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
                    showAllEinsaetze();
                }
                else if (radioButton2.Checked)
                {
                    if (!String.IsNullOrEmpty(textBox1.Text))
                    {
                        showJahrEinsaetze(textBox1.Text);
                    }
                    else
                    {
                        MessageBox.Show("Bitte Jahr eingeben");
                    }
                }
                else if (radioButton3.Checked)
                {
                    if (comboBox1.SelectedIndex > -1)
                    {
                        showMonatsEinsaetze(monate[comboBox1.SelectedIndex].getZahl());
                    }
                    else
                    {
                        MessageBox.Show("Bitte Monat eingeben");
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("schwerer Fehler aufgetreten: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExcelEintragen(@"F:\GPB\C#\CSharp-Projekte\MitarbeiterZeitmanagement\bin\Debug\AuswertungEinsatz.xlsx");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            richTextBox1.Text = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            richTextBox1.Text = "";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
        }

        private void showAllEinsaetze()
        {
            richTextBox1.Clear();

            foreach (Einsatz e in einsatz)
            {
                if (Benutzerlogin.mitarbeiter == e.getMaid())
                {
                    richTextBox1.AppendText(e.getDatum().Substring(0, 10) + ": " + e.getEinsatzvon() + " - " + e.getEinsatzbis() + Environment.NewLine);
                }
            }
        }

        private void showMonatsEinsaetze(string monat)
        {
            richTextBox1.Clear();

            foreach (Einsatz e in einsatz)
            {
                if (Benutzerlogin.mitarbeiter == e.getMaid() && e.getDatum().Contains(monat))
                {
                    richTextBox1.AppendText(e.getDatum().Substring(0, 10) + ": " + e.getEinsatzvon() + " - " + e.getEinsatzbis() + Environment.NewLine);
                }
            }
        }

        private void showJahrEinsaetze(string jahr)
        {
            richTextBox1.Clear();

            foreach (Einsatz e in einsatz)
            {
                if (Benutzerlogin.mitarbeiter == e.getMaid() && e.getDatum().Contains(jahr))
                {
                    richTextBox1.AppendText(e.getDatum().Substring(0, 10) + ": " + e.getEinsatzvon() + " - " + e.getEinsatzbis() + Environment.NewLine);
                }
            }
        }

        private List<Monatsauswahl> getMonate() {
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
                bRange.Value = "Einsatz-Auswertung für " + label1.Text;

                bRange = worksheet.get_Range("B4:B4");
                bRange.Value = "Datum";
                bRange = worksheet.get_Range("C4:C4");
                bRange.Value = "Einsatz von";
                bRange = worksheet.get_Range("D4:D4");
                bRange.Value = "Einsatz bis";
                bRange = worksheet.get_Range("E4:E4");
                bRange.Value = "Stunden";


                if (radioButton1.Checked == true)
                {
                    for (int i = 0; i < einsatz.Count; i++)
                    {

                        if (Benutzerlogin.mitarbeiter == einsatz[i].getMaid())
                        {
                            
                            double stunde = Convert.ToDouble(einsatz[i].getEinsatzbis().Substring(0, 5).Replace(":",",")) - Convert.ToDouble(einsatz[i].getEinsatzvon().Substring(0, 5).Replace(":", ","));

                            bRange = worksheet.get_Range("B" + (i + 5) + ":B" + (i + 5));
                            bRange.Value = einsatz[i].getDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("C" + (i + 5) + ":C" + (i + 5));
                            bRange.Value = einsatz[i].getEinsatzvon().Substring(0, 5);
                            bRange = worksheet.get_Range("D" + (i + 5) + ":D" + (i + 5));
                            bRange.Value = einsatz[i].getEinsatzbis().Substring(0, 5);
                            bRange = worksheet.get_Range("E" + (i + 5) + ":E" + (i + 5));
                            bRange.Value = stunde.ToString();
                        }
                    }
                }

                else if (radioButton2.Checked == true)
                {
                    for (int i = 0; i < einsatz.Count; i++)
                    {
                        if (Benutzerlogin.mitarbeiter == einsatz[i].getMaid() && einsatz[i].getDatum().Contains(textBox1.Text))
                        {
                            double stunde = Convert.ToDouble(einsatz[i].getEinsatzbis().Substring(0, 5).Replace(":", ",")) - Convert.ToDouble(einsatz[i].getEinsatzvon().Substring(0, 5).Replace(":", ","));

                            bRange = worksheet.get_Range("B" + (i + 5) + ":B" + (i + 5));
                            bRange.Value = einsatz[i].getDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("C" + (i + 5) + ":C" + (i + 5));
                            bRange.Value = einsatz[i].getEinsatzvon().Substring(0, 5);
                            bRange = worksheet.get_Range("D" + (i + 5) + ":D" + (i + 5));
                            bRange.Value = einsatz[i].getEinsatzbis().Substring(0, 5);
                            bRange = worksheet.get_Range("E" + (i + 5) + ":E" + (i + 5));
                            bRange.Value = stunde.ToString();
                        }
                    }
                }

                else if (radioButton3.Checked == true)
                {
                    for (int i = 0; i < einsatz.Count; i++)
                    {
                         if (Benutzerlogin.mitarbeiter == einsatz[i].getMaid() && einsatz[i].getDatum().Contains(monate[comboBox1.SelectedIndex].getZahl()))
                        {
                            double stunde = Convert.ToDouble(einsatz[i].getEinsatzbis().Substring(0, 5).Replace(":", ",")) - Convert.ToDouble(einsatz[i].getEinsatzvon().Substring(0, 5).Replace(":", ","));

                            bRange = worksheet.get_Range("B" + (i + 5) + ":B" + (i + 5));
                            bRange.Value = einsatz[i].getDatum().Substring(0, 10);
                            bRange = worksheet.get_Range("C" + (i + 5) + ":C" + (i + 5));
                            bRange.Value = einsatz[i].getEinsatzvon().Substring(0, 5);
                            bRange = worksheet.get_Range("D" + (i + 5) + ":D" + (i + 5));
                            bRange.Value = einsatz[i].getEinsatzbis().Substring(0, 5);
                            bRange = worksheet.get_Range("E" + (i + 5) + ":E" + (i + 5));
                            bRange.Value = stunde.ToString();
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Bitte Auswahl festlegen");
                }

                workbook.SaveAs(@"F:\GPB\C#\CSharp-Projekte\MitarbeiterZeitmanagement\bin\Debug\AuswertungEinsatz-" + label1.Text + ".xlsx");
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
