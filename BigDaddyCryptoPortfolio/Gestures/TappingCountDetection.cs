using BigDaddyCryptoPortfolio.Gestures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Gestures
{
	internal class TappingCountDetection
	{
		private Dictionary<ListView, GestureTapData> _managedListViews = [];
		private long _lastHandledTimestamp = 0;

		public void Register(ListView listView, int maximumTappings, long duration)
		{
			if (!_managedListViews.ContainsKey(listView))
			{
				_managedListViews.Add(listView, new GestureTapData()
				{
					MaximumTappings = maximumTappings,
					LastSelectedIndex = -1,
					Duration = duration
				});
			}
		}

		public void HandleTapping(ListView listView, int selectedIndex, Action onTappingCountReached)
		{
			EvalTappingData(_managedListViews[listView], selectedIndex, onTappingCountReached);
		}

		private void EvalTappingData(GestureTapData data, int selectedIndex, Action whenReached)
		{
			if (data.LastSelectedIndex == -1)
			{
				data.LastSelectedIndex = selectedIndex;
			}

			if (data.LastSelectedIndex != -1 && data.LastSelectedIndex == selectedIndex)
			{
				var duration = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _lastHandledTimestamp;
				if (duration <= data.Duration)
				{
					data.TappedCount++;
					if (data.TappedCount >= data.MaximumTappings)
					{
						data.LastSelectedIndex = -1;
						data.TappedCount = 0;
						whenReached?.Invoke();
					}
				}
				else data.TappedCount = data.MaximumTappings;

				_lastHandledTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			}
			else if (data.LastSelectedIndex != -1 && data.LastSelectedIndex != selectedIndex)
			{
				data.LastSelectedIndex = selectedIndex;
			}
		}
	}
}
