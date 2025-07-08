# 🎯 MAS Event Management System

This project is a comprehensive **Event Management System** developed as part of the MAS (Modeling and Analysis of Information Systems) course at PJATK. It combines object-oriented modeling, persistence, and a functional GUI to simulate real-world event planning, promotion, and participation workflows.

## 📌 Overview

The system allows:

- **Two user types**: `RegularUser` and `Organizer`
- **Event creation and enrollment**
- **Promoted event requests and payment tracking**
- **Category-based filtering and tag system**
- **Discussion system restricted to enrolled participants**
- **Messaging between users (including following/followers)**
- **Dynamic GUI interactions with persistent data**

## 🧩 Features

- 🔐 **Authentication-Free Role Management**: Regular and Organizer users are handled through OOP principles, without a database login system.
- 🗓️ **Event Management**: Events include categories, tags, venue, enrollment limits, and timelines.
- 💬 **Discussion Threads**: Enrolled users can post comments on events.
- 💸 **Payment Integration**: Organizers can pay to promote events. All payments are linked to transactions and a `PaymentDetail` account.
- 🔄 **Object Persistence**: All data is stored using extent-based design and serialized with `BinaryFormatter`.
- 💻 **Blazor GUI**: A simple, responsive interface supports event viewing, enrollment, and navigation.

## 🛠️ Technologies

- **C# (.NET 6)**
- **Blazor WebAssembly**
- **Object-Oriented Programming (OOP)**
- **BinaryFormatter / FileStream**
- **UML-driven Design**
- **Rider / Visual Studio**

## 🗃️ Project Structure
MAS_Project/
│
├── Models/
│ ├── Base/ # Business logic classes (Event, User, Enrollment, etc.)
│ └── Persistence/ # Extent save/load implementations
│
├── MAS_GUI/ # Blazor GUI frontend
├── UML Diagram.png # Class design overview
└── MAS-information-en.pdf# Course project requirements


## 🚀 Getting Started

1. Clone this repository
2. Open the solution in **Rider** or **Visual Studio 2022+**
3. Make sure `.NET 6 SDK` is installed
4. Set `MAS_GUI` as the startup project
5. Run the project – Blazor frontend will open in your browser

## 🎓 Academic Note

This application was developed for educational purposes as a final project for the MAS course. It demonstrates correct use of:

- OOP principles (inheritance, composition, associations)
- GUI usability practices
- Subset, qualified and composition associations
- Constraints and validation logic
- Manual persistence with `.cs` and `.dat` storage

## 📬 Contact

For questions or collaboration opportunities, feel free to contact me through GitHub or reach out at my email
kysallabas@gmail.com

---


