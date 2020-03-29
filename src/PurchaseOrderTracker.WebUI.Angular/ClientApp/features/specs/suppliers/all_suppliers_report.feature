Feature: All Suppliers report

  In order to quickly understand what suppliers exist in the system
  As a supplier maintainer
  I want to view a list of all the suppliers that exist

  Scenario: View All Suppliers report
    Given Alexa is logged in
    When she opens the All Suppliers report
    Then she should see the following suppliers
      | Supplier           |
      | Furniture Max      |
      | Office Supplies A+ |
      | Techzon            |

  Scenario: Navigate to a supplier's details
    Given Alexa navigates to the All Suppliers report
    When she clicks on a supplier's id
    Then she should be navigated to the supplier's details page

  Scenario: Navigate to a supplier's products
    Given Alexa navigates to the All Suppliers report
    When she clicks on a supplier's products link
    Then she should be navigated to the supplier's products page
