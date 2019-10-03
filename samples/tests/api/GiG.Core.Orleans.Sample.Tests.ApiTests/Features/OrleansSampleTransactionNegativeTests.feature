@OrleansSampleTransaction
@OrleansSampleTransactionNegative
Feature: Orleans Sample Transactions Negative Tests

@GetBalance
Scenario Outline: Get Player Balance
	When I request the balance of account for player with '<playerId>' id and IP '<ipAddress>'
	Then the status code for 'GetBalance' is '<responseCode>'
	And the error message for 'GetBalance' is '<message>'

	Examples:
    | amount | playerId | ipAddress | responseCode               | message               |
    | 10.50  | unknown  | 127.0.0.1 | UnprocessableEntity        | Player ID is missing  |
    | 10.50  | invalid	| 127.0.0.1 | UnprocessableEntity        | Player ID is invalid  |
    | 10.50  |			| invalidIp | UnprocessableEntity        | IP Address is invalid |

@Deposit
Scenario Outline: Deposit to player account
	Given I Deposit '<amount>' on the account for player with '<playerId>' id and IP '<ipAddress>'
	Then the status code for 'Deposit' is '<responseCode>'
	And the error message for 'Deposit' is '<message>'

	Examples:
    | amount | playerId | ipAddress | responseCode        | message								    |
    | 10.50  | unknown	| 127.0.0.1 | UnprocessableEntity | Player ID is missing					|
    | 13.00  | invalid	|		    | UnprocessableEntity | Player ID is invalid					|
    | 13.00  |	  	    | invalidIp | UnprocessableEntity | IP Address is invalid				    |
    | -1.5	 |	  	    | 127.0.0.1 | BadRequest		  | Deposit Amount must be greater than 10. |
    | 0		 |	  	    | 127.0.0.1 | BadRequest		  | Deposit Amount must be greater than 10. |
    | 9.99	 |	  	    | 127.0.0.1 | BadRequest		  | Deposit Amount must be greater than 10. |

@Withdraw
Scenario Outline: Withdraw from player account
	Given I Deposit '<amount>' on the account for player with IP '127.0.0.1'
	When I withdraw '<withdrawalAmount>' from account for player with '<playerId>' id and IP '<ipAddress>'
	Then the status code for 'Withdraw' is '<responseCode>'
	And the error message for 'Withdraw' is '<message>'

	Examples:
    | amount | withdrawalAmount | playerId | ipAddress | responseCode        | message                                                                      |
    | 10.50  | 10.50            | unknown  | 127.0.0.1 | UnprocessableEntity | Player ID is missing														    |
    | 13.00  | 12.00            | invalid  |           | UnprocessableEntity | Player ID is invalid														    |
    | 13.00  | 12.00            |          | invalidIp | UnprocessableEntity | IP Address is invalid														|
    | 13.00  | 500.00           |          |           | BadRequest          | Withdraw Amount must be smaller or equal to the Balance, and greater than 0. |
    | 13.00  | 0	            |          |           | BadRequest          | Withdraw Amount must be smaller or equal to the Balance, and greater than 0. |
    | 13.00  | -1	            |          |           | BadRequest          | Withdraw Amount must be smaller or equal to the Balance, and greater than 0. |