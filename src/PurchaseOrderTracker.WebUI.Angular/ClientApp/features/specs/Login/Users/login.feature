Feature: Login
  IN order to protect the system from unauthorised access
  AS a product owner
  I want users to authenticate before they use the system

  Background:
    Given Alexa navigates to the Login page

  Scenario Outline: Authenticate using username and password
    When she enters username <username>
    And she enters password <password>
    And she submits the login form
    Then she should be shown the default landing page

    Examples:
      | username | password |
      | super    | super    |
      | basic    | basic    |

  Scenario Outline: Authenticate using username and password fails
    When she enters username <username>
    And she enters password <password>
    And she submits the login form
    Then she should be shown a login error message "<message>"

    Examples:
      | username | password | message                         |
      | super    | SUPER    | Username or password is invalid |
      | basic    | wrongpw  | Username or password is invalid |
