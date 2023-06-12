using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHPluginCoreLib;
using NetworkMessages;
using UnityEngine.UI;

namespace BankTransferUtilityProgramMod
{
	class BankTransferUtilityProgram : GreyOSProgram
	{
		private InputField bankAccountNumberInputField = null;

		private InputField bankAccountPasswordInputField = null;

		private InputField transferAmountInputField = null;

		private Button transferButton = null;

		private Button viewBankAccountDetailsButton = null;

		private Toggle isPasswordEncryptedToggle = null;

		private Text bankAccountBalanceLabel = null;

		private void PerformTransaction(string originBankAccountPlainTextPassword, string originBankAccountNumber, string destinationBankAccountNumber, int transferAmount)
		{
			MessageServer messageServer = new MessageServer(IdServer.BankTransactionServerRpc);

			Message message = messageServer;

			List<string> stringList = new List<string>();

			stringList.Add(originBankAccountPlainTextPassword);

			stringList.Add(originBankAccountNumber);

			stringList.Add(destinationBankAccountNumber);

			message.AddString(stringList);

			Message message2 = messageServer;

			List<int> intList = new List<int>();

			intList.Add(transferAmount);

			intList.Add(this.ventana.PID);

			message2.AddInt(intList);

			PlayerClient.Singleton.SendData(messageServer);
		}

		private void OnTransferButtonClicked()
		{
			string plainTextTargetBankAccountPassword = bankAccountPasswordInputField.text;

			if (isPasswordEncryptedToggle.isOn == true)
			{
				plainTextTargetBankAccountPassword = Database.Singleton.Decipher(
					bankAccountPasswordInputField.text
				);
			}

			this.PerformTransaction(
				plainTextTargetBankAccountPassword,
				bankAccountNumberInputField.text,
				PlayerClient.Singleton.player.pc.userBank.user,
				Convert.ToInt32(transferAmountInputField.text)
			);

			this.OnViewBankAccountDetailsButtonClicked();
		}

		private void OnViewBankAccountDetailsButtonClicked()
		{
			BankAccount targetBankAccount = Database.Singleton.GetBankAccount(this.bankAccountNumberInputField.text);

			if (targetBankAccount != null)
			{
				this.bankAccountBalanceLabel.text = "Target bank account balance ($): " + targetBankAccount.GetMoney();
			}
			else
			{
				this.bankAccountBalanceLabel.text = "Target bank account balance ($):";
			}
		}

		private void Init()
		{
			this.bankAccountNumberInputField = GHPluginUtilities.GetComponentInGameObjectByName<InputField>(
				this.dialog.GO_Content,
				"BankAccountNumberInputField"
			);

			this.bankAccountPasswordInputField = GHPluginUtilities.GetComponentInGameObjectByName<InputField>(
				this.dialog.GO_Content,
				"BankAccountPasswordInputField"
			);

			this.transferAmountInputField = GHPluginUtilities.GetComponentInGameObjectByName<InputField>(
				this.dialog.GO_Content,
				"TransferAmountInputField"
			);

			this.isPasswordEncryptedToggle = GHPluginUtilities.GetComponentInGameObjectByName<Toggle>(
				this.dialog.GO_Content,
				"IsPasswordEncryptedToggle"
			);

			this.transferButton = GHPluginUtilities.GetComponentInGameObjectByName<Button>(
				this.dialog.GO_Content,
				"TransferButton"
			);

			this.viewBankAccountDetailsButton = GHPluginUtilities.GetComponentInGameObjectByName<Button>(
				this.dialog.GO_Content,
				"ViewBankAccountDetailsButton"
			);

			this.bankAccountBalanceLabel = GHPluginUtilities.GetComponentInGameObjectByName<Text>(
				this.dialog.GO_Content,
				"BankAccountBalanceLabel"
			);

			this.transferButton.onClick.AddListener(this.OnTransferButtonClicked);

			this.viewBankAccountDetailsButton.onClick.AddListener(this.OnViewBankAccountDetailsButtonClicked);
		}

		public BankTransferUtilityProgram() : base("Bank Transfer Utility", BankTransferUtilityProgramPlugin.instance.pluginDataDirectoryPath + "/experimental_grey_hack_mod_asset_bundle", "BankTransferUtility")
		{

		}

		public override string Procesar(string[] comando, FileSystem.Archivo currentFile, int PID, Terminal terminal)
		{
			base.Procesar(comando, currentFile, PID, terminal);

			this.Init();

			return "";
		}
	}
}
