@OrleansSampleWallets
@OrleansSampleWalletsPositive
Feature: Orleans Sample Wallets Tests

@GetTransactions
Scenario Outline: Get Wallet Balance
	Given I 'Successfully' Deposit '<amount>' on the account for player with IP '<ipAddress>'
	When I request the wallet balance for player with IP '<ipAddress>'
	Then the status code for get wallet balance is 'OK'
	And the wallet balance is '<amount>'

	Examples:
    | amount | ipAddress |
    | 20.00  | 127.0.0.1 |
    | 13.00  |           |