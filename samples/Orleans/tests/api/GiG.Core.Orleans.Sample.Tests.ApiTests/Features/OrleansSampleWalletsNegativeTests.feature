@OrleansSampleWallets
@OrleansSampleWalletsNegative
Feature: Orleans Sample Wallets Negative Tests

@GetTransactions
Scenario Outline: Get Wallet Balance
	When I request the wallet balance for player with IP '<ipAddress>'
	Then the status code for get wallet balance is '<responseCode>'
	And the error message for wallet balance is '<message>'

	Examples:
    | ipAddress | responseCode               | message               |
    | invalidIp | UnprocessableEntity        | IP Address is invalid |