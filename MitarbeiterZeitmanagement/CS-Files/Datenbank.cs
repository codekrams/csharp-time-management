using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MitarbeiterZeitmanagement
{
    class Datenbank
    {

        private MySqlConnection dbConnection;

        public void dbOeffnen()
        {
            try
            {
                dbConnection = new MySqlConnection("SERVER=127.0.0.1; DATABASE=personal;UID=" + Benutzerlogin.benutzer + "; PASSWORD=" + Benutzerlogin.password + ";  SSLMode=None;");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void dbSchliessen()
        {
            dbConnection.Close();
        }

        public List<Mitarbeiter> getAllMitarbeiter()
        {
            List<Mitarbeiter> liste = new List<Mitarbeiter>();

            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; SELECT * FROM personal.mitarbeiter;";
            dbConnection.Open();
            MySqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                Mitarbeiter m = new Mitarbeiter(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), reader.GetInt32(7));
                liste.Add(m);
            }
            reader.Close();
            dbConnection.Close();
            dbSchliessen();

            return liste;
        }

        public List<Fehlzeit> getAllFehlzeiten()
        {
            List<Fehlzeit> liste = new List<Fehlzeit>();

            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; SELECT * FROM personal.fehlzeit;";
            dbConnection.Open();
            MySqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                Fehlzeit f = new Fehlzeit(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5));
                liste.Add(f);
            }
            reader.Close();
            dbConnection.Close();
            dbSchliessen();

            return liste;
        }

        public List<Fehlgrund> getAllFehlgruende()
        {

            List<Fehlgrund> liste = new List<Fehlgrund>();

            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; SELECT * FROM personal.fehlgrund;";
            dbConnection.Open();
            MySqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                Fehlgrund fg = new Fehlgrund(reader.GetInt32(0), reader.GetString(1));
                liste.Add(fg);
            }
            reader.Close();
            dbConnection.Close();
            dbSchliessen();

            return liste;
        }

        public List<Einsatz> getAllEinsaetze()
        {

            List<Einsatz> liste = new List<Einsatz>();

            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; SELECT * FROM personal.einsatz;";
            dbConnection.Open();
            MySqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                Einsatz e = new Einsatz(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                liste.Add(e);
            }
            reader.Close();
            dbConnection.Close();
            dbSchliessen();

            return liste;
        }

        public void safeMitarbeiter(string nachname, string vorname, string gebdat, string arbeitszeit, string urlaub, string bild, string bewertung)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();

            comm.CommandText = "USE personal; INSERT INTO personal.mitarbeiter VALUES (NULL, '" +nachname + "', '"  + vorname + "', '" + gebdat + "', '" +arbeitszeit + "', '" +urlaub + "', '" + bild + "', '" + bewertung+ "'); ";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void safeFehlgrund(string grund)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();

            comm.CommandText = "USE personal; INSERT INTO personal.fehlgrund VALUES (NULL, '" + grund + "');";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void safeFehlzeit(int maid, string vondatum, string bisdatum, int fehlid, string fehltage)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();

            comm.CommandText = "USE personal; INSERT INTO personal.fehlzeit VALUES (NULL, '" + maid +"', '" + vondatum + "', '" +bisdatum +"', '" +fehlid +"', '"+ fehltage +"');";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void safeEinsatz(int maid, string datum, string einsatzvon, string einsatzbis)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();

            comm.CommandText = "USE personal; INSERT INTO personal.einsatz VALUES (NULL, '" + maid +"', '" + datum + "', '"  + einsatzvon + "', '" + einsatzbis + "');";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void updateMitarbeiter(string nachname, string vorname, string gebdat, string tagesarbeitszeit, string urlaubsanspruch, string bewertung, int maid)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();


            comm.CommandText = "USE personal; UPDATE personal.mitarbeiter SET nachname= '" + nachname + "', vorname='" + vorname + "', gebdat= '" + gebdat + "', tagesarbeitszeit= '" + tagesarbeitszeit + "', urlaubsanspruch= '" + urlaubsanspruch + "', bewertung='" + bewertung + "' WHERE maid = '" + maid + "';";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        

        public void updateFehlgrund(string grund, int id)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();

            comm.CommandText = "USE personal; UPDATE personal.fehlgrund SET bezeichnung = '" + grund + "' WHERE fehlid = '" + id +"'; ";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void updateFehlzeit(int maid, string vondatum, string bisdatum, int fid, string fehltage, int fzid)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();

            comm.CommandText = "UPDATE personal.fehlzeit SET maid = '" + maid +"', von_Datum = '" + vondatum +"', bis_Datum = '" + bisdatum + "', fid = '" +fid + "', fehltage = '" + fehltage + "' WHERE fzid = '" + fzid+ "';";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void updateEinsatz(int maid, string datum, string einsatzVon_Zeit, string einsatzBis_Zeit, int eid)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; UPDATE personal.einsatz SET maid = '" + maid + "', datum = '" + datum + "', einsatzVon_Zeit = '" + einsatzVon_Zeit + "', einsatzBis_Zeit = '" + einsatzBis_Zeit + "' WHERE eid = '" + eid + "';";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void deleteMitarbeiter(int id)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal;  DELETE from personal.einsatz WHERE maid = " + id + "; DELETE from personal.fehlzeit WHERE maid = " + id + "; DELETE from personal.mitarbeiter WHERE maid = " + id + ";";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void deleteFehlgrund(int id)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; DELETE from personal.fehlzeit WHERE fid = " + id + "; DELETE from personal.fehlgrund WHERE fehlid = " + id + ";";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void deleteFehlzeit(int id)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; DELETE from personal.fehlzeit WHERE fzid = " + id + ";";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void deleteEinsatz(int id)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();
            comm.CommandText = "USE personal; DELETE from personal.einsatz WHERE eid = " + id + ";";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }

        public void safePicture(string bild, int maid)
        {
            dbOeffnen();
            MySqlCommand comm = dbConnection.CreateCommand();


            comm.CommandText = "USE personal; UPDATE personal.mitarbeiter SET  bild='" + bild + "' WHERE maid = '" + maid + "';";

            dbConnection.Open();
            comm.ExecuteNonQuery();

            dbConnection.Close();
            dbSchliessen();
        }
    

}
}