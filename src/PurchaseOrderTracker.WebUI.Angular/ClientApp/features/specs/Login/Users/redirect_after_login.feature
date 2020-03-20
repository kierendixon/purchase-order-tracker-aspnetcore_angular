Feature: Redirect after login

  In order to improve the end user's experience
  As a product owner
  I want users to be redirected to the page they tried to view after they login

  If a user tries to view a restricted page when they are unauthenticated they will be redirected to the login page.
  After logging in they will be redirected to the page they tried to view.

  Scenario Outline: Redirect to page after login
    Given Alexa navigates to <url>
    And she is asked to login
    When she logs in
    Then she should be redirected to <url>

    Examples:
      | url                  |
      | /main-site/shipments |
      | /main-site/suppliers |

  Scenario Outline: Do not redirect to external pages
    Given Alexa navigates to <url>
    And she is asked to login
    When she logs in
    Then she should be redirected to the default landing page

    Examples:
      | url                                            |
      | /account?redirectUrl=http:%2F%2Fwww.google.com |
