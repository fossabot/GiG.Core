@OrleansSampleTransaction
@OrleansSampleTransactionPositive
Feature: Orleans Sample Transactions Tests

@GetBalance
Scenario Outline: Get Player Balance
	Given I Deposit <amount> on the account for player with id <playerId> and IP <ipAddress> using key DepositResponse
	When I request the balance of account for player with id <playerId> and IP <ipAddress> using key <responseKey>
	Then the status code for key <responseKey> is OK
	And the balance of the account using key <responseKey> is <amount>

	Examples:
    | amount | playerId | ipAddress | responseKey        |
    | 10.50  | 123456   | 127.0.0.1 | GetBalanceResponse |
    | 13.00  | 654321   |           | GetBalanceResponse |

@Deposit
Scenario Outline: Deposit to player account
	Given I Deposit <amount> on the account for player with id <playerId> and IP <ipAddress> using key <responseKey>
	Then the status code for key <responseKey> is OK
	And the balance of the account using key <responseKey> is <amount>

	Examples:
    | amount | playerId | ipAddress | responseKey     |
    | 10.50  | 234567   | 127.0.0.1 | DepositResponse |
    | 13.00  | 765432   |           | DepositResponse |

@Withdraw
Scenario Outline: Withdraw from player account
	Given I Deposit <amount> on the account for player with id <playerId> and IP <ipAddress> using key DepositResponse
	When I withdraw <withdrawalAmount> from account for player with id <playerId> and IP <ipAddress> using key <responseKey>
	Then the status code for key <responseKey> is OK
	And the balance of the account using key <responseKey> is <balance>

	Examples:
    | amount | withdrawalAmount | playerId | ipAddress | responseKey      | balance |
    | 10.50  | 10.50            | 345678   | 127.0.0.1 | WithdrawResponse | 0.00    |
    | 13.00  | 12.00            | 876543   |           | WithdrawResponse | 1.00    |