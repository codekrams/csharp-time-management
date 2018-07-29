using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitarbeiterZeitmanagement
{
    class Fehlgrund
    {
        private int id;
        private string fehlgrund;

        public Fehlgrund(int id, string grund) {
            this.id = id;
            fehlgrund = grund;
        }

        public int getId() {
            return id;
        }

        public string getFehlgrund() {
            return fehlgrund;
        }
    }
}
