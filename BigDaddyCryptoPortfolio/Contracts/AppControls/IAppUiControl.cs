using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.AppControls
{
	public interface IAppUiControl
	{
		public event Action<ShellContent> AddTabRequested;
		public event Action<string> RemoveTabRequested;

		public void AddTab(ShellContent content);
		public void RemoveTab(string title);
	}
}
