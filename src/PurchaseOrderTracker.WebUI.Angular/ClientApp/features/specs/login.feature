Feature: Login
  IN order to protect the system from unauthorised access
  AS a product owner
  I want users to authenticate before they use the system

  Background:
    Given a user navigates to the Login page

  @only
  Scenario Outline: Authenticate using username and password
    When a user enters username <username>
    And they enter password <password>
    And they submit the login form
    Then they should be shown the MainSiteLanding page

    Examples:
      | username | password |
      | super    | super    |
      | basic    | basic    |

  Scenario Outline: Authenticate using username and password fails
    When a user enters username <username>
    And they enter password <password>
    And they submit the login form
    Then they should be shown a login error message "<message>"

    Examples:
      | username | password | message                         |
      | super    | SUPER    | Username or password is invalid |
      | basic    | wrongpw  | Username or password is invalid |

  Scenario Outline: Redirect to navigated url after login
    Given a user navigates to <url>
    And they are asked to login
    When they login
    Then they should be redirected to <url>

    Examples:
      | url                 |
      | main-site/shipments |
      | main-site/suppliers |

  Scenario Outline: Do not navigate to external url after login
    Given a user navigates to <url>
    And they are asked to login
    When they login
    Then they should be shown the MainSiteLanding page

    Examples:
      | url                                                             |
      | http://localhost:4200/account?redirectUrl=http://www.google.com |
