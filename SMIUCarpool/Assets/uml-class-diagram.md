# SMIU Carpool Matchmaker UML

```mermaid
classDiagram
    class User {
      +UserID
      +FullName
      +Email
      +Phone
      +Role
      +GetDashboardText()
    }

    class Rider {
      +VehicleType
    }

    class Passenger
    class Ride
    class Booking

    User <|-- Rider
    User <|-- Passenger
```
