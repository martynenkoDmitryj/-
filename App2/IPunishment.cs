using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public interface IPunishment
    {
         string PunishmentSize { get; set; }
         bool IsGood { get; set; }
         int GetInt();
         
             
         
    }
}
