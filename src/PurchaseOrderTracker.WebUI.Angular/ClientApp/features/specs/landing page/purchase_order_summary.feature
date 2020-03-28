Feature: Purchase order summary landing page
  In order for users to understand the current state of purchase orders in the systems
  As a product owner
  I want users to see a snapshot of the current state right after they login

  Scenario: Purchase order summary
    When Alexa logs in
    Then she should be shown the default landing page
    And she should see the following purchase order summary
      | OpenOrders | DeliveriesToday | LateDeliveries | VeryLateDeliveries |
      | 9          | 0               | 2              | 1                  |
