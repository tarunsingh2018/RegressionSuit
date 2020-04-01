Feature: SignInPageFeature

Background: 
Given the user has already loaded the application

@Regression
Scenario Outline: Verify Sign-in Validation messages
Given User enters <Email> and <Password> in Sign in Page
When user clicks Sign in button
Then verify below validation messages
Examples:
| Email          | Password | Message                    |
| <blank>        | <blank>  | An email address required. |
| test@gmail.com | <blank>  | Password is required.      |
| <blank>        | pass123  | An email address required. |
| test@gmail.com | pass123  | Authentication failed.     |