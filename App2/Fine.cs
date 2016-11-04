using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public class Fine : IPunishment
    {
        public string PunishmentSize { get; set; }
        public bool IsGood { get; set; }
        public Fine(string punishmentSize, bool isGood)
        {
            PunishmentSize = punishmentSize;
            IsGood = isGood;
        }

        public int GetInt()
        {
            return int.Parse(PunishmentSize);
        }
    }
}
