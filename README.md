# Task Manager 🗂️

**Status:** 🚧 Under Development

A collaborative task management API built with **ASP.NET Core**, **Clean Architecture**, **CQRS**, and **MediatR**.
Designed for teams to manage projects, boards, lists, and cards with secure JWT authentication and role‑based authorization.

---

## 🗂️ Domain Model Overview

The Task Manager API follows a clear hierarchy:

**Project → Board → List → Card**

* **Project**: The top‑level container. Users can be members of one or more projects.
* **Board**: Belongs to a project. Organizes lists for a specific workflow or topic.
* **List**: Belongs to a board. Holds cards in a specific stage or category.
* **Card**: Belongs to a list. Represents a single task or work item.

**Special Rules & Relationships**

* Each project has a default **Inbox** list for quick capture of tasks.
* Cards can be **assigned to one or more project members**.
* Lists can be moved between boards in the same project or in another project where the user is a member.
* Cards can be moved between lists or into/out of the Inbox (within projects the user has access to).

## ✨ Features

### ✅ Implemented

* **Authentication & Authorization**
  * JWT‑based login and registration
  * Authorize Service to manage entities

* **Projects**
  * CRUD
  * Automatic creation of a default **Inbox** list for each project

* **Inbox**
  * Special list accessible from all boards within a project

* **Boards**
  * CRUD

* **Lists**
  * CRUD
  * Moving lists

* **Cards**
  * CRUD
  * Moving Cards
  * Assign members to Cards

---

### 🛠 Planned

* Metrics & logging
* Email confirmation
* Real‑time updates
* Notifications
* Unit & integration tests
* Add more features like checklists/labels/comments...

---

## 🏗️ Tech Stack

* **Backend:** ASP.NET Core 8, C#
* **Frontend:** React + TypeScript
* **Architecture:** Clean Architecture (CQRS, MediatR)
* **Database:** SQL Server
* **Auth:** JWT Bearer
* **Docs:** Swagger / OpenAPI
