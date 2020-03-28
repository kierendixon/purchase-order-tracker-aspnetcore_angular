Feature: User guide

  The Purchase Order Tracker user guide is published as a Wiki in its GitHub repository.

  Scenario: Display user guide
    Given Alexa is logged in
    When she opens the user guide
    Then the user guide should be displayed
