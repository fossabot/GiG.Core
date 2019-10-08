@OrleansSamplePaymentTransactions
@OrleansSamplePaymentTransactionsPositive
Feature: Orleans Sample Payment Transactions Tests

@GetTransactions
Scenario Outline: Get Player Transactions
	Given I Deposit '<depositAmount>' on the account for player with IP '<ipAddress>'
	Then I withdraw '<withdrawalAmount>' from account for player with IP '<ipAddress>'
	When I request the transactions of account for player with IP '<ipAddress>'
	Then the status code is 'OK'
	And the number of transactions is '2' with transaction types '<transactionTypes>' and values '<depositAmount>,<withdrawalAmount>'

	Examples:
    | depositAmount | withdrawalAmount | ipAddress | transactionTypes   |
    | 20.00         | 10.00            | 127.0.0.1 | Deposit,Withdrawal |
    | 13.00         | 12.50            |           | Deposit,Withdrawal |