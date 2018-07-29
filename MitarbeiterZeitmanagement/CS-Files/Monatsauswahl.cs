using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitarbeiterZeitmanagement
{
    class Monatsauswahl
    {
        private string monat;
        private string zahl;

        public Monatsauswahl(string m, string z) {
            monat = m;
            zahl = z;
        }

        public string getMonat() {
            return monat;
        }

        public string getZahl()
        {
            return zahl;
        }



    }
}
