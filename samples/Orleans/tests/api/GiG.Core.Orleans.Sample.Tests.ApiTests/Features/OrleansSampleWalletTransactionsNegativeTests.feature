@OrleansSampleWalletTransactions
@OrleansSampleWalletTransactionsNegative
Feature: Orleans Sample Wallet Transactions Negative Tests

@GetTransactions
Scenario Outline: Get Wallet Transactions
	When I request the wallet transactions of account for player with IP '<ipAddress>'
	Then the status code for wallet transactions is '<responseCode>'
	And the error message for wallet transactions is '<message>'

	Examples:
    | ipAddress | responseCode               | message               |
    | invalidIp | UnprocessableEntity        | IP Address is invalid |