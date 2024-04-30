using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels
{
	public interface IViewChange<T> where T : Enum
	{
		public event Action<T> ViewChange;
	}
}
