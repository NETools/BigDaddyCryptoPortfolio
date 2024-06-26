﻿using BigDaddyCryptoPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels
{
	public interface ICoinsViewModel : INotifyPropertyChanged
	{
        public List<string> Categories { get; }
		public List<Color> CategoryColor { get; }
		public bool IsCategorySelectorExpanded { get; set; }
		public string SelectedCategory { get; set; }	

        public List<Coin> Coins { get; }
		public Coin? SelectedCoin { get; }
		public string UiInfoMessage { get; }

        public void SelectCategory(int index);
		public void SelectCoin(Coin coin);
		public void AddCoin(Coin coin);
		public void DeleteCoin(Coin coin);

		// public void SetIndices(int startIndex, int endIndex);
	}
}
