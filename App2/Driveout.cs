using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public class Driveout : IPunishment
    {
        public string PunishmentSize { get; set; }
        public bool IsGood { get; set; }

        public Driveout(string punishmentSize, bool isGood)
        {
            IsGood = isGood;
            PunishmentSize = punishmentSize;
        }

        public int GetInt()
        {
            return int.Parse(PunishmentSize);
        }
    }
}
