@OrleansSampleTransaction
@OrleansSampleTransactionNegative
Feature: Orleans Sample Transactions Negative Tests

@GetBalance
Scenario Outline: Get Player Balance
	Given I Deposit <amount> on the account for player with id 12345678 and IP <ipAddress>
	When I request the balance of account for player with id <playerId> and IP <ipAddress>
	Then the status code for key <responseKey> is <responseCode>
	And the error message using key <responseKey> is <message>

	Examples:
    | amount | playerId | ipAddress | responseKey        | responseCode               | message               |
    | 10.50  |          | 127.0.0.1 | GetBalanceResponse | UnprocessableEntity        | Player ID is missing  |
    | 10.50  | *&^%     | 127.0.0.1 | GetBalanceResponse | UnprocessableEntity        | Player ID is invalid  |
    | 10.50  | 12345678 | invalidIp | GetBalanceResponse | UnprocessableEntity        | IP Address is invalid |

@Deposit
Scenario Outline: Deposit to player account
	Given I Deposit <amount> on the account for player with id <playerId> and IP <ipAddress>
	Then the status code for key <responseKey> is <responseCode>
	And the error message using key <responseKey> is <message>

	Examples:
    | amount | playerId | ipAddress | responseKey     | responseCode               | message               |
    | 10.50  |			| 127.0.0.1 | DepositResponse | UnprocessableEntity        | Player ID is missing  |
    | 13.00  | *&^%		|           | DepositResponse | UnprocessableEntity        | Player ID is invalid  |
    | 13.00  | 1234567  | invalidIp | DepositResponse | UnprocessableEntity        | IP Address is invalid |

@Withdraw
Scenario Outline: Withdraw from player account
	Given I Deposit <amount> on the account for player with id 12345678 and IP <ipAddress>
	When I withdraw <withdrawalAmount> from account for player with id <playerId> and IP <ipAddress>
	Then the status code for key <responseKey> is <responseCode>
	And the error message using key <responseKey> is <message>

	Examples:
    | amount | withdrawalAmount | playerId | ipAddress | responseKey      | responseCode          | message														|
    | 10.50  | 10.50            |		   | 127.0.0.1 | WithdrawResponse | UnprocessableEntity   | Player ID is missing										|
    | 13.00  | 12.00            | *&^%	   |           | WithdrawResponse | UnprocessableEntity   | Player ID is invalid										|
    | 13.00  | 12.00            | 12345678 | invalidIp | WithdrawResponse | UnprocessableEntity   | IP Address is invalid										|
    | 13.00  | 500.00           | 12345678 |           | WithdrawResponse | BadRequest            | Withdraw Amount must be smaller or equal to the Balance.	|