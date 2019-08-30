Feature: Web Sample Negative Tests


@DepositLessThanLimit
Scenario: Deposit an amount to player less than deposit limit
	Given I get the minimum deposit limit amount
	When I deposit '10' 'less' than the mimimum deposit amount
	Then the deposit response is a 'BadRequest'

Scenario: Withdraw amount more than balance
	Given I get the current balance of the player
	When I withdraw '10' 'more' than the current balance
	Then the withdraw response is a 'BadRequest'
	
