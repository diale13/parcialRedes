using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface ISenderService
    {
        void CreateLog(string command, byte[] frame, int size);
        List<ServerEvent> GetFromMsMq(string filter);
        void CreateApiLog(ServerEvent e);

    }
}
