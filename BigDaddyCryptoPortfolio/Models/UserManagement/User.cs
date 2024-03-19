using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.UserManagement
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
    }
}
