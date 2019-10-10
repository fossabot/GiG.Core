@OrleansSamplePayments
@OrleansSamplePaymentsNegative
Feature: Orleans Sample Payments Negative Tests

@Deposit
Scenario Outline: Deposit to player account
	Given I 'Unsuccessfully' Deposit '<amount>' on the account for player with IP '<ipAddress>'
	Then the status code for 'Deposit' is '<responseCode>'
	And the error message for 'Deposit' is '<message>'

	Examples:
    | amount | ipAddress  | responseCode        | message								  |
    | 13.00  |	invalidIp | UnprocessableEntity | IP Address is invalid				      |
    | -1.5	 |	127.0.0.1 | BadRequest		    | Deposit Amount must be greater than 10. |
    | 0		 |	127.0.0.1 | BadRequest		    | Deposit Amount must be greater than 10. |
    | 9.99	 |	127.0.0.1 | BadRequest		    | Deposit Amount must be greater than 10. |

@Withdraw
Scenario Outline: Withdraw from player account
	Given I Deposit '<amount>' on the account for player with IP '127.0.0.1'
	When I 'Unsuccessfully' withdraw '<withdrawalAmount>' from account for player with IP '<ipAddress>'
	Then the status code for 'Withdraw' is '<responseCode>'
	And the error message for 'Withdraw' is '<message>'

	Examples:
    | amount | withdrawalAmount | ipAddress | responseCode        | message                                                                      |
    | 13.00  | 12.00            | invalidIp | UnprocessableEntity | IP Address is invalid														 |
    | 13.00  | 500.00           |           | BadRequest          | Withdraw Amount must be smaller or equal to the Balance, and greater than 0. |
    | 13.00  | 0	            |           | BadRequest          | Withdraw Amount must be smaller or equal to the Balance, and greater than 0. |
    | 13.00  | -1	            |           | BadRequest          | Withdraw Amount must be smaller or equal to the Balance, and greater than 0. |