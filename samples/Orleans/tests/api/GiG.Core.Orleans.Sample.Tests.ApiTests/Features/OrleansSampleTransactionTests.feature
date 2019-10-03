@OrleansSampleTransaction
@OrleansSampleTransactionPositive
Feature: Orleans Sample Transactions Tests

@GetBalance
Scenario Outline: Get Player Balance
	Given I Deposit '<amount>' on the account for player with IP '<ipAddress>'
	When I request the balance of account for player with IP '<ipAddress>'
	Then the status code for 'GetBalance' is 'OK'
	And the 'GetBalance' balance is '<amount>'

	Examples:
    | amount | ipAddress |
    | 10.00  | 127.0.0.1 |
    | 13.00  |           |

@Deposit
Scenario Outline: Deposit to player account
	Given I Deposit '<amount>' on the account for player with IP '<ipAddress>'
	Then the status code for 'Deposit' is 'OK'
	And the 'Deposit' balance is '<amount>'

	Examples:
    | amount | ipAddress |
    | 10.50  | 127.0.0.1 |
    | 13.00  |           |

@Withdraw
Scenario Outline: Withdraw from player account
	Given I Deposit '<amount>' on the account for player with IP '<ipAddress>'
	When I withdraw '<withdrawalAmount>' from account for player with IP '<ipAddress>'
	Then the status code for 'Withdraw' is 'OK'
	And the 'Withdraw' balance is '<balance>'

	Examples:
    | amount | ipAddress | withdrawalAmount | balance |
    | 10.50  | 127.0.0.1 | 10.50            | 0.00    |
    | 13.00  |           | 12.00            | 1.00    |