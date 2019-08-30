﻿@WebSamplePositive
Feature: Web Sample Tests

Background:
	Given I get the current balance of the player
	And I get the minimum deposit limit amount
	
@GetBalance
Scenario: Get balance of player
	And I deposit '10' 'more' than the mimimum deposit amount
	When I get the new balance of the player
	Then the balance should be updated correctly

@Deposit
Scenario: Deposit an amount to player
	And I deposit '10' 'more' than the mimimum deposit amount
	When I get the new balance of the player
	Then the balance should be updated correctly

@Withdraw
Scenario: Withdraw an amount
	And I deposit '10' 'more' than the mimimum deposit amount
	When I withdraw '5'
	And I get the new balance of the player
	Then the balance should be updated correctly