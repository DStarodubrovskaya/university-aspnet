# ASP.NET Course - Projects

This repo contains three projects I built during my ASP.NET course.  
Each assignment was a separate project, and I always tried to go a bit further than required.

---

## 01 – Library Management (CSV + Razor Pages)
A small app to manage books: search, add and delete.  
Data is stored in a CSV file (used CsvHelper), and I added state management with IMemoryCache.  
**Grade: 95**
*Comment: points were reduced because I used two separate id fields for search and delete (could be one), 
and for inserting a book I should have bound directly to a Book object instead of separate fields.*

---

## 02 – Task Manager (Razor Pages + Database)
A task manager system with two entities: **Users** and **Tasks**, each mapped to its own MongoDB collection.  
Users can be added and deleted, tasks can be created, assigned, displayed, and managed.

**Main features (as required):**
- Add and delete users  
- Add tasks and assign them to existing users  
- View tasks by user (with routing /id)  
- Dependency Injection for accessing services  
- TempData for passing messages  

**What I added beyond requirements:**
- Automatic unassigning of tasks when a user is deleted  
- Updating tasks with pre-loaded data  
- Filtering tasks not only by user, but also by status   
- UX improvements:
  - TempData success/error messages  
  - Date restrictions (no past dates, default today)  
  - Proper date formatting without default placeholders  
**Grade: 97**
*Comment: points were reduced because I bound Users and Tasks in the PageModel classes without need,
and because IDs were not displayed, so different users or tasks could look identical.*

---

## 03 – Task Scheduler (Linked List + Recursion)
This project was the most fun for me. I started with simple recursion and linked lists, but quickly realized I wanted more than just saving in session. 
So I connected MongoDB, added multiple users with their own sessions, and even built an archive of completed tasks.
The hardest part was designing how to combine session state with database saving, but it taught me a lot.
What I enjoyed most was Dependency Injection. I could just plug in the UserService I had built in the previous project and reuse it without rewriting.

Overall, I really felt like I was building a real application, not just a homework exercise.
**Grade: 100**

---

## What I learned
- Building apps with Razor Pages and state management  
- Working with MongoDB/MySQL and dependency injection  
- Recursion and linked lists on a real project  
