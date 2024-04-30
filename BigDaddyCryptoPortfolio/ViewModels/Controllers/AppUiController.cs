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
        public event Action<ShellContent> AddTabRequested;
        public event Action<string> RemoveTabRequested;

        public void AddTab(ShellContent content)
        {
            AddTabRequested?.Invoke(content);
        }

        public void RemoveTab(string title)
        {
            RemoveTabRequested?.Invoke(title);
        }
    }
}
