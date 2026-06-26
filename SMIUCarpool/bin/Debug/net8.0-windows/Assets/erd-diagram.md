# SMIU Carpool Matchmaker ERD

```mermaid
erDiagram
    USERS ||--o{ RIDES : posts
    USERS ||--o{ BOOKINGS : makes
    RIDES ||--o{ BOOKINGS : has
```
