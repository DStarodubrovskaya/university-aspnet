 - General Description -

My application is a task management system. You can add tasks, delete them, filter by priority, run them with a time simulation, and see how long it will take to complete all the tasks.

 - Project Structure -

My project was built on Razor Pages. Each action has its own page:
	"Index" - login page where you select a user.
	"AddUser" - page for managing users (adding and deleting).
	"ManageTasks" - page for managing tasks: adding, deleting, filtering...
	"Execute" - page for running a selected task with time simulation.

In addition, I built a Models' layer:
	"User" - represents a user with an ID (Id) and a list of tasks.
	"TaskItem" - represents a single task.

All the central logic is in Services:
	"TasksService" - All recursive functions with nodes of tasks.
	"UsersService" - Works with MongoDB.
	"SessionService" - Responsible for managing the current user during the session, remembering who is logged in and what tasks they have.


 - The building process -

The main task was to work with recursion and linked lists.
It is located in `TasksService`.

But I decided to expand and work with other topics from the course:
-Added a MongoDB database to save the tasks even after exiting the application.
- built separate Services for the session, the database management, and the tasks.
-You can choose a user when entering the application, so that tracking the tasks in the database is personal and meaningful.
-I used Route parameters to pass the task's id to the execution page.
-Added an ExecutedTasks log where the tasks that have been executed are saved.
-Added CSS to improve the appearance of the application and make it more pleasing to the eye.


 - GPT usage - 

GPT helped me especially with CSS. This saved me time in the design.
When I had bugs in the code and couldn't figure out what the problem was, it showed me where the mistake was.
It helped me display the priority of the tasks in words instead of numbers, and helped to extract unique task names for delete feature. 
It suggested where to put nullable values so that there would be no green lines in the reader.
It also gave me the idea of adding additional functions in UserService, which translate between a regular list and a Linked List. So that tasks could be saved and retrieved from MongoDB conveniently.

 - Personal summary -

It was a very interesting project, in which I tried to combine many topics that we learned in the course.
I started with simple tasks with recursion, and then I decided that saving only in the session was not enough for me and I wanted a more stable database. From there, I also wanted to allow multiple users, with separate sessions and personal saving.
Every time I had more ideas on how to improve an app.

I enjoyed using Dependency Injection the most and felt its power. For example, I used the User Service from a previous project again. I just connected it with the DI and I could use it directly without rewriting all the functions.
The hardest part for me was designing the logic of how the information was loaded and saved part in the session and part in the database. It was hard for me to imagine it in my head.

Overall, I really enjoyed this work. I felt a bit like a real developer. Thanks for an interesting and meaningful course!

