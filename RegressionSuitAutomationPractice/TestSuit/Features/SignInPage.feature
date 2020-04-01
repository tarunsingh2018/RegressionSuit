Feature: SignInPageFeature

@Regression
Scenario Outline: Verify Sign-in Validation messages
Given the user has already loaded the application
And User enters <Email> and <Password> in Sign in Page
When user clicks Sign in button
Then verify below error <Message> on the page
Examples:
| Email          | Password | Message                    |
| <blank>        | <blank>  | An email address required. |
| test@gmail.com | <blank>  | Password is required.      |
| <blank>        | pass123  | An email address required. |
| test@gmail.com | pass123  | Authentication failed.     |