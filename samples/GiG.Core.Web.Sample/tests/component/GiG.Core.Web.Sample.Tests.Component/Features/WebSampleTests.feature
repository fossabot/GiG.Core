Feature: Web Sample Tests

@GetBalance
Scenario: Get balance of player
	Given I get the current balance of the player
	And I deposit '10'
	When I get the new balance of the player
	Then the balance should be updated correctly

@Deposit
Scenario: Deposit an amount to player
	Given I get the current balance of the player
	And I deposit '10'
	When I get the new balance of the player
	Then the balance should be updated correctly

@Withdraw
Scenario: Withdraw an amount
	Given I get the current balance of the player
	And I deposit '10'
	When I withdraw '5'
	And I get the new balance of the player
	Then the balance should be updated correctly




