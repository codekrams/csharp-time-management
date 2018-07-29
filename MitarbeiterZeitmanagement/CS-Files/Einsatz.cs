using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitarbeiterZeitmanagement
{
    class Einsatz
    {
        private int id;
        private int maid;
        private string datum;
        private string einsatzvon;
        private string einsatzbis;

        public Einsatz(int id, int maid, string datum, string einsatzvon, string einsatzbis) {
            this.id = id;
            this.maid = maid;
            this.datum = datum;
            this.einsatzvon = einsatzvon;
            this.einsatzbis = einsatzbis;
        }

        public int getId() {
            return id;
        }
        public int getMaid()
        {
            return maid;
        }

        public string getDatum()
        {
            return datum;
        }
        public string getEinsatzvon()
        {
            return einsatzvon;
        }
        public string getEinsatzbis()
        {
            return einsatzbis;
        }
    }
}
