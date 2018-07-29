using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitarbeiterZeitmanagement
{
    class Mitarbeiter
    {
        private int maid;
        private string nachname;
        private string vorname;
        private string gebdat;
        private int tagesarbeitszeit;
        private int urlaubsanspruch;
        private string bildurl;
        private int bewertung;

        public Mitarbeiter(int id, string nname, string vname, string geb, int zeit, int urlaub, string url, int b)
        {
            maid = id;
            nachname = nname;
            vorname = vname;
            gebdat = geb;
            tagesarbeitszeit = zeit;
            urlaubsanspruch = urlaub;
            bildurl = url;
            bewertung = b;
        }

        public int getMaid()
        {
            return maid;
        }

        public int getBewertung()
        {
            return bewertung;
        }

        public string getBild() {
            return bildurl;
        }

        public string getNachname()
        {
            return nachname;
        }

        public string getVorname()
        {
            return vorname;
        }

        public string getGebdat()
        {
            return gebdat;
        }

        public int getZeit() {
            return tagesarbeitszeit;
        }

        public int getUrlaub() {
            return urlaubsanspruch;
        }
    }
}
