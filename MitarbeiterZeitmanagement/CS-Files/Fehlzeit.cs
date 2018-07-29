using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitarbeiterZeitmanagement
{
    class Fehlzeit
    {
        private int id;
        private int maid;
        private string vondatum;
        private string bisdatum;
        private int fehlid;
        private int fehltage;


        public Fehlzeit(int id, int maid, string vondatum, string bisdatum, int fehlid, int fehltage) {
            this.id = id;
            this.maid = maid;
            this.vondatum = vondatum;
            this.bisdatum = bisdatum;
            this.fehlid = fehlid;
            this.fehltage = fehltage;
        }

        public int getId()
        {
            return id;
        }

        public int getMaid()
        {
            return maid;
        }
        public string getVonDatum()
        {
            return vondatum;
        }
        public string getBisDatum()
        {
            return bisdatum;
        }
        public int getFehlid()
        {
            return fehlid;
        }
        public int getFehltage()
        {
            return fehltage;
        }

    }
}
