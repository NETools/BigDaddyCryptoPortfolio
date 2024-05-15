﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models
{
	[Flags]
	public enum CoinCategory
	{
		Ai = 1 << 0,
		Web3 = 1 << 1,
		Defi = 1 << 2,
		Green = 1 << 3,
		Gaming = 1 << 4,
		BtcAssociates = 1 << 5,
		CBDCNetwork = 1 << 6,
		ECommerce = 1 << 7,
		Tokenization = 1 << 8,
		NoHype = 1 << 9,
	}

	public enum GenericMessageType
	{
		Failed,
		EncryptionHandshake,
		ActivationMessage,
		SignInMessage,
		SignUpMessage,
		UpdatePortfolioMessage,
		UpdateTransactionMessage,
		RetrievePortfolio
	}

	public enum CredentialType
	{
		Md5,
		Sha256,
		Sha512,
		NoHash
	}

	public enum TransactionType
	{
		Add,
		Remove
	}

	public enum Subscription
	{
		Essential,
		Plus,
		Premium
	}

	public enum AuthViews
	{
		Login,
		Register,
		Confirmation
	}

	public enum RetrievalType
	{
		Null,
		Portfolio,
		Transaction
	}

	public enum BitvavoApiType
	{
		REST,
		WebSockets
	}

	public enum LoggingType
	{
		Info,
		Error,
		Warning
	}

	public enum WebsocketResponseCodes
	{
		Auth = 100,
		Ticker = 110,
		Balance = 120,
	}

	public enum TransactionSide
	{
		Buy,
		Sell
	}
}
