Feature: Logout
  In order to prevent a user's account from being reused by another person
  As a product owner
  I want users to have the option to logout when they are finished using the system

  Scenario: Logout
    Given Alexa is logged in
    When she logs out
    Then she will no longer be authenticated
