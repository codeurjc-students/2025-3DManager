using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DMANAGER.DAL.Base
{
    public interface IDataSource<T>
    {
        T GetDataSource();
    }
}
