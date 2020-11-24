import { Ensure, startsWith, endsWith, equals } from '@serenity-js/assertions';
import { actorCalled, actorInTheSpotlight, Duration } from '@serenity-js/core';
import { Navigate, Website, Enter, Target, Click, Wait, Text } from '@serenity-js/protractor';
import { Then, When, Given, TableDefinition } from 'cucumber';
import { by, protractor } from 'protractor';
import { User } from '../support/screenplay/questions/User';

Then(/^s?he should see the following purchase order summary$/, (dataTable: TableDefinition) => {
  interface DataTable {
    OpenOrders: string;
    DeliveriesToday: string;
    LateDeliveries: string;
    VeryLateDeliveries: string;
  }
  const expectedValues: DataTable = (dataTable.hashes()[0] as unknown) as DataTable;

  return actorInTheSpotlight().attemptsTo(
    Ensure.that(
      Text.of(Target.the('total open orders').located(by.css('main span:nth-child(3)'))),
      equals(`Total open orders: ${expectedValues.OpenOrders}`)
    ),
    Ensure.that(
      Text.of(Target.the('deliveries for today').located(by.css('main span:nth-child(5)'))),
      equals(`Shipments scheduled for delivered today: ${expectedValues.DeliveriesToday}`)
    ),
    Ensure.that(
      Text.of(Target.the('late deliveries').located(by.css('main span:nth-child(7)'))),
      equals(`Shipments past delivery due date: ${expectedValues.LateDeliveries}`)
    ),
    Ensure.that(
      Text.of(Target.the('very late deliveries').located(by.css('main span:nth-child(9)'))),
      equals(`Shipments past delivery due date more than 7 days: ${expectedValues.VeryLateDeliveries}`)
    )
  );
});
