Feature: Get Users API
  As a consumer of the API
  I want to retrieve user data
  So that I can validate the response

  Scenario: Retrieve users successfully
    When I send a GET request
    Then the response status should be 200
    And the response should contain "data"