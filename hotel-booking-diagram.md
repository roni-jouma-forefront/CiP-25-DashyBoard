erDiagram
    Hotel ||--|{ Room : Has
    Room ||--o{ Booking : "Has Bookings"
    Booking }|--|| Guest : "For Guest"
    Booking }o--|| Flight : "Departure Flight"
    Hotel ||--o{ Message : Sends
    Hotel ||--o{ Admin : Employs
    Admin ||--o{ Message : Creates
    Booking ||--o{ Message : "Receives"

    Hotel {
        int Id PK
        string Name
        string IcaoCode
    }

    Room {
        int Id PK
        int HotelId FK
        string RoomNumber
    }

    Guest {
        int Id PK
        string FullName
    }

    Admin {
        int Id PK
        int HotelId FK
        string Username
        string PasswordHash
        string FullName
        enum Role
    }

    Booking {
        int Id PK
        int RoomId FK
        int GuestId FK
        int DepartureFlightId FK
        DateTime CheckIn
        DateTime CheckOut
        enum Status
    }

    Flight {
        int Id PK
        string FlightNumber
        string DestinationIcao
        DateTime ScheduledDeparture
    }

    Message {
        int Id PK
        int HotelId FK
        int BookingId FK
        int CreatedBy FK
        string Content
        DateTime CreatedAt
        DateTime ExpiresAt
        bool IsActive
    }
