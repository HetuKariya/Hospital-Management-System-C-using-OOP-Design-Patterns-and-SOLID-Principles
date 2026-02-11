# Hospital Management System

A comprehensive console-based Hospital Management System built in C# that demonstrates **SOLID principles** and **Design Patterns** for managing patients, doctors, appointments, and billing operations.

## 📋 Table of Contents

- [Features](#features)
- [Design Patterns](#design-patterns)
- [SOLID Principles](#solid-principles)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Future Enhancements](#future-enhancements)

## ✨ Features

- **Patient Management**: Register, update, and manage patient records with medical history
- **Doctor Management**: Add and manage doctors with specializations and availability
- **Appointment System**: Book, cancel, and complete appointments with real-time notifications
- **Billing System**: Dynamic billing calculations based on patient type (General, Premium, Emergency)
- **Notification System**: Automated notifications via SMS and Email
- **Logging System**: Comprehensive activity logging with timestamps
- **Type-Safe Operations**: Strongly-typed enums for appointment status and patient types

## 🎨 Design Patterns

This project implements several industry-standard design patterns:

### 1. **Singleton Pattern**
- **Class**: `HospitalLogger`
- **Purpose**: Ensures a single instance of the logger throughout the application
- **Implementation**: Thread-safe double-checked locking

### 2. **Repository Pattern**
- **Classes**: `PatientRepository`, `DoctorRepository`, `AppointmentRepository`
- **Purpose**: Abstracts data access logic and provides a clean separation of concerns
- **Benefits**: Easy to test, swap data sources, and maintain

### 3. **Factory Pattern**
- **Class**: `NotificationFactory`
- **Purpose**: Creates notification service instances (SMS, Email) without exposing creation logic
- **Benefits**: Centralized object creation, easy to extend with new notification types

### 4. **Strategy Pattern**
- **Classes**: `StandardBillingStrategy`, `PremiumBillingStrategy`, `EmergencyBillingStrategy`
- **Purpose**: Dynamically selects billing algorithm based on patient type
- **Benefits**: Open/Closed principle - easy to add new billing strategies without modifying existing code

### 5. **Observer Pattern**
- **Classes**: `AppointmentSubject`, `PatientNotificationObserver`, `DoctorNotificationObserver`
- **Purpose**: Notifies multiple parties (patients, doctors) when appointment status changes
- **Benefits**: Loose coupling between appointment system and notification system

## 🏛️ SOLID Principles

### Single Responsibility Principle (SRP)
- Each class has one reason to change
- `PatientRepository` only manages patient data
- `AppointmentService` only handles appointment business logic
- `BillingService` only handles billing calculations

### Open/Closed Principle (OCP)
- System is open for extension, closed for modification
- New billing strategies can be added without changing existing code
- New notification types can be added via factory without changing notification logic

### Liskov Substitution Principle (LSP)
- All billing strategies can be substituted for `IBillingStrategy` interface
- All repositories can be substituted for `IRepository<T>` interface

### Interface Segregation Principle (ISP)
- Small, focused interfaces: `IRepository<T>`, `INotificationService`, `IBillingStrategy`, `IAppointmentObserver`
- Classes only implement interfaces they actually use

### Dependency Inversion Principle (DIP)
- High-level modules depend on abstractions (interfaces), not concrete implementations
- Services depend on `IRepository<T>` rather than concrete repository classes
- Notification observers depend on `INotificationService` interface

## 🏗️ Architecture

```
┌─────────────────────────────────────────┐
│          Presentation Layer             │
│              (Console UI)               │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│          Service Layer                  │
│   ┌─────────────────────────────┐      │
│   │  AppointmentService         │      │
│   │  BillingService             │      │
│   └─────────────────────────────┘      │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│       Repository Layer                  │
│   ┌─────────────────────────────┐      │
│   │  PatientRepository          │      │
│   │  DoctorRepository           │      │
│   │  AppointmentRepository      │      │
│   └─────────────────────────────┘      │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│          Data Layer                     │
│      (In-Memory Collections)            │
└─────────────────────────────────────────┘
```

## 🚀 Getting Started

### Prerequisites

- .NET SDK 6.0 or higher
- C# 9.0 or higher
- A C# IDE (Visual Studio, VS Code, or Rider)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/hospital-management-system.git
```

2. Navigate to the project directory:
```bash
cd hospital-management-system
```

3. Build the project:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run
```

## 💻 Usage

The application runs a demonstration workflow that includes:

1. **Adding Patients**: Creates sample patients with different types (General, Premium, Emergency)
2. **Adding Doctors**: Registers doctors with their specializations
3. **Booking Appointments**: Schedules appointments between patients and doctors
4. **Completing Appointments**: Marks appointments as completed
5. **Generating Bills**: Calculates bills using different billing strategies
6. **Cancelling Appointments**: Demonstrates appointment cancellation
7. **Viewing Appointments**: Lists all appointments with their current status
8. **System Logs**: Displays all logged activities

### Example Output

```
========================================
   HOSPITAL MANAGEMENT SYSTEM
========================================

1. Adding Patients...

[2025-02-11 10:30:15] Patient added: John Doe
[2025-02-11 10:30:15] Patient added: Jane Smith
[2025-02-11 10:30:15] Patient added: Bob Johnson

2. Adding Doctors...

[2025-02-11 10:30:16] Doctor added: Dr. Sarah Williams
[2025-02-11 10:30:16] Doctor added: Dr. Michael Brown

...
```

## 📁 Project Structure

```
HospitalManagementSystem/
│
├── Entities (Models)
│   ├── Patient.cs
│   ├── Doctor.cs
│   ├── Appointment.cs
│   └── Enums (AppointmentStatus, PatientType)
│
├── Interfaces
│   ├── IRepository<T>
│   ├── INotificationService
│   ├── IBillingStrategy
│   └── IAppointmentObserver
│
├── Design Patterns
│   ├── Singleton (HospitalLogger)
│   ├── Repository (PatientRepository, DoctorRepository, AppointmentRepository)
│   ├── Factory (NotificationFactory)
│   ├── Strategy (Billing strategies)
│   └── Observer (Notification observers)
│
├── Services
│   ├── AppointmentService
│   └── BillingService
│
└── Program.cs (Entry point)
```

## 🔧 Key Components

### Patient Types
- **General**: Standard patients with base billing rates
- **Premium**: VIP patients with premium billing and services
- **Emergency**: Urgent cases with priority treatment and emergency billing

### Appointment Status
- **Scheduled**: Newly booked appointment
- **Completed**: Finished consultation
- **Cancelled**: Cancelled by patient or doctor

### Billing Strategies
- **Standard**: Base rate of $50 + $2/minute
- **Premium**: Base rate of $100 + $3/minute
- **Emergency**: Base rate of $200 + $5/minute (24/7 availability premium)

### Notification Services
- **SMS**: Text message notifications to patients
- **Email**: Email notifications to doctors and staff

## 🔮 Future Enhancements

- [ ] Database integration (SQL Server, PostgreSQL)
- [ ] RESTful API implementation
- [ ] Web-based UI (ASP.NET Core MVC/Blazor)
- [ ] Advanced scheduling with time conflict detection
- [ ] Medical records management with file uploads
- [ ] Prescription management system
- [ ] Insurance claim processing
- [ ] Multi-language support
- [ ] Reporting and analytics dashboard
- [ ] Role-based access control (RBAC)
- [ ] Integration with external lab systems
- [ ] Telemedicine capabilities
- [ ] Mobile app support

## 📝 Learning Objectives

This project is ideal for learning:

- Object-Oriented Programming (OOP) concepts
- SOLID principles in practice
- Common design patterns implementation
- Clean code architecture
- Dependency injection
- Interface-based programming
- Event-driven architecture (Observer pattern)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👨‍💻 Author

Your Name - [your-email@example.com](mailto:your-email@example.com)

## 🙏 Acknowledgments

- Design pattern references from "Design Patterns: Elements of Reusable Object-Oriented Software"
- SOLID principles inspired by Robert C. Martin's work
- Community feedback and contributions

---

**Note**: This is an educational project demonstrating software design principles. For production use, additional features like authentication, database persistence, error handling, and security measures should be implemented.
