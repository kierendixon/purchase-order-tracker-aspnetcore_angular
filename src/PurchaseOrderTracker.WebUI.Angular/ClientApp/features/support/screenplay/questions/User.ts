import { Question } from '@serenity-js/core';
import { BrowseTheWeb } from '@serenity-js/protractor';

export class User {
  static isAuthenticated(): Question<Promise<boolean>> {
    return Question.about(
      `user authentication status`,
      actor =>
        BrowseTheWeb.as(actor).executeScript(
          'check if user is authenticated',
          'return window.localStorage.getItem("currentUser") != null;'
        ) as Promise<boolean>
    );
  }
}
