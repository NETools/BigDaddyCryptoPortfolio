using BigDaddyCryptoPortfolio.Gestures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Gestures
{
	internal class TappingCountDetection<T>
	{
		private Dictionary<T, GestureTapData> _managedListViews = [];
		private long _lastHandledTimestamp = 0;

		public void Register(T element, int maximumTappings, long duration)
		{
			if (!_managedListViews.ContainsKey(element))
			{
				_managedListViews.Add(element, new GestureTapData()
				{
					MaximumTappings = maximumTappings,
					LastSelectedItem = null,
					Duration = duration
				});
			}
		}

		public void HandleTapping(T element, object selectedObject, Action onTappingCountReached)
		{
			EvalTappingData(_managedListViews[element], selectedObject, onTappingCountReached);
		}

		private void EvalTappingData(GestureTapData data, object selectedObject, Action whenReached)
		{
			if (data.LastSelectedItem == null)
			{
				data.LastSelectedItem = selectedObject;
			}

			if (data.LastSelectedItem != null && data.LastSelectedItem == selectedObject)
			{
				var duration = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _lastHandledTimestamp;
				if (duration <= data.Duration)
				{
					data.TappedCount++;
					if (data.TappedCount >= data.MaximumTappings)
					{
						data.LastSelectedItem = -1;
						data.TappedCount = 0;
						whenReached?.Invoke();
					}
				}
				else data.TappedCount = data.MaximumTappings;

				_lastHandledTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			}
			else if (data.LastSelectedItem != null && data.LastSelectedItem != selectedObject)
			{
				data.LastSelectedItem = selectedObject;
			}
		}
	}
}
