@OrleansSamplePaymentTransactions
@OrleansSamplePaymentTransactionsNegative
Feature: Orleans Sample Payment Transactions Negative Tests

@GetBalance
Scenario Outline: Get Player Transactions
	When I request the transactions of account for player with '<playerId>' id and IP '<ipAddress>'
	Then the status code is '<responseCode>'
	And the error message is '<message>'

	Examples:
    | amount | playerId | ipAddress | responseCode               | message               |
#    | 10.50  | unknown  | 127.0.0.1 | UnprocessableEntity       | Player ID is missing  |
#    | 10.50  | invalid	| 127.0.0.1 | UnprocessableEntity        | Player ID is invalid  |
    | 10.50  |			| invalidIp | UnprocessableEntity        | IP Address is invalid |