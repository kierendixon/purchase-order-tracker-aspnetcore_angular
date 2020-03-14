Feature: Open targetted page after login

  Background:
    Given a user navigates to the Login page

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
