using BigDaddyCryptoPortfolio.Contracts.AppControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Controllers
{
    class AppUiController : IAppUiControl
    {
        public event Action<string> AddTabRequested;
        public event Action<string> RemoveTabRequested;

        public void AddTab(string tabName)
        {
            AddTabRequested?.Invoke(tabName);
        }
    }
}
