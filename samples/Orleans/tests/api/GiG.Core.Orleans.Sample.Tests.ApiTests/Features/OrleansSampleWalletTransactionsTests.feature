@OrleansSampleWalletTransactions
@OrleansSampleWalletTransactionsPositive
Feature: Orleans Sample Wallet Transactions Tests

@GetWalletTransactions
Scenario Outline: Get Wallet Transactions
	Given I 'Successfully' Deposit '<depositAmount>' on the account for player with IP '<ipAddress>'
	Then I 'Successfully' withdraw '<withdrawalAmount>' from account for player with IP '<ipAddress>'
	When I request the wallet transactions of account for player with IP '<ipAddress>'
	Then the status code for wallet transactions is 'OK'
	And the number of transactions is '2' with transaction types '<transactionTypes>', amount '<depositAmount>,<withdrawalAmount>' and new balance '<depositAmount>,<finalBalance>'

	Examples:
    | depositAmount | withdrawalAmount | finalBalance | ipAddress | transactionTypes   |
    | 20.00         | 10.00            | 10.00        | 127.0.0.1 | Credit,Debit       |
    | 13.00         | 12.50            | 0.50         |           | Credit,Debit       |