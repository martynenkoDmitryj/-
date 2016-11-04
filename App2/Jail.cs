using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public class Jail : IPunishment
    {
        public string PunishmentSize { get; set; }
        public bool IsGood { get; set; }

        public Jail(string punishmentSize, bool isGood)
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
