@OrleansSamplePayments
@OrleansSamplePaymentsPositive
Feature: Orleans Sample Payments Tests

@Deposit
Scenario Outline: Deposit to player account
	Given I Deposit '<amount>' on the account for player with IP '<ipAddress>'
	Then the status code for 'Deposit' is 'OK'
	And the 'DepositBalance' is '<amount>'

	Examples:
    | amount | ipAddress |
    | 10.50  | 127.0.0.1 |
    | 13.00  |           |

@Withdraw
Scenario Outline: Withdraw from player account
	Given I Deposit '<amount>' on the account for player with IP '<ipAddress>'
	When I withdraw '<withdrawalAmount>' from account for player with IP '<ipAddress>'
	Then the status code for 'Withdraw' is 'OK'
	And the 'WithdrawalBalance' is '<balance>'

	Examples:
    | amount | ipAddress | withdrawalAmount | balance |
    | 10.50  | 127.0.0.1 | 10.50            | 0.00    |
    | 13.00  |           | 12.00            | 1.00    |