using _3DMANAGER.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DMANAGER.DAL.Interfaces
{
    public interface IPrinterDbManager
    {
        List<PrinterDbObject> GetPrinterList(out int error);
    }
}
