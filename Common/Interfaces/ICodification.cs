using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICodification<T>
    {
        Byte[] Encode(T data);
        T Decode(Byte[] message);
    }
}
